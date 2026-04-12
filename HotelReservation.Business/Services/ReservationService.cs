using HotelReservation.Domain;
using HotelReservation.Domain.Entities;
using HotelReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business.Services;

internal class ReservationService : IReservationService
{
    private readonly HotelReservationDbContext _context;

    public ReservationService(HotelReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ReservationDTO>> GetAsync(ReservationSearchCriteria? searchCriteria = null)
    {
        var query = _context.Reservations.Include(r => r.Customer).AsQueryable();

        if (searchCriteria != null)
        {
            // Filter by CustomerId if provided
            if (searchCriteria.CustomerId.HasValue)
            {
                query = query.Where(r => r.CustomerId == searchCriteria.CustomerId.Value);
            }

            // Filter by reservation date range if provided
            if (searchCriteria.ReservationDateStartRange.HasValue)
            {
                query = query.Where(r => r.CheckInDate >= searchCriteria.ReservationDateStartRange.Value);
            }

            if (searchCriteria.ReservationDateEndRange.HasValue)
            {
                query = query.Where(r => r.CheckInDate <= searchCriteria.ReservationDateEndRange.Value);
            }

            // Filter by checkout date range if provided
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
            RoomPrice = e.RoomPrice
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
            throw new RoomHasBeenPlacedException();
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

    public async Task UpdateReservationStatus(int reservationId, ReservationStatus status)
    {
        var reservation = await _context.Reservations.Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
        {
            throw new ResourceNotFoundException($"Reservation with id {reservationId} not found.");
        }

        var currentStatus = (ReservationStatus)(int)reservation.Status;

        if (currentStatus == ReservationStatus.Cancelled || currentStatus == ReservationStatus.CheckedOut)
        {
            throw new InvalidReservationStatusException("Cannot update reservation status when it is already cancelled or checked out.");
        }

        if (currentStatus == ReservationStatus.CheckedIn && status == ReservationStatus.NotArrived)
        {
            throw new InvalidReservationStatusException("Cannot change status from CheckedIn to NotArrived.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        reservation.Status = status;

        // If status is being set to CheckedOut, update room availability and actual checkout date
        if (status == ReservationStatus.CheckedOut)
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
