using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GothicClassGenerator
{
    static class Utils
    {
        public static bool TryParseHex(string str, out int value)
        {
            if (str != null)
            {
                str = str.Trim();
                if (str.Length > 0)
                {
                    return int.TryParse(str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? str.Substring(2) : str,
                                        NumberStyles.HexNumber,
                                        CultureInfo.InvariantCulture,
                                        out value);
                }
            }
            value = 0;
            return false;
        }

        static readonly StringBuilder stringBuilder = new StringBuilder(1024);
        public static StringBuilder GetStringBuilder()
        {
            stringBuilder.Clear();
            return stringBuilder;
        }

        public static string FindBetween(this string str, string front, string back)
        {
            int startIndex = str.IndexOf(front);
            if (startIndex < 0) return null;
            startIndex += front.Length;

            int endIndex = str.LastIndexOf(back);
            if (endIndex < startIndex) return null;
            
            return str.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }
}
