using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

public class DateInTheFutureAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not DateTime dateTime)
        {
            return false;
        }

        var tomorrow = DateTime.UtcNow;
        
        return dateTime.Date >= tomorrow;
    }

    public override string FormatErrorMessage(string name)
    {
        return base.FormatErrorMessage(name) ?? $"The field {name} must be at least the next day.";
    }
}