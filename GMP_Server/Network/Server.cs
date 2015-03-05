#define D_SERVER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Server.Network.Messages.Connection;
using GUC.Enumeration;
using GUC.Server.Network.Messages;
using GUC.Server.Network.Messages.PlayerCommands;
using GUC.Server.Network.Messages.VobCommands;
using GUC.Server.Network.Messages.NpcCommands;
using GUC.Server.Network.Messages.MobInterCommands;
using GUC.Server.Network.Messages.Callbacks;

namespace GUC.Server.Network
{
    public class Server
    {

        public RakPeerInterface ServerInterface { get; private set; }
        public Dictionary<byte, IMessage> MessageListener { get; private set;}
        public BitStream ReceiveBitStream { get; private set; }
        public BitStream SendBitStream { get; private set; }


        public Server()
        {
            ServerInterface = RakPeer.GetInstance();
            MessageListener = new Dictionary<byte,IMessage>();
            ReceiveBitStream = new BitStream();
            SendBitStream = new BitStream();


            initMessageListener();
        }

        protected void initMessageListener()
        {
            MessageListener.Add((byte)NetworkID.ConnectionMessage, new ConnectionMessage());
            MessageListener.Add((byte)NetworkID.NPCSpawnMessage, new NPCSpawnMessage());
            MessageListener.Add((byte)NetworkID.GuiMessage, new GuiMessage());
            MessageListener.Add((byte)NetworkID.OnDamageMessage, new OnDamageMessage());

            MessageListener.Add((byte)NetworkID.TakeItemMessage, new TakeItemMessage());
            MessageListener.Add((byte)NetworkID.DropItemMessage, new DropItemMessage());

            MessageListener.Add((byte)NetworkID.SetVobPosDirMessage, new SetVobPosDirMessage());
            MessageListener.Add((byte)NetworkID.AnimationUpdateMessage, new AnimationUpdateMessage());

            MessageListener.Add((byte)NetworkID.NPCUpdateMessage, new NPCUpdateMessage());
            MessageListener.Add((byte)NetworkID.MobInterMessage, new MobInterMessage());

            MessageListener.Add((byte)NetworkID.ItemRemovedByUsing, new ItemRemovedByUsing());
            MessageListener.Add((byte)NetworkID.ContainerItemChangedMessage, new GUC.Server.Network.Messages.ContainerCommands.ItemChangedMessage());

            MessageListener.Add((byte)NetworkID.CallbackNPCCanSee, new CallbackNPCCanSee());
            MessageListener.Add((byte)NetworkID.DoDieMessage, new DoDieMessage());

            MessageListener.Add((byte)NetworkID.ReadIniEntryMessage, new ReadIniEntryMessage());
            MessageListener.Add((byte)NetworkID.ReadMd5Message, new ReadMd5Message());

            MessageListener.Add((byte)NetworkID.EquipItemMessage, new EquipItemMessage());

            MessageListener.Add((byte)NetworkID.ChangeWorldMessage, new ChangeWorldMessage());

            MessageListener.Add((byte)NetworkID.PlayerKeyMessage, new PlayerKeyMessage());
            MessageListener.Add((byte)NetworkID.UseItemMessage, new UseItemMessage());

            MessageListener.Add((byte)NetworkID.CastSpell, new CastSpell());
            MessageListener.Add((byte)NetworkID.SpellInvestMessage, new SpellInvestMessage());


            MessageListener.Add((byte)NetworkID.PlayerOpenInventoryMessage, new OpenInventoryMessage());
        }

        public void Start(ushort port, ushort maxConnections, String pw)
        {
            pw = GUC.Options.Constants.VERSION + pw;


            SocketDescriptor socketDescriptor = new SocketDescriptor();
            socketDescriptor.port = port;

            bool started = ServerInterface.Startup(maxConnections, socketDescriptor, 1) == StartupResult.RAKNET_STARTED;
            ServerInterface.SetMaximumIncomingConnections(maxConnections);
            ServerInterface.SetOccasionalPing(true);
            
            if (pw.Length != 0)
            {
                ServerInterface.SetIncomingPassword(pw, pw.Length);
            }

            if (!started)
                Log.Logger.logError("Port is already in use");
            else
                Log.Logger.log("Server start listening on port "+port);
        }

        public ushort ConnectionCount()
        {
            SystemAddress[] sa = null;
            ushort numbers = 0;
            ServerInterface.GetConnectionList(out sa, ref numbers);
            return numbers;
        }

        public void Update()
        {
            Packet p = ServerInterface.Receive();

            while (p != null)
            {
                switch (p.data[0])
                {
                    case (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST:
                    case (byte)DefaultMessageIDTypes.ID_DISCONNECTION_NOTIFICATION:
                        {
                            if (!sWorld.GUIDToPlayer.ContainsKey(p.guid.g))
                            {
                                Log.Logger.log("Disconnected: " + ConnectionCount() + " " + p.guid);
                            }
                            else
                            {
                                Player pl = sWorld.GUIDToPlayer[p.guid.g];
                                pl.IsConnected = false;

                                foreach (NPC npc in pl.NPCControlledList)
                                {
                                    npc.NpcController = null;
                                }
                                new DisconnectMessage().Write(ReceiveBitStream, this, pl);

                                sWorld.removeVob(pl);
                                if(pl.IsSpawned)
                                    sWorld.getWorld(pl.Map).removeVob(pl);

                                if (p.data[0] == (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST)
                                {
                                    Scripting.Objects.Character.Player.isOnConnectionLost((Scripting.Objects.Character.Player)pl.ScriptingNPC);
                                }
                            }

                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                        {
                            Log.Logger.log("New Connections: " + ConnectionCount() + " " + p.guid+" "+p.systemAddress);
                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM:

                        ReceiveBitStream.Reset();
                        ReceiveBitStream.Write(p.data, p.length);
                        ReceiveBitStream.IgnoreBytes(2);
                        try
                        {
                            if (MessageListener.ContainsKey(p.data[1]))
                                MessageListener[p.data[1]].Read(ReceiveBitStream, p, this);
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.log(Log.Logger.LOG_ERROR, ex.Source + " <br>" + ex.Message + "<br>" + ex.StackTrace);
                            ServerInterface.CloseConnection(p.guid, true);
                        }
                        break;
                    default:
                        Log.Logger.log(Log.Logger.LOG_INFO, "Message-Type: " + ((DefaultMessageIDTypes)p.data[0]));
                        break;
                }
                ServerInterface.DeallocatePacket(p);
                p = ServerInterface.Receive();
            }
        }
    }
}
