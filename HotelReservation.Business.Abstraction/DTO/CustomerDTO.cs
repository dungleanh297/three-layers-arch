namespace HotelReservation.Business;

public class CustomerDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string SocietyCardNumber { get; set; } = string.Empty;
}