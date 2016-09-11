using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Options;
using GUC.Log;

namespace GUC.Network
{
    public static class GameServer
    {
        readonly static Dictionary<ulong, GameClient> clientDict = new Dictionary<ulong, GameClient>();

        internal readonly static RakPeerInterface ServerInterface = RakPeer.GetInstance();

        readonly static PacketReader pktReader = new PacketReader();
        readonly static PacketWriter pktWriter = new PacketWriter(128000);

        static GameServer()
        {
            using (SocketDescriptor socketDescriptor = new SocketDescriptor())
            {
                socketDescriptor.port = ServerOptions.Port;

                StartupResult res = ServerInterface.Startup(ServerOptions.Slots, socketDescriptor, 1);
                if (res == StartupResult.RAKNET_STARTED)
                {
                    Logger.Log("Server starts RakNet on port " + ServerOptions.Port);
                }
                else
                {
                    throw new Exception("RakNet startup failed: " + res.ToString());
                }
                ServerInterface.SetMaximumIncomingConnections(ServerOptions.Slots);
                ServerInterface.SetOccasionalPing(true);

                if (ServerOptions.Password != null)
                {
                    string pwStr = Convert.ToBase64String(ServerOptions.Password.ToArray());
                    ServerInterface.SetIncomingPassword(pwStr, pwStr.Length);
                }
            }
        }

        static void ReadUserMessage(ClientMessages id, GameClient client, PacketReader stream)
        {
            switch (id)
            {
                case ClientMessages.WorldLoadedMessage:
                    GameClient.Messages.ReadLoadWorldMessage(stream, client);
                    break;
                case ClientMessages.ScriptMessage:
                    client.ScriptObject.ReadScriptMessage(stream);
                    break;
                case ClientMessages.GuidedVobMessage:
                    WorldObjects.VobGuiding.GuidedVob.Messages.ReadPosDir(stream, client, client.World);
                    break;
                case ClientMessages.GuidedNPCMessage:
                    WorldObjects.NPC.Messages.ReadPosDir(stream, client, client.World);
                    break;
                case ClientMessages.ScriptCommandMessage:
                    GameClient.Messages.ReadScriptCommandMessage(stream, client, client.World, false);
                    break;
                case ClientMessages.ScriptCommandHeroMessage:
                    GameClient.Messages.ReadScriptCommandMessage(stream, client, client.World, true);
                    break;

                default:
                    Logger.LogWarning("Client sent unknown NetworkID '{0}'. Kicked: {1} IP:{2}", id, client.ID, client.SystemAddress);
                    DisconnectClient(client);
                    return;
            }
        }


        /**
         *   Game loop which receives data from clients and redirects/reacts accordingly.
         *   In this surrounding loop data is received from individual clients and the server reacts depending 
         *   on the network message types (see class attributes for these types). This is done for each
         *   network message received by individual clients until there is no more (buffered) message
         *   left.
         */
        internal static void Update()
        {
            GameClient client = null;

            Packet packet;
            while ((packet = ServerInterface.Receive()) != null)
            {
                try
                {
                    clientDict.TryGetValue(packet.guid.g, out client);

                    pktReader.Load(packet.data, (int)packet.length);
                    ClientMessages id = (ClientMessages)pktReader.ReadByte();

                    switch (id)
                    {
                        case ClientMessages.RakNet_ConnectionLost:
                        case ClientMessages.RakNet_DisconnectionNotification:
                            if (client != null)
                            {
                                Logger.Log("Client disconnected: {0} IP: {1}", client.ID, client.SystemAddress);
                                DisconnectClient(client);
                            }
                            else
                            {
                                ServerInterface.CloseConnection(packet.guid, false); //just to be sure
                            }
                            break;
                        case ClientMessages.RakNet_NewIncomingConnection:
                            if (client != null) //there is already someone with this GUID. Should never happen.
                            {
                                throw new Exception("Duplicate RakNet-GUID! " + packet.guid);
                            }
                            else
                            {
                                Logger.Log("Client connected: IP: " + packet.systemAddress);
                            }
                            break;
                        default:
                            if (client == null)
                            {
                                if (id == ClientMessages.ConnectionMessage) //sends mac & drive string, should always be sent first
                                {
                                    if (GameClient.Messages.ReadConnection(pktReader, packet.guid, packet.systemAddress, out client))
                                    {
                                        clientDict.Add(client.Guid.g, client);
                                        client.Create();
                                    }
                                    else
                                    {
                                        Logger.LogWarning("Client was not allowed to connect: {0}", packet.systemAddress);
                                        ServerInterface.CloseConnection(packet.guid, false);
                                    }
                                }
                                else
                                {
                                    Logger.LogWarning("Client sent {0} before ConnectionMessage. Kicked IP: {1}", id, packet.systemAddress);
                                    ServerInterface.CloseConnection(packet.guid, false);
                                }
                            }
                            else
                            {
                                if (id > ClientMessages.ScriptMessage && !client.IsIngame)
                                {
                                    //Logger.LogWarning("Client sent {0} without being ingame. Kicked: {1} IP:{2}", msgID, p.guid, p.systemAddress);
                                    //DisconnectClient(client);
                                    break;
                                }
                                else
                                {
                                    ReadUserMessage(id, client, pktReader);
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    if (packet.length > 0)
                        Logger.LogError("{0}: {1}: {2}\n{3}", (ClientMessages)packet.data[0], e.Source, e.Message, e.StackTrace);
                    else
                        Logger.LogError("{0}: {1}\n{2}", e.Source, e.Message, e.StackTrace);

                    if (client == null)
                    {
                        ServerInterface.CloseConnection(packet.guid, false);
                    }
                    else
                    {
                        DisconnectClient(client);
                    }
                }
                finally
                {
                    ServerInterface.DeallocatePacket(packet);
                }
            }
        }

        public static void DisconnectClient(GameClient client)
        {
            if (client == null)
                throw new ArgumentNullException("Client is null!");

            ServerInterface.CloseConnection(client.Guid, true);
            clientDict.Remove(client.Guid.g);

            client.Delete();
        }

        internal static PacketWriter SetupStream(ServerMessages ID)
        {
            pktWriter.Reset();
            pktWriter.Write((byte)ID);
            return pktWriter;
        }

        public static void AddToBanList(string systemAddress)
        {
            ServerInterface.AddToBanList(systemAddress);
            Logger.Log("IP banned: " + systemAddress);
        }

        public static void RemoveFromBanList(string systemAddress)
        {
            ServerInterface.RemoveFromBanList(systemAddress);
            Logger.Log("IP unbanned: " + systemAddress);
        }
    }
}
