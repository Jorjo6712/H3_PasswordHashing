using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace H3_PasswordHashing.Utils;

public class HashUtils
{
    internal static class PasswordUtils
    {
        private const int SaltSize = 16;
        private const int KeySize = 32; 
        private const int Iterations = 100000;

        private static byte[] GenerateSalt()
        {
            byte[] saltBytes = new byte[SaltSize];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            return saltBytes;
        }

        internal static (string,string) HashPassword(string password)
        {
            byte[] saltBytes = GenerateSalt();

            byte[] hashedBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize
            );

            string hashedPassword = Convert.ToBase64String(hashedBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return new(hashedPassword, salt);
        }

        internal static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] hashedBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize
            );

            return Convert.ToBase64String(hashedBytes) == hashedPassword;
        }
        
        internal static bool IsValid(string password)
        {
            return password.Length >= 8 && 
                   Regex.IsMatch(password, @"\d") &&
                   Regex.IsMatch(password, @"[A-Z]") &&
                   Regex.IsMatch(password, @"[a-z]") && 
                   !Regex.IsMatch(password, @"(.)\1{2,}");
        }
    }
}