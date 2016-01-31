using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GUC.Scripts.Sumpfkraut.Utilities
{
    public class DateTimeUtil : ScriptObject
    {

        new public static readonly string _staticName = "DateTimeUtil (static)"; 

        protected static string dateFormat = "yyyy-MM-dd HH:mm:ss";


        /**
         *   Creates a standardized string-representation of the given DateTime.object using a format.
         *   It uses a standard-format by default ("yyyy-MM-dd HH:mm:ss"), however, a custom one can be 
         *   provided as well.
         *   @param format is a pattern-string according to the rules of System.DateTime-class
         *   @see System.Datetime
         */
        public static string DateTimeToString (System.DateTime dt, string format=null)
        {
            if (format == null)
            {
                format = dateFormat;
            }

            return dt.ToString(format);
        }

        public static bool TryDateTimeToString (DateTime dt, out string str, string format=null)
        {
            if (format == null)
            {
                format = dateFormat;
            }

            str = dt.ToString(format);

            if (str == null)
            {
                return false;
            }

            return true;
        }

        //public static DateTime StringToDateTime (string str)
        //{
        //    DateTime dt;

        //    if (DateTime.TryParseExact(str, dateFormat, CultureInfo.InvariantCulture,
        //        DateTimeStyles.None, out dt))
        //    {
        //        return dt;
        //    }

        //    return dt;
        //}

        public static bool TryStringToDateTime (string str, out DateTime dt)
        {
            if (DateTime.TryParseExact(str, dateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out dt))
            {
                return true;
            }
            // else
            return false;
        }

    }
}
