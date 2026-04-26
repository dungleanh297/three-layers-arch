using HotelReservation.Domain.Entities;
using HotelReservation.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Business;

internal class RoomService : IRoomService
{
    private readonly HotelReservationDbContext _context;

    public RoomService(HotelReservationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomDTO>> GetAsync()
    {
        var rooms = await _context.Rooms.AsNoTracking().Select(r => new RoomDTO
        {
            Id = r.Id,
            RoomNumber = r.RoomNumber,
            Floor = r.Floor,
            Price = r.Price,
            HasBeenTaken = r.HasBeenTaken
        }).ToListAsync();

        return rooms;
    }

    public async Task CreateAsync(PutRoomInformation room)
    {
        var newRoom = new Room
        {
            RoomNumber = room.RoomNumber,
            Floor = room.Floor,
            Price = room.Price,
            HasBeenTaken = false
        };

        _context.Rooms.Add(newRoom);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, PutRoomInformation room)
    {
        var existingRoom = await _context.Rooms.FindAsync(id);

        if (existingRoom == null)
        {
            throw new ResourceNotFoundException();
        }

        existingRoom.RoomNumber = room.RoomNumber;
        existingRoom.Floor = room.Floor;
        existingRoom.Price = room.Price;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null)
        {
            throw new ResourceNotFoundException();
        }

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }
}
