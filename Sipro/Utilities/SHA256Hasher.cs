using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
  public static class SHA256Hasher
  {
        public static string[] ComputeHash(string data)
        {
                byte[] bytes = new byte[128 / 8];
                var number = RandomNumberGenerator.Create();
                number.GetBytes(bytes);
                string salt = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                var hasher = SHA256.Create();
                string hashed = Encoding.UTF8.GetString((hasher.ComputeHash(Encoding.UTF8.GetBytes(data+salt))));
                return new string[] { hashed,salt};
        }

        public static string ComputeHash(string data, string salt)
        {
            var hasher = SHA256.Create();
            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(data + salt));
            return Convert.ToBase64String(hashed);
        }
    }
}