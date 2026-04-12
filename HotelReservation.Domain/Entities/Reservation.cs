namespace HotelReservation.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int CustomerId { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime ExpectedCheckOutDate { get; set; }

    public DateTime? ActualCheckOutDate { get; set; }

    public decimal RoomPrice { get; set; }

    public Customer Customer { get; set; } = null!;

    public Room Room { get; set; } = null!;

}