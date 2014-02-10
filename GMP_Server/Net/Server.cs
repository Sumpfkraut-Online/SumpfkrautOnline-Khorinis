using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using GMP_Server.Net.Message;

namespace GMP_Server.Net
{
    public class Server
    {
        public RakPeerInterface server;
        public Dictionary<byte, Message.Message> messageListener = new Dictionary<byte, Message.Message>();
        public BitStream receiveBitStream = new BitStream();
        public BitStream sendBitStream = new BitStream();
        public Server()
        {
            server = RakPeer.GetInstance();

            messageListener.Add((byte)NetWorkIDS.ConnectionRequest, new ConnectionRequest());
            messageListener.Add((byte)NetWorkIDS.PlayerListRequest, new PlayerListRequest());
            messageListener.Add((byte)NetWorkIDS.PlayerStatusMessage, new PlayerStatusMessage());
            messageListener.Add((byte)NetWorkIDS.PlayerStatusMessage2, new PlayerStatusMessage2());
            messageListener.Add((byte)NetWorkIDS.AnimationMessage, new AnimationMessage());
            messageListener.Add((byte)NetWorkIDS.AnimationOverlayMessage, new AnimationOverlayMessage());
            messageListener.Add((byte)NetWorkIDS.AllPlayerSynchMessage, new AllPlayerSynchMessage());
            messageListener.Add((byte)NetWorkIDS.WeaponModeMessage, new WeaponModeMessage());
            messageListener.Add((byte)NetWorkIDS.ItemSynchro_DoDrop, new ItemSynchro_DoDrop());
            messageListener.Add((byte)NetWorkIDS.MobSynch, new MobSynch());
            messageListener.Add((byte)NetWorkIDS.Status, new StatusMessage());
            messageListener.Add((byte)NetWorkIDS.DownloadModulesMessage, new DownloadModulesMessage());
            messageListener.Add((byte)NetWorkIDS.ChatMessage, new ChatMessage());
            messageListener.Add((byte)NetWorkIDS.LevelChangeMessage, new LevelChangeMessage());
            messageListener.Add((byte)NetWorkIDS.VisualSynchro_SetAsPlayer, new VisualSynchro_SetAsPlayer());
            messageListener.Add((byte)NetWorkIDS.InventorySynch, new InventorySynch());
            messageListener.Add((byte)NetWorkIDS.MagSetupSynch, new MagSetupMessage());
            messageListener.Add((byte)NetWorkIDS.StaticNPCMessage, new StaticNPCMessage());
            messageListener.Add((byte)NetWorkIDS.SpawnNPCMessage, new SpawnNPCMessage());
            messageListener.Add((byte)NetWorkIDS.NPCControllerMessage, new NPCControllerMessage());

            messageListener.Add((byte)NetWorkIDS.PassivePerceptionMessage, new PassivePerceptionMessage());
            messageListener.Add((byte)NetWorkIDS.AttackSynchMessage, new AttackSynchMessage());
            messageListener.Add((byte)NetWorkIDS.FriendMessage, new FriendMessage());
            messageListener.Add((byte)NetWorkIDS.AssessPlayerMessage, new AssessPlayerMessage());
            messageListener.Add((byte)NetWorkIDS.SoundSynch, new OutputSynchMessage());
            messageListener.Add((byte)NetWorkIDS.AssessTalkMessage, new AssessTalkMessage());
            messageListener.Add((byte)NetWorkIDS.LevelDataMessage, new LevelDataMessage());
            messageListener.Add((byte)NetWorkIDS.ItemStealMessage, new ItemStealMessage());

            messageListener.Add((byte)NetWorkIDS.CommandoMessage, new CommandoMessage());

            messageListener.Add((byte)NetWorkIDS.PlayerKeyMessage, new PlayerKeyMessage());
            messageListener.Add((byte)NetWorkIDS.TextBoxSendMessage, new TextBoxSendMessage());

            messageListener.Add((byte)NetWorkIDS.MobInteractDiffMessage, new MobInteractDiffMessage());
            messageListener.Add((byte)NetWorkIDS.UseItemMessage, new UseItemMessage());
        }

        public void Start(ushort port, ushort maxConnections, String pw)
        {
            pw = "ver0.17" + pw;
            SocketDescriptor socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = port;

            bool started = server.Startup(maxConnections, socketDescriptor, 1) == StartupResult.RAKNET_STARTED;
            server.SetMaximumIncomingConnections(maxConnections);
            server.SetOccasionalPing(true);
            if (pw.Length != 0)
            {
                server.SetIncomingPassword(pw, pw.Length);
            }

            if (!started)
                Log.Logger.log(Log.Logger.LOG_ERROR, "Port is already in use");
        }

        public ushort ConnectionCount()
        {
            SystemAddress[] sa = null;
            ushort numbers = 0;
            server.GetConnectionList(out sa, ref numbers);
            return numbers;
        }




        public class CONNECTIONEventArgs : EventArgs
        {
            public ulong guid;
        }
        public static event EventHandler<CONNECTIONEventArgs> NEW_INCOMING_CONNECTION;

        public void Update()
        {
            Packet p = server.Receive();
            while (p != null)
            {
                switch (p.data[0])
                {
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST:
                    case (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                        {
                            Player pl = Player.getPlayerByGuid(p.guid.g, Program.playList);
                            if (pl != null)
                            {
                                Console.WriteLine("Disconnected: " + ConnectionCount() + " " + p.guid + " " + pl.name);
                                Program.scriptManager.OnPlayerDiconnected(new Scripting.Player(pl));
                            }
                            else
                                Console.WriteLine("Disconnected: " + ConnectionCount() + " " + p.guid);
                            if(pl != null)
                                new DisconnectedMessage(pl.id, p.guid.g).Write(receiveBitStream, this);
                            else
                                new DisconnectedMessage(-1, p.guid.g).Write(receiveBitStream, this);

                            


                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                        {
                            if (NEW_INCOMING_CONNECTION != null)
                            {
                                NEW_INCOMING_CONNECTION(this, new CONNECTIONEventArgs() { guid = p.guid.g });
                            }
                            Console.WriteLine("New Connections: " + ConnectionCount() + " " + p.guid);
                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM:
                        receiveBitStream.Reset();
                        receiveBitStream.Write(p.data, p.length);
                        receiveBitStream.IgnoreBytes(2);
                        try
                        {
                            if (messageListener.ContainsKey(p.data[1]))
                                messageListener[p.data[1]].Read(receiveBitStream, p, this);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            server.CloseConnection(p.guid, true);
                        }
                            break;
                }
                server.DeallocatePacket(p);
                p = server.Receive();
            }
        }
    }
}
