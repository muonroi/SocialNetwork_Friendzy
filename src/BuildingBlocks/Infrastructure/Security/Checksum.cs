using System.Text;

namespace Infrastructure.Security
{
    public static class Checksum
    {
        public static bool VerifyChecksum(string checksum, string checksumSecretKey, params object[] data)
        {
            string compareChecksum = GetChecksumString(checksumSecretKey, data);
            return BCrypt.Verify(compareChecksum, checksum);
        }

        public static string CreateChecksum(string checksumSecretKey, params object[] data)
        {
            string checksumString = GetChecksumString(checksumSecretKey, data);
            return BCrypt.Hash(checksumString);
        }

        private static string GetChecksumString(string checksumSecretKey, params object[] data)
        {
            StringBuilder sb = new();

            foreach (object x in data)
            {
                if (x == null)
                {
                    continue;
                }
                _ = sb.Append(x.ToString());
            }

            return checksumSecretKey + sb.ToString();
        }
    }
}