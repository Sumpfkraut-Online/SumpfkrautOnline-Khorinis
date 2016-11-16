using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace GUC.Utilities
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a 16 bytes long MD5-Hash computed of the unicode byte format of the string.
        /// </summary>
        public static byte[] GetMD5Hash(this string str)
        {
            return str.GetMD5Hash(Encoding.Unicode);
        }

        /// <summary> Returns a 16 bytes long MD5-Hash computed of the string with the given encoding. </summary>
        public static byte[] GetMD5Hash(this string str, Encoding encoding)
        {
            byte[] ret;
            using (MD5 md5 = new MD5CryptoServiceProvider())
                ret = md5.ComputeHash(encoding.GetBytes(str));

            return ret;
        }
    }
}
