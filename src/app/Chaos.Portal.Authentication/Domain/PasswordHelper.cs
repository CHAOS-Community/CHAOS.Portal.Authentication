namespace Chaos.Portal.Authentication.Domain
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Configuration;

    public class PasswordHelper
    {
        public PasswordSettings Settings { get; set; }

        public PasswordHelper(PasswordSettings settings)
        {
            Settings = settings;
        }

        public string GenerateHash(string password, string salt)
        {
            var byteHash = Hash(password, salt);

            return BitConverter.ToString(byteHash).Replace("-", "").ToLower();
        }

        private byte[] Hash(string password, string salt)
        {
            if(Settings.UseSalt)
                return new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), (int) Settings.Iterations).GetBytes(512);

            var sha1 = SHA1.Create();
            var byteHash = sha1.ComputeHash(Encoding.Unicode.GetBytes(password));

            for (var i = 1; i < Settings.Iterations; i++)
            {
                byteHash = sha1.ComputeHash(byteHash);
            }

            return byteHash;
        }
    }
}