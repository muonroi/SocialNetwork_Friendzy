namespace AgoraIO.Media;

public class DynamicKey4
{
    private const string PUBLIC_SHARING_SERVICE = "APSS";
    private const string RECORDING_SERVICE = "ARS";
    private const string MEDIA_CHANNEL_SERVICE = "ACS";

    public static string GeneratePublicSharingKey(string appID, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs)
    {
        return GenerateDynamicKey(appID, appCertificate, channelName, unixTs, randomInt, uid, expiredTs, PUBLIC_SHARING_SERVICE);
    }

    public static string GenerateRecordingKey(string appID, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs)
    {
        return GenerateDynamicKey(appID, appCertificate, channelName, unixTs, randomInt, uid, expiredTs, RECORDING_SERVICE);
    }

    public static string GenerateMediaChannelKey(string appID, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs)
    {
        return GenerateDynamicKey(appID, appCertificate, channelName, unixTs, randomInt, uid, expiredTs, MEDIA_CHANNEL_SERVICE);
    }

    private static string GenerateDynamicKey(string appID, string appCertificate, string channelName, int unixTs, int randomInt, string uid, int expiredTs, string serviceType)
    {
        const string version = "004";
        string unixTsStr = unixTs.ToString().PadLeft(10, '0');
        string randomIntStr = randomInt.ToString("x8").PadLeft(8, '0');
        string uidStr = uid;
        string expiredTsStr = expiredTs.ToString().PadLeft(10, '0');

        string signature = GenerateSignature(appID, appCertificate, channelName, unixTsStr, randomIntStr, uidStr, expiredTsStr, serviceType);
        return $"{version}{signature}{appID}{unixTsStr}{randomIntStr}{expiredTsStr}";
    }

    private static string GenerateSignature(string appID, string appCertificate, string channelName, string unixTsStr, string randomIntStr, string uidStr, string expiredTsStr, string serviceType)
    {
        using MemoryStream ms = new();
        using (BinaryWriter writer = new(ms))
        {
            writer.Write(Encoding.ASCII.GetBytes(serviceType));
            writer.Write(Encoding.ASCII.GetBytes(appID));
            writer.Write(Encoding.ASCII.GetBytes(unixTsStr));
            writer.Write(Encoding.ASCII.GetBytes(randomIntStr));
            writer.Write(Encoding.ASCII.GetBytes(channelName));
            writer.Write(Encoding.ASCII.GetBytes(uidStr));
            writer.Write(Encoding.ASCII.GetBytes(expiredTsStr));
        }

        byte[] sign = EncodeHMAC(appCertificate, ms.ToArray());
        return BytesToHex(sign);
    }

    private static byte[] EncodeHMAC(string appCertificate, byte[] data)
    {
        using HMACSHA1 hmac = new(Encoding.ASCII.GetBytes(appCertificate));
        return hmac.ComputeHash(data);
    }

    private static string BytesToHex(byte[] bytes)
    {
        StringBuilder sb = new();
        foreach (byte b in bytes)
        {
            _ = sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}