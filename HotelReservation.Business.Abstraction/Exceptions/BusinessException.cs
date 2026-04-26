namespace HotelReservation.Business;

public class BusinessException : Exception
{
    public int ErrorCode { get; }

    public string ErrorProperty { get; }

    public BusinessException(int errorCode, string errorProperty, string message) : base(message)
    {
        ErrorCode = errorCode;
        ErrorProperty = errorProperty;
    }
}