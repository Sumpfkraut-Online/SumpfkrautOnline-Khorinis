using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Types;
using GUC.Enumeration;
using GUC.Log;
using GUC.Client.GUI;
using GUC.Client.Network.Messages;
using GUC.Scripting;
using GUC.WorldObjects;
using Gothic.Objects;
using Gothic;
using GUC.Animations;

namespace GUC.Network
{
    public partial class GameClient
    {
        public static readonly GameClient Client = new GameClient();

        public partial interface IScriptClient : IScriptGameObject
        {
            void ReadScriptMsg(PacketReader stream);
        }

        #region Commands

        NPCStates nextState = NPCStates.Stand;
        const int DelayBetweenMessages = 1000000; //100ms
        public void DoSetHeroState(NPCStates state)
        {
            if (this.character == null)
                return;

            if (this.character.IsDead)
                return;

            if (this.nextState == state)
                return;

            this.nextState = state;
            this.character.nextStateUpdate = 0;
            UpdateHeroState(GameTime.Ticks);
        }

        void UpdateHeroState(long now)
        {
            if (this.character == null)
                return;

            if (this.character.IsDead)
                return;

            if (this.character.State == nextState)
                return;

            if (now < this.character.nextStateUpdate)
                return;
            
            NPCMessage.WriteState(this.character, nextState);
            this.character.nextStateUpdate = now + DelayBetweenMessages;
        }

        long nextAniUpdate = 0;
        public void DoStartAni(AniJob job)
        {
            if (this.character == null)
                return;

            if (this.character.IsDead)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                NPCMessage.WriteAniStart(job);
                nextAniUpdate = GameTime.Ticks + DelayBetweenMessages;
            }
        }
        
        public void DoJump()
        {
            if (this.character == null)
                return;

            if (this.character.IsDead)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                NPCMessage.WriteJump(this.character);
                nextAniUpdate = GameTime.Ticks + DelayBetweenMessages;
            }
        }

        #endregion

        #region Script Menu Message

        public PacketWriter GetMenuMsgStream()
        {
            return SetupStream(NetworkIDs.ScriptMessage);
        }

        public void SendMenuMsg(PacketWriter stream, PktPriority pr, PktReliability rl)
        {
            Send(stream, (PacketPriority)pr, (PacketReliability)rl);
        }

        #endregion
        
        #region Hero
        
        void ReadTakeControl(PacketReader stream)
        {
            int characterID = stream.ReadUShort();
            NPC npc;
            if (!World.current.TryGetVob(characterID, out npc))
            {
                throw new Exception("Hero not found!");
            }
            npc.ReadTakeControl(stream);

            Logger.Log("Take control of npc " + npc.ID);

            // Remove the gothic hero if he's there
            var hero = oCNpc.GetPlayer();
            if (!World.Current.ContainsVobAddress(hero.Address))
            {
                hero.Disable();
                oCGame.GetWorld().RemoveVob(hero);
            }
            
            this.ScriptObject.SetControl(npc);
        }

        partial void pSetControl(NPC npc)
        {
            this.character = npc;
            Character.gVob.SetAsPlayer();
        }

        #region Position updates

        internal void UpdateCharacters(long now)
        {
            World.Current.ForEachVob(v =>
            {
                if (v is NPC)
                    ((NPC)v).Update(now);
            });

            if (this.character == null || this.character.IsDead)
                return;

            VobMessage.WritePosDirMessage(now);

            UpdateHeroState(now);
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
            if (id != NetworkIDs.VobPosDirMessage && id != NetworkIDs.NPCStateMessage)
                Logger.Log(id);

            switch (id)
            {
                /*
                *   OUTGAME MESSAGES
                */

                // CONNECTION MESSAGE
                case NetworkIDs.ConnectionMessage:
                    ConnectionMessage.Read(stream);
                    break;

                // Player Messages
                case NetworkIDs.PlayerControlMessage:
                    this.ReadTakeControl(stream);
                    break;

                // World Messages
                case NetworkIDs.LoadWorldMessage:
                    WorldMessage.ReadLoadWorldMessage(stream);
                    break;

                // Instance Messages
                case NetworkIDs.InstanceCreateMessage:
                    InstanceMessage.ReadCreateMessage(stream);
                    break;
                case NetworkIDs.InstanceDeleteMessage:
                    InstanceMessage.ReadDeleteMessage(stream);
                    break;

                // Model Messages
                case NetworkIDs.ModelCreateMessage:
                    ModelMessage.ReadCreateMessage(stream);
                    break;
                case NetworkIDs.ModelDeleteMessage:
                    ModelMessage.ReadDeleteMessage(stream);
                    break;

                // Script Message
                case NetworkIDs.ScriptMessage:
                    this.ScriptObject.ReadScriptMsg(stream);
                    break;

                /*
                *   INGAME MESSAGES
                */

                // Script Message
                case NetworkIDs.ScriptVobMessage:
                    ScriptMessage.ReadVobMsg(stream);
                    break;

                // World Messages
                case NetworkIDs.WorldSpawnMessage:
                    WorldMessage.ReadVobSpawnMessage(stream);
                    break;
                case NetworkIDs.WorldDespawnMessage:
                    WorldMessage.ReadVobDespawnMessage(stream);
                    break;
                case NetworkIDs.WorldCellMessage:
                    WorldMessage.ReadCellMessage(stream);
                    break;
                case NetworkIDs.WorldTimeMessage:
                    WorldMessage.ReadTimeMessage(stream);
                    break;
                case NetworkIDs.WorldTimeStartMessage:
                    WorldMessage.ReadTimeStartMessage(stream);
                    break;
                case NetworkIDs.WorldWeatherMessage:
                    WorldMessage.ReadWeatherMessage(stream);
                    break;
                case NetworkIDs.WorldWeatherTypeMessage:
                    WorldMessage.ReadWeatherTypeMessage(stream);
                    break;

                // Vob Messages
                case NetworkIDs.VobPosDirMessage:
                    VobMessage.ReadPosDirMessage(stream);
                    break;

                // Inventory Messages
                case NetworkIDs.InventoryAddMessage:
                    InventoryMessage.ReadAddItem(stream);
                    break;
                case NetworkIDs.InventoryRemoveMessage:
                    InventoryMessage.ReadRemoveItem(stream);
                    break;

                // NPC Messages
                case NetworkIDs.NPCStateMessage:
                    NPCMessage.ReadState(stream);
                    break;
                case NetworkIDs.NPCEquipMessage:
                    NPCMessage.ReadEquipMessage(stream);
                    break;
                case NetworkIDs.NPCUnequipMessage:
                    NPCMessage.ReadUnequipMessage(stream);
                    break;

                case NetworkIDs.NPCApplyOverlayMessage:
                    NPCMessage.ReadApplyOverlay(stream);
                    break;
                case NetworkIDs.NPCRemoveOverlayMessage:
                    NPCMessage.ReadRemoveOverlay(stream);
                    break;

                case NetworkIDs.NPCAniStartMessage:
                    NPCMessage.ReadAniStart(stream);
                    break;
                case NetworkIDs.NPCAniStopMessage:
                    NPCMessage.ReadAniStop(stream);
                    break;

                case NetworkIDs.NPCHealthMessage:
                    NPCMessage.ReadHealthMessage(stream);
                    break;

                case NetworkIDs.NPCJumpMessage:
                    NPCMessage.ReadJump(stream);
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

                Logger.Log("Connection attempt started.");

            }
            catch (Exception e)
            {
                Logger.LogError("Verbindung fehlgeschlagen! " + e);
            }

            nextConnectionTry = DateTime.UtcNow.Ticks + 500 * TimeSpan.TicksPerMillisecond;
            connectionTrys++;
        }

        public void Disconnect()
        {
            clientInterface.CloseConnection(clientInterface.GetSystemAddressFromIndex(0), true, 0);
            isConnected = false;
        }

        long receivedBytes = 0;
        long sentBytes = 0;
        long lastInfoUpdate = 0;
        GUCVisual abortInfo;
        GUCVisual devInfo;

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
                
                devInfo = new GUCVisual();
                int zDist = devInfo.zView.FontY() + 2;

                devInfo = GUCVisualText.Create("Ping", 0x2000, 0, true);
                devInfo.CreateText("Received", 0x2000, zDist, true);
                devInfo.CreateText("Sent", 0x2000, 2*zDist, true);

                devInfo.CreateText("Position", 0x2000, 3 * zDist, true);
                devInfo.CreateText("Direction", 0x2000, 4 * zDist, true);

                for (int i = 0; i < devInfo.Texts.Count; i++)
                    devInfo.Texts[i].Format = GUCVisualText.TextFormat.Right;
            }

            int counter = 0;
            Packet packet = clientInterface.Receive();
            DefaultMessageIDTypes msgDefaultType;

            while (packet != null)
            {
                try
                {
                    receivedBytes += packet.length;

                    pktReader.Load(packet.data, (int)packet.length);
                    msgDefaultType = (DefaultMessageIDTypes)pktReader.ReadByte();

                    switch (msgDefaultType)
                    {
                        case DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                            isConnected = true;
                            connectionTrys = 0;
                            Logger.Log("Connection request accepted from server.");
                            ScriptManager.Interface.OnClientConnection(this);
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
                    if (packet.length >= 2)
                        Logger.LogError("{0} {1}: {2}: {3}\n{4}", (DefaultMessageIDTypes)packet.data[0], (NetworkIDs)packet.data[1], e.Source, e.Message, e.StackTrace);
                    else if (packet.length >= 1)
                        Logger.LogError("{0}: {1}: {2}\n{3}", (DefaultMessageIDTypes)packet.data[0], e.Source, e.Message, e.StackTrace);
                    else
                        Logger.LogError("{0}: {1}\n{2}", e.Source, e.Message, e.StackTrace);
                }
            }

            long time = GameTime.Ticks - lastInfoUpdate;
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
                GUCVisualText pingText = devInfo.Texts[0];
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

                // update kB/s text on screen
                int kbs = (int)(receivedBytes);
                devInfo.Texts[1].Text = ("Net received: " + kbs + "B/s");
                kbs = (int)(sentBytes);
                devInfo.Texts[2].Text = ("Net Sent: " + kbs + "B/s");
                lastInfoUpdate = GameTime.Ticks;
                receivedBytes = 0;
                sentBytes = 0;

                if (World.Current != null && character != null)
                {
                    devInfo.Texts[3].Text = "Pos: " + character.GetPosition();
                    devInfo.Texts[4].Text = "Dir: " + character.GetDirection();
                }
            }

            devInfo.Show();
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
            Client.sentBytes += stream.GetLength();
        }
    }
}
