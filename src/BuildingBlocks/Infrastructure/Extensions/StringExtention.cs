namespace Infrastructure.Extensions;

public static class StringExtention
{
    public static bool CompareIgnoreCase(this string value, string compareValue)
    {
        return value.Equals(compareValue, StringComparison.InvariantCultureIgnoreCase);
    }
}