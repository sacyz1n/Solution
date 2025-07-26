using System.Security.Cryptography;

namespace GameService.Utility
{
    public class Security
    {
        private const String Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private const int SaltLength = 64;

        private const int AuthTokenLength = 25;

        //public static String MakeHashingPassWord(String saltValue, String pw)
        //{
        //    var sha = SHA256.Create();
        //    var hash = sha.ComputeHash(Encoding.ASCII.GetBytes(saltValue + pw));
        //    var stringBuilder = new StringBuilder();
        //    foreach (var b in hash)
        //    {
        //        stringBuilder.AppendFormat("{0:x2}", b);
        //    }

        //    return stringBuilder.ToString();
        //}

        public static String SaltString()
        {
            Span<byte> bytes = stackalloc byte[SaltLength];
            RandomNumberGenerator.Fill(bytes);

            int charsLength = Chars.Length;
            Span<char> chars = stackalloc char[SaltLength];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i] = Chars[(bytes[i] % charsLength)];
            }

            return new(chars);
        }

        public static String CreateAuthToken()
        {
            Span<byte> bytes = stackalloc byte[AuthTokenLength];

            // 전역 RNG 사용
            RandomNumberGenerator.Fill(bytes); 

            int charsLength = Chars.Length;
            Span<char> chars = stackalloc char[AuthTokenLength];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i] = Chars[(bytes[i] % charsLength)];
            }

            return new(chars);
        }
    }
}
