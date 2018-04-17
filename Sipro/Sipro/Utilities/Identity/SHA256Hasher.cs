using System;
using System.Security.Cryptography;
using System.Text;

namespace Sipro.Utilities.Identity
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
                string hashed = BitConverter.ToString(hasher.ComputeHash(Encoding.UTF8.GetBytes(salt+data)));
                return new string[] { hashed,salt};
        }

        public static string ComputeHash(string data, string salt)
        {
            var hasher = SHA256.Create();
            return BitConverter.ToString(hasher.ComputeHash(Encoding.UTF8.GetBytes(salt + data)));
        }
    }
}