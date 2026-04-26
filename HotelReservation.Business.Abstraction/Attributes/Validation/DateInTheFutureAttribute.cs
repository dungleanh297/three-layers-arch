using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DateInTheFutureAttribute : ValidationAttribute
{
    private static readonly ValidationResult s_invalidResult = new ValidationResult("The date must be the date in the future, not today or in the past.");

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is not DateTime dateTime)
        {
            return ValidationResult.Success;
        }

        var tomorrow = DateTime.UtcNow;
        
        if (dateTime.Date >= tomorrow)
        {
            return ValidationResult.Success;
        }

        return s_invalidResult;
    }
}