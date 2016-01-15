using System;
using System.Collections.Generic;

namespace GUC.Types
{

    public struct IgTime
    {
        public int day;
        public int hour;
        public int minute;

        public IgTime (IgTime igTime)
            : this (igTime.day, igTime.hour, igTime.minute)
        { }

        public IgTime (int minute)
            : this (0, 0, minute)
        { }

        public IgTime (long totalMinute)
        {
            long min, hour, day;
            min = (Math.Abs(totalMinute) % 60L) * Math.Sign(totalMinute);
            hour = totalMinute / 60L;
            day = hour / 24L;
            hour = (Math.Abs(hour) % 24L) * Math.Sign(hour);
            this.minute = (int) min;
            this.hour = (int) hour;
            this.day = (int) day;
        }

        public IgTime (int hour, int minute)
            : this (0, hour, minute)
        { }

        public IgTime (int day, int hour, int minute)
        {
            this.minute = (Math.Abs(minute) % 60) * Math.Sign(minute);
            this.hour = hour + (minute / 60);
            this.day = day + ((Math.Abs(hour) % 24) * Math.Sign(hour));
            this.hour = this.hour % 24;
        }



        public static IgTime operator -(IgTime t1)
        {
            return new IgTime(-t1.day, -t1.hour, -t1.minute);
        }

        public static IgTime operator +(IgTime t1, IgTime t2)
        {
            IgTime t3 = new IgTime(t1.day + t2.day, t1.hour + t2.hour,
                t1.minute + t2.minute);

            t3.hour += t3.minute / 60;
            t3.minute = (Math.Abs(t3.minute) % 60) * Math.Sign(t3.minute);
            t3.day += t3.hour / 24;
            t3.hour = (Math.Abs(t3.hour) % 24) * Math.Sign(t3.hour);
            
            return t3;
        }

        public static IgTime operator +(IgTime t1, int min)
        {
            IgTime t3 = new IgTime(t1);

            t3.minute += min;
            t3.hour += t3.minute / 60;
            t3.minute = (Math.Abs(t3.minute) % 60) * Math.Sign(t3.minute);
            t3.day += t3.hour / 24;
            t3.hour = (Math.Abs(t3.hour) % 24) * Math.Sign(t3.hour);

            return t3;
        }

        public static IgTime operator +(int min, IgTime t1)
        {
            return t1 + min;
        }

        public static IgTime operator -(IgTime t1, IgTime t2)
        {
            return t1 + (-t2);
        }

        public static IgTime operator *(IgTime t1, IgTime t2)
        {
            long totalMin = ToMinutes(t1) * ToMinutes(t2);

            return new IgTime(totalMin);
        }

        public static IgTime operator *(IgTime t1, int min)
        {
            long totalMin = ToMinutes(t1) * min;

            return new IgTime(totalMin);
        }

        public static IgTime operator *(int min, IgTime t1)
        {
            return t1 * min;
        }

        public static IgTime operator /(IgTime t1, IgTime t2)
        {
            long totalMin = ToMinutes(t1) / ToMinutes(t2);

            return new IgTime(totalMin);
        }

        public static IgTime operator /(IgTime t1, int min)
        {
            long totalMin = ToMinutes(t1) / min;

            return new IgTime(totalMin);
        }

        public static IgTime operator /(int min, IgTime t1)
        {
            long totalMin = min / ToMinutes(t1);

            return new IgTime(totalMin);
        }

        public static bool operator >(IgTime t1, IgTime t2)
        {
            if (t1.day > t2.day)
            {
                return true;
            }
            if (t1.hour > t2.hour)
            {
                return true;
            }
            if (t1.minute > t2.minute)
            {
                return true;
            }
            return false;
        }

        public static bool operator <(IgTime t1, IgTime t2)
        {
            return t2 > t1;
        }

        public static bool operator ==(IgTime t1, IgTime t2)
        {
            if ((t1.day == t2.day) && (t1.hour == t2.hour) 
                && (t1.minute == t2.minute))
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(IgTime t1, IgTime t2)
        {
            return !(t1 == t2);
        }

        public static bool operator >=(IgTime t1, IgTime t2)
        {
            if (t1 == t2)
            {
                return true;
            }
            return t1 > t2;
        }

        public static bool operator <=(IgTime t1, IgTime t2)
        {
            return t2 >= t1;
        }



        public static long ToMinutes (IgTime igTime)
        {
            return igTime.minute + ((igTime.hour + (igTime.day * 24L)) * 60L);
        }

        public override string ToString()
        {
            return String.Format("IGTime: day {0} hour {1} minute {2}", 
                this.day, this.hour, this.minute);
        }

        public static bool TryParse (String timeString, out IgTime igTime)
        {
            igTime = new IgTime();

            String[] timeStringArr;
            int[] timeIntArr;

            if (timeString == null)
            {
                return false;
            }

            timeStringArr = timeString.Split(':');
            if ((timeStringArr == null) || (timeStringArr.Length < 1))
            {
                return false;
            }

            timeIntArr = new int[timeStringArr.Length];
            int tempInt = -1;
            for (int t = 0; t < timeStringArr.Length; t++)
            {
                if(!int.TryParse(timeStringArr[t], out tempInt))
                {
                    return false;
                }

                if (tempInt < 0)
                {
                    return false;
                }

                switch (t)
                {
                    case 0:
                        igTime.day = tempInt;
                        break;
                    case 1:
                        igTime.hour = tempInt;
                        break;
                    case 2:
                        igTime.minute = tempInt;
                        break;
                    default:
                        break;
                }
            }

            return true;
        }
    }

}