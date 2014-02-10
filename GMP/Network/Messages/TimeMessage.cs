using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using WinApi;
using Gothic.zClasses;

namespace GMP.Network.Messages
{
    public class TimeMessage : Message
    {
        public static int day;
        public static int hour;
        public static int minute;
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int day; int hour, minute;
            stream.Read(out day);
            stream.Read(out hour);
            stream.Read(out minute);

            setTime(day, hour, minute);
            //oCGame.Game(process).WorldTimer.SetDay(day);
            //oCGame.Game(process).WorldTimer.SetTime(hour, minute);
            //oCGame.Game(process).WorldTimer.Timer();
            ////oCGame.Game(process).SetObjectRoutineTimeChange(0, 0, hour, minute);
            //oCRtnManager.GetRtnManager(process).SetDailyRoutinePos(1);
        }


        static long lastTimeUpdate = 0;
        public static bool firstTimeUpdate = false;
        public static void setTime(int _day, int _hour, int _minute)
        {
            Process process = Process.ThisProcess();
            day = _day;
            hour = _hour;
            minute = _minute;
            if (firstTimeUpdate)
            //if(true)
            {
                oCGame.Game(process).SetTime(day, hour, minute);
                firstTimeUpdate = true;
            }
            else
            {
                oCGame.Game(process).WorldTimer.SetDay(day);
                oCGame.Game(process).WorldTimer.SetTime(hour, minute);
                oCGame.Game(process).WorldTimer.Timer();
            }
        }
    }
}
