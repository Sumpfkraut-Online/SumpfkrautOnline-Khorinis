using GUC.Scripts.Sumpfkraut.CommandConsole.InfoObjects;
using GUC.Scripts.Sumpfkraut.Web.WS.Protocols;
using GUC.Server.WorldObjects;
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.CommandConsole
{
    public class TestCommands : GUC.Utilities.ExtendedObject
    {

        new public static readonly string _staticName = "TestCommands (static)";



        public TestCommands ()
        {
            SetObjName("TestCommands (default)");
        }



        public static void GetPlayerList (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            returnVal = new Dictionary<string, object>()
            {
                { "rawText", "Failed to retrieve player list!" },
            };

            List<object> playerInfo = new List<object>();
            Network.GameClient.ForEach(client =>
            {
                PlayerInfo info = new PlayerInfo();

                info.ClientID = client.ID;
                info.DriveHash = client.DriveHash;
                info.MacHash = client.MacHash;
                info.Ping = client.GetLastPing();

                info.VobID = client.Character.ID;
                info.VobType = client.Character.VobType;
                info.NPCName = client.Character.Name;
                info.MapName = "TESTWORLD";
                info.Position = client.Character.GetPosition();
                info.Direction = client.Character.GetDirection();

                playerInfo.Add(info);
            });

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", "List of players: " },
                { "data", playerInfo },
            };
        }

        //private static List<Vob> GetVobsByVobID (List<int> ids)
        //{
        //    List<Vob> vobList = new List<Vob>();
        //    Vob vob;
        //    int prevID = -999;
        //    ids.Sort();

        //    for (int i = 0; i < ids.Count; i++)
        //    {
        //        if (ids[i] == prevID)
        //        {
        //            continue;
        //        }
        //    }
        //    WorldInst.Current
        //    World.ForEach(world =>
        //    {
        //        //world.ForEachVob(vob =>
        //        //{
        //        //    index = ids.IndexOf(vob.ID);
        //        //    if (index > -1)
        //        //    {
        //        //        vobList.Add(vob);
        //        //    }
        //        //});
        //    });

        //    return vobList;
        //}

        private static List<Network.GameClient> GetAllClients ()
        {
            List<Network.GameClient> clients = new List<Network.GameClient>();
            Network.GameClient.ForEach(c => clients.Add(c));
            return clients;
        }

        private static List<Network.GameClient> GetClientsByID (List<int> ids)
        {
            List<Network.GameClient> clients = new List<Network.GameClient>();
            Network.GameClient client;
            int prevID = -1;

            ids.Sort();
            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i] == prevID) { continue; }
                if (Network.GameClient.TryGetClient(ids[i], out client))
                {
                    clients.Add(client);
                }
            }

            return clients;
        }

        private static List<Network.GameClient> GetClientsByCharName (List<string> charNames)
        {
            List<Network.GameClient> clients = new List<Network.GameClient>();
            int index = -1;

            Network.GameClient.ForEach(client => 
            {
                index = charNames.IndexOf(client.Character.Name);
                if (index > -1)
                {
                    clients.Add(client);
                }
            });

            return clients;
        }

        private static List<Network.GameClient> PrepareClientList (string[] param)
        {
            List<int> ids = new List<int>(); int id;
            List<string> charNames = new List<string>();
            List<Network.GameClient> clients = null;
            bool getAll = false;

            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] == "all")
                {
                    getAll = true;
                    clients = GetAllClients();
                    break;
                }
                else if (int.TryParse(param[i], out id)) { ids.Add(id); }
                else { charNames.Add(param[i]); }
            }

            if (!getAll)
            {
                clients = GetClientsByID(ids);
                clients.AddRange(GetClientsByCharName(charNames));
            }

            return clients;
        }

        private static List<List<string>> SplitToArgumentList (string[] param)
        {
            List<List<string>> argList = new List<List<string>>();

            // !!! TO DO !!!

            return argList;
        }

        //private static void ManipulatePlayers (object sender, string cmd, string[] playerParam,
        //    out Dictionary<string, object> returnVal)
        //{
        //    string successMsg = "Failed to use " + cmd + " on player(s)!";
        //    List<Network.GameClient> clients = new List<Network.GameClient>();
        //    Network.GameClient client = null;
        //    int clientID = -1;
        //    bool preperationComplete = false;

        //    if ((playerParam == null) || (playerParam.Length < 1))
        //    {
        //        successMsg += " No player-id or -name was provided.";
        //    }
        //    else
        //    {
        //        int p = 0;
        //        while (p < playerParam.Length)
        //        {
        //            preperationComplete = false;

        //            if (int.TryParse(playerParam[p], out clientID))
        //            {
        //                // possible id was provided
        //                if (Network.GameClient.TryGetClient(clientID, out client))
        //                {
        //                    clients.Add(client);
        //                }
        //                else
        //                {
        //                    successMsg += " Invalid player-id " + clientID + ".";
        //                }
        //            }
        //            else
        //            {
        //                // possible name was provided
        //                Network.GameClient.ForEach(c =>
        //                {
        //                    if ((!preperationComplete) && (playerParam[p] == c.Character.Name))
        //                    {
        //                        clients.Add(c);
        //                        preperationComplete = true;
        //                    }
        //                });
        //            }

        //            p++;
        //        }
        //    }

        //    // manipulate chosen players given the method
        //    if ((clients != null) && (clients.Count > 0))
        //    {
        //        cmd = cmd.ToUpper();
        //        switch (cmd)
        //        {
        //            case "BAN":
        //                successMsg = "Banned player(s): ";
        //                foreach (Network.GameClient c in clients)
        //                {
        //                    c.Ban();
        //                    successMsg += string.Format(" [clientID: {0}, VobID: {1}, VobName: {2}, Mac: {3}]", 
        //                        c.ID, c.Character.ID, c.Character.Name, string.Join(" ", c.MacHash));
        //                }
        //                break;
        //            case "KICK":
        //                successMsg = "Kicked player(s):";
        //                foreach (Network.GameClient c in clients)
        //                {
        //                    c.Kick();
        //                    successMsg += string.Format(" [clientID: {0}, VobID: {1}, VobName: {2}, Mac: {3}]", 
        //                        c.ID, c.Character.ID, c.Character.Name, string.Join(" ", c.MacHash));
        //                }
        //                break;
        //            case "KILL":
        //                successMsg = "Killed player(s):";
        //                foreach (Network.GameClient c in clients)
        //                {
        //                    c.Character.SetHealth(0);
        //                    successMsg += string.Format(" [clientID: {0}, VobID: {1}, VobName: {2}, Mac: {3}]", 
        //                        c.ID, c.Character.ID, c.Character.Name, string.Join(" ", c.MacHash));
        //                }
        //                break;
        //            default:
        //                successMsg += " No correct method provided on server side.";
        //                break;
        //        }
        //    }
            
        //    // finally complete the return-value for the original command operation
        //    returnVal = new Dictionary<string, object>
        //    {
        //        { "type", WSProtocolType.chatData },
        //        { "sender", "SERVER" },
        //        { "rawText", successMsg },
        //    };
        //}

        public static void BanPlayers (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder successMsgSB = new StringBuilder();
            List<Network.GameClient> clients = PrepareClientList(param);

            if ((clients == null) || (clients.Count < 1))
            {
                successMsgSB.AppendFormat("Didn't use {0} on any players.", cmd);
            }
            else
            {
                bool first = true;
                successMsgSB.AppendFormat("Used {0} on players:", cmd);
                for (var i = 0; i < clients.Count; i++)
                {
                    clients[i].Ban();
                    if (!first) { successMsgSB.Append(","); }
                    else { first = false; }
                    successMsgSB.AppendFormat("[{0}, {1}]", clients[i].ID, clients[i].Character.Name);
                }
            }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", successMsgSB.ToString() },
            };
        }

        public static void KickPlayers (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder successMsgSB = new StringBuilder();
            List<Network.GameClient> clients = PrepareClientList(param);

            if ((clients == null) || (clients.Count < 1))
            {
                successMsgSB.AppendFormat("Didn't use {0} on any players.", cmd);
            }
            else
            {
                bool first = true;
                successMsgSB.AppendFormat("Used {0} on players:", cmd);
                for (var i = 0; i < clients.Count; i++)
                {
                    clients[i].Kick();
                    if (!first) { successMsgSB.Append(","); }
                    else { first = false; }
                    successMsgSB.AppendFormat("[{0}, {1}]", clients[i].ID, clients[i].Character.Name);
                }
            }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", successMsgSB.ToString() },
            };
        }

        public static void KillPlayers (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            PrintStatic(typeof(TestCommands), cmd + " -> " + string.Join(", ", param));
            StringBuilder successMsgSB = new StringBuilder();
            List<Network.GameClient> clients = PrepareClientList(param);

            if ((clients == null) || (clients.Count < 1))
            {
                successMsgSB.AppendFormat("Didn't use {0} on any players.", cmd);
            }
            else
            {
                bool first = true;
                successMsgSB.AppendFormat("Used {0} on players:", cmd);
                for (var i = 0; i < clients.Count; i++)
                {
                    clients[i].Character.SetHealth(0);
                    if (!first) { successMsgSB.Append(","); }
                    else { first = false; }
                    successMsgSB.AppendFormat("[{0}, {1}]", clients[i].ID, clients[i].Character.Name);
                }
            }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", successMsgSB.ToString() },
            };
        }

        //public static void TeleportPlayers (object sender, string cmd, string[] param,
        //    out Dictionary<string, object> returnVal)
        //{
        //    ManipulatePlayers(sender, cmd, param, out returnVal);
        //}

        //// set the ig-time of NewWorld
        //public static void SetIgTime (object sender, String cmd, String[] param,
        //    out Dictionary<string, object> returnVal)
        //{
        //    IgTime igTime = new IgTime();
        //    float igTimeRate = 0f;
        //    bool igTimeCheck = false;
        //    bool igTimeRateCheck = false;
        //    int igTimeIndex = -1;
        //    int igTimeRateIndex = -1;

        //    // handle premature failure
        //    returnVal = new Dictionary<string, object>()
        //    {
        //        { "rawText", "Failed to set ingame-time!" },
        //    };
        //    if ((param == null) || (param.Length < 1))
        //    {
        //        return;
        //    }

        //    // try to parse any parameters given to eventually
        //    // find patterns of igTime and igTimeRate
        //    for (int i = 0; i < param.Length; i++)
        //    {
        //        if ((!igTimeCheck) && (i != igTimeRateIndex))
        //        {
        //            if (igTimeCheck = IgTime.TryParse(param[0], out igTime))
        //            {
        //                igTimeIndex = i;
        //            }
        //        }
        //        if ((!igTimeRateCheck) && (i != igTimeIndex))
        //        {
        //            if (igTimeRateCheck = float.TryParse(param[i], out igTimeRate))
        //            {
        //                igTimeRateIndex = i;
        //            }
        //        }
        //    }

        //    if (igTimeCheck || igTimeRateCheck)
        //    {
        //        returnVal["rawText"] = "";
        //    }

        //    // prepare response message and variables to set time and time-rate
        //    if (igTimeCheck)
        //    {
        //        returnVal["rawText"] += "Set igTime to: " + igTime + ". ";
        //    }
        //    else
        //    {
        //        igTime = World.NewWorld.GetIgTime();
        //    }
        //    if (igTimeRateCheck)
        //    {
        //        returnVal["rawText"] += "Set igTimeRate (ingame- to reallife-time-ratio) to: "
        //            + igTimeRate + ". ";
        //    }
        //    else
        //    {
        //        igTimeRate = World.NewWorld.GetIgTimeRate();
        //    }

        //    World.NewWorld.ChangeIgTime(igTime, igTimeRate);
        //}

        //public static void SetIgWeather (object sender, String cmd, String[] param,
        //    out Dictionary<string, object> returnVal)
        //{
        //    returnVal = new Dictionary<string, object>()
        //    {
        //        { "rawText", "Failed to set ingame-weather!" },
        //    };

        //    if ((param == null) || (param.Length < 3))
        //    {
        //        // insufficient paramters
        //        Log.Logger.Log("#1");
        //        return;
        //    }

        //    World world = World.NewWorld;
        //    WeatherType weatherType;
        //    IgTime startTime, endTime;

        //    try
        //    {
        //        int weatherTypeInt;
        //        if (!int.TryParse(param[0], out weatherTypeInt))
        //        {
        //            returnVal["rawText"] += " Couldn't parse weatherType.";
        //            return;
        //        }

        //        weatherType = (WeatherType) weatherTypeInt;
        //        if (!Enum.IsDefined(typeof(WeatherType), weatherType))
        //        {
        //            returnVal["rawText"] += " Invalid weatherType.";
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnVal["rawText"] += " Couldn't parse weatherType: " + ex;
        //        return;
        //    }

        //    if (!IgTime.TryParse(param[1], out startTime))
        //    {
        //        returnVal["rawText"] += " Couldn't parse startTime.";
        //        return;
        //    }
        //    if (!IgTime.TryParse(param[2], out endTime))
        //    {
        //        returnVal["rawText"] += " Couldn't parse endTime.";
        //        return;
        //    }
        //    //if (startTime > endTime)
        //    //{
        //    //    returnVal["rawText"] += " startTime is bigger than endTime.";
        //    //    return;
        //    //}

        //    world.ChangeIgWeather(new WeatherEvent(weatherType, startTime, endTime));

        //    returnVal["rawText"] = String.Format("Successfully set ingame-weather to type {0}"
        //        + " between times {1} and {2}", weatherType, startTime, endTime);
        //}

    }
}
