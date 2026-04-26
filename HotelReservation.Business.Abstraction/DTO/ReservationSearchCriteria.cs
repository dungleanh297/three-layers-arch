namespace HotelReservation.Business;

public class ReservationSearchCriteria
{
    public int? CustomerId { get; set; }

    public DateTime? ReservationDateStartRange { get; set; }

    [GreaterThanOrEqualsTo(Property = nameof(ReservationDateStartRange))]
    public DateTime? ReservationDateEndRange { get; set; }

    public DateTime? CheckoutDateStartRange { get; set; }

    [GreaterThanOrEqualsTo(Property = nameof(ReservationDateStartRange))]
    public DateTime? CheckoutDateEndRange { get; set; }

    public Pagination Pagination { get; set; } = Pagination.DefaultPagination;
}
