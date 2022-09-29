using System.ComponentModel.DataAnnotations;

namespace DanskeBank.Application.Dtos.Request;

public class NonZeroEnumAttribute : RequiredAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
            return false;

        var type = value.GetType();
        var isValidEnum = type.IsEnum && Enum.IsDefined(type, value);

        if (!isValidEnum)
            return false;

        return (int)value > 0;
    }
}
