using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

public class PutRoomInformation
{
    [Range(1, int.MaxValue)]
    public int RoomNumber { get; set; }

    [Range(1, int.MaxValue)]
    public int Floor { get; set; }

    [Range(1, double.MaxValue)]
    public decimal Price { get; set; }
}
