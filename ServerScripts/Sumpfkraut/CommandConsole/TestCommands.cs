using GUC.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole
{
    public class TestCommands : ScriptObject
    {

        new public static readonly String _staticName = "TestCommands (static)";



        public TestCommands ()
        {
            SetObjName("TestCommands (default)");
        }



        // set the ig-time of NewWorld
        public static void SetIgTime (CommandConsole console, String cmd, String param) 
        {
            String[] paramArr, timeStringArr;
            int[] timeIntArr;
            IGTime igTime;

            paramArr = param.Split(' ');
            if ((paramArr == null) || (paramArr.Length < 1))
            {
                return;
            }

            timeStringArr = paramArr[0].Split(':');
            
            if ((timeStringArr == null) || (timeStringArr.Length < 1))
            {
                return;
            }

            timeIntArr = new int[timeStringArr.Length];
            igTime = new IGTime();

            int tempInt = -1;
            for (int t = 0; t < timeStringArr.Length; t++)
            {
                if(!int.TryParse(timeStringArr[t], out tempInt))
                {
                    console.MakeLogError(String.Format("Unparsable partial time parameter"
                        + " detected while calling /setTime: {0} at position {1}",
                        timeStringArr[t], t));
                    return;
                }

                if (tempInt < 0)
                {
                    console.MakeLogError(String.Format("Invalid partial time parameter"
                        + " detected while calling /setTime: {0} at position {1}",
                        tempInt, t));
                    return;
                }

                switch (t)
                {
                    case 0:
                        //console.Print("day = " + tempInt);
                        igTime.day = tempInt;
                        break;
                    case 1:
                        //console.Print("hour = " + tempInt);
                        igTime.hour = tempInt;
                        break;
                    case 2:
                        //console.Print("minute = " + tempInt);
                        igTime.minute = tempInt;
                        break;
                    default:
                        //console.Print("GOTCHA");
                        return;
                }
            }

            World.NewWorld.ChangeTime(igTime);
        }

    }
}
