using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace GUC.Server.Scripts.Sumpfkraut.Utilities
{
    class DateTimeUtil
    {

        private static string dateFormat = "yyyy-MM-dd HH:mm:ss";


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

    }
}
