namespace HotelReservation.Business;

public abstract class BusinessException : Exception
{
    public int ErrorCode { get; set; }

    public string[] ErrorsProperties { get; set; } = Array.Empty<string>();

    protected BusinessException() : base()
    {
    }

    protected BusinessException(string message) : base(message)
    {
    }
}