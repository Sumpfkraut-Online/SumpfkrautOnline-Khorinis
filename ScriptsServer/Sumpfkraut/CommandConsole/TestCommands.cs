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
                if (client.Character != null)
                {
                    index = charNames.IndexOf(client.Character.Name);
                    if (index > -1) { clients.Add(client); }
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

            //List<object> playerInfo = new List<object>();
            //Network.GameClient.ForEach(client =>
            //{
            //    PlayerInfo info = new PlayerInfo();

            //    info.ClientID = client.ID;
            //    //info.DriveHash = client.DriveHash;
            //    //info.MacHash = client.MacHash;
            //    //info.Ping = client.GetLastPing();

            //    if (client.Character == null) { return; }

            //    info.VobID = client.Character.ID;
            //    //info.VobType = client.Character.VobType;
            //    info.NPCName = client.Character.Name;
            //    //info.MapName = "TESTWORLD";
            //    info.Position = client.Character.GetPosition();
            //    //info.Direction = client.Character.GetDirection();

            //    playerInfo.Add(info);
            //});

            //returnVal = new Dictionary<string, object>
            //{
            //    { "type", WSProtocolType.chatData },
            //    { "sender", "SERVER" },
            //    { "rawText", "List of players: " },
            //    { "data", playerInfo },
            //};

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

        protected static List<Network.GameClient> PrepareClientList (string[] param)
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

        public static void BanPlayers (object sender, string cmd, string[] param,
            out Dictionary<string, object> returnVal)
        {
            StringBuilder successMsgSB = new StringBuilder();
            List<Network.GameClient> clients = PrepareClientList(param);
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
                    if (clients[i].Character != null) { charName = clients[i].Character.Name; }
                    else { charName = "?"; }
                    clients[i].Ban();
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
            List<Network.GameClient> clients = PrepareClientList(param);
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
                    if (clients[i].Character != null) { charName = clients[i].Character.Name; }
                    else { charName = "?"; }
                    clients[i].Kick();
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
                    if (clients[i].Character != null)
                    {
                        clients[i].Character.SetHealth(0);
                        if (!first) { successMsgSB.Append(","); }
                        else { first = false; }
                        successMsgSB.AppendFormat("[{0}, {1}]", clients[i].ID, clients[i].Character.Name);
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

    }
}
