#define D_SERVER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.Network.Messages;
using System.Diagnostics;
using GUC.Network;
using GUC.Server.WorldObjects;
using System.Security.Cryptography;

namespace GUC.Server.Network
{
    delegate void MsgReader(PacketReader stream, Client client, NPC character);

    public class Server
    {   
        // All player npcs on this server
        internal static Dictionary<uint, NPC> sPlayerDict = new Dictionary<uint, NPC>();
        public static NPC GetPlayer(uint ID) { NPC npc; sPlayerDict.TryGetValue(ID, out npc); return npc; }
        public static IEnumerable<NPC> GetPlayers() { return sPlayerDict.Values; }

        // All bot npcs on this server
        internal static Dictionary<uint, NPC> sNpcDict = new Dictionary<uint, NPC>();
        public static NPC GetNpc(uint ID) { NPC npc; sNpcDict.TryGetValue(ID, out npc); return npc; }
        public static IEnumerable<NPC> GetNpcs() { return sNpcDict.Values; }

        // All bot npcs & players on this server
        internal static Dictionary<uint, NPC> sAllNpcsDict = new Dictionary<uint, NPC>();
        public static NPC GetNpcOrPlayer(uint ID) { NPC npc; sAllNpcsDict.TryGetValue(ID, out npc); return npc; }
        public static IEnumerable<NPC> GetNpcsAndPlayers() { return sAllNpcsDict.Values; }


        // All items on this server
        internal static Dictionary<uint, Item> sItemDict = new Dictionary<uint, Item>();
        public static Item GetItem(uint ID) { Item item; sItemDict.TryGetValue(ID, out item); return item; }
        public static IEnumerable<Item> GetItems() { return sItemDict.Values; }


        // All vobs and mobs on this server
        internal static Dictionary<uint, Vob> sVobDict = new Dictionary<uint, Vob>();
        public static Vob GetVob(uint ID) { Vob vob; sVobDict.TryGetValue(ID, out vob); return vob; }
        public static IEnumerable<Vob> GetVobs() { return sVobDict.Values; }


        // All vobs which currently exist on the server
        internal static Dictionary<uint, Vob> sAllVobsDict = new Dictionary<uint, Vob>();
        public static Vob GetAnyVob(uint ID) { Vob vob; sAllVobsDict.TryGetValue(ID, out vob); return vob; }
        public static IEnumerable<Vob> GetAllVobs() { return sAllVobsDict.Values; }


        // All worlds which currently exist on the server
        internal static Dictionary<string, World> sWorldDict = new Dictionary<string, World>();
        public static World GetWorld(string worldName) { World world; sWorldDict.TryGetValue(worldName.ToUpper(), out world); return world; }
        public static IEnumerable<World> GetWorlds() { return sWorldDict.Values; }




        public static IEnumerable<Client> GetClients() { return Program.server.clientDict.Values; }
        public static void DisconnectClient(Client client)
        {
            Program.server.KickClient(client);
        }

        public static void AddToBanList(string systemAddress)
        {
            Program.server.ServerInterface.AddToBanList(systemAddress);
        }

        public static Action<PacketReader,Client, PacketWriter> OnLoginMessage;
        static void HandleLoginMsg(PacketReader stream, Client client, NPC character)
        {
            Log.Logger.log("LOGINMESSAGE");
            if (OnLoginMessage != null)
            {
                
                OnLoginMessage(stream, client, Program.server.SetupStream(NetworkID.LoginMessage));
            }
        }

        internal RakPeerInterface ServerInterface { get; private set; }

        Dictionary<NetworkID, MsgReader> MessageListener;

        Dictionary<ulong, Client> clientDict;

        internal PacketReader pktReader = new PacketReader();
        internal PacketWriter pktWriter = new PacketWriter(64000);

        /** 
        * Server-class which defines and manages network transfer as well as the game loop.
        * It uses RakNet network messages to establish communication channels between server and clients.
        * It further counts the connections and initialiszes the overall game loop.
        */

        internal Server()
        {
            ServerInterface = RakPeer.GetInstance(); /**< Instance of RakNets main interface for network communication. */

            clientDict = new Dictionary<ulong, Client>();

            InitMessageListener();
        }

        protected void InitMessageListener()
        {
            MessageListener = new Dictionary<NetworkID, MsgReader>();

            MessageListener.Add(NetworkID.ConnectionMessage, ConnectionMessage.Read);
            MessageListener.Add(NetworkID.LoginMessage, HandleLoginMsg);
            MessageListener.Add(NetworkID.PlayerControlMessage, NPC.ReadControl);

            /*MessageListener.Add((byte)NetworkID.PlayerPickUpItemMessage, NPC.ReadPickUpItem);

            MessageListener.Add((byte)NetworkID.VobPosDirMessage, VobMessage.ReadPosDir);

            MessageListener.Add((byte)NetworkID.NPCAniStartMessage, NPCMessage.ReadAniStart);
            MessageListener.Add((byte)NetworkID.NPCAniStopMessage, NPCMessage.ReadAniStop);
            MessageListener.Add((byte)NetworkID.NPCStateMessage, NPC.ReadCmdState);
            MessageListener.Add((byte)NetworkID.NPCTargetStateMessage, NPC.ReadCmdTargetState);
            MessageListener.Add((byte)NetworkID.NPCDrawItemMessage, NPC.ReadCmdDrawEquipment);
            MessageListener.Add((byte)NetworkID.NPCUndrawItemMessage, NPC.ReadCmdUndrawItem);
            MessageListener.Add((byte)NetworkID.NPCJumpMessage, NPC.ReadCmdJump);

            MessageListener.Add((byte)NetworkID.InventoryDropItemMessage, InventoryMessage.ReadDropItem);

            MessageListener.Add((byte)NetworkID.InventoryUseItemMessage, NPC.ReadCmdUseItem);

            MessageListener.Add((byte)NetworkID.MobUseMessage, NPC.ReadCmdUseMob);
            MessageListener.Add((byte)NetworkID.MobUnUseMessage, NPC.ReadCmdUnuseMob);

            MessageListener.Add((byte)NetworkID.TradeMessage, TradeMessage.Read);*/
        }

        /**
         *   Initializes/Starts the RakPeerInterface server to actually listen and answer network messages.
         *   Completes the setup of the network interface/server and starts it, so it can listen on the given port.
         *   @param port an ushort which is the main port of game server network communication.
         *   @param maxConnections an ushort which limits the maximum accepted client-server-connections.
         *   @param pw a String which defines the password to verify client-server-connections (???).
         */
        internal void Start(ushort port, ushort maxConnections, String pw)
        {
            pw = GUC.Options.Constants.VERSION + pw;

            SocketDescriptor socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = port;

            bool started = ServerInterface.Startup(maxConnections, socketDescriptor, 1) == StartupResult.RAKNET_STARTED;
            ServerInterface.SetMaximumIncomingConnections(maxConnections);
            ServerInterface.SetOccasionalPing(true);

            if (pw.Length != 0)
            {
                ServerInterface.SetIncomingPassword(pw, pw.Length);
            }

            if (!started)
                Log.Logger.logError("Port is already in use");
            else
                Log.Logger.log("Server start listening on port " + port);
        }

        /**
         *   Counts the current amount of client-server-connections and returns them.
         *   Counts the current amount of client-server-connections and returns them.
         *   @return numbers as ushort amount of client-server-connections
         */
        internal ushort ConnectionCount()
        {
            SystemAddress[] sa;
            ushort numbers = 0;
            ServerInterface.GetConnectionList(out sa, ref numbers);
            return numbers;
        }

        /**
         *   Game loop which receives data from clients and redirects/reacts accordingly.
         *   In this surrounding loop data is received from individual clients and the server reacts depending 
         *   on the network message types (see class attributes for these types). This is done for each
         *   network message received by individual clients until there is no more (buffered) message
         *   left. Character creation is done here on successful connection as well as the respective 
         *   deletion of disconnect.
         */
        internal void Update()
        {
            Packet p = ServerInterface.Receive();
            Client client;
            MsgReader readFunc;
            NetworkID msgID;
            DefaultMessageIDTypes msgDefaultType;

            while (p != null)
            {
                try
                {
                    pktReader.Load(p.data);

                    clientDict.TryGetValue(p.guid.g, out client);

                    msgDefaultType = (DefaultMessageIDTypes)pktReader.ReadByte();
                    switch (msgDefaultType)
                    {
                        case DefaultMessageIDTypes.ID_CONNECTION_LOST:
                        case DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                            if (client != null)
                            {
                                KickClient(client);
                            }
                            else
                            {
                                ServerInterface.CloseConnection(p.guid, false); //just to be sure
                            }
                            Log.Logger.log("Client disconnected: " + p.guid);
                            break;
                        case DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                            if (client != null) //there is already someone with this GUID. Should not happen.
                            {
                                KickClient(client);
                                Log.Logger.logError("Kicked duplicate GUID " + p.guid);
                            }
                            else
                            {
                                clientDict.Add(p.guid.g, new Client(p.guid, p.systemAddress));
                                Log.Logger.log(String.Format("Client connected: {0} IP:{1}", p.guid, p.systemAddress));
                            }
                            break;
                        case DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                            if (client != null)
                            {
                                msgID = (NetworkID)pktReader.ReadByte();
                                if (!client.isValid)
                                {
                                    if (msgID == NetworkID.ConnectionMessage) //sends mac & drive string, should always be sent first
                                    {
                                        ConnectionMessage.Read(pktReader, client, null);
                                    }
                                    else
                                    {
                                        KickClient(client);
                                        Log.Logger.logWarning(String.Format("Client sent {0} before ConnectionMessage or is banned. Kicked: {0} IP:{1}", msgID, p.guid, p.systemAddress));
                                    }
                                }
                                else
                                {
                                    if (client.MainChar == null) // not ingame yet
                                    {
                                        if (msgID == NetworkID.LoginMessage) // login menu message
                                        {
                                            HandleLoginMsg(pktReader, client, null);
                                        }
                                        else if (msgID == NetworkID.PlayerControlMessage && client.Character != null) // confirmation to take control of a npc / start in the world
                                        {
                                            NPC.ReadControl(pktReader, client, client.Character);
                                        }
                                        else
                                        {
                                            KickClient(client);
                                            Log.Logger.logWarning(String.Format("Client sent {0} before being ingame. Kicked: {0} IP:{1}", msgID, p.guid, p.systemAddress));
                                        }
                                    }
                                    else
                                    {
                                        if (msgID == NetworkID.ConnectionMessage)
                                        {
                                            KickClient(client);
                                            Log.Logger.logWarning(String.Format("Client sent another ConnectionMessage. Kicked: {0} IP:{1}", msgID, p.guid, p.systemAddress));
                                        }
                                        else
                                        {
                                            
                                            MessageListener.TryGetValue(msgID, out readFunc);
                                            if (readFunc == null)
                                            {
                                                KickClient(client);
                                                Log.Logger.logWarning(String.Format("Client sent unknown NetworkID. Kicked: {0} IP:{1}", p.guid, p.systemAddress));
                                            }
                                            else
                                            {
                                                readFunc(pktReader, client, client.Character);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ServerInterface.CloseConnection(p.guid, false);
                                Log.Logger.logWarning(String.Format("Client sent 'user' packets before 'new connection' packet. Kicked: {0} IP:{1}", p.guid, p.systemAddress));
                            }
                            break;
                        default:
                            if (client == null)
                            {
                                ServerInterface.CloseConnection(p.guid, false);
                            }
                            else
                            {
                                KickClient(client);
                            }
                            Log.Logger.log(String.Format("Received unused DefaultMessageIDType {0}. Kicked: {1} IP:{2}", msgDefaultType, p.guid, p.systemAddress));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.logError((NetworkID)p.data[1] + ":\n" + ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace);
                    ServerInterface.CloseConnection(p.guid, false);
                }
                ServerInterface.DeallocatePacket(p);
                p = ServerInterface.Receive();
            }
        }


        internal void KickClient(Client client)
        {
            ServerInterface.CloseConnection(client.guid, false);
            client.Delete();
            clientDict.Remove(client.guid.g);
            client.guid.Dispose();
        }

        internal PacketWriter SetupStream(NetworkID ID)
        {
            pktWriter.Reset();
            pktWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            pktWriter.Write((byte)ID);
            return pktWriter;
        }
    }
}
