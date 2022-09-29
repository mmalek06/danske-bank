using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DanskeBank.Infrastructure.TypeConversions;

internal class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter() : base(
        dateOnly => dateOnly.ToDateTime(TimeOnly.MinValue),
        dateTime => DateOnly.FromDateTime(dateTime))
    {
    }
}
