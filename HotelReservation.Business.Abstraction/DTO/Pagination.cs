using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

public class Pagination
{
    public const int DefaultPaginationSize = 100;

    public static readonly Pagination DefaultPagination = new Pagination();

    public int StartRecordId { get; set; } = int.MinValue;

    [Range(1, DefaultPaginationSize)]
    public int Size { get; set; } = DefaultPaginationSize;
}
