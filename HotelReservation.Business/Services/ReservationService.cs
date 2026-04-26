using HotelReservation.Domain;
using HotelReservation.Domain.Entities;
using HotelReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business;

internal class ReservationService : IReservationService
{
    private readonly HotelReservationDbContext _context;

    public ReservationService(HotelReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReservationDTO>> GetAsync(ReservationSearchCriteria? searchCriteria = null)
    {
        var query = _context.Reservations.AsNoTracking();

        if (searchCriteria != null)
        {
            if (searchCriteria.CustomerId.HasValue)
            {
                query = query.Where(r => r.CustomerId == searchCriteria.CustomerId.Value);
            }

            if (searchCriteria.ReservationDateStartRange.HasValue)
            {
                query = query.Where(r => r.CheckInDate >= searchCriteria.ReservationDateStartRange.Value);
            }

            if (searchCriteria.ReservationDateEndRange.HasValue)
            {
                query = query.Where(r => r.CheckInDate <= searchCriteria.ReservationDateEndRange.Value);
            }

            if (searchCriteria.CheckoutDateStartRange.HasValue)
            {
                query = query.Where(r => r.ExpectedCheckOutDate >= searchCriteria.CheckoutDateStartRange.Value);
            }

            if (searchCriteria.CheckoutDateEndRange.HasValue)
            {
                query = query.Where(r => r.ExpectedCheckOutDate <= searchCriteria.CheckoutDateEndRange.Value);
            }
        }

        var reservations = await query.Select(e => new ReservationDTO
        {
            Id = e.Id,
            RoomId = e.RoomId,
            CustomerId = e.CustomerId,
            CustomerName = e.Customer.Name,
            CheckInDate = e.CheckInDate,
            ExpectedCheckOutDate = e.ExpectedCheckOutDate,
            ActualCheckOutDate = e.ActualCheckOutDate,
            RoomPrice = e.RoomPrice,
            Status = e.Status
        }).ToListAsync();

        return reservations;
    }

    public async Task PlaceReservation(PlaceReservationRequest request)
    {
        var room = await _context.Rooms.FindAsync(request.RoomId);

        if (room == null)
        {
            throw new ResourceNotFoundException($"Room with id {request.RoomId} not found.");
        }

        var customer = await _context.Customers.FindAsync(request.CustomerId);

        if (customer == null)
        {
            throw new ResourceNotFoundException($"Customer with id {request.CustomerId} not found.");
        }

        if (room.HasBeenTaken)
        {
            throw new BusinessException(ErrorCodes.RoomHasBeenTaken, nameof(PlaceReservationRequest.RoomId), "Room has been taken");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        
        var reservation = new Reservation
        {
            RoomId = request.RoomId,
            CustomerId = request.CustomerId,
            CheckInDate = DateTime.UtcNow,
            ExpectedCheckOutDate = request.ExpectedCheckOutDate,
            ActualCheckOutDate = null,
            RoomPrice = room.Price,
            Status = request.GuestHasArrived ? ReservationStatus.CheckedIn : ReservationStatus.NotArrived
        };

        _context.Reservations.Add(reservation);

        room.HasBeenTaken = true;
        _context.Rooms.Update(room);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task UpdateReservationStatus(PutReservationStatusRequest request)
    {
        var reservation = await _context.Reservations.Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == request.ReservationId);

        if (reservation == null)
        {
            throw new ResourceNotFoundException($"Reservation with id {request.ReservationId} not found.");
        }

        var currentStatus = (ReservationStatus)(int)reservation.Status;

        if (currentStatus == ReservationStatus.Cancelled || currentStatus == ReservationStatus.CheckedOut)
        {
            throw new BusinessException(ErrorCodes.InvalidReservationStatus, nameof(PutReservationStatusRequest.Status), "Cannot update reservation status when it is already cancelled or checked out.");
        }

        if (currentStatus == ReservationStatus.CheckedIn && request.Status == ReservationStatus.NotArrived)
        {
            throw new BusinessException(ErrorCodes.InvalidReservationStatus, nameof(PutReservationStatusRequest.Status), "Cannot change status from CheckedIn to NotArrived.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        reservation.Status = request.Status;

        if (request.Status == ReservationStatus.CheckedOut)
        {
            reservation.Room.HasBeenTaken = false;
            reservation.ActualCheckOutDate = DateTime.UtcNow;
            _context.Rooms.Update(reservation.Room);
        }

        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }
}
