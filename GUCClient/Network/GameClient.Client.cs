using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Types;
using GUC.Enumeration;
using GUC.Log;
using GUC.GUI;
using GUC.Network.Messages;
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

        #region Guided vobs

        internal HashSet<int> guidedIDs = new HashSet<int>();

        #endregion
        
        #region Spectator

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir)
        {
            var cam = oCGame.GetCameraVob();
            cam.SetAI(specCam);

            cam.SetPositionWorld(pos.X, pos.Y, pos.Z);
            using (var vec = Gothic.Types.zVec3.Create(dir.X, dir.Y, dir.Z))
                cam.SetHeadingAtWorld(vec);
            this.isSpectating = true;
            this.character = null;
        }

        oCAICamera specCam = oCAICamera.Create();
        void ReadSpectatorMessage(PacketReader stream)
        {
            Vec3f pos = stream.ReadVec3f();
            Vec3f dir = stream.ReadVec3f();

            this.ScriptObject.SetToSpectator(World.current, pos, dir);
        }

        Vec3f lastSpecPos;
        static long specNextUpdate = 0;
        const long SpecPosUpdateInterval = 2000000;
        internal void UpdateSpectator(long now)
        {
            if (now < specNextUpdate)
                return;

            var cam = oCGame.GetCameraVob();
            var pos = new Vec3f(cam.Position).CorrectPosition();
            cam.SetPositionWorld(pos.X, pos.Y, pos.Z);

            if (now - specNextUpdate < TimeSpan.TicksPerSecond && pos.GetDistance(lastSpecPos) < 10)
                return;

            lastSpecPos = pos;

            PacketWriter stream = SetupStream(NetworkIDs.SpecPosMessage);
            stream.WriteCompressedPosition(pos);
            Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);

            specNextUpdate = now + SpecPosUpdateInterval;
        }

        #endregion

        #region Commands

        MoveState nextState = MoveState.Stand;
        const int DelayBetweenMessages = 800000; //80ms
        public void DoSetHeroState(MoveState state)
        {
            if (this.character == null)
                return;

            if (this.character.IsDead)
                return;

            MoveState s = state;

            if (s == MoveState.Forward)
            {
                if (!character.gVob.AniCtrl.CheckEnoughSpaceMoveForward(false))
                    s = MoveState.Stand;
            }
            else if (s == MoveState.Backward)
            {
                if (!character.gVob.AniCtrl.CheckEnoughSpaceMoveBackward(false))
                    s = MoveState.Stand;
            }
            else if (s == MoveState.Left)
            {
                if (!character.gVob.AniCtrl.CheckEnoughSpaceMoveLeft(false))
                    s = MoveState.Stand;
            }
            else if (s == MoveState.Right)
            {
                if (!character.gVob.AniCtrl.CheckEnoughSpaceMoveRight(false))
                    s = MoveState.Stand;
            }

            if (this.nextState == s)
                return;

            this.nextState = s;
            this.character.nextStateUpdate = 0;
            UpdateHeroState(GameTime.Ticks);
        }

        internal void UpdateHeroState(long now)
        {
            if (this.character == null)
                return;

            if (this.character.IsDead)
                return;

            if (this.character.Movement == nextState)
                return;

            if (now < this.character.nextStateUpdate)
                return;

            NPCMessage.WriteMoveState(this.character, nextState);
            this.character.nextStateUpdate = now + DelayBetweenMessages;
        }

        long nextAniUpdate = 0;
        public void DoStartAni(AniJob job)
        {
            if (this.character == null || job == null)
                return;

            if (this.character.IsDead)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                NPCMessage.WriteAniStart(job);
                nextAniUpdate = GameTime.Ticks + DelayBetweenMessages;
            }
        }

        public void DoStartAni(AniJob job, params object[] netArgs)
        {
            if (this.character == null || job == null)
                return;

            if (this.character.IsDead)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                NPCMessage.WriteAniStart(job, netArgs);
                nextAniUpdate = GameTime.Ticks + DelayBetweenMessages;
            }
        }

        public void DoEquipItem(int slot, Item item)
        {
            if (this.character == null || item == null)
                return;

            if (slot < 0 || slot >= Item.SlotNumUnused)
                return;

            if (this.character.IsDead || item.Container != this.character)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                InventoryMessage.WriteEquipMessage(slot, item);
                nextAniUpdate = GameTime.Ticks + DelayBetweenMessages;
            }
        }

        public void DoUnequipItem(Item item)
        {
            if (this.character == null || item == null)
                return;

            if (this.character.IsDead || item.Container != this.character)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                InventoryMessage.WriteUnequipMessage(item);
                nextAniUpdate = GameTime.Ticks + DelayBetweenMessages;
            }
        }

        public void DoSetFightMode(bool fightMode)
        {
            if (this.character == null)
                return;

            if (this.character.IsDead || this.character.IsInFightMode == fightMode)
                return;

            if (GameTime.Ticks > nextAniUpdate)
            {
                if (fightMode)
                {
                    NPCMessage.WriteSetFightModeMessage();
                }
                else
                {
                    NPCMessage.WriteUnsetFightModeMessage();
                }
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

            Logger.Log("Taking control of npc " + npc.ID);
            this.ScriptObject.SetControl(npc);
        }

        partial void pSetControl(NPC npc)
        {
            // Remove the gothic hero if he's there
            oCNpc dummyHero = oCNpc.GetPlayer();
            if (!World.Current.ContainsVobAddress(dummyHero.Address))
            {
                dummyHero.Disable();
                oCGame.GetWorld().RemoveVob(dummyHero);
            }

            this.character = npc;
            Character.gVob.SetAsPlayer();
            oCGame.GetCameraVob().SetAI(oCGame.GetCameraAI());
            this.isSpectating = false;
        }

        #endregion

        RakPeerInterface clientInterface = null;
        SocketDescriptor socketDescriptor = null;
        bool isConnected = false; public bool IsConnected { get { return isConnected; } }

        /// <summary>
        /// Is true when we've received a disconnect message (f.e. from a kick). No further connection attempts are then started.
        /// </summary>
        bool isDisconnected = false; public bool IsDisconnected { get { return this.isDisconnected; } }

        PacketReader pktReader = new PacketReader();
        PacketWriter pktWriter = new PacketWriter(128000);

        void ReadMessage(NetworkIDs id, PacketReader stream)
        {
            if (id != NetworkIDs.VobPosDirMessage)
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

                // Spectator Messages
                case NetworkIDs.SpectatorMessage:
                    this.ReadSpectatorMessage(stream);
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
                case NetworkIDs.WorldCellMessage:
                    WorldMessage.ReadCellMessage(stream);
                    break;
                case NetworkIDs.WorldJoinMessage:
                    WorldMessage.ReadJoinWorldMessage(stream);
                    break;
                case NetworkIDs.WorldLeaveMessage:
                    WorldMessage.ReadLeaveWorldMessage(stream);
                    break;
                case NetworkIDs.WorldSpawnMessage:
                    WorldMessage.ReadVobSpawnMessage(stream);
                    break;
                case NetworkIDs.WorldDespawnMessage:
                    WorldMessage.ReadVobDespawnMessage(stream);
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
                case NetworkIDs.InventoryAmountMessage:
                    InventoryMessage.ReadChangeItemAmount(stream);
                    break;
                case NetworkIDs.InventoryEquipMessage:
                    InventoryMessage.ReadEquipMessage(stream);
                    break;
                case NetworkIDs.InventoryUnequipMessage:
                    InventoryMessage.ReadUnequipMessage(stream);
                    break;

                // NPC Messages
                case NetworkIDs.NPCStateMessage:
                    NPCMessage.ReadMoveState(stream);
                    break;

                case NetworkIDs.NPCEquipMessage:
                    NPCMessage.ReadEquipMessage(stream);
                    break;
                case NetworkIDs.NPCEquipSwitchMessage:
                    NPCMessage.ReadEquipSwitchMessage(stream);
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
                case NetworkIDs.NPCAniStartWithArgsMessage:
                    NPCMessage.ReadAniStartWithArgs(stream);
                    break;
                case NetworkIDs.NPCAniStopMessage:
                    NPCMessage.ReadAniStop(stream);
                    break;

                case NetworkIDs.NPCHealthMessage:
                    NPCMessage.ReadHealthMessage(stream);
                    break;

                case NetworkIDs.NPCSetFightModeMessage:
                    NPCMessage.ReadSetFightModeMessage(stream);
                    break;
                case NetworkIDs.NPCUnsetFightModeMessage:
                    NPCMessage.ReadUnsetFightModeMessage(stream);
                    break;

                case NetworkIDs.GuideAddMessage:
                    GuideMessage.ReadGuideAddMessage(stream);
                    break;
                case NetworkIDs.GuideAddCmdMessage:
                    GuideMessage.ReadGuideAddCmdMessage(stream);
                    break;
                case NetworkIDs.GuideRemoveMessage:
                    GuideMessage.ReadGuideRemoveMessage(stream);
                    break;
                case NetworkIDs.GuideSetCmdMessage:
                    GuideMessage.ReadGuideSetCmdMessage(stream);
                    break;
                case NetworkIDs.GuideRemoveCmdMessage:
                    GuideMessage.ReadGuideRemoveCmdMessage(stream);
                    break;

                default:
                    Logger.LogWarning("Received message with invalid NetworkID! " + id);
                    break;
            }
        }

        private GameClient()
        {
            clientInterface = RakPeer.GetInstance();
            clientInterface.SetOccasionalPing(true);

            socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = 0;

            if (clientInterface.Startup(1, socketDescriptor, 1) != StartupResult.RAKNET_STARTED)
            {
                Logger.LogError("RakNet failed to start!");
            }
        }

        bool isConnecting = false; public bool IsConnecting { get { return this.isConnecting; } }
        int connectionAttempts = 0; public int ConnectionAttempts { get { return this.connectionAttempts; } }
        internal void Connect()
        {
            try
            {
                if (this.isConnected || this.isConnecting)
                    return;

                this.isConnecting = true;
                

                ConnectionAttemptResult res = clientInterface.Connect(Program.ServerIP, Program.ServerPort, Program.Password, Program.Password == null ? 0 : Program.Password.Length);
                
                if (res != ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED)
                {
                    throw new Exception("Connection couldn't be established: " + res);
                }

                Logger.Log("Connection attempt {0} to '{1}:{2}' started.", ++connectionAttempts, Program.ServerIP, Program.ServerPort);
            }
            catch (Exception e)
            {
                Logger.LogError("Verbindungsaufbau fehlgeschlagen! " + e);
            }
        }

        public void Disconnect()
        {
            this.clientInterface.CloseConnection(clientInterface.GetSystemAddressFromIndex(0), true, 0);
            this.isConnected = false;
            this.isConnecting = false;
            this.isDisconnected = true;
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

                for (int pos = 0; pos < 0x2000; pos += devInfo.zView.FontY() + 5)
                {
                    var t = devInfo.CreateText("", 0x2000, pos, true);
                    t.Format = GUCVisualText.TextFormat.Right;
                }
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
                            this.isConnected = true;
                            this.isConnecting = false;
                            this.connectionAttempts = 0;
                            ConnectionMessage.Write();
                            ScriptManager.Interface.OnClientConnection(this);
                            break;
                        case DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                            this.isConnected = false;
                            this.isConnecting = false;
                            break;
                        case DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                            Logger.LogWarning("Already Connected!");
                            return;
                        case DefaultMessageIDTypes.ID_CONNECTION_BANNED:
                            Logger.LogError("Client banned!");
                            return;
                        case DefaultMessageIDTypes.ID_INVALID_PASSWORD:
                            Logger.LogError("Wrong password");
                            return;
                        case DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
                            Logger.LogError("ID_INCOMPATIBLE_PROTOCOL_VERSION");
                            return;
                        case DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                            Logger.LogError("ID_NO_FREE_INCOMING_CONNECTIONS");
                            return;
                        case DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                        case DefaultMessageIDTypes.ID_CONNECTION_LOST:
                            this.isConnected = false;
                            this.isConnecting = false;
                            this.isDisconnected = true;
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
                if (this.isDisconnected)
                {
                    abortInfo.Texts[0].Text = "Verbindung geschlossen!";
                    abortInfo.Show();
                }
                else if (this.isConnected)
                {
                    if (ping > 300 || ping < 0)
                    {
                        abortInfo.Show();
                    }
                    else
                    {
                        abortInfo.Hide();
                    }
                }

                // update ping text on screen
                int devIndex = 0;
                GUCVisualText pingText = devInfo.Texts[devIndex++];
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
                devInfo.Texts[devIndex++].Text = ("Net received: " + kbs + "B/s");
                kbs = (int)(sentBytes);
                devInfo.Texts[devIndex++].Text = ("Net Sent: " + kbs + "B/s");
                lastInfoUpdate = GameTime.Ticks;
                receivedBytes = 0;
                sentBytes = 0;

                if (World.Current != null)
                {
                    devIndex++;
                    devInfo.Texts[devIndex++].Text = World.current.VobCount + " Vobs";
                    devInfo.Texts[devIndex++].Text = guidedIDs.Count + " guided";

                    if (character != null)
                    {
                        devInfo.Texts[devIndex++].Text = "Pos: " + character.GetPosition();
                        devInfo.Texts[devIndex++].Text = "Dir: " + character.GetDirection();
                    }
                    else
                    {
                        devInfo.Texts[devIndex++].Text = "Pos: " + new Vec3f(oCGame.GetCameraVob().TrafoObjToWorld.Position);
                        devInfo.Texts[devIndex++].Text = "Dir: " + new Vec3f(oCGame.GetCameraVob().TrafoObjToWorld.Direction);
                    }
                    devInfo.Texts[devIndex++].Text = "Weather: " + World.current.SkyCtrl.CurrentWeight + " " + World.current.Clock.Time.ToString(false);

                    if (character != null)
                    {
                        devInfo.Texts[devIndex++].Text = character.Movement.ToString();
                        devInfo.Texts[devIndex++].Text = character.EnvState.ToString();
                    }

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
