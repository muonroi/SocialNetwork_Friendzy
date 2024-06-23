namespace AgoraIO.Utils
{
    public class UtilHelper
    {
        public static int GetCurrentTimestamp()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (int)timeSpan.TotalSeconds;
        }

        public static int GenerateRandomInt()
        {
            Random random = new();
            return random.Next();
        }

        public static byte[] Pack(PrivilegeMessage packable)
        {
            ByteBuf buffer = new();
            _ = packable.Marshal(buffer);
            return buffer.ReadByteArray();
        }

        public static byte[] Pack(IPackable packable)
        {
            ByteBuf buffer = new();
            _ = packable.Marshal(buffer);
            return buffer.ReadByteArray();
        }

        public static string Base64Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }
}