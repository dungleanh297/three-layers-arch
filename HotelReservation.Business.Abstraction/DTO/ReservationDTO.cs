namespace HotelReservation.Business;

public class ReservationDTO
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime CheckInDate { get; set; }

    public DateTime ExpectedCheckOutDate { get; set; }

    public DateTime? ActualCheckOutDate { get; set; }

    public decimal RoomPrice { get; set; }
}
