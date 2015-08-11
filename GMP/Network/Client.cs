#define D_CLIENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using Gothic.zClasses;
using WinApi;
using GUC.Enumeration;
using Gothic.zTypes;
using GUC.Client.Network.Messages;
using GUC.Client.WorldObjects;
using GUC.Client.GUI;
using GUC.Types;

namespace GUC.Client.Network
{
    public delegate void PacketReader(BitStream stream);

    public class Client : IDisposable
    {
        public RakPeerInterface client = null;
        protected SocketDescriptor socketDescriptor = null;
        protected bool isConnected = false;

        protected String ip, pw;
        protected ushort port;

        private static long lastConnectionTry = 0;
        private static int connectionTrys = 0;

        BitStream receiveBitStream = new BitStream();
        public BitStream sendBitStream = new BitStream();

        public Dictionary<byte, PacketReader> messageListener = new Dictionary<byte, PacketReader>();

        public Client()
        {
            client = RakPeer.GetInstance();

            messageListener.Add((byte)NetworkID.AccountErrorMessage, AccountMessage.Error);
            messageListener.Add((byte)NetworkID.AccountLoginMessage, AccountMessage.GetCharList);
            messageListener.Add((byte)NetworkID.ConnectionMessage, ConnectionMessage.Read);

            messageListener.Add((byte)NetworkID.PlayerControlMessage, PlayerMessage.ReadControl);

            messageListener.Add((byte)NetworkID.VobPosDirMessage, VobMessage.ReadPosDir);

            messageListener.Add((byte)NetworkID.WorldVobDeleteMessage, WorldMessage.ReadVobDelete);
            messageListener.Add((byte)NetworkID.WorldVobSpawnMessage, WorldMessage.ReadVobSpawn);
            messageListener.Add((byte)NetworkID.WorldNPCSpawnMessage, WorldMessage.ReadNPCSpawn);
            messageListener.Add((byte)NetworkID.WorldItemSpawnMessage, WorldMessage.ReadItemSpawn);

            messageListener.Add((byte)NetworkID.NPCAniStartMessage, NPCMessage.ReadAniStart);
            messageListener.Add((byte)NetworkID.NPCAniStopMessage, NPCMessage.ReadAniStop);
            messageListener.Add((byte)NetworkID.NPCStateMessage, NPCMessage.ReadState);
            messageListener.Add((byte)NetworkID.NPCAttackMessage, NPCMessage.ReadAttack);
            messageListener.Add((byte)NetworkID.NPCWeaponStateMessage, NPCMessage.ReadWeaponState);
            messageListener.Add((byte)NetworkID.NPCEquipMessage, NPCMessage.ReadEquipMessage);

            messageListener.Add((byte)NetworkID.InventoryAddMessage, InventoryMessage.ReadAddItem);
            messageListener.Add((byte)NetworkID.InventoryRemoveMessage, InventoryMessage.ReadRemoveItem);

            messageListener.Add((byte)NetworkID.ControlAddVobMessage, ControlMessage.ReadAddVob);
            messageListener.Add((byte)NetworkID.ControlRemoveVobMessage, ControlMessage.ReadRemoveVob);
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
            if (isConnected || lastConnectionTry + 10000 * 100 > DateTime.Now.Ticks)
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
            lastConnectionTry = DateTime.Now.Ticks;
        }

        public void Disconnect()
        {
            client.CloseConnection(client.GetSystemAddressFromIndex(0), true, 0);
            isConnected = false;
        }


        public void logError(String conn)
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

            while (packet != null)
            {
                //WinApi.Kernel.Process.SetWindowText(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle, "Gothic II - Untold Chapters - " + client.GetLastPing(client.GetSystemAddressFromIndex(0)));
                packetKB += packet.length;
                switch (packet.data[0])
                {
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                        isConnected = true;
                        connectionTrys = 0;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                        logError("Connection Failed!");
                        isConnected = false;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                        logError("Already Connected!");
                        break;
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_BANNED:
                        logError("Client banned!");
                        break;
                    case (byte)DefaultMessageIDTypes.ID_INVALID_PASSWORD:
                        logError("Wrong password");
                        break;
                    case (byte)DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
                        logError("ID_INCOMPATIBLE_PROTOCOL_VERSION");
                        break;
                    case (byte)DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                        logError("ID_NO_FREE_INCOMING_CONNECTIONS");
                        break;
                    case (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST:
                        isConnected = false;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                        try
                        {
                            receiveBitStream.Reset();
                            receiveBitStream.Write(packet.data, packet.length);
                            receiveBitStream.IgnoreBytes(2);

                            PacketReader func;
                            messageListener.TryGetValue(packet.data[1], out func);
                            if (func != null)
                            {
                                func(receiveBitStream);
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

            long time = DateTime.Now.Ticks - lastInfoUpdate;
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
                lastInfoUpdate = DateTime.Now.Ticks;
                packetKB = 0;
                kbsInfo.Show();
            }
        }

        public BitStream SetupSendStream(NetworkID id)
        {
            sendBitStream.Reset();
            sendBitStream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            sendBitStream.Write((byte)id);
            return sendBitStream;
        }

        public void SendStream(BitStream stream, PacketPriority pp, PacketReliability pr)
        {
            client.Send(stream, pp, pr, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
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
                    if (receiveBitStream != null)
                        receiveBitStream.Dispose();
                    if (sendBitStream != null)
                        sendBitStream.Dispose();
                }

                client = null;
                socketDescriptor = null;
                receiveBitStream = null;
                sendBitStream = null;


                _disposed = true;
            }
        }
        #endregion
    }
}
