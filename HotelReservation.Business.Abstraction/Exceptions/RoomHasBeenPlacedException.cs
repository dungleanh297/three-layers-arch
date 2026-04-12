using System;

namespace HotelReservation.Business;

public class RoomHasBeenPlacedException : BusinessException
{
    public RoomHasBeenPlacedException() : base("Room has been taken.")
    {
        ErrorCode = 409;
    }

    public RoomHasBeenPlacedException(string message) : base(message)
    {
        ErrorCode = 409;
    }
}
