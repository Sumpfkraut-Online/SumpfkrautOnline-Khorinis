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

namespace GUC.Server.Network
{
    delegate void PacketReader(BitStream stream, Client client);

    public class Server
    {
        public RakPeerInterface ServerInterface { get; private set; }
        Dictionary<byte, PacketReader> MessageListener;
        public BitStream ReceiveBitStream { get; private set; }
        public BitStream SendBitStream { get; private set; }

        Dictionary<ulong, Client> clientList;

        /** 
        * Server-class which defines and manages network transfer as well as the game loop.
        * It uses RakNet network messages to establish communication channels between server and clients.
        * It further counts the connections and initialiszes the overall game loop.
        */

        public Server()
        {
            ServerInterface = RakPeer.GetInstance(); /**< Instance of RakNets main interface for network communication. */
            MessageListener = new Dictionary<byte, PacketReader>();
            ReceiveBitStream = new BitStream();
            SendBitStream = new BitStream();

            clientList = new Dictionary<ulong, Client>();

            initMessageListener();
        }

        protected void initMessageListener()
        {
            MessageListener.Add((byte)NetworkID.ConnectionMessage, ConnectionMessage.Read);

            MessageListener.Add((byte)NetworkID.AccountCreationMessage, AccountMessage.Register);
            MessageListener.Add((byte)NetworkID.AccountLoginMessage, AccountMessage.Login);
            MessageListener.Add((byte)NetworkID.AccountCharCreationMessage, AccountMessage.CreateCharacter);
            MessageListener.Add((byte)NetworkID.AccountCharLoginMessage, AccountMessage.LoginCharacter);

            MessageListener.Add((byte)NetworkID.PlayerControlMessage, NPC.ReadControl);
            MessageListener.Add((byte)NetworkID.PlayerPickUpItemMessage, NPC.ReadPickUpItem);

            MessageListener.Add((byte)NetworkID.VobPosDirMessage, VobMessage.ReadPosDir);

            MessageListener.Add((byte)NetworkID.NPCAniStartMessage, NPCMessage.ReadAniStart);
            MessageListener.Add((byte)NetworkID.NPCAniStopMessage, NPCMessage.ReadAniStop);
            MessageListener.Add((byte)NetworkID.NPCStateMessage, NPC.ReadCmdState);
            MessageListener.Add((byte)NetworkID.NPCTargetStateMessage, NPC.ReadTargetState);
            MessageListener.Add((byte)NetworkID.NPCDrawItemMessage, NPC.ReadCmdDrawEquipment);
            MessageListener.Add((byte)NetworkID.NPCUndrawItemMessage, NPC.ReadCmdUndrawItem);
            MessageListener.Add((byte)NetworkID.NPCHitMessage, NPCMessage.ReadHitMessage);
            MessageListener.Add((byte)NetworkID.NPCJumpMessage, NPC.ReadCmdJump);

            MessageListener.Add((byte)NetworkID.InventoryDropItemMessage, InventoryMessage.ReadDropItem);
            MessageListener.Add((byte)NetworkID.InventoryUseItemMessage, NPC.ReadCmdUseItem);

            MessageListener.Add((byte)NetworkID.MobUseMessage, NPC.ReadCmdUseMob);
            MessageListener.Add((byte)NetworkID.MobUnUseMessage, NPC.ReadCmdUnuseMob);
        }

        /**
         *   Initializes/Starts the RakPeerInterface server to actually listen and answer network messages.
         *   Completes the setup of the network interface/server and starts it, so it can listen on the given port.
         *   @param port an ushort which is the main port of game server network communication.
         *   @param maxConnections an ushort which limits the maximum accepted client-server-connections.
         *   @param pw a String which defines the password to verify client-server-connections (???).
         */
        public void Start(ushort port, ushort maxConnections, String pw)
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
        public ushort ConnectionCount()
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
        public void Update()
        {
            Packet p = ServerInterface.Receive();
            Client client;
            PacketReader func;
            byte msgID;

            while (p != null)
            {
                try
                {
                    clientList.TryGetValue(p.guid.g, out client);

                    switch (p.data[0])
                    {
                        case (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST:
                        case (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                            if (client != null)
                            {
                                DisconnectClient(client);
                            }
                            else
                            {
                                ServerInterface.CloseConnection(p.guid, false); //just to be sure
                            }
                            Log.Logger.log("Disconnected: " + p.guid);
                            break;
                        case (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                            if (client != null) //whut?
                            {
                                DisconnectClient(client);
                            }
                            clientList.Add(p.guid.g, new Client(p.guid, p.systemAddress));
                            Log.Logger.log("New Connection: " + p.guid + " " + p.systemAddress);
                            break;
                        case (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                            if (client != null)
                            {
                                ReceiveBitStream.Reset();
                                ReceiveBitStream.Write(p.data, p.length);
                                ReceiveBitStream.IgnoreBytes(2);
                                msgID = p.data[1];
                                if (client.isValid)
                                {
                                    //for safety:
                                    if (msgID > (byte)NetworkID.AccountLoginMessage)
                                    {
                                        if (client.accountID == -1)
                                        {   //not even logged in but trying to send non-login-packets
                                            DisconnectClient(client);
                                            break;
                                        }

                                        if (msgID >= (byte)NetworkID.PlayerControlMessage)
                                        {
                                            if (client.character == null)
                                            { //not even controlling a character but trying to send character/world packets
                                                DisconnectClient(client);
                                                break;
                                            }

                                            if (!client.character.Spawned && msgID > (byte)NetworkID.PlayerControlMessage)
                                            { //not even spawned but trying to send world packets
                                                DisconnectClient(client);
                                                break;
                                            }
                                        }
                                    }

                                    MessageListener.TryGetValue(msgID, out func);
                                    if (func != null)
                                    {
                                        func(ReceiveBitStream, client);
                                    }
                                }
                                else
                                {
                                    if (msgID == (byte)NetworkID.ConnectionMessage) //sends mac & drive string
                                    {
                                        ConnectionMessage.Read(ReceiveBitStream, client);
                                    }
                                    else //bye bye
                                    {
                                        DisconnectClient(client);
                                    }
                                }
                            }
                            break;
                        default:
                            Log.Logger.log(Log.Logger.LOG_INFO, "Message-Type: " + ((DefaultMessageIDTypes)p.data[0]));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.log(Log.Logger.LOG_ERROR, (NetworkID)p.data[1] + ":\n" + ex.Source + "\n" + ex.Message + "\n" + ex.StackTrace);
                    ServerInterface.CloseConnection(p.guid, false);
                }
                ServerInterface.DeallocatePacket(p);
                p = ServerInterface.Receive();
            }
        }

        void DisconnectClient(Client client)
        {
            ServerInterface.CloseConnection(client.guid, false);
            client.Disconnect();
            clientList.Remove(client.guid.g);
            client.Dispose();
        }

        public BitStream SetupStream(NetworkID id)
        {
            SendBitStream.Reset();
            SendBitStream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            SendBitStream.Write((byte)id);
            return SendBitStream;
        }
    }
}
