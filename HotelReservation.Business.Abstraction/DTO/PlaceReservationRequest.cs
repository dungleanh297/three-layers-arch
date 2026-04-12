namespace HotelReservation.Business;

public class PlaceReservationRequest
{
    public int RoomId { get; set; }

    public int CustomerId { get; set; }

    public bool GuestHasArrived { get; set; }

    [DateInTheFuture]
    public DateTime ExpectedCheckOutDate { get; set; }
}
