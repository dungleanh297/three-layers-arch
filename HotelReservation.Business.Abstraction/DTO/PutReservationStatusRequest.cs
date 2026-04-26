using HotelReservation.Domain;

namespace HotelReservation.Business;

public class PutReservationStatusRequest
{
    public int ReservationId { get; set; }

    public ReservationStatus Status { get; set; }
}
