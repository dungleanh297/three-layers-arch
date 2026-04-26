using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HotelReservation.Business;

public class GreaterThanOrEqualsToAttribute : ValidationAttribute
{
    public required string Property { get; init; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        Type? comparableType = GetComparableTypeOfValue(value);

        if (comparableType is null)
        {
            return ValidationResult.Success;
        }

        object? comparandValue = GetPropertyValue(validationContext.ObjectInstance, Property);

        if (comparandValue is null)
        {
            return ValidationResult.Success;
        }

        if (!comparandValue.GetType().IsAssignableTo(comparableType))
        {
            return ValidationResult.Success;
        }

        int compareResult = (int) comparableType.GetMethod(nameof(IComparable<>.CompareTo))!.Invoke(value, [comparandValue])!;

        if (compareResult < 0)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }

    private static object? GetPropertyValue(object instance, string propertyName)
    {
        PropertyInfo? propertyInfo = instance.GetType().GetProperty(propertyName);
        
        if (propertyInfo is null || !propertyInfo.CanRead)
        {
            return null;
        }

        return propertyInfo.GetValue(instance);
    }

    private static Type? GetComparableTypeOfValue(object value)
    {
        Type currentType = value.GetType();
        Type comparableType = typeof(IComparable<>).MakeGenericType(value.GetType());

        if (comparableType.IsAssignableFrom(currentType))
        {
            return comparableType;
        }

        return null;
    }
}
