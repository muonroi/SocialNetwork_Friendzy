namespace AgoraIO.Extensions
{
    public static class MemoryStreamExtensions
    {
        public static void WriteString(this MemoryStream stream, string data)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            stream.Write(byteArray, (int)stream.Length, byteArray.Length);
        }

        public static byte[] GetByteArray(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static byte[] GetBytes(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}