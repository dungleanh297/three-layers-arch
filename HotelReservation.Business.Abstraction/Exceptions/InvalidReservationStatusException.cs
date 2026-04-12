using System;

namespace HotelReservation.Business;

public class InvalidReservationStatusException : BusinessException
{
    public InvalidReservationStatusException() : base("Invalid reservation status transition.")
    {
        ErrorCode = 400;
    }

    public InvalidReservationStatusException(string message) : base(message)
    {
        ErrorCode = 400;
    }
}
