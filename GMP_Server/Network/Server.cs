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
        public RakPeerInterface server;
        public Dictionary<byte, IMessage> messageListener = new Dictionary<byte, IMessage>();
        public BitStream receiveBitStream = new BitStream();
        public BitStream sendBitStream = new BitStream();


        public Server()
        {
            server = RakPeer.GetInstance();

            messageListener.Add((byte)NetworkIDS.ConnectionMessage, new ConnectionMessage());
            messageListener.Add((byte)NetworkIDS.NPCSpawnMessage, new NPCSpawnMessage());
            messageListener.Add((byte)NetworkIDS.GuiMessage, new GuiMessage());
            messageListener.Add((byte)NetworkIDS.OnDamageMessage, new OnDamageMessage());

            messageListener.Add((byte)NetworkIDS.TakeItemMessage, new TakeItemMessage());
            messageListener.Add((byte)NetworkIDS.DropItemMessage, new DropItemMessage());

            messageListener.Add((byte)NetworkIDS.SetVobPosDirMessage, new SetVobPosDirMessage());
            messageListener.Add((byte)NetworkIDS.AnimationUpdateMessage, new AnimationUpdateMessage());

            messageListener.Add((byte)NetworkIDS.NPCUpdateMessage, new NPCUpdateMessage());
            messageListener.Add((byte)NetworkIDS.MobInterMessage, new MobInterMessage());

            messageListener.Add((byte)NetworkIDS.ItemRemovedByUsing, new ItemRemovedByUsing());
            messageListener.Add((byte)NetworkIDS.ContainerItemChangedMessage, new GUC.Server.Network.Messages.ContainerCommands.ItemChangedMessage());

            messageListener.Add((byte)NetworkIDS.CallbackNPCCanSee, new CallbackNPCCanSee());
            messageListener.Add((byte)NetworkIDS.DoDieMessage, new DoDieMessage());

            messageListener.Add((byte)NetworkIDS.ReadIniEntryMessage, new ReadIniEntryMessage());
            messageListener.Add((byte)NetworkIDS.ReadMd5Message, new ReadMd5Message());

            messageListener.Add((byte)NetworkIDS.EquipItemMessage, new EquipItemMessage());

            messageListener.Add((byte)NetworkIDS.ChangeWorldMessage, new ChangeWorldMessage());

            messageListener.Add((byte)NetworkIDS.PlayerKeyMessage, new PlayerKeyMessage());
            messageListener.Add((byte)NetworkIDS.UseItemMessage, new UseItemMessage());

            messageListener.Add((byte)NetworkIDS.CastSpell, new CastSpell());
            messageListener.Add((byte)NetworkIDS.SpellInvestMessage, new SpellInvestMessage());
        }

        public void Start(ushort port, ushort maxConnections, String pw)
        {
            pw = "ver2.02" + pw;
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
            else
                Log.Logger.log(Log.Logger.LOG_INFO, "Server start listening on port "+port);
        }

        public ushort ConnectionCount()
        {
            SystemAddress[] sa = null;
            ushort numbers = 0;
            server.GetConnectionList(out sa, ref numbers);
            return numbers;
        }

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
                            if (!sWorld.GUIDToPlayer.ContainsKey(p.guid.g))
                            {
                                Console.WriteLine("Disconnected: " + ConnectionCount() + " " + p.guid);
                            }
                            else
                            {
                                Player pl = sWorld.GUIDToPlayer[p.guid.g];
                                pl.IsConnected = false;

                                foreach (NPC npc in pl.NPCControlledList)
                                {
                                    npc.NpcController = null;
                                }
                                new DisconnectMessage().Write(receiveBitStream, this, pl);

                                sWorld.removeVob(pl);
                                if(pl.IsSpawned)
                                    sWorld.getWorld(pl.Map).removeVob(pl);
                                
                            }

                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                        {
                            Console.WriteLine("New Connections: " + ConnectionCount() + " " + p.guid+" "+p.systemAddress);
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
                            Log.Logger.log(Log.Logger.LOG_ERROR, ex.Source + " <br>" + ex.Message + "<br>" + ex.StackTrace);
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
