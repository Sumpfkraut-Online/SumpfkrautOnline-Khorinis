using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;

namespace MapExport.ZenArchive
{
    public class ByteHelper
    {
        public static byte[] GetStringToBytes(string value)
        {
            SoapHexBinary shb = SoapHexBinary.Parse(value);
            return shb.Value;
        }

        public static string GetBytesToString(byte[] value)
        {
            SoapHexBinary shb = new SoapHexBinary(value);
            return shb.ToString();
        }

        public static DateTime getStringToDateTime(String value)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture("de-DE");
            DateTime dout;
            DateTime.TryParse(value, ci, DateTimeStyles.AssumeLocal, out dout);
            return dout;
        }

        public static String RealStringtoByteString(String value)
        {
            ASCIIEncoding Encoding = new ASCIIEncoding();
            return ByteHelper.GetBytesToString(Encoding.GetBytes(value));
        }
    }
}
