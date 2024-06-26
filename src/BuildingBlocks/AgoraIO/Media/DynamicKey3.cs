namespace AgoraIO.Media;

public class DynamicKeyGenerator
{
    public static string GenerateDynamicKey(string appID, string appCertificate, string channelName, int unixTs, int randomInt, long uid, int expiredTs)
    {
        string version = "003";
        string unixTsStr = unixTs.ToString().PadLeft(10, '0');
        string randomIntStr = randomInt.ToString("x4").PadLeft(8, '0');
        uid &= 0xFFFFFFFFL;
        string uidStr = uid.ToString().PadLeft(10, '0');
        string expiredTsStr = expiredTs.ToString().PadLeft(10, '0');
        string signature = GenerateSignature(appID, appCertificate, channelName, unixTsStr, randomIntStr, uidStr, expiredTsStr);
        return string.Concat(version, signature, appID, unixTsStr, randomIntStr, uidStr, expiredTsStr);
    }

    private static string GenerateSignature(string appID, string appCertificate, string channelName, string unixTsStr, string randomIntStr, string uidStr, string expiredTsStr)
    {
        using MemoryStream ms = new();
        using BinaryWriter baos = new(ms);
        baos.Write(appID.GetByteArray());
        baos.Write(unixTsStr.GetByteArray());
        baos.Write(randomIntStr.GetByteArray());
        baos.Write(channelName.GetByteArray());
        baos.Write(uidStr.GetByteArray());
        baos.Write(expiredTsStr.GetByteArray());
        baos.Flush();

        byte[] sign = DynamicKeyUtil.EncodeHmac(appCertificate, ms.ToArray());
        return DynamicKeyUtil.BytesToHex(sign);
    }
}