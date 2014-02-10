using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using GMP.Logger;
using GMP.Injection;
using Network;
using Injection;
using GMP.Net.Messages;
using System.Windows.Forms;
using Gothic.zClasses;
using GMP.Modules;
using Gothic.zTypes;
using WinApi;
using System.Threading;
using GMP.Helper;
using GMP.Network.Messages;

namespace GMP.Net
{
    public class Client
    {
        public RakPeerInterface client;
        public bool isConnected;
        public Dictionary<byte, Message> messageListener = new Dictionary<byte, Message>();
        BitStream receiveBitStream = new BitStream();
        public BitStream sentBitStream = new BitStream();


        private zCViewText connectionView;

        public Client()
        {
            client = RakPeer.GetInstance();
            messageListener.Add((byte)NetWorkIDS.ConnectionRequest, new ConnectionRequest());
            messageListener.Add((byte)NetWorkIDS.DisconnectedMessage, new DisconnectedMessage());
            messageListener.Add((byte)NetWorkIDS.NewPlayerMessage, new NewPlayerMessage());
            messageListener.Add((byte)NetWorkIDS.PlayerListRequest, new PlayerListRequest());
            messageListener.Add((byte)NetWorkIDS.PlayerStatusMessage, new PlayerStatusMessage());

            messageListener.Add((byte)NetWorkIDS.PlayerStatusMessage2, new PlayerStatusMessage2());
            messageListener.Add((byte)NetWorkIDS.AnimationMessage, new AnimationMessage());
            messageListener.Add((byte)NetWorkIDS.AllPlayerSynchMessage, new AllPlayerSynchMessage());
            messageListener.Add((byte)NetWorkIDS.WeaponModeMessage, new WeaponModeMessage());
            messageListener.Add((byte)NetWorkIDS.ItemSynchro_DoDrop, new ItemSynchro_DoDrop());
            messageListener.Add((byte)NetWorkIDS.MobSynch, new MobSynch());
            messageListener.Add((byte)NetWorkIDS.Status, new StatusMessage());
            messageListener.Add((byte)NetWorkIDS.DownloadModulesMessage, new DownloadModulesMessage());
            messageListener.Add((byte)NetWorkIDS.LevelChangeMessage, new LevelChangeMessage());
            messageListener.Add((byte)NetWorkIDS.AnimationOverlayMessage, new AnimationOverlayMessage());
            messageListener.Add((byte)NetWorkIDS.VisualSynchro_SetAsPlayer, new VisualSynchro_SetAsPlayer());
            messageListener.Add((byte)NetWorkIDS.MagSetupSynch, new MagSetupMessage());
            messageListener.Add((byte)NetWorkIDS.TimeMessage, new TimeMessage());
            messageListener.Add((byte)NetWorkIDS.StaticNPCMessage, new StaticNPCMessage());
            messageListener.Add((byte)NetWorkIDS.NPCControllerMessage, new NPCControllerMessage());

            messageListener.Add((byte)NetWorkIDS.SpawnNPCMessage, new SpawnNPCMessage());
            messageListener.Add((byte)NetWorkIDS.PassivePerceptionMessage, new PassivePerceptionMessage());
            messageListener.Add((byte)NetWorkIDS.AttackSynchMessage, new AttackSynchMessage());
            
            messageListener.Add((byte)NetWorkIDS.FriendMessage, new FriendMessage());
            //messageListener.Add((byte)NetWorkIDS.AssessPlayerMessage, new AssessPlayerMessage());//Laggy but why? Dunno!
            messageListener.Add((byte)NetWorkIDS.SoundSynch, new OutputSynchMessage());
            messageListener.Add((byte)NetWorkIDS.AssessTalkMessage, new AssessTalkMessage());
            messageListener.Add((byte)NetWorkIDS.LevelDataMessage, new LevelDataMessage());
            messageListener.Add((byte)NetWorkIDS.ItemStealMessage, new ItemStealMessage());

            messageListener.Add((byte)NetWorkIDS.StartLevelChangeMessage, new StartLevelChangeMessage());

            messageListener.Add((byte)NetWorkIDS.CommandoMessage, new CommandoMessage());
        }

        public void Startup()
        {
            SocketDescriptor socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = 0;

            bool started = client.Startup(1, socketDescriptor, 1) == StartupResult.RAKNET_STARTED;
            isConnected = false;
        }

        String ip, pw; ushort port;
        private static long lastConnectionTry = 0;
        private static int connectionTrys = 0;
        public void Connect()
        {
            Connect(ip, port, pw); ;
        }

        public void Connect(String ip, ushort port, String pw)
        {
            if (isConnected || lastConnectionTry + 10000 * 100 > DateTime.Now.Ticks)
                return;
            if(connectionTrys >= 100)
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Verbindung nicht möglich!", 0, "Client.cs", 0);
            this.ip = ip; this.port = port; this.pw = pw;
            bool b;
            pw = "ver0.17" + pw;
            b = client.Connect(ip, port, pw, pw.Length) == ConnectionAttemptResult.CONNECTION_ATTEMPT_STARTED;
            client.SetOccasionalPing(true);
            if (!b)
            {
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

        private void ShowPing()
        {
            
            if (!isConnected)
                return;
            Process Process = Process.ThisProcess();
            
            //if (connectionView == null)
            //{
            //    connectionView = StaticVars.printView.CreateText(0, 0, zString.Create(Process, ""));
            //    connectionView.Timed = 0;
            //    connectionView.Timer = -1;
            //}

            //connectionView.Text.Set("" + client.GetLastPing(client.GetSystemAddressFromIndex(0)));
            
            
        }

        public void Update()
        {
            ShowPing();

            if (client == null)
                return;

            Packet packet = client.Receive();
            //if(packet != null)
            int counter = 0;
            while (packet != null)
            {

                WinApi.Kernel.Process.SetWindowText(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle, "Gothic II - Untold Chapters - " + client.GetLastPing(client.GetSystemAddressFromIndex(0)));
                //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', WindowProcess.GetWindowText() + " Gothic II - Untold Chapters - " + client.GetLastPing(client.GetSystemAddressFromIndex(0)), 0, "Client.cs", 0);

                switch (packet.data[0])
                {
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_REQUEST_ACCEPTED:
                        isConnected = true;
                        connectionTrys = 0;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_ATTEMPT_FAILED:
                        ErrorLog.Log(typeof(Client), "Connection Failed!");
                        //connectionView.Text.Set("Connection Failed!");
                        isConnected = false;
                        break;
                    case (byte)DefaultMessageIDTypes.ID_ALREADY_CONNECTED:
                        ErrorLog.Log(typeof(Client), "Already Connected!");
                        break;
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_BANNED:
                        ErrorLog.Log(typeof(Client), "Client banned!");
                        //connectionView.Text.Set("Banned!");
                        MessageFunc.ShowAlreadyBanned();
                        break;
                    case (byte)DefaultMessageIDTypes.ID_INVALID_PASSWORD:
                        ErrorLog.Log(typeof(Client), "Wrong password");
                        //connectionView.Text.Set("Wrong Password!");
                        MessageFunc.ShowInvalidPassword();
                        break;
                    case (byte)DefaultMessageIDTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
                        ErrorLog.Log(typeof(Client), "ID_INCOMPATIBLE_PROTOCOL_VERSION");
                        MessageFunc.ShowInvalidPassword();
                        break;
                    case (byte)DefaultMessageIDTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
                        MessageFunc.ShowServerFull();
                        break;
                    case (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST:
                        {
                            Player[] plList = Program.playerList.ToArray();
                            foreach (Player pl in plList)
                            {
                                if (pl.isPlayer)
                                    continue;
                                NPCHelper.RemovePlayer(pl, true);
                            }
                            if (Program.chatBox != null)
                            {
                                for (byte i = 0; i < Program.chatBox.maxType; i++)
                                    Program.chatBox.addRow(i, "Connection lost...");
                            }
                            isConnected = false;
                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                        try
                        {
                            receiveBitStream.Reset();
                            receiveBitStream.Write(packet.data, packet.length);
                            receiveBitStream.IgnoreBytes(2);
                            if (messageListener.ContainsKey(packet.data[1]))
                                messageListener[packet.data[1]].Read(receiveBitStream, packet, this);
                        }
                        catch (Exception ex) { zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.Message+ex.StackTrace, 0, "Client.cs", 0); }
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
    }
}
