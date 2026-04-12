namespace HotelReservation.Business;

public class ReservationSearchCriteria
{
    public int? CustomerId { get; init; }

    public DateTime? ReservationDateStartRange { get; init; }

    public DateTime? ReservationDateEndRange { get; init; }

    public DateTime? CheckoutDateStartRange { get; init; }

    public DateTime? CheckoutDateEndRange { get; init; }

    public Pagination Pagination { get; set; } = Pagination.DefaultPagination;
}
