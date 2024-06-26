namespace AgoraIO.Utils;

public class DynamicKeyUtil
{
    public static byte[] EncodeHmac(string key, byte[] message, string algorithm = "SHA1")
    {
        return EncodeHmac(Encoding.UTF8.GetBytes(key), message, algorithm);
    }

    public static byte[] EncodeHmac(byte[] keyBytes, byte[] textBytes, string algorithm = "SHA1")
    {
        using KeyedHashAlgorithm hashAlgorithm = GetHashAlgorithm(algorithm, keyBytes);
        byte[] hashBytes = hashAlgorithm.ComputeHash(textBytes);
        return hashBytes;
    }

    private static KeyedHashAlgorithm GetHashAlgorithm(string algorithm, byte[] keyBytes)
    {
        return algorithm.ToUpper() switch
        {
            "MD5" => new HMACMD5(keyBytes),
            "SHA256" => new HMACSHA256(keyBytes),
            "SHA384" => new HMACSHA384(keyBytes),
            "SHA512" => new HMACSHA512(keyBytes),
            _ => new HMACSHA1(keyBytes),
        };
    }

    public static string BytesToHex(byte[] data)
    {
        StringBuilder builder = new();
        foreach (byte b in data)
        {
            _ = builder.Append(b.ToString("X2"));
        }
        return builder.ToString().ToLower();
    }
}