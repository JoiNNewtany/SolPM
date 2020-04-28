/*
 * Originally written by Hirotada Kobayashi
 * Source: https://gist.github.com/pierre3/6840538
 */

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace SolPM.Core.Helpers
{
    public static class SecureStringExtension
    {
        public static bool Equals(this SecureString a, SecureString b)
        {
            if (a == null && b == null)
            { return true; }

            if (a == null || b == null)
            { return false; }

            if (a.Length != b.Length)
            { return false; }

            var aPtr = Marshal.SecureStringToBSTR(a);
            var bPtr = Marshal.SecureStringToBSTR(b);

            try
            {
                return Enumerable.Range(0, a.Length)
                  .All(i => Marshal.ReadInt16(aPtr, i) == Marshal.ReadInt16(bPtr, i));
            }
            finally
            {
                Marshal.ZeroFreeBSTR(aPtr);
                Marshal.ZeroFreeBSTR(bPtr);
            }
        }

        public static void CopyFromBSTR(this SecureString self, IntPtr bstr, int count)
        {
            self.Clear();

            var chars = Enumerable.Range(0, count)
                .Select(i => (char)Marshal.ReadInt16(bstr, i * 2));

            foreach (var c in chars)
            {
                self.AppendChar(c);
            }
        }
    }
}