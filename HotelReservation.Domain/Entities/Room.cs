namespace HotelReservation.Domain.Entities;

public class Room
{
    public int Id { get; set; }

    public int RoomNumber { get; set; }

    public int Floor { get; set; }

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;
}