﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.Network.Messages;
using GUC.Network;
using GUC.Server.Options;
using GUC.Log;
using GUC.WorldObjects.Collections;

namespace GUC.Server.Network
{
    public static class GameServer
    {
        internal readonly static ServerOptions Options = ServerOptions.Init();

        readonly static Dictionary<ulong, GameClient> clientDict = new Dictionary<ulong, GameClient>();

        internal readonly static RakPeerInterface ServerInterface = RakPeer.GetInstance();

        readonly static PacketReader pktReader = new PacketReader();
        readonly static PacketWriter pktWriter = new PacketWriter(128000);

        static bool started = false;
        internal static void Start()
        {
            if (started)
                return;
            started = true;

            using (SocketDescriptor socketDescriptor = new SocketDescriptor())
            {
                socketDescriptor.port = Options.Port;

                StartupResult res = ServerInterface.Startup(Options.Port, socketDescriptor, 1);
                if (res == StartupResult.RAKNET_STARTED)
                {
                    Logger.Log("Server start listening on port " + Options.Port);
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

        static void ReadMessage(NetworkIDs id, GameClient client, PacketReader stream)
        {
            switch (id)
            {
                case NetworkIDs.ConnectionMessage:
                    DisconnectClient(client);
                    Logger.LogWarning("Client sent another ConnectionMessage. Kicked: {0} IP:{1}", client.guid.g, client.systemAddress);
                    break;
                case NetworkIDs.LoadWorldMessage:
                    client.Character.InsertInWorld();
                    break;
                case NetworkIDs.MenuMessage:
                    if (client.ScriptObject != null)
                        client.ScriptObject.OnReadMenuMsg(pktReader);
                    break;

                // Ingame:

                case NetworkIDs.IngameMessage:
                    if (client.ScriptObject != null)
                        client.ScriptObject.OnReadIngameMsg(pktReader);
                    break;

                case NetworkIDs.VobPosDirMessage:
                    VobMessage.ReadPosDir(stream, client, client.character);
                    break;

                case NetworkIDs.NPCStateMessage:
                    NPCMessage.ReadState(stream, client, client.character, client.character.World);
                    break;

                case NetworkIDs.NPCApplyOverlayMessage:
                    NPCMessage.ReadApplyOverlay(stream, client.character);
                    break;
                case NetworkIDs.NPCRemoveOverlayMessage:
                    NPCMessage.ReadRemoveOverlay(stream, client.character);
                    break;

                case NetworkIDs.NPCAniStartMessage:
                    NPCMessage.ReadAniStart(stream, client.character);
                    break;
                case NetworkIDs.NPCAniStopMessage:
                    NPCMessage.ReadAniStop(stream, client.character);
                    break;

                case NetworkIDs.InventoryEquipMessage:
                    InventoryMessage.ReadEquipMessage(stream, client.character);
                    break;
                case NetworkIDs.InventoryUnequipMessage:
                    InventoryMessage.ReadUnequipMessage(stream, client.character);
                    break;

                default:
                    Logger.LogWarning("Client sent unknown NetworkID. Kicked: {0} IP:{1}", client.guid.g, client.systemAddress);
                    DisconnectClient(client);
                    return;
            }

            // flooding protection
            client.PacketCount++;
            if (client.nextCheck < GameTime.Ticks)
            {
                if (client.PacketCount >= 20)
                {
                    Logger.LogWarning("Client spammed too many packets. Kicked: {0} IP:{1}", client.guid.g, client.systemAddress);
                    DisconnectClient(client);
                }
                client.nextCheck = GameTime.Ticks + 1000000; // 100ms
                client.PacketCount = 0;
            }
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
            GameClient client = null;
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
                                Logger.Log("Client disconnected: {0} IP:{1}", p.guid, p.systemAddress);
                                DisconnectClient(client);
                            }
                            else
                            {
                                ServerInterface.CloseConnection(p.guid, false); //just to be sure
                            }
                            break;
                        case DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                            if (client != null) //there is already someone with this GUID. Should not happen.
                            {
                                throw new Exception("Duplicate RakNet-GUID!" + p.guid);
                            }
                            else
                            {
                                Logger.Log("Client connected: {0} IP:{1}", p.guid, p.systemAddress);
                            }
                            break;
                        case DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                            msgID = (NetworkIDs)pktReader.ReadByte();
                            if (client == null)
                            {
                                if (msgID == NetworkIDs.ConnectionMessage) //sends mac & drive string, should always be sent first
                                {
                                    if (ConnectionMessage.Read(pktReader, p.guid, p.systemAddress, out client))
                                    {
                                        clientDict.Add(client.guid.g, client);
                                        client.Create();
                                    }
                                    else
                                    {
                                        DisconnectClient(client);
                                    }
                                }
                                else
                                {
                                    Logger.LogWarning("Client sent {0} before ConnectionMessage. Kicked: {1} IP:{2}", msgID, p.guid, p.systemAddress);
                                    ServerInterface.CloseConnection(p.guid, false);
                                }
                            }
                            else
                            {
                                if (msgID > NetworkIDs.MenuMessage && (client.Character == null || !client.Character.IsSpawned))
                                {
                                    Logger.LogWarning("Client sent {0} without being ingame. Kicked: {1} IP:{2}", msgID, p.guid, p.systemAddress);
                                    DisconnectClient(client);
                                }
                                else
                                {
                                    ReadMessage(msgID, client, pktReader);
                                }
                            }
                            break;
                        default:
                            Logger.LogWarning("Received unused DefaultMessageIDType {0}. Kicked: {1} IP:{2}", msgDefaultType, p.guid, p.systemAddress);
                            if (client == null)
                            {
                                ServerInterface.CloseConnection(p.guid, false);
                            }
                            else
                            {
                                DisconnectClient(client);
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    if (p.length >= 2)
                        Logger.LogError("{0} {1}: {2}: {3}\n{4}", (DefaultMessageIDTypes)p.data[0], (NetworkIDs)p.data[1], e.Source, e.Message, e.StackTrace);
                    else if (p.length >= 1)
                        Logger.LogError("{0}: {1}: {2}\n{3}", (DefaultMessageIDTypes)p.data[0], e.Source, e.Message, e.StackTrace);
                    else
                        Logger.LogError("{0}: {1}\n{2}", e.Source, e.Message, e.StackTrace);

                    if (client == null)
                    {
                        ServerInterface.CloseConnection(p.guid, false);
                    }
                    else
                    {
                        DisconnectClient(client);
                    }
                }
                ServerInterface.DeallocatePacket(p);
                p = ServerInterface.Receive();
            }
        }


        public static void DisconnectClient(GameClient client)
        {
            if (client == null)
                throw new ArgumentNullException("Client is null!");

            if (client.character != null)
            {
                client.character.client = null;
                if (client.character.IsSpawned)
                {
                    client.character.World.RemoveFromPlayers(client);
                    client.Character.Cell.Clients.Remove(ref client.cellID);
                    /*if (client.Character.npcCell != null)
                        client.Character.npcCell.Remove(client.Character);*/
                }
            }

            if (client.ScriptObject != null)
                client.ScriptObject.OnDisconnection();

            client.character = null;

            ServerInterface.CloseConnection(client.guid, true);
            clientDict.Remove(client.guid.g);
            client.guid.Dispose();
            client.systemAddress.Dispose();
            client.Delete();
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

        public static void RemoveFromBanList(string systemAddress)
        {
            ServerInterface.RemoveFromBanList(systemAddress);
        }

        #region Script Menu Message

        public static PacketWriter GetMenuMsgStream()
        {
            return SetupStream(NetworkIDs.MenuMessage);
        }

        public static void SendMenuMsg(GameClient client, PacketWriter stream)
        {
            ServerInterface.Send(stream.GetData(), stream.GetLength(), PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'M', client.guid, false);
        }

        #endregion

        #region Script Ingame Message

        public static PacketWriter GetIngameMsgStream()
        {
            return SetupStream(NetworkIDs.IngameMessage);
        }

        public static void SendIngameMsg(GameClient client, PacketWriter stream)
        {
            ServerInterface.Send(stream.GetData(), stream.GetLength(), PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #endregion
    }
}