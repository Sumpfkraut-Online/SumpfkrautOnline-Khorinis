using System;
using System.Collections.Generic;
using System.Text;

namespace GMP.Helper
{
    public class TimeManager
    {
        
        /// <summary>
        /// Liefert einen TimeStamp im Format
        /// yyyyMMddHHmmss plus millisekunden.
        /// </summary>
        public static string GetExactTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public static long conv_Date2Timestam()
        {
            DateTime date1 = new DateTime(1970, 1, 1);  //Refernzdatum (festgelegt)
            DateTime date2 = DateTime.Now;              //jetztiges Datum / Uhrzeit
            TimeSpan ts = new TimeSpan(date2.Ticks - date1.Ticks);  // das Delta ermitteln
            // Das Delta als gesammtzahl der sekunden ist der Timestamp
            return (long)ts.TotalMilliseconds;
        }
    }
}
