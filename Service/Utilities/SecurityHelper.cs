using System.Security.Cryptography;
using System.Text;

namespace Service.Utilities
{
    public class SecurityHelper
    {
        public static string CalculateSecretHash(string clientId, string clientSecret, string username)
        {
            var message = username + clientId;
            var key = Encoding.UTF8.GetBytes(clientSecret);

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
