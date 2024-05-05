using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.ORMs.Dapper.EntityFrameworkCore.Storage.Converters
{
    public class StringConverter : ValueConverter<string, string>
    {
        public StringConverter()
            : base(
                v => v,
                v => v.Trim())
        {
        }
    }
}