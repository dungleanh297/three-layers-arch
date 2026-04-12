namespace HotelReservation.Domain;

public enum ReservationStatus : int
{
    NotArrived = 0,

    CheckedIn = 1,

    CheckedOut = 2,

    Cancelled = 3,
}
