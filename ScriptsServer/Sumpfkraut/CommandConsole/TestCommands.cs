using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Web.WS.Protocols;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.CommandConsole
{
    public class TestCommands : GUC.Utilities.ExtendedObject
    {

        protected TestCommands ()
        { }



        private static List<Networking.ScriptClient> GetAllClients ()
        {
            var clients = new List<Networking.ScriptClient>();
            Networking.ScriptClient.ForEach(c => clients.Add(c));
            return clients;
        }

        private static List<Networking.ScriptClient> GetClientsByID (List<int> ids)
        {
            ids.Sort();
            var clients = new List<Networking.ScriptClient>();
            var allClients = GetAllClients();
            var prevIDs = new List<int>();

            allClients.ForEach((Networking.ScriptClient c) =>
            {
                var cID = c.ID;
                if ((!prevIDs.Contains(cID)) && ids.Contains(cID))
                {
                    clients.Add(c);
                    prevIDs.Add(cID);
                }
            });

            return clients;
        }

        private static List<Networking.ScriptClient> GetClientsByCharName (List<string> charNames)
        {
            charNames.Sort();
            var clients = new List<Networking.ScriptClient>();
            var allClients = GetAllClients();
            var prevNames = new List<string>();

            allClients.ForEach((Networking.ScriptClient c) =>
            {
                var cName = c.Character.CustomName;
                if ((!prevNames.Contains(cName)) && charNames.Contains(cName))
                {
                    clients.Add(c);
                    prevNames.Add(cName);
                }
            });

            return clients;
        }

        public static void GetPlayerList(object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            returnVal = new Dictionary<string, object>()
            {
                { "rawText", "Failed to retrieve player list!" },
            };

            StringBuilder infoSB = new StringBuilder();
            infoSB.Append("List of players: ");
            string sep = "";
            Network.GameClient.ForEach(client =>
            {
                infoSB.AppendFormat("{0}[clientID: {1}", sep, client.ID);

                if (client.Character != null)
                {
                    infoSB.AppendFormat(", charID: {0}, charName: {1}, charPos: {2}], ", client.Character.ID,
                        client.Character.Name, client.Character.GetPosition());
                }

                infoSB.Append("]");
                sep = ", ";
            });

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", infoSB.ToString() },
            };
        }

        protected static List<Networking.ScriptClient> PrepareClientList (string[] param)
        {
            List<int> ids = new List<int>(); int id;
            List<string> charNames = new List<string>();
            List<Networking.ScriptClient> clients = null;
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

        public static void BanPlayers (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder successMsgSB = new StringBuilder();
            List<Networking.ScriptClient> clients = PrepareClientList(param);
            int clientID = -1;
            string charName = "";

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
                    clientID = clients[i].ID;
                    if (clients[i].Character != null) { charName = clients[i].Character.CustomName; }
                    else { charName = "?"; }
                    clients[i].BaseClient.Ban();
                    if (!first) { successMsgSB.Append(","); }
                    else { first = false; }
                    successMsgSB.AppendFormat("[{0}, {1}]", clientID, charName);
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
            List<Networking.ScriptClient> clients = PrepareClientList(param);
            int clientID = -1;
            string charName = "";

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
                    clientID = clients[i].ID;
                    if (clients[i].Character != null) { charName = clients[i].Character.CustomName; }
                    else { charName = "?"; }
                    clients[i].BaseClient.Kick();
                    if (!first) { successMsgSB.Append(","); }
                    else { first = false; }
                    successMsgSB.AppendFormat("[{0}, {1}]", clientID, charName);
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
            StringBuilder successMsgSB = new StringBuilder();
            List<Networking.ScriptClient> clients = PrepareClientList(param);

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
                    if (clients[i].Character != null)
                    {
                        //clients[i].Character.SetHealth(0);
                        ((Arena.ArenaClient)clients[i]).KillCharacter();

                        if (!first) { successMsgSB.Append(","); }
                        else { first = false; }
                        successMsgSB.AppendFormat("[{0}, {1}]", clients[i].ID, clients[i].Character.CustomName);
                    }
                }
            }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", successMsgSB.ToString() },
            };
        }

        public static void SetTime (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder successMsgSB = new StringBuilder();

            WorldTime time = WorldTime.Zero;
            float rate = 0f;
            bool foundTime = false, foundRate = false;
            for (int i = 0; i < param.Length; i++)
            {
                if ((!foundRate) && (float.TryParse(param[i], out rate))) { foundRate = true; }
                if ((!foundTime) && (WorldTime.TryParseDayHourMin(param[i], out time))) { foundTime = true; }
                if (foundTime && foundRate) { break; }
            }

            if (!(foundTime || foundRate)) { successMsgSB.Append("No valid WorldTime or rate provided!"); }
            else
            {
                if (!foundTime) { time = WorldInst.List[0].Clock.Time; }
                if (!foundRate) { rate = WorldInst.List[0].Clock.Rate; }
                WorldInst.List[0].Clock.SetTime(time, rate);
                successMsgSB.Append("Changed WorldTime to: ");
                successMsgSB.AppendFormat("( time: {0}, rate: {1} )", time, rate);
            }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", successMsgSB.ToString() },
            };
        }

        public static void SetWeatherType (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder msgSB = new StringBuilder();

            WeatherTypes wt = WeatherTypes.Rain;
            var foundWT = false;
            for (int i = 0; i < param.Length; i++)
            {
                if (Enum.TryParse(param[i], out wt))
                {
                    foundWT = true;
                    break;
                }
            }
            if (foundWT)
            {
                WorldInst.List[0].Weather.SetWeatherType(wt);
                msgSB.AppendFormat("Applied WeatherType: {0}", wt);
            }
            else { msgSB.Append("No valid WeatherType found!"); }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", msgSB.ToString() },
            };
        }

        public static void SetRainWeight (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder msgSB = new StringBuilder();

            var weight = 0f;
            var foundWeight = false;
            for (int i = 0; i < param.Length; i++)
            {
                if (float.TryParse(param[i], out weight))
                {
                    foundWeight = true;
                    break;
                }
            }
            if (foundWeight)
            {
                WorldInst.List[0].Weather.SetNextWeight(WorldInst.List[0].Clock.Time, weight);
                msgSB.AppendFormat("Applied rain / precipitation weight: {0}", weight);
            }
            else { msgSB.Append("No valid rain / precipitation weight found!"); }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", msgSB.ToString() },
            };
        }

        public static void TeleportToPosition (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder successMsgSB = new StringBuilder();
            List<Networking.ScriptClient> clients = PrepareClientList(param);
            int clientID = -1;
            string charName = "";

            if ((clients == null) || (clients.Count < 1))
            {
                successMsgSB.AppendFormat("Didn't use {0} on any players.", cmd);
            }
            else
            {
                bool first = true;
                successMsgSB.AppendFormat("Used {0} on players:", cmd);

                //Arena.ArenaClient.ForEach((Networking.ScriptClient c) => { c.Character. });

                for (var i = 0; i < clients.Count; i++)
                {
                    clientID = clients[i].ID;
                    if (clients[i].Character != null) { charName = clients[i].Character.CustomName; }
                    else { charName = "?"; }
                    
                    if (!first) { successMsgSB.Append(","); }
                    else { first = false; }
                    successMsgSB.AppendFormat("[{0}, {1}]", clientID, charName);
                }
            }

            returnVal = new Dictionary<string, object>
            {
                { "type", WSProtocolType.chatData },
                { "sender", "SERVER" },
                { "rawText", successMsgSB.ToString() },
            };
        }

    }
}
