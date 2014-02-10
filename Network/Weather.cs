using System;
using System.Collections.Generic;
using System.Text;

namespace Network
{
    public class GothicTime
    {
        public long time;//Minutes

        public static long GetTime(int day, int hour, int minute)
        {
            return (minute + hour * 60 + day * 60 * 24);
        }

        public int GetDay()
        {
            return (int)(time / 60 / 24);
        }

        public long GetHours()
        {
            return time / 60;
        }

        public long GetMinutes()
        {
            return time;
        }

        public int getHourInDay()
        {
            long rVal = time;

            rVal = rVal - (GetDay() * 60 * 24);//Tag abziehen
            rVal = rVal / 60;
            return (int)rVal;
        }

        public int GetMinuteInHour()
        {
            long rVal = time;

            rVal = rVal - (GetHours() * 60);

            return (int)rVal;
        }
    }
    public class Weather
    {
        float start;
        float end;

        public void RandomRain()
        {
            Random rand = new Random();
            float doub = (float)rand.NextDouble();
            float length = (float)rand.NextDouble();

            start = doub;
            end = doub + length;
        }
    }
}
