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
