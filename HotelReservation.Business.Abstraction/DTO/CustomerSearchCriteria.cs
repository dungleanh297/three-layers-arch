using System;
using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

public class CustomerSearchCriteria
{
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; } = string.Empty;

    [DigitOnly]
    public string? SocietyCardNumber { get; set; } = string.Empty;
    
    public Pagination Pagination { get; init; } = Pagination.DefaultPagination;
}
