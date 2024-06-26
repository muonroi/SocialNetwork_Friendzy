namespace AgoraIO.Media;

public class SignalingToken
{
    public static string GenerateToken(string appId, string certificate, string account, int expiredTsInSeconds)
    {
        StringBuilder digestString = new StringBuilder().Append(account).Append(appId).Append(certificate).Append(expiredTsInSeconds);
        byte[] output = MD5.HashData(Encoding.UTF8.GetBytes(digestString.ToString()));
        string token = Hexlify(output);
        string tokenString = new StringBuilder().Append('1').Append(':').Append(appId).Append(':').Append(expiredTsInSeconds).Append(':').Append(token).ToString();
        return tokenString;
    }

    private static string Hexlify(byte[] data)
    {
        char[] digitsLower = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        char[] toDigits = digitsLower;
        int length = data.Length;
        char[] outCharArray = new char[length << 1];
        for (int i = 0, j = 0; i < length; i++)
        {
            outCharArray[j++] = toDigits[(uint)(0xF0 & data[i]) >> 4];
            outCharArray[j++] = toDigits[0x0F & data[i]];
        }
        return new string(outCharArray);
    }
}