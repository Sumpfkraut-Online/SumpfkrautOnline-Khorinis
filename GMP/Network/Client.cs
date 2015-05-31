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

            messageListener.Add((byte)NetworkID.ConnectionMessage, new ConnectionMessage());
            messageListener.Add((byte)NetworkID.CreateItemInstanceMessage, new CreateItemInstanceMessage());
            messageListener.Add((byte)NetworkID.PlayerFreezeMessage, new PlayerFreezeMessage());
            messageListener.Add((byte)NetworkID.AddItemMessage, new AddItemMessage());
            messageListener.Add((byte)NetworkID.StartDialogAnimMessage, new StartDialogAnimMessage());
            messageListener.Add((byte)NetworkID.AnimationMessage, new AnimationMessage());
            messageListener.Add((byte)NetworkID.NPCChangeAttributeMessage, new NPCChangeAttributeMessage());
            messageListener.Add((byte)NetworkID.NPCChangeSkillMessage, new NPCChangeSkillMessage());

            messageListener.Add((byte)NetworkID.NPCSpawnMessage, new NPCSpawnMessage());
            messageListener.Add((byte)NetworkID.GuiMessage, new GuiMessage());

            messageListener.Add((byte)NetworkID.DropUnconsciousMessage, new DropUnconsciousMessage());
            messageListener.Add((byte)NetworkID.ReviveMessage, new ReviveMessage());
            messageListener.Add((byte)NetworkID.SetVisualMessage, new SetVisualMessage());
            messageListener.Add((byte)NetworkID.SetVobChangeMessage, new SetVobChangeMessage());
            messageListener.Add((byte)NetworkID.DropItemMessage, new DropItemMessage());
            messageListener.Add((byte)NetworkID.TakeItemMessage, new TakeItemMessage());
            messageListener.Add((byte)NetworkID.CamToVobMessage, new CamToVobMessage());
            messageListener.Add((byte)NetworkID.ChangeNameMessage, new ChangeNameMessage());

            messageListener.Add((byte)NetworkID.CreateVobMessage, new CreateVobMessage());
            messageListener.Add((byte)NetworkID.SpawnVobMessage, new SpawnVobMessage());
            messageListener.Add((byte)NetworkID.DespawnVobMessage, new DespawnVobMessage());

            messageListener.Add((byte)NetworkID.SetVobPositionMessage, new SetVobPositionMessage());
            messageListener.Add((byte)NetworkID.SetVobDirectionMessage, new SetVobDirectionMessage());
            messageListener.Add((byte)NetworkID.SetVobPosDirMessage, new SetVobPosDirMessage());

            messageListener.Add((byte)NetworkID.DisconnectMessage, new DisconnectMessage());
            messageListener.Add((byte)NetworkID.AnimationUpdateMessage, new AnimationUpdateMessage());

            messageListener.Add((byte)NetworkID.NPCUpdateMessage, new NPCUpdateMessage());

            messageListener.Add((byte)NetworkID.ItemChangeAmount, new ItemChangeAmount());

            messageListener.Add((byte)NetworkID.ItemChangeContainer, new ItemChangeContainer());

            messageListener.Add((byte)NetworkID.ClearInventory, new ClearInventory());

            messageListener.Add((byte)NetworkID.TimerMessage, new TimerMessage());
            messageListener.Add((byte)NetworkID.RainMessage, new RainMessage());

            messageListener.Add((byte)NetworkID.CallbackNPCCanSee, new CallbackNPCCanSee());

            messageListener.Add((byte)NetworkID.ExitGameMessage, new ExitGameMessage());

            messageListener.Add((byte)NetworkID.ReadIniEntryMessage, new ReadIniEntryMessage());
            messageListener.Add((byte)NetworkID.ReadMd5Message, new ReadMd5Message());

            messageListener.Add((byte)NetworkID.EquipItemMessage, new EquipItemMessage());
            messageListener.Add((byte)NetworkID.ChangeWorldMessage, new ChangeWorldMessage());

            messageListener.Add((byte)NetworkID.MobInterMessage, new MobInterMessage());

            messageListener.Add((byte)NetworkID.NPCSetInvisibleMessage, new NPCSetInvisibleMessage());
            messageListener.Add((byte)NetworkID.NPCSetInvisibleName, new NPCSetInvisibleName());
            messageListener.Add((byte)NetworkID.PlayVideo, new PlayVideo());

            messageListener.Add((byte)NetworkID.NPCControllerMessage, new NPCControllerMessage());

            messageListener.Add((byte)NetworkID.ScaleMessage, new ScaleMessage());
            messageListener.Add((byte)NetworkID.NPCFatnessMessage, new NPCFatnessMessage());


            messageListener.Add((byte)NetworkID.NPCEnableMessage, new NPCEnableMessage());
            messageListener.Add((byte)NetworkID.CreateSpellMessage, new CreateSpellMessage());

            messageListener.Add((byte)NetworkID.PlayEffectMessage, new PlayEffectMessage());

            messageListener.Add((byte)NetworkID.CastSpell, new CastSpellMessage());
            messageListener.Add((byte)NetworkID.SpellInvestMessage, new SpellInvestMessage());

            messageListener.Add((byte)NetworkID.NPCProtoSetWeaponMode, new NPCProtoSetWeaponMode());
            messageListener.Add((byte)NetworkID.SetSlotMessage, new SetSlotMessage());

            messageListener.Add((byte)NetworkID.CamToPlayerFront, new CamToPlayerFront());

            messageListener.Add((byte)NetworkID.InterfaceOptionsMessage, new InterfaceOptionsMessage());

            messageListener.Add((byte)NetworkID.PlayerOpenInventoryMessage, new OpenInventoryMessage());
            
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
            pw = GUC.Options.Constants.VERSION + pw;
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
                connectionView = zCView.Create(process, 0xB00, 0x500, 0x1500, 0x700);
                zString str = zString.Create(process, "Connection aborted!");
                zCViewText vt = connectionView.CreateText((0x2000 - connectionView.FontSize(str))/2, (0x2000 - connectionView.FontY())/2, str);
                str.Set("Menu_Choice_Back.tga");
                connectionView.InsertBack(str);
                str.Dispose();
                vt.Color.G = 0;
                vt.Color.B = 0;
                vt.Color.R = 255;
                vt.Color.A = 255;
            }

            if (!shown)
            {
                if (client.GetLastPing(client.GetSystemAddressFromIndex(0)) > 1000 || client.GetLastPing(client.GetSystemAddressFromIndex(0)) <= -1)
                {
                    zCView.GetStartscreen(Process.ThisProcess()).InsertItem(connectionView, 1);
                    shown = true;
                }
            }
            else if (shown)
            {
                if (client.GetLastPing(client.GetSystemAddressFromIndex(0)) < 700 && client.GetLastPing(client.GetSystemAddressFromIndex(0)) > -1)
                {
                    zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(connectionView);
                    shown = false;
                }
                else
                {   //always on top
                    zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(connectionView);
                    zCView.GetStartscreen(Process.ThisProcess()).InsertItem(connectionView, 1);
                }
            }

            int counter = 0;
            Packet packet = client.Receive();

            while (packet != null)
            {
                //WinApi.Kernel.Process.SetWindowText(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle, "Gothic II - Untold Chapters - " + client.GetLastPing(client.GetSystemAddressFromIndex(0)));

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
