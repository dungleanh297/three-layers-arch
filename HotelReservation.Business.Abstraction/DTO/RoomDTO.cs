using System;

namespace HotelReservation.Business;

public class RoomDTO
{
    public int Id { get; set; }

    public int RoomNumber { get; set; }

    public int Floor { get; set; }

    public decimal Price { get; set; }

    public bool HasBeenTaken { get; set; }
}
