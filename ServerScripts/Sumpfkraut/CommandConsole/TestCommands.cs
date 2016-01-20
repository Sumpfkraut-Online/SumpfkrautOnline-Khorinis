using GUC.Server.Scripts.Sumpfkraut.CommandConsole.InfoObjects;
using GUC.Server.Scripts.Sumpfkraut.Web.WS.Protocols;
using GUC.Server.WorldObjects;
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole
{
    public class TestCommands : GUC.Utilities.ExtendedObject
    {

        new public static readonly String _staticName = "TestCommands (static)";



        public TestCommands ()
        {
            SetObjName("TestCommands (default)");
        }



        public static void GetPlayerList (object sender, String cmd, String[] param, 
            out Dictionary<string, object> returnVal)
        {
            returnVal = new Dictionary<string, object>()
            {
                { "rawText", "Failed to retrieve player list!" },
            };

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
            IgTime igTime = new IgTime();
            float igTimeRate = 0f;
            bool igTimeCheck = false;
            bool igTimeRateCheck = false;
            int igTimeIndex = -1;
            int igTimeRateIndex = -1;

            // handle premature failure
            returnVal = new Dictionary<string, object>()
            {
                { "rawText", "Failed to set ingame-time!" },
            };
            if ((param == null) || (param.Length < 1))
            {
                return;
            }

            // try to parse any parameters given to eventually
            // find patterns of igTime and igTimeRate
            for (int i = 0; i < param.Length; i++)
            {
                if ((!igTimeCheck) && (i != igTimeRateIndex))
                {
                    if (igTimeCheck = IgTime.TryParse(param[0], out igTime))
                    {
                        igTimeIndex = i;
                    }
                }
                if ((!igTimeRateCheck) && (i != igTimeIndex))
                {
                    if (igTimeRateCheck = float.TryParse(param[i], out igTimeRate))
                    {
                        igTimeRateIndex = i;
                    }
                }
            }

            if (igTimeCheck || igTimeRateCheck)
            {
                returnVal["rawText"] = "";
            }

            // prepare response message and variables to set time and time-rate
            if (igTimeCheck)
            {
                returnVal["rawText"] = "Set igTime to: " + igTime + ". ";
            }
            else
            {
                igTime = World.NewWorld.GetIGTime();
            }
            if (igTimeRateCheck)
            {
                returnVal["rawText"] = "Set ingameTimeRate (ingame- to reallife-time-ratio) to: " 
                    + igTimeRate + ". ";
            }
            else
            {
                igTimeRate = World.NewWorld.GetIgTimeRate();
            }

            World.NewWorld.ChangeIgTime(igTime, igTimeRate);
        }

        public static void SetIgWeather (object sender, String cmd, String[] param, 
            out Dictionary<string, object> returnVal)
        {
            returnVal = new Dictionary<string, object>()
            {
                { "rawText", "Failed to set ingame-weather!" },
            };

            if ((param == null) || (param.Length < 3))
            {
                // insufficient paramters
                Log.Logger.log("#1");
                return;
            }

            World world = World.NewWorld;
            WeatherType weatherType;
            IgTime startTime, endTime;

            try
            {
                int weatherTypeInt;
                if (!int.TryParse(param[0], out weatherTypeInt))
                {
                    returnVal["rawText"] += " Couldn't parse weatherType.";
                    return;
                }

                weatherType = (WeatherType) weatherTypeInt;
                if (!Enum.IsDefined(typeof(WeatherType), weatherType))
                {
                    returnVal["rawText"] += " Invalid weatherType.";
                    return;
                }
            }
            catch (Exception ex)
            {
                returnVal["rawText"] += " Couldn't parse weatherType: " + ex;
                return;
            }

            if (!IgTime.TryParse(param[1], out startTime))
            {
                returnVal["rawText"] += " Couldn't parse startTime.";
                return;
            }
            if (!IgTime.TryParse(param[2], out endTime))
            {
                returnVal["rawText"] += " Couldn't parse endTime.";
                return;
            }
            //if (startTime > endTime)
            //{
            //    returnVal["rawText"] += " startTime is bigger than endTime.";
            //    return;
            //}

            world.ChangeIgWeather(weatherType, startTime, endTime);

            returnVal["rawText"] = String.Format("Successfully set ingame-weather to type {0}"
                + " between times {1} and {2}", weatherType, startTime, endTime);
        }

        //public static void TeleportVobTo (object sender, String cmd, String[] param, 
        //    out Dictionary<string, object> returnVal)
        //{
        //    returnVal = null;
        //}

    }
}
