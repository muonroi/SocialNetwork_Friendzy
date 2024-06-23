namespace Infrastructure.Helper;

public static class StringHelper
{
    public static string StringToBase64(this string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        string base64String = Convert.ToBase64String(plainTextBytes);

        return base64String;
    }

    public static string Base64ToString(this string base64Encoded)
    {
        base64Encoded = base64Encoded.Trim();

        try
        {
            byte[] base64Bytes = Convert.FromBase64String(base64Encoded);

            string decodedString = Encoding.UTF8.GetString(base64Bytes);

            return decodedString;
        }
        catch (FormatException)
        {
            // Handle invalid Base64 input
            return base64Encoded;
        }
    }
}