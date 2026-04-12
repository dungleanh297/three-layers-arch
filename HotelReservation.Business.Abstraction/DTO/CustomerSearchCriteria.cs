using System;

namespace HotelReservation.Business;

public class CustomerSearchCriteria
{
    public string? Name { get; set; }

    public string? Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; } = string.Empty;

    public string? SocietyCardNumber { get; set; } = string.Empty;
    
    public Pagination Pagination { get; init; } = Pagination.DefaultPagination;
}
