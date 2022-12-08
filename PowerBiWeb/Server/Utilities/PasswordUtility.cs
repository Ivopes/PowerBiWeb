using System.Security.Cryptography;
using System.Text;

namespace PowerBiWeb.Server.Utilities
{
    public static class PasswordUtility
    {
        private static readonly int _saltByteLength = 32;
        /// <summary>
        /// Generate password salt with length of 32 bytes
        /// </summary>
        /// <returns>byte array containing generated salt</returns>
        public static byte[] GenerateSalt()
        {
            return GenerateSalt(_saltByteLength);
        }
        /// <summary>
        /// Generate password salt with length of passes bytes
        /// </summary>
        /// <param name="numberOfBytes">length of generated salt</param>
        /// <returns>byte array containing generated salt</returns>
        public static byte[] GenerateSalt(int numberOfBytes)
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(numberOfBytes);

            return bytes;
        }
        /// <summary>
        /// Hash password using SHA256
        /// </summary>
        /// <param name="password">password to be hashed</param>
        /// <returns>hashed password</returns>
        public static byte[] HashPassword(string password)
        {
            using var sha = SHA256.Create();
             
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha.ComputeHash(bytes);

            return hash;
        }
        /// <summary>
        /// Hash password with salt using SHA256
        /// </summary>
        /// <param name="password">password to be hashed</param>
        /// <param name="salt">salt to be added to the password</param>
        /// <returns>hashed password</returns>
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using var sha = SHA256.Create();

            string saltString = Encoding.UTF8.GetString(salt);
            string passToHash = password + saltString;
            byte[] bytes = Encoding.UTF8.GetBytes(passToHash);
            byte[] hash = sha.ComputeHash(bytes);
            return hash;
        }
    }
}
