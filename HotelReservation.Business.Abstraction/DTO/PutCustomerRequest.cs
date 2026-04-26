using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

public class PutCustomerRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [DigitOnly]
    public string SocietyCardNumber { get; set; } = string.Empty;
}
