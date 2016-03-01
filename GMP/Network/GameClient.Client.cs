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
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Collections;
using GUC.WorldObjects;

namespace GUC.Network
{
    public partial class GameClient
    {
        public static readonly GameClient Client = new GameClient();

        #region Script Menu Message

        public PacketWriter GetMenuMsgStream()
        {
            return SetupStream(NetworkIDs.MenuMessage);
        }

        public void SendMenuMsg(PacketWriter stream)
        {
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        #endregion

        #region Script Ingame Message

        public PacketWriter GetIngameMsgStream()
        {
            return SetupStream(NetworkIDs.IngameMessage);
        }

        public void SendIngameMsg(PacketWriter stream)
        {
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE);
        }

        #endregion

        #region Hero

        int charID;
        public int CharacterID { get { return this.charID; } }

        byte[] charData = null;

        void ReadTakeControl(PacketReader stream)
        {
            character = null;
            charID = stream.ReadUShort();
            charData = stream.GetRemainingData(); // save for later
            UpdateHeroControl();
        }

        internal void UpdateHeroControl()
        {
            if (World.Current == null)
                return;

            BaseVob vob = World.Current.Vobs.Get(CharacterID);
            if (vob == null || !(vob is NPC))
            {
                return;
            }

            character = (NPC)vob;

            if (charData != null)
            {
                pktReader.Load(charData, charData.Length);
                Character.ReadTakeControl(pktReader);
                charData = null;
            }

            WinApi.Process.Write(Character.gVob.Address, Gothic.Objects.oCNpc.player);
        }

        #region Position updates

        long nextUpdate = 0;
        const long updateTime = 1000000; // 100ms
        internal void UpdateCharacters()
        {
            if (DateTime.UtcNow.Ticks > nextUpdate && Character != null)
            {
                PacketWriter stream = SetupStream(NetworkIDs.VobPosDirMessage);
                stream.Write(Character.GetPosition());
                stream.Write(Character.GetDirection());
                Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);

                nextUpdate = DateTime.UtcNow.Ticks + updateTime;
            }
        }

        #endregion

        #endregion

        RakPeerInterface clientInterface = null;
        SocketDescriptor socketDescriptor = null;
        bool isConnected = false;
        public bool IsConnected { get { return isConnected; } }

        PacketReader pktReader = new PacketReader();
        PacketWriter pktWriter = new PacketWriter(128000);

        void ReadMessage(NetworkIDs id, PacketReader stream)
        {
            Logger.Log("ReadMessage: " + id);

            switch (id)
            {
                case NetworkIDs.ConnectionMessage:
                    ConnectionMessage.Read(stream);
                    break;
                case NetworkIDs.PlayerControlMessage:
                    ReadTakeControl(stream);
                    break;
                case NetworkIDs.LoadWorldMessage:
                    ScriptManager.Interface.OnLoadWorldMsg(out World.Current, stream);
                    World.Current.SendConfirmation();
                    break;
                case NetworkIDs.InstanceCreateMessage:
                    ScriptManager.Interface.OnCreateInstanceMsg((VobTypes)stream.ReadByte(), stream);
                    break;
                case NetworkIDs.InstanceDeleteMessage:
                    BaseVobInstance inst = InstanceCollection.Get(stream.ReadUShort());
                    if (inst != null) ScriptManager.Interface.OnDeleteInstanceMsg(inst);
                    break;
                case NetworkIDs.MenuMessage:
                    if (this.ScriptObject != null)
                        this.ScriptObject.OnReadMenuMsg(stream);
                    break;

                // ingame

                case NetworkIDs.IngameMessage:
                    if (this.ScriptObject != null)
                        this.ScriptObject.OnReadIngameMsg(stream);
                    break;


                case NetworkIDs.WorldSpawnMessage:
                    ScriptManager.Interface.OnSpawnVobMsg((VobTypes)stream.ReadByte(), stream);
                    break;
                case NetworkIDs.WorldDespawnMessage:
                    BaseVob vob = World.Current.Vobs.Get(stream.ReadUShort());
                    if (vob != null) ScriptManager.Interface.OnDespawnVobMsg(vob);
                    break;
                case NetworkIDs.VobPosDirMessage:
                    vob = World.Current.Vobs.Get(stream.ReadUShort());
                    if (vob != null)
                    {
                        vob.SetPosition(stream.ReadVec3f());
                        vob.SetDirection(stream.ReadVec3f());
                    }
                    break;

                default:
                    Logger.LogWarning("Received message with invalid NetworkID! " + id);
                    break;
            }
        }

        private GameClient()
        {
            clientInterface = RakPeer.GetInstance();

            socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = 0;

            if (clientInterface.Startup(1, socketDescriptor, 1) != StartupResult.RAKNET_STARTED)
            {
                Logger.LogError("RakNet failed to start!");
            }
        }

        long nextConnectionTry = 0;
        int connectionTrys = 0;
        internal void Connect()
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

        public void Disconnect()
        {
            clientInterface.CloseConnection(clientInterface.GetSystemAddressFromIndex(0), true, 0);
            isConnected = false;
        }

        long packetKB = 0;
        long lastInfoUpdate = 0;
        GUCVisual abortInfo;
        GUCVisual kbsInfo;
        GUCVisual pingInfo;

        GUCVisual instInfo;

        internal void Update()
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
                kbsInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;

                pingInfo = GUCVisualText.Create("", 0x2000, kbsInfo.zView.FontY() + 1, true);
                pingInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;

                instInfo = GUCVisualText.Create("0", 0x2000, kbsInfo.zView.FontY() + pingInfo.zView.FontY() + 2, true);
                instInfo.Texts[0].Format = GUCVisualText.TextFormat.Right;
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
                            NetworkIDs id = (NetworkIDs)pktReader.ReadByte();
                            ReadMessage(id, pktReader);
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
                if (ping > 300 || ping < 0)
                {
                    abortInfo.Show();
                }
                else
                {
                    abortInfo.Hide();
                }

                // update ping text on screen
                GUCVisualText pingText = pingInfo.Texts[0];
                pingText.Text = ("Ping: " + ping + "ms");
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
                kbsInfo.Texts[0].Text = ("Net: " + kbs + "kB/s");
                lastInfoUpdate = DateTime.UtcNow.Ticks;
                packetKB = 0;
                kbsInfo.Show(); // bring to front
                
                instInfo.Texts[0].Text = ("Instances: " + InstanceCollection.GetCount());
                instInfo.Show();
            }
        }

        internal static PacketWriter SetupStream(NetworkIDs id)
        {
            Client.pktWriter.Reset();
            Client.pktWriter.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            Client.pktWriter.Write((byte)id);
            return Client.pktWriter;
        }

        internal static void Send(PacketWriter stream, PacketPriority pp, PacketReliability pr)
        {
            Client.clientInterface.Send(stream.GetData(), stream.GetLength(), pp, pr, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
