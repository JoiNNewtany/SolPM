/*
 * Originally written by Kevin Stewart
 * Source: https://www.codeproject.com/articles/2393/a-c-password-generator
 */

using System;
using System.Security.Cryptography;
using System.Text;

namespace SolPM.Core.Cryptography.PwdGenerator
{
    public class PwdGenerator
    {
        public PwdGenerator()
        {
            Length = DefaultLength;
            ConsecutiveCharacters = false;
            ExcludeSymbols = false;
            Exclusions = null;

            rng = new RNGCryptoServiceProvider();
        }

        protected int GetCryptographicRandomNumber(int lBound, int uBound)
        {
            // Assumes lBound >= 0 && lBound < uBound
            // Returns an int >= lBound and < uBound
            uint urndnum;
            byte[] rndnum = new byte[4];

            if (lBound == uBound - 1)
            {
                // Test for degenerate case where only lBound can be returned
                return lBound;
            }

            uint xcludeRndBase = (uint.MaxValue -
                (uint.MaxValue % (uint)(uBound - lBound)));

            do
            {
                rng.GetBytes(rndnum);
                urndnum = BitConverter.ToUInt32(rndnum, 0);
            } while (urndnum >= xcludeRndBase);

            return (int)(urndnum % (uBound - lBound)) + lBound;
        }

        protected char GetRandomCharacter()
        {
            string avaliableChars = string.Empty;

            if (!ExcludeLowerCase)
            {
                avaliableChars += lowerCaseChars;
            }

            if (!ExcludeUpperCase)
            {
                avaliableChars += upperCaseChars;
            }

            if (!ExcludeNumbers)
            {
                avaliableChars += numbersChars;
            }

            if (!ExcludeSymbols)
            {
                avaliableChars += symbolsChars;
            }

            if (string.IsNullOrWhiteSpace(avaliableChars))
            {
                return char.MinValue;
            }

            int randomCharPosition = GetCryptographicRandomNumber(
                0, avaliableChars.Length);

            char randomChar = avaliableChars[randomCharPosition];

            return randomChar;
        }

        public string Generate()
        {
            StringBuilder pwdBuffer = new StringBuilder();
            pwdBuffer.Capacity = Length;

            // Generate random characters
            char lastCharacter, nextCharacter;

            // Initial dummy character flag
            lastCharacter = nextCharacter = '\n';

            for (int i = 0; i < Length; i++)
            {
                nextCharacter = GetRandomCharacter();

                if (false == ConsecutiveCharacters)
                {
                    while (lastCharacter == nextCharacter)
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                if (!string.IsNullOrWhiteSpace(Exclusions))
                {
                    while (-1 != Exclusions.IndexOf(nextCharacter))
                    {
                        nextCharacter = GetRandomCharacter();
                    }
                }

                pwdBuffer.Append(nextCharacter);
                lastCharacter = nextCharacter;
            }

            if (null != pwdBuffer)
            {
                return pwdBuffer.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public string Exclusions { get; set; }

        public int Length { get; set; }

        public bool ExcludeSymbols { get; set; }

        public bool ExcludeNumbers { get; set; }

        public bool ExcludeUpperCase { get; set; }

        public bool ExcludeLowerCase { get; set; }

        public bool ConsecutiveCharacters { get; set; }

        private const int DefaultLength = 20;

        private RNGCryptoServiceProvider rng;

        private string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
        private string upperCaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string numbersChars = "0123456789";
        private string symbolsChars = "`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?";
    }
}