using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using Gothic.zClasses;
using WinApi;
using GUC.Enumeration;
using GUC.Network.Messages.Connection;
using GUC.Network.Messages.PlayerCommands;
using GUC.Network.Messages.ContainerCommands;
using GUC.Network.Messages;
using GUC.Network.Messages.CameraCommands;
using GUC.Network.Messages.VobCommands;
using GUC.WorldObjects.Character;
using GUC.Network.Messages.NpcCommands;
using GUC.Network.Messages.ItemCommands;
using GUC.Network.Messages.WorldCommands;
using GUC.Network.Messages.Callbacks;
using Gothic.zTypes;
using GUC.Network.Messages.MobInterCommands;

namespace GUC.Network
{
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
        public BitStream sentBitStream = new BitStream();

        public Dictionary<byte, IMessage> messageListener = new Dictionary<byte, IMessage>();

        public Client()
        {
            client = RakPeer.GetInstance();

            messageListener.Add((byte)NetworkIDS.ConnectionMessage, new ConnectionMessage());
            messageListener.Add((byte)NetworkIDS.CreateItemInstanceMessage, new CreateItemInstanceMessage());
            messageListener.Add((byte)NetworkIDS.PlayerFreezeMessage, new PlayerFreezeMessage());
            messageListener.Add((byte)NetworkIDS.AddItemMessage, new AddItemMessage());
            messageListener.Add((byte)NetworkIDS.StartDialogAnimMessage, new StartDialogAnimMessage());
            messageListener.Add((byte)NetworkIDS.AnimationMessage, new AnimationMessage());
            messageListener.Add((byte)NetworkIDS.NPCChangeAttributeMessage, new NPCChangeAttributeMessage());
            messageListener.Add((byte)NetworkIDS.NPCChangeSkillMessage, new NPCChangeSkillMessage());

            messageListener.Add((byte)NetworkIDS.NPCSpawnMessage, new NPCSpawnMessage());
            messageListener.Add((byte)NetworkIDS.GuiMessage, new GuiMessage());

            messageListener.Add((byte)NetworkIDS.DropUnconsciousMessage, new DropUnconsciousMessage());
            messageListener.Add((byte)NetworkIDS.ReviveMessage, new ReviveMessage());
            messageListener.Add((byte)NetworkIDS.SetVisualMessage, new SetVisualMessage());
            messageListener.Add((byte)NetworkIDS.DropItemMessage, new DropItemMessage());
            messageListener.Add((byte)NetworkIDS.TakeItemMessage, new TakeItemMessage());
            messageListener.Add((byte)NetworkIDS.CamToVobMessage, new CamToVobMessage());
            messageListener.Add((byte)NetworkIDS.ChangeNameMessage, new ChangeNameMessage());

            messageListener.Add((byte)NetworkIDS.CreateVobMessage, new CreateVobMessage());
            messageListener.Add((byte)NetworkIDS.SpawnVobMessage, new SpawnVobMessage());

            messageListener.Add((byte)NetworkIDS.SetVobPositionMessage, new SetVobPositionMessage());
            messageListener.Add((byte)NetworkIDS.SetVobDirectionMessage, new SetVobDirectionMessage());
            messageListener.Add((byte)NetworkIDS.SetVobPosDirMessage, new SetVobPosDirMessage());

            messageListener.Add((byte)NetworkIDS.DisconnectMessage, new DisconnectMessage());
            messageListener.Add((byte)NetworkIDS.AnimationUpdateMessage, new AnimationUpdateMessage());

            messageListener.Add((byte)NetworkIDS.NPCUpdateMessage, new NPCUpdateMessage());

            messageListener.Add((byte)NetworkIDS.ItemChangeAmount, new ItemChangeAmount());

            messageListener.Add((byte)NetworkIDS.ItemChangeContainer, new ItemChangeContainer());

            messageListener.Add((byte)NetworkIDS.ClearInventory, new ClearInventory());

            messageListener.Add((byte)NetworkIDS.TimerMessage, new TimerMessage());
            messageListener.Add((byte)NetworkIDS.RainMessage, new RainMessage());

            messageListener.Add((byte)NetworkIDS.CallbackNPCCanSee, new CallbackNPCCanSee());

            messageListener.Add((byte)NetworkIDS.ExitGameMessage, new ExitGameMessage());

            messageListener.Add((byte)NetworkIDS.ReadIniEntryMessage, new ReadIniEntryMessage());
            messageListener.Add((byte)NetworkIDS.ReadMd5Message, new ReadMd5Message());

            messageListener.Add((byte)NetworkIDS.EquipItemMessage, new EquipItemMessage());
            messageListener.Add((byte)NetworkIDS.ChangeWorldMessage, new ChangeWorldMessage());

            messageListener.Add((byte)NetworkIDS.MobInterMessage, new MobInterMessage());

            messageListener.Add((byte)NetworkIDS.NPCSetInvisibleMessage, new NPCSetInvisibleMessage());
            messageListener.Add((byte)NetworkIDS.NPCSetInvisibleName, new NPCSetInvisibleName());
            messageListener.Add((byte)NetworkIDS.PlayVideo, new PlayVideo());

            messageListener.Add((byte)NetworkIDS.NPCControllerMessage, new NPCControllerMessage());

            messageListener.Add((byte)NetworkIDS.ScaleMessage, new ScaleMessage());
            messageListener.Add((byte)NetworkIDS.NPCFatnessMessage, new NPCFatnessMessage());


            messageListener.Add((byte)NetworkIDS.NPCEnableMessage, new NPCEnableMessage());
            messageListener.Add((byte)NetworkIDS.CreateSpellMessage, new CreateSpellMessage());

            messageListener.Add((byte)NetworkIDS.PlayEffectMessage, new PlayEffectMessage());

            messageListener.Add((byte)NetworkIDS.CastSpell, new CastSpellMessage());
            messageListener.Add((byte)NetworkIDS.SpellInvestMessage, new SpellInvestMessage());

            messageListener.Add((byte)NetworkIDS.NPCProtoSetWeaponMode, new NPCProtoSetWeaponMode());
            messageListener.Add((byte)NetworkIDS.SetSlotMessage, new SetSlotMessage());

            messageListener.Add((byte)NetworkIDS.CamToPlayerFront, new CamToPlayerFront());
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
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Verbindung nicht möglich!", 0, "Client.cs", 0);
            this.ip = ip; this.port = port; this.pw = pw;
            bool b;
            pw = "ver2.06" + pw;
            b = client.Connect(ip, port, pw, pw.Length) == ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED;
            client.SetOccasionalPing(true);
            if (!b)
            {
                zCView.GetStartscreen(Process.ThisProcess()).PrintTimedCXY("The connection could not be estabilshed", 10, 255, 0, 0, 255);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Es konnte keine Verbindung aufgebaut werden.", 0, "Client.cs", 0);
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

        zCView connectionView = null;
        bool shown = false;
        public void Update()
        {
            if (client == null)
                return;

            if (connectionView == null)
            {
                Process process = Process.ThisProcess();
                connectionView = zCView.Create(process, 0, 0, 0x2000, 0x2000);
                zString str = zString.Create(process, "Connection aborted!");
                zCViewText vt = connectionView.CreateText(0x950, 0x800, str);
                str.Dispose();
                vt.Color.G = 0;
                vt.Color.B = 0;
                vt.Color.R = 255;
                vt.Color.A = 255;

                vt.Timed = 0;
                vt.Timer = -1;
            }

            if (!shown && (client.GetLastPing(client.GetSystemAddressFromIndex(0)) > 1000 || client.GetLastPing(client.GetSystemAddressFromIndex(0)) <= -1))
            {
                zCView.GetStartscreen(Process.ThisProcess()).InsertItem(connectionView, 0);
                shown = true;
            }
            else if (shown && (client.GetLastPing(client.GetSystemAddressFromIndex(0)) < 700 && client.GetLastPing(client.GetSystemAddressFromIndex(0)) > -1))
            {
                zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(connectionView);
                shown = false;
            }

            int counter = 0;
            Packet packet = client.Receive();


            while (packet != null)
            {
                WinApi.Kernel.Process.SetWindowText(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle, "Gothic II - Untold Chapters - " + client.GetLastPing(client.GetSystemAddressFromIndex(0)));

                switch (packet.data[0])
                {
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                        isConnected = true;
                        connectionTrys = 0;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                        logError( "Connection Failed!" );
                        isConnected = false;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                        logError( "Already Connected!" );
                        break;
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_BANNED:
                        logError( "Client banned!" );
                        break;
                    case (byte)DefaultMessageIDTypes.ID_INVALID_PASSWORD:
                        logError( "Wrong password" );
                        break;
                    case (byte)DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
                        logError( "ID_INCOMPATIBLE_PROTOCOL_VERSION" );
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
                            if ((Player.Hero == null || Player.Hero.ID == 0) && packet.data[1] != (byte)NetworkIDS.ConnectionMessage)
                                break;
                            if (messageListener.ContainsKey(packet.data[1]))
                                messageListener[packet.data[1]].Read(receiveBitStream, packet, this);
                        }
                        catch (Exception ex) { zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.Message + ex.StackTrace, 0, "Client.cs", 0); }
                        break;
                }

                client.DeallocatePacket(packet);


                counter++;
                if (counter >= 1000)
                {
                    counter = 0;
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "1000 Packete hintereinander", 0, "Client.cs", 0);
                }

                packet = client.Receive();
            }
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
                    if (sentBitStream != null)
                        sentBitStream.Dispose();
                }

                client = null;
                socketDescriptor = null;
                receiveBitStream = null;
                sentBitStream = null;


                _disposed = true;
            }
        }
        #endregion
    }
}
