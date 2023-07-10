using System.Security.Cryptography;
using System.Text;

namespace PayAppApi.Common.Helpers
{
    public class StaticData
    {
        public static string ComputeSHA256(string text)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(text); // Convert the input string to bytes

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes); // Compute the SHA-256 hash

                string base64String = Convert.ToBase64String(hashBytes); // Convert the hash to a Base64 string

                return base64String;
            }
        }

        public static bool ComparePassword(string inputPassword, string password)
        {
            var passwordHash = ComputeSHA256(inputPassword);
            return passwordHash.Equals(password, StringComparison.OrdinalIgnoreCase);
        }

    }
}
