using System;
using System.Security.Cryptography;

namespace Utilities.PBKDF2Hashing
{
    /// <summary>
    /// Implement PBKDF2 cryptography with SHA256 hash algorithm.This class takes a password, a salt, and an iteration count, and then generates keys through calls to the GetBytes method. 
    /// </summary>
    public class PBKDF2Cryptography
    {
        /// <summary>
        /// Initializes a new instance of the PBKDF2Cryptography. Set the default value of HashIterations = 2000. HashSize = 128. SaltSize = 32
        /// </summary>
        public PBKDF2Cryptography()
        {
            HashIterations = 2000;
            HashSize = 128;
            SaltSize = 32;
        }

        /// <summary>
        /// Gets or sets the number of iterations the hash will go through.
        /// </summary>
        public int HashIterations { get; private set; }

        /// <summary>
        /// Gets or sets the size of hash that will be generated.
        /// </summary>
        public int HashSize { get; private set; }

        /// <summary>
        /// Gets or sets the size of salt that will be randomly generated if not salt defined.
        /// </summary>
        public int SaltSize { get; private set; }

        /// <summary>
        /// Compute a hash code of the plain text password.
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <returns>Hash and Salt values</returns>
        public PBKDF2Password CreateHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password cannot be null");
            }
            PBKDF2Password hashPass = new PBKDF2Password();
            byte[] salt = new byte[SaltSize];
            byte[] hash;
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt);
            }
            using (var hashGenerator = new Rfc2898DeriveBytes(password, salt, HashIterations, HashAlgorithmName.SHA256))
            {
                hash = hashGenerator.GetBytes(HashSize);
            }
            hashPass.Salt = salt;
            hashPass.Hash = hash;
            if (hashPass.Salt.Length < 1 || hashPass.Hash.Length < 1)
            {
                throw new CryptographicException("Unable to generate Hash and Salt value");
            }
            return hashPass;
        }

        /// <summary>
        /// Compute a hash code of the plain text password based on the supplied salt value
        /// </summary>
        /// <param name="password">Plain text password</param>
        /// <param name="salt">salt value</param>
        /// <returns>Hash and Salt values</returns>
        public PBKDF2Password CreateHash(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password cannot be null");
            }
            PBKDF2Password hashPass = new PBKDF2Password();
            byte[] hash;
            using (var hashGenerator = new Rfc2898DeriveBytes(password, salt, HashIterations, HashAlgorithmName.SHA256))
            {
                hash = hashGenerator.GetBytes(HashSize);
            }
            hashPass.Salt = salt;
            hashPass.Hash = hash;
            if (hashPass.Salt.Length < 1 || hashPass.Hash.Length < 1)
            {
                throw new CryptographicException("Unable to generate Hash and Salt value");
            }
            return hashPass;
        }

        /// <summary>
        /// Validate password against to correct password.
        /// </summary>
        /// <param name="password">plain text password</param>
        /// <param name="correctHash">Correct hash value. This value should be in Base64String type.</param>
        /// <param name="correctSalt">Correct Salt value. This value should be in Base64String type.</param>
        /// <returns>Return True if password matched with hash otherwise false.</returns>
        public bool ValidatePassword(string password, string correctHash, string correctSalt)
        {
            var hash = Convert.FromBase64String(correctHash);
            var salt = Convert.FromBase64String(correctSalt);

            var passwordHash = CreateHash(password, salt).Hash;

            var diff = (uint)hash.Length ^ (uint)passwordHash.Length;
            for (int i = 0; i < hash.Length && i < passwordHash.Length; i++)
            {
                diff |= (uint)(hash[i] ^ passwordHash[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// Validate password against to correct password.
        /// </summary>
        /// <param name="password">plain text password</param>
        /// <param name="correctHash">Correct hash value.</param>
        /// <param name="correctSalt">Correct Salt value.</param>
        /// <returns>Return True if password matched with hash otherwise false.</returns>
        public bool ValidatePassword(string password, byte[] correctHash, byte[] correctSalt)
        {
            var passwordHash = CreateHash(password, correctSalt).Hash;
            var diff = (uint)correctHash.Length ^ (uint)passwordHash.Length;
            for (int i = 0; i < correctHash.Length && i < passwordHash.Length; i++)
            {
                diff |= (uint)(correctHash[i] ^ passwordHash[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// Validate password against to correct password.
        /// </summary>
        /// <param name="hash1">Hash value.</param>
        /// <param name="hash2">Hash value.</param>
        /// <param name="Salt">Salt value.</param>
        /// <returns>Return True if both are hashes equal otherwise false.</returns>
        public bool ValidateHash(byte[] hash1, byte[] hash2, byte[] Salt)
        {
            var diff = (uint)hash2.Length ^ (uint)hash1.Length;
            for (int i = 0; i < hash2.Length && i < hash1.Length; i++)
            {
                diff |= (uint)(hash2[i] ^ hash1[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// Compare password hashes for equality. Uses a constant time comparison method.
        /// </summary>
        /// <param name="hash1">Hash value.</param>
        /// <param name="hash2">Hash value.</param>
        /// <returns>Return True if both are hashes equal otherwise false</returns>
        public bool Compare(string hash1, string hash2)
        {
            if (hash1 == null || hash2 == null)
                return false;

            int min_length = Math.Min(hash1.Length, hash2.Length);
            int result = 0;

            for (int i = 0; i < min_length; i++)
                result |= hash1[i] ^ hash2[i];

            return 0 == result;
        }
    }
}
