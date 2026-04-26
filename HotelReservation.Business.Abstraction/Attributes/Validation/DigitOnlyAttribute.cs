using System.ComponentModel.DataAnnotations;

namespace HotelReservation.Business;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DigitOnlyAttribute : ValidationAttribute
{
    private static readonly ValidationResult s_invalidResult = new ValidationResult("This property must only contain number characters");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string str)
        {
            return ValidationResult.Success;
        }

        ReadOnlySpan<char> strAsSpan = str.AsSpan();

        for (int i = strAsSpan.Length - 1; i >= 0; --i)
        {
            if (!char.IsAsciiDigit(strAsSpan[i]))
            {
                return s_invalidResult;
            }
        }

        return ValidationResult.Success;
    }
}
