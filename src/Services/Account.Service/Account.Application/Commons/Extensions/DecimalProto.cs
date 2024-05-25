
using Shared.DTOs;

namespace Account.Application.Commons.Extensions
{
    public static class DecimalProto
    {
        public static DecimalValue FromDecimal(this decimal value)
        {
            long units = (long)value;
            int nanos = (int)((value - units) * 1e9m);
            return new DecimalValue { Units = units, Nanos = nanos };
        }

    }
}
