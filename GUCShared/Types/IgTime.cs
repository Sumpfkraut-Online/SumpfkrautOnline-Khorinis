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

        public IgTime (int totalMinute)
            : this (0, 0, totalMinute)
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

        public IgTime (float totalMinute)
            : this ((long)(Math.Round(totalMinute)))
        { }

        public IgTime (double totalMinute)
            : this ((long)(Math.Round(totalMinute)))
        { }

        public IgTime (int hour, int minute)
            : this (0, hour, minute)
        { }

        public IgTime (int d, int h, int m)
        {
            minute = (Math.Abs(m) % 60) * Math.Sign(m);
            hour = h + (m / 60);
            day = d + (h / 24);

            if (minute < 0)
            {
                hour -= 1;
                minute += 60;
            }
            if (hour < 0)
            {
                if ((Math.Abs(hour) % 24) == 0)
                {
                    day += hour / 24;
                    hour = 0;
                }
                else
                {
                    day += (hour / 24) - 1;
                    hour = 24 + ((Math.Abs(hour) % 24) * Math.Sign(hour));
                }
            }
            else
            {
                hour = hour % 24;
            }
        }



        public static IgTime operator -(IgTime t1)
        {
            return new IgTime(-t1.day, -t1.hour, -t1.minute);
        }

        public static IgTime operator ++(IgTime t1)
        {
            return t1 + 1;
        }

        public static IgTime operator --(IgTime t1)
        {
            return t1 - 1;
        }

        public static IgTime operator +(IgTime t1, IgTime t2)
        {
            return new IgTime(t1.day + t2.day, t1.hour + t2.hour, t1.minute + t2.minute);
        }

        public static IgTime operator +(IgTime t1, int min)
        {
            return new IgTime(t1.day, t1.hour, t1.minute + min);
        }

        public static IgTime operator +(int min, IgTime t1)
        {
            return t1 + min;
        }

        public static IgTime operator +(IgTime t1, long min)
        {
            return new IgTime(IgTime.ToMinutes(t1) + min);
        }

        public static IgTime operator +(long min, IgTime t1)
        {
            return t1 + min;
        }

        public static IgTime operator -(IgTime t1, IgTime t2)
        {
            return new IgTime(t1.day - t2.day, t1.hour - t2.hour, t1.minute - t2.minute);
        }

        public static IgTime operator -(IgTime t1, int min)
        {
            return new IgTime(t1.day, t1.hour, t1.minute - min);
        }

        public static IgTime operator -(int min, IgTime t1)
        {
            return min + (-t1);
        }

        public static IgTime operator -(IgTime t1, long min)
        {
            return new IgTime(IgTime.ToMinutes(t1) - min);
        }

        public static IgTime operator -(long min, IgTime t1)
        {
            return min + (-t1);
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
            else if (t1.day < t2.day)
            {
                return false;
            }

            if (t1.hour > t2.hour)
            {
                return true;
            }
            else if (t1.hour < t2.hour)
            {
                return false;
            }
            
            if (t1.minute > t2.minute)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            return String.Format("{0}:{1}:{2}", 
                this.day, this.hour, this.minute);
        }

        public static bool TryParse (String timeString, out IgTime igTime)
        {
            int day, hour, minute;
            day = hour = minute = 0;

            String[] timeStringArr;
            int[] timeIntArr;

            if (timeString == null)
            {
                igTime = new IgTime();
                return false;
            }

            timeStringArr = timeString.Split(':');
            if ((timeStringArr == null) || (timeStringArr.Length < 1))
            {
                igTime = new IgTime();
                return false;
            }

            timeIntArr = new int[timeStringArr.Length];
            int tempInt = -1;
            for (int t = 0; t < timeStringArr.Length; t++)
            {
                if(!int.TryParse(timeStringArr[t], out tempInt))
                {
                    igTime = new IgTime();
                    return false;
                }

                switch (t)
                {
                    case 0:
                        day = tempInt;
                        break;
                    case 1:
                        hour = tempInt;
                        break;
                    case 2:
                        minute = tempInt;
                        break;
                    default:
                        break;
                }
            }

            igTime = new IgTime(day, hour, minute);

            return true;
        }
    }

}