using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts
{
    public static class DayTime
    {
        static long timeStart = 0;
        static int day = -1;
        public static void Init()
        {
            timeStart = DateTime.Now.Ticks;

            Timer timer = new Timer(10000 * 1000* 3);//Alle 3 Sekunden!
            timer.OnTick += new Events.TimerEvent(onTick);

            timer.Start();
        }



        public static void setTime(int day, int hour, int minute)
        {
            TimeSpan ts = new TimeSpan(day, hour, minute, 0);
            long l = ts.Ticks;
            l /= (24 * 2);

            timeStart = DateTime.Now.Ticks - l;
            onTick();
        }

        public static void setTime(int hour, int minute)
        {
            setTime(day, hour, minute);
        }

        public static void onTick()
        {
            long time = DateTime.Now.Ticks - (timeStart);
            time *= 24*2;//*1 => Echt-Zeit! *24 => 1Stunde => 1 Tag
            TimeSpan ts = new TimeSpan(time);
            
            World.setTimeFast(ts.Days, (byte)ts.Hours, (byte)ts.Minutes);
        
            if (ts.Days > day && ts.Hours > 12)
            {
                day = ts.Days;
                //Calculating Rain:
                Random rand = new Random();
                double startRain =  12.0 + rand.NextDouble() * 23;
                startRain %= 24.0;

                double maxEndTime = 36.0 - startRain;
                if (startRain < 12.0)
                    maxEndTime = 12.0 - startRain;

                if (maxEndTime < 1.0)
                {
                    Console.WriteLine("Kein regen heute!");
                    return;
                }
                maxEndTime -= 1.0;
                double endRain = (startRain + rand.NextDouble() * maxEndTime + 1.0);

                byte startHour = (byte)startRain;
                byte startMinute = (byte)((startRain - startHour) * 60);
                byte endHour = (byte)endRain;
                byte endMinute = (byte)((endRain - endHour) * 60);
                Console.WriteLine(startHour+":"+startMinute+" | "+endHour+":"+endMinute);
                World.setRainTime(World.WeatherType.Snow, startHour, startMinute, endHour, endMinute);
            }
        }
    }
}
