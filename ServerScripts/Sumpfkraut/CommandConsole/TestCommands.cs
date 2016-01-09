using GUC.Server.Scripts.Sumpfkraut.CommandConsole.InfoObjects;
using GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols;
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



        public static void GetPlayerList (object sender, String cmd, String[] param, 
            out Dictionary<string, object> returnVal)
        {
            returnVal = null;

            List<object> playerInfo = new List<object>();
            foreach (KeyValuePair<uint, NPC> keyVal in World.NewWorld.PlayerDict)
            {
                NpcInfo info = new NpcInfo();
                info.id = (int) keyVal.Value.ID;
                info.mapName = keyVal.Value.World.MapName;
                info.direction = keyVal.Value.Direction;
                info.position = keyVal.Value.Position;
                playerInfo.Add(info);
            }
            
            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", "List of players: " },
                { "data", playerInfo },
            };
        }
        
        // set the ig-time of NewWorld
        public static void SetIgTime (object sender, String cmd, String[] param, 
            out Dictionary<string, object> returnVal) 
        {
            PrintStatic(typeof(TestCommands), "GOTCHA");
            foreach (String p in param)
            {
                PrintStatic(typeof(TestCommands), p);
            }

            String[] timeStringArr;
            int[] timeIntArr;
            IGTime igTime;
            returnVal = null;

            if ((param == null) || (param.Length < 1))
            {
                return;
            }

            timeStringArr = param[0].Split(':');
            
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
                    //console.MakeLogError(String.Format("Unparsable partial time parameter"
                    //    + " detected while calling /setTime: {0} at position {1}",
                    //    timeStringArr[t], t));
                    return;
                }

                if (tempInt < 0)
                {
                    //console.MakeLogError(String.Format("Invalid partial time parameter"
                    //    + " detected while calling /setTime: {0} at position {1}",
                    //    tempInt, t));
                    return;
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
                        return;
                }
            }
            PrintStatic(typeof(TestCommands), igTime);
            World.NewWorld.ChangeTime(igTime);

            returnVal = new Dictionary<string, object>()
            {
                { "rawText", "Ingame-Time" },
                { "data", igTime },
            };
        }

        public void TeleportVobTo (object sender, String cmd, String[] param, 
            out Dictionary<string, object> returnVal)
        {
            returnVal = null;
        }

    }
}
