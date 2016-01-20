using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.Network.Messages;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Server.WorldObjects.Collections;
using GUC.Server.Options;

namespace GUC.Server.Network
{
    delegate void MsgReader(PacketReader stream, Client client, NPC character, World world);

    public static class Server
    {
        internal readonly static ServerOptions Options = ServerOptions.Init();

        public readonly static VobCollection Vobs = new VobCollection();
        public readonly static InstanceCollection Instances = new InstanceCollection();

        readonly static Dictionary<ulong, Client> clientDict = new Dictionary<ulong, Client>();
        public static IEnumerable<Client> GetClients() { return clientDict.Values; }

        internal readonly static RakPeerInterface ServerInterface = RakPeer.GetInstance();

        readonly static Dictionary<NetworkIDs, MsgReader> messageListener = new Dictionary<NetworkIDs, MsgReader>();
        
        readonly static PacketReader pktReader = new PacketReader();
        readonly static PacketWriter pktWriter = new PacketWriter(128000);

        static bool started = false;
        internal static void Start()
        {
            if (started)
                return;
            started = true;

            InitMessages();
            
            using (SocketDescriptor socketDescriptor = new SocketDescriptor())
            {
                socketDescriptor.port = Options.Port;

                StartupResult res = ServerInterface.Startup(Options.Port, socketDescriptor, 1);
                if (res == StartupResult.RAKNET_STARTED)
                {
                    Log.Logger.Log("Server start listening on port " + Options.Port);
                }
                else
                {
                    throw new Exception("RakNet startup failed: " + res.ToString());
                }
                ServerInterface.SetMaximumIncomingConnections(Options.Slots);
                ServerInterface.SetOccasionalPing(true);

                string pw = Constants.VERSION + Options.Password;
                if (pw.Length > 0)
                {
                    ServerInterface.SetIncomingPassword(pw, pw.Length);
                }
            }
        }

        static void InitMessages()
        {
            /*messageListener.Add(NetworkIDs.ConnectionMessage, ConnectionMessage.Read);
            messageListener.Add(NetworkIDs.MenuMessage, HandleLoginMsg);
            messageListener.Add(NetworkIDs.PlayerControlMessage, NPC.ReadControl);

            MessageListener.Add((byte)NetworkID.PlayerPickUpItemMessage, NPC.ReadPickUpItem);

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
         *   Game loop which receives data from clients and redirects/reacts accordingly.
         *   In this surrounding loop data is received from individual clients and the server reacts depending 
         *   on the network message types (see class attributes for these types). This is done for each
         *   network message received by individual clients until there is no more (buffered) message
         *   left. Character creation is done here on successful connection as well as the respective 
         *   deletion of disconnect.
         */
        internal static void Update()
        {
            Packet p = ServerInterface.Receive();
            Client client;
            MsgReader readFunc;
            NetworkIDs msgID;
            DefaultMessageIDTypes msgDefaultType;

            while (p != null)
            {
                try
                {
                    pktReader.Load(p.data, (int)p.length);

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
                            Log.Logger.Log("Client disconnected: " + p.guid);
                            break;
                        case DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                            if (client != null) //there is already someone with this GUID. Should not happen.
                            {
                                KickClient(client);
                                Log.Logger.LogError("Kicked duplicate GUID " + p.guid);
                            }
                            else
                            {
                                clientDict.Add(p.guid.g, new Client(p.guid, p.systemAddress));
                                Log.Logger.Log(String.Format("Client connected: {0} IP:{1}", p.guid, p.systemAddress));
                            }
                            break;
                        case DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                            if (client != null)
                            {
                                msgID = (NetworkIDs)pktReader.ReadByte();
                                if (!client.isValid)
                                {
                                    if (msgID == NetworkIDs.ConnectionMessage) //sends mac & drive string, should always be sent first
                                    {
                                        ConnectionMessage.Read(pktReader, client, null, null);
                                    }
                                    else
                                    {
                                        KickClient(client);
                                        Log.Logger.LogWarning(String.Format("Client sent {0} before ConnectionMessage or is banned. Kicked: {0} IP:{1}", msgID, p.guid, p.systemAddress));
                                    }
                                }
                                else
                                {
                                    if (client.MainChar == null) // not ingame yet
                                    {
                                        if (msgID == NetworkIDs.MenuMessage) // login menu message
                                        {
                                            if (OnMenuMsg != null)
                                            {
                                                OnMenuMsg(client, pktReader);
                                            }
                                        }
                                        else if (msgID == NetworkIDs.PlayerControlMessage && client.Character != null) // confirmation to take control of a npc / start in the world
                                        {
                                            NPC.ReadControl(pktReader, client, client.Character, client.Character.World);
                                        }
                                        else
                                        {
                                            KickClient(client);
                                            Log.Logger.LogWarning(String.Format("Client sent {0} before being ingame. Kicked: {0} IP:{1}", msgID, p.guid, p.systemAddress));
                                        }
                                    }
                                    else
                                    {
                                        if (msgID == NetworkIDs.ConnectionMessage)
                                        {
                                            KickClient(client);
                                            Log.Logger.LogWarning(String.Format("Client sent another ConnectionMessage. Kicked: {0} IP:{1}", msgID, p.guid, p.systemAddress));
                                        }
                                        else if (client.Character != null && client.Character.IsSpawned)
                                        {
                                            messageListener.TryGetValue(msgID, out readFunc);
                                            if (readFunc == null)
                                            {
                                                KickClient(client);
                                                Log.Logger.LogWarning(String.Format("Client sent unknown NetworkID. Kicked: {0} IP:{1}", p.guid, p.systemAddress));
                                            }
                                            else
                                            {
                                                readFunc(pktReader, client, client.Character, client.Character.World);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ServerInterface.CloseConnection(p.guid, false);
                                Log.Logger.LogWarning(String.Format("Client sent 'user' packets before 'new connection' packet. Kicked: {0} IP:{1}", p.guid, p.systemAddress));
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
                            Log.Logger.Log(String.Format("Received unused DefaultMessageIDType {0}. Kicked: {1} IP:{2}", msgDefaultType, p.guid, p.systemAddress));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.LogError((NetworkIDs)p.data[1] + ":\n" + ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace);
                    ServerInterface.CloseConnection(p.guid, false);
                }
                ServerInterface.DeallocatePacket(p);
                p = ServerInterface.Receive();
            }
        }


        internal static void KickClient(Client client)
        {
            ServerInterface.CloseConnection(client.guid, false);
            client.Delete();
            clientDict.Remove(client.guid.g);
            client.guid.Dispose();
        }

        internal static PacketWriter SetupStream(NetworkIDs ID)
        {
            pktWriter.Reset();
            pktWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            pktWriter.Write((byte)ID);
            return pktWriter;
        }

        public static void AddToBanList(string systemAddress)
        {
            ServerInterface.AddToBanList(systemAddress);
        }



        public static Action<Client, PacketReader> OnMenuMsg;

        public static PacketWriter GetMenuMsgStream()
        {
            return SetupStream(NetworkIDs.MenuMessage);
        }

        public static void SendMenuMsg(Client client, PacketWriter stream)
        {
            ServerInterface.Send(stream.GetData(), stream.GetLength(), PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'M', client.guid, false);
        }
    }
}
