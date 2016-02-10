using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Types;
using GUC.Network;
using GUC.Enumeration;
using GUC.Log;
using GUC.Client.GUI;
using GUC.Client.Network.Messages;
using GUC.Scripting;

namespace GUC.Client.Network
{
    public static class GameClient
    {
        #region Client Rank

        static int rank = 0;
        public static int Rank { get { return rank; } }

        static void ReadRank(PacketReader stream)
        {
            rank = stream.ReadByte();
            if (rank == 0) rankInfo.Texts[0].Text = "Statist";
            else if (rank == 1) rankInfo.Texts[0].Text = "Supporter";
            else if (rank >= 2) rankInfo.Texts[0].Text = "Admin";
        }

        #endregion

        #region Script Menu Message

        static void ReadMenuMessage(PacketReader stream)
        {
            ScriptManager.Interface.OnReadMenuMsg(stream);
        }

        public static PacketWriter GetMenuMsgStream()
        {
            return SetupStream(NetworkIDs.MenuMessage);
        }

        public static void SendMenuMsg(PacketWriter stream)
        {
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        #endregion

        #region Script Ingame Message

        static void ReadIngameMessage(PacketReader stream)
        {
            ScriptManager.Interface.OnReadIngameMsg(stream);
        }

        public static PacketWriter GetIngameMsgStream()
        {
            return SetupStream(NetworkIDs.IngameMessage);
        }

        public static void SendIngameMsg(PacketWriter stream)
        {
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        #endregion

        static RakPeerInterface clientInterface = null;
        static SocketDescriptor socketDescriptor = null;
        static bool isConnected = false;
        public static bool IsConnected { get { return isConnected; } }

        static PacketReader pktReader = new PacketReader();
        static PacketWriter pktWriter = new PacketWriter(128000);

        static Dictionary<NetworkIDs, MsgReader> messageListener = new Dictionary<NetworkIDs, MsgReader>();

        delegate void MsgReader(PacketReader stream);
        static void InitMsgs()
        {
            messageListener.Add(NetworkIDs.ConnectionMessage, ConnectionMessage.Read);

            messageListener.Add(NetworkIDs.RankMessage, ReadRank);

            messageListener.Add(NetworkIDs.MenuMessage, ReadMenuMessage);
            messageListener.Add(NetworkIDs.IngameMessage, ReadIngameMessage);
            /*
            messageListener.Add(NetworkIDs.AccountErrorMessage, AccountMessage.Error);
            messageListener.Add(NetworkIDs.AccountLoginMessage, AccountMessage.GetCharList);
            messageListener.Add(NetworkIDs.ConnectionMessage, ConnectionMessage.Read);

            messageListener.Add(NetworkIDs.PlayerControlMessage, PlayerMessage.ReadControl);

            messageListener.Add(NetworkIDs.VobPosDirMessage, VobMessage.ReadPosDir);

            messageListener.Add(NetworkIDs.WorldVobDeleteMessage, WorldMessage.ReadVobDelete);
            messageListener.Add(NetworkIDs.WorldVobSpawnMessage, WorldMessage.ReadVobSpawn);
            messageListener.Add(NetworkIDs.WorldNPCSpawnMessage, WorldMessage.ReadNPCSpawn);
            messageListener.Add(NetworkIDs.WorldItemSpawnMessage, WorldMessage.ReadItemSpawn);
            messageListener.Add(NetworkIDs.WorldTimeMessage, WorldMessage.ReadTimeChange);
            messageListener.Add(NetworkIDs.WorldWeatherMessage, WorldMessage.ReadWeatherChange);

            messageListener.Add(NetworkIDs.NPCAniStartMessage, NPCMessage.ReadAniStart);
            messageListener.Add(NetworkIDs.NPCAniStopMessage, NPCMessage.ReadAniStop);
            messageListener.Add(NetworkIDs.NPCStateMessage, NPCMessage.ReadState);
            messageListener.Add(NetworkIDs.NPCTargetStateMessage, NPCMessage.ReadTargetState);
            messageListener.Add(NetworkIDs.NPCDrawItemMessage, NPCMessage.ReadDrawItem);
            messageListener.Add(NetworkIDs.NPCUndrawItemMessage, NPCMessage.ReadUndrawItem);
            messageListener.Add(NetworkIDs.NPCHealthMessage, NPCMessage.ReadHealthMessage);
            messageListener.Add(NetworkIDs.NPCEquipMessage, NPCMessage.ReadEquipMessage);
            messageListener.Add(NetworkIDs.NPCUnequipMessage, NPCMessage.ReadUnequipMessage);
            messageListener.Add(NetworkIDs.NPCJumpMessage, NPCMessage.ReadJump);

            messageListener.Add(NetworkIDs.InventoryAddMessage, InventoryMessage.ReadAddItem);
            messageListener.Add(NetworkIDs.InventoryAmountMessage, InventoryMessage.ReadAmountUpdate);

            messageListener.Add(NetworkIDs.ControlAddVobMessage, ControlMessage.ReadAddVob);
            messageListener.Add(NetworkIDs.ControlRemoveVobMessage, ControlMessage.ReadRemoveVob);

            messageListener.Add(NetworkIDs.MobUseMessage, MobMessage.ReadUseMob);
            messageListener.Add(NetworkIDs.MobUnUseMessage, MobMessage.ReadUnUseMob);

            messageListener.Add(NetworkIDs.TradeMessage, TradeMessage.Read);

            messageListener.Add(NetworkIDs.ControlCmdMessage, ControlMessage.ReadVobControlCmd);*/
        }

        static GameClient()
        {
            clientInterface = RakPeer.GetInstance();

            InitMsgs();

            socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = 0;

            if (clientInterface.Startup(1, socketDescriptor, 1) != StartupResult.RAKNET_STARTED)
            {
                Logger.LogError("RakNet failed to start!");
            }
        }

        static long nextConnectionTry = 0;
        static int connectionTrys = 0;
        internal static void Connect()
        {
            if (isConnected || nextConnectionTry > DateTime.UtcNow.Ticks)
                return;

            if (connectionTrys >= 10)
                throw new Exception("Connection failed!");

            try
            {
                Logger.Log("Trying to connect to the server.");

                string[] serverAddress = Environment.GetEnvironmentVariable("ServerAddress").Split(':');
                string ip = serverAddress[0];
                ushort port = Convert.ToUInt16(serverAddress[1]);

                string pw = Constants.VERSION + "";
                ConnectionAttemptResult res = clientInterface.Connect(ip, port, pw, pw.Length);
                if (res != ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED)
                {
                    throw new Exception("Connection couldn't be established: " + res);
                }
                clientInterface.SetOccasionalPing(true);

                nextConnectionTry = DateTime.UtcNow.Ticks + 500 * TimeSpan.TicksPerMillisecond;
                connectionTrys++;

                Logger.Log("Connection attempt started.");
            }
            catch (Exception e)
            {
                Logger.LogError("Verbindung fehlgeschlagen! " + e);
            }
        }

        public static void Disconnect()
        {
            clientInterface.CloseConnection(clientInterface.GetSystemAddressFromIndex(0), true, 0);
            isConnected = false;
        }

        static uint packetKB = 0;
        static long lastInfoUpdate = 0;
        static GUCVisual abortInfo;
        static GUCVisual kbsInfo;
        static GUCVisual pingInfo;

        static GUCVisual rankInfo;

        internal static void Update()
        {
            if (clientInterface == null)
                return;

            if (abortInfo == null)
            {
                int[] screenSize = GUCView.GetScreenSize();
                abortInfo = new GUCVisual((screenSize[0] - 300) / 2, 150, 300, 40);
                abortInfo.SetBackTexture("Menu_Choice_Back.tga");
                GUCVisualText visText = abortInfo.CreateText("Verbindung unterbrochen!");
                visText.SetColor(ColorRGBA.Red);

                kbsInfo = GUCVisualText.Create("", 0x2000, 0, true);
                kbsInfo.Font = GUCView.Fonts.Menu;
                kbsInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;

                pingInfo = GUCVisualText.Create("", 0x2000, kbsInfo.zView.FontY(), true);
                pingInfo.Font = GUCView.Fonts.Menu;
                pingInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;

                rankInfo = GUCVisualText.Create("Statist", 0x2000, kbsInfo.zView.FontY() + pingInfo.zView.FontY() + 1, true);
                rankInfo.Font = GUCView.Fonts.Menu;
                rankInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;
            }

            int counter = 0;
            Packet packet = clientInterface.Receive();
            DefaultMessageIDTypes msgDefaultType;

            while (packet != null)
            {
                try
                {
                    packetKB += packet.length;

                    pktReader.Load(packet.data, (int)packet.length);
                    msgDefaultType = (DefaultMessageIDTypes)pktReader.ReadByte();

                    switch (msgDefaultType)
                    {
                        case DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                            isConnected = true;
                            connectionTrys = 0;
                            Logger.Log("Connection request accepted from server.");
                            ConnectionMessage.Write();
                            break;
                        case DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                            Logger.LogError("Connection Failed!");
                            isConnected = false;
                            break;
                        case DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                            Logger.LogError("Already Connected!");
                            break;
                        case DefaultMessageIDTypes.ID_CONNECTION_BANNED:
                            Logger.LogError("Client banned!");
                            break;
                        case DefaultMessageIDTypes.ID_INVALID_PASSWORD:
                            Logger.LogError("Wrong password");
                            break;
                        case DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
                            Logger.LogError("ID_INCOMPATIBLE_PROTOCOL_VERSION");
                            break;
                        case DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                            Logger.LogError("ID_NO_FREE_INCOMING_CONNECTIONS");
                            break;
                        case DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                        case DefaultMessageIDTypes.ID_CONNECTION_LOST:
                            isConnected = false;
                            Logger.Log("Disconnected from server.");
                            break;
                        case DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                            MsgReader func;
                            NetworkIDs id = (NetworkIDs)pktReader.ReadByte();
                            messageListener.TryGetValue(id, out func);
                            if (func != null)
                            {
                                func(pktReader);
                            }
                            break;
                    }
                    clientInterface.DeallocatePacket(packet);

                    counter++;
                    if (counter >= 1000)
                    {
                        counter = 0;
                        Logger.LogWarning("1000 Pakete hintereinander");
                    }

                    packet = clientInterface.Receive();
                }
                catch (Exception e)
                {
                    Logger.LogError(e);
                }
            }

            long time = DateTime.UtcNow.Ticks - lastInfoUpdate;
            if (time > TimeSpan.TicksPerSecond)
            {
                // get last ping time
                int ping = clientInterface.GetLastPing(clientInterface.GetSystemAddressFromIndex(0));
                if (ping > 800 || ping < 0)
                {
                    abortInfo.Show();
                }
                else
                {
                    abortInfo.Hide();
                }

                // update ping text on screen
                GUCVisualText pingText = pingInfo.Texts[0];
                pingText.Text = (ping + "ms");
                if (ping <= 120)
                {
                    pingText.SetColor(new ColorRGBA((byte)(40 + 180 * ping / 120), 220, 40));
                }
                else if (ping <= 220)
                {
                    pingText.SetColor(new ColorRGBA(220, (byte)(220 - 180 * (ping - 100) / 120), 40));
                }
                else
                {
                    pingText.SetColor(ColorRGBA.Red);
                }
                pingInfo.Show(); // bring to front

                // update kB/s text on screen
                int kbs = (int)(((double)packetKB / 1024d) / ((double)time / (double)TimeSpan.TicksPerSecond));
                kbsInfo.Texts[0].Text = (kbs + "kB/s");
                lastInfoUpdate = DateTime.UtcNow.Ticks;
                packetKB = 0;
                kbsInfo.Show(); // bring to front

                rankInfo.Show();
            }
        }

        internal static PacketWriter SetupStream(NetworkIDs id)
        {
            pktWriter.Reset();
            pktWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            pktWriter.Write((byte)id);
            return pktWriter;
        }

        internal static void Send(PacketWriter stream, PacketPriority pp, PacketReliability pr)
        {
            clientInterface.Send(stream.GetData(), stream.GetLength(), pp, pr, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
