#define D_CLIENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Client.Network.Messages;
using GUC.Client.GUI;
using GUC.Types;
using GUC.Network;

namespace GUC.Client.Network
{
    delegate void MsgReader(PacketReader stream);

    public class Client : IDisposable
    {
        public RakPeerInterface client = null;
        protected SocketDescriptor socketDescriptor = null;
        protected bool isConnected = false;

        protected String ip, pw;
        protected ushort port;

        private static long lastConnectionTry = 0;
        private static int connectionTrys = 0;

        PacketReader pktReader = new PacketReader();
        PacketWriter pktWriter = new PacketWriter(64000);

        Dictionary<NetworkID, MsgReader> messageListener = new Dictionary<NetworkID, MsgReader>();

        public Client()
        {
            client = RakPeer.GetInstance();

            messageListener.Add(NetworkID.LoginMessage, AccountMessage.GetCharList);
            messageListener.Add(NetworkID.ConnectionMessage, ConnectionMessage.Read);
            messageListener.Add(NetworkID.PlayerControlMessage, PlayerMessage.ReadControl);
            /*
            messageListener.Add((byte)NetworkID.VobPosDirMessage, VobMessage.ReadPosDir);

            messageListener.Add((byte)NetworkID.WorldVobDeleteMessage, WorldMessage.ReadVobDelete);
            messageListener.Add((byte)NetworkID.WorldVobSpawnMessage, WorldMessage.ReadVobSpawn);
            messageListener.Add((byte)NetworkID.WorldNPCSpawnMessage, WorldMessage.ReadNPCSpawn);
            messageListener.Add((byte)NetworkID.WorldItemSpawnMessage, WorldMessage.ReadItemSpawn);
            messageListener.Add((byte)NetworkID.WorldTimeMessage, WorldMessage.ReadTimeChange);
            messageListener.Add((byte)NetworkID.WorldWeatherMessage, WorldMessage.ReadWeatherChange);

            messageListener.Add((byte)NetworkID.NPCAniStartMessage, NPCMessage.ReadAniStart);
            messageListener.Add((byte)NetworkID.NPCAniStopMessage, NPCMessage.ReadAniStop);
            messageListener.Add((byte)NetworkID.NPCStateMessage, NPCMessage.ReadState);
            messageListener.Add((byte)NetworkID.NPCTargetStateMessage, NPCMessage.ReadTargetState);
            messageListener.Add((byte)NetworkID.NPCDrawItemMessage, NPCMessage.ReadDrawItem);
            messageListener.Add((byte)NetworkID.NPCUndrawItemMessage, NPCMessage.ReadUndrawItem);
            messageListener.Add((byte)NetworkID.NPCHealthMessage, NPCMessage.ReadHealthMessage);
            messageListener.Add((byte)NetworkID.NPCEquipMessage, NPCMessage.ReadEquipMessage);
            messageListener.Add((byte)NetworkID.NPCUnequipMessage, NPCMessage.ReadUnequipMessage);
            messageListener.Add((byte)NetworkID.NPCJumpMessage, NPCMessage.ReadJump);

            messageListener.Add((byte)NetworkID.InventoryAddMessage, InventoryMessage.ReadAddItem);
            messageListener.Add((byte)NetworkID.InventoryAmountMessage, InventoryMessage.ReadAmountUpdate);

            messageListener.Add((byte)NetworkID.ControlAddVobMessage, ControlMessage.ReadAddVob);
            messageListener.Add((byte)NetworkID.ControlRemoveVobMessage, ControlMessage.ReadRemoveVob);

            messageListener.Add((byte)NetworkID.MobUseMessage, MobMessage.ReadUseMob);
            messageListener.Add((byte)NetworkID.MobUnUseMessage, MobMessage.ReadUnUseMob);

            messageListener.Add((byte)NetworkID.TradeMessage, TradeMessage.Read);

            messageListener.Add((byte)NetworkID.ControlCmdMessage, ControlMessage.ReadVobControlCmd);*/
        }

        public void Startup()
        {
            socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = 0;

            bool started = client.Startup(1, socketDescriptor, 1) == StartupResult.RAKNET_STARTED;
            isConnected = false;
        }

        public void Connect()
        {
            Connect(ip, port, pw); ;
        }

        public void Connect(String ip, ushort port, String pw)
        {
            if (isConnected || lastConnectionTry + 10000 * 100 > DateTime.UtcNow.Ticks)
                return;
            if (connectionTrys >= 100)
                zERROR.GetZErr(Program.Process).Report(4, 'G', "Verbindung nicht möglich!", 0, "Client.cs", 0);
            this.ip = ip; this.port = port; this.pw = pw;
            bool b;
            pw = GUC.Options.Constants.VERSION + pw;
            b = client.Connect(ip, port, pw, pw.Length) == ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED;
            client.SetOccasionalPing(true);
            if (!b)
            {
                zCView.GetStartscreen(Program.Process).PrintTimedCXY("The connection could not be estabilshed", 10, 255, 0, 0, 255);
                zERROR.GetZErr(Program.Process).Report(2, 'G', "Es konnte keine Verbindung aufgebaut werden.", 0, "Client.cs", 0);
            }
            connectionTrys++;
            lastConnectionTry = DateTime.UtcNow.Ticks;
        }

        public void Disconnect()
        {
            client.CloseConnection(client.GetSystemAddressFromIndex(0), true, 0);
            isConnected = false;
        }

        void logError(string msg)
        {

        }

        uint packetKB = 0;
        long lastInfoUpdate = 0;
        GUCVisual abortInfo;
        GUCVisual kbsInfo;
        GUCVisual pingInfo;

        public void Update()
        {
            if (client == null)
                return;

            if (abortInfo == null)
            {
                int[] screenSize = GUCView.GetScreenSize();
                abortInfo = new GUCVisual((screenSize[0] - 300) / 2, 150, 300, 40);
                abortInfo.SetBackTexture("Menu_Choice_Back.tga");
                GUCVisualText visText = abortInfo.CreateText("Verbindung unterbrochen!");
                visText.SetColor(ColorRGBA.Red);

                kbsInfo = GUCVisualText.Create("", 0x2000, 0, true);
                kbsInfo.Font = GUCVisual.Fonts.Menu;
                kbsInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;

                pingInfo = GUCVisualText.Create("", 0x2000, kbsInfo.zView.FontY(), true);
                pingInfo.Font = GUCVisual.Fonts.Menu;
                pingInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;
            }

            int counter = 0;
            Packet packet = client.Receive();
            DefaultMessageIDTypes type;

            while (packet != null)
            {
                pktReader.Load(packet.data);
                packetKB += packet.length;
                type = (DefaultMessageIDTypes)pktReader.ReadByte();

                switch (type)
                {
                    case DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                        isConnected = true;
                        connectionTrys = 0;
                        break;
                    case DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                        logError("Connection Failed!");
                        isConnected = false;
                        break;
                    case DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                        logError("Already Connected!");
                        break;
                    case DefaultMessageIDTypes.ID_CONNECTION_BANNED:
                        logError("Client banned!");
                        break;
                    case DefaultMessageIDTypes.ID_INVALID_PASSWORD:
                        logError("Wrong password");
                        break;
                    case DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
                        logError("ID_INCOMPATIBLE_PROTOCOL_VERSION");
                        break;
                    case DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                        logError("ID_NO_FREE_INCOMING_CONNECTIONS");
                        break;
                    case DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                    case DefaultMessageIDTypes.ID_CONNECTION_LOST:
                        isConnected = false;
                        break;
                    case DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                        try
                        {
                            NetworkID netID = (NetworkID)pktReader.ReadByte();

                            MsgReader func;
                            messageListener.TryGetValue(netID, out func);
                            if (func != null)
                            {
                                func(pktReader);
                            }
                        }
                        catch (Exception ex) { zERROR.GetZErr(Program.Process).Report(2, 'G', ex.Source + " " + ex.Message + " " + ex.StackTrace, 0, "Client.cs", 0); }
                        break;
                }
                client.DeallocatePacket(packet);

                counter++;
                if (counter >= 1000)
                {
                    counter = 0;
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "1000 Pakete hintereinander", 0, "Client.cs", 0);
                }

                packet = client.Receive();
            }

            long time = DateTime.UtcNow.Ticks - lastInfoUpdate;
            if (time > TimeSpan.TicksPerSecond)
            {
                int Ping = client.GetLastPing(client.GetSystemAddressFromIndex(0));
                if (Ping > 800 || Ping < 0)
                {
                    abortInfo.Show();
                }
                else
                {
                    abortInfo.Hide();
                }
                pingInfo.Texts[0].Text = Ping + "ms";
                pingInfo.Show();

                int kbs = (int)(((double)packetKB / 1024.0f) / ((double)time / (double)TimeSpan.TicksPerSecond));
                kbsInfo.Texts[0].Text = kbs + "kB/s";
                lastInfoUpdate = DateTime.UtcNow.Ticks;
                packetKB = 0;
                kbsInfo.Show();
            }
        }

        public PacketWriter SetupSendStream(NetworkID id)
        {
            pktWriter.Reset();
            pktWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            pktWriter.Write((byte)id);
            return pktWriter;
        }

        public void SendStream(PacketWriter stream, PacketPriority pp, PacketReliability pr)
        {
            client.Send(stream.GetData(), stream.GetLength(), pp, pr, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        #region Disposable
        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    if (client != null)
                    {
                        client.CloseConnection(client.GetSystemAddressFromIndex(0), true, 0);
                        client.Dispose();
                    }
                    if (socketDescriptor != null)
                        socketDescriptor.Dispose();
                }

                client = null;
                socketDescriptor = null;
                _disposed = true;
            }
        }
        #endregion
    }
}
