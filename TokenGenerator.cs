using System;
using System.Security.Cryptography;

namespace todoApp{
    class TokenGenerator
    {
        public string GenerateHexToken(int length = 5){
            byte[] bytes = new byte[(int)Math.Ceiling(length / 2.0)];
            RandomNumberGenerator.Fill(bytes);
            string hex = BitConverter.ToString(bytes).Replace("-", "").ToLower();
            return hex.Substring(0, length);
        }

        // static void Main()
        // {
        //     string token = GenerateHexToken();
        //     Console.WriteLine("Generated Token: " + token);
        // }
    }
}