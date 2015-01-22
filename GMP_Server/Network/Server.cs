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

        /** 
        * Server-class which defines and manages network transfer as well as the game loop.
        * It uses RakNet network messages to establish communication channels between server and clients.
        * It further counts the connections and initialiszes the overall game loop.
        */
        public Server()
        {
            
            server = RakPeer.GetInstance(); /**< Instance of RakNets main interface for network communication. */

            messageListener.Add((byte)NetworkIDS.ConnectionMessage, new ConnectionMessage()); /**< Network messages concerning player connection state. */
            messageListener.Add((byte)NetworkIDS.NPCSpawnMessage, new NPCSpawnMessage()); /**< Network messages concerning spawning npcs. */
            messageListener.Add((byte)NetworkIDS.GuiMessage, new GuiMessage()); /**< Network messages concerning GUI events. */
            messageListener.Add((byte)NetworkIDS.OnDamageMessage, new OnDamageMessage()); /**< Network messages concerning damage events. */

            messageListener.Add((byte)NetworkIDS.TakeItemMessage, new TakeItemMessage()); /**< Network messages concerning events which are triggered when world-items are taken. */
            messageListener.Add((byte)NetworkIDS.DropItemMessage, new DropItemMessage()); /**< Network messages concerning events which are triggered when items are dropped into the world. */

            messageListener.Add((byte)NetworkIDS.SetVobPosDirMessage, new SetVobPosDirMessage()); /**< Network messages concerning forced vob positioning (in world). */
            messageListener.Add((byte)NetworkIDS.AnimationUpdateMessage, new AnimationUpdateMessage()); /**< Network messages concerning npc animations. */

            messageListener.Add((byte)NetworkIDS.NPCUpdateMessage, new NPCUpdateMessage()); /**< Network messages concerning npc-status-updates. */
            messageListener.Add((byte)NetworkIDS.MobInterMessage, new MobInterMessage()); /**< Network messages concerning mob interaction (e.g. using an avil). */

            messageListener.Add((byte)NetworkIDS.ItemRemovedByUsing, new ItemRemovedByUsing()); /**< Network messages concerning item removable following item use events. */
            messageListener.Add((byte)NetworkIDS.ContainerItemChangedMessage, new GUC.Server.Network.Messages.ContainerCommands.ItemChangedMessage()); /**< Network messages concerning container exchange (chests, inventory, etc.). */

            messageListener.Add((byte)NetworkIDS.CallbackNPCCanSee, new CallbackNPCCanSee()); /**< Network messages concerning visibility by npcs. */
            messageListener.Add((byte)NetworkIDS.DoDieMessage, new DoDieMessage()); /**< Network messages concerning object/npc/player death. */

            messageListener.Add((byte)NetworkIDS.ReadIniEntryMessage, new ReadIniEntryMessage()); /**< Network messages concerning ???. */
            messageListener.Add((byte)NetworkIDS.ReadMd5Message, new ReadMd5Message()); /**< Network messages concerning MD5 data encryption. */

            messageListener.Add((byte)NetworkIDS.EquipItemMessage, new EquipItemMessage()); /**< Network messages concerning item equipping. */

            messageListener.Add((byte)NetworkIDS.ChangeWorldMessage, new ChangeWorldMessage()); /**< Network messages concerning switching the world (go to another world-instance) */

            messageListener.Add((byte)NetworkIDS.PlayerKeyMessage, new PlayerKeyMessage()); /**< Network messages concerning keys triggered/pressed by players. */
            messageListener.Add((byte)NetworkIDS.UseItemMessage, new UseItemMessage()); /**< Network messages concerning item use. */

            messageListener.Add((byte)NetworkIDS.CastSpell, new CastSpell()); /**< Network messages concerning cast spells. */
            messageListener.Add((byte)NetworkIDS.SpellInvestMessage, new SpellInvestMessage()); /**< Network messages concerning investments of an npcs to cast a spell (e.g. mana use). */
        }

        /**
         *   Initializes/Starts the RakPeerInterface server to actually listen and answer network messages.
         *   Completes the setup of the network interface/server and starts it, so it can listen on the given port.
         *   @param port an ushort which is the main port of game server network communication.
         *   @param maxConnections an ushort which limits the maximum accepted client-server-connections.
         *   @param pw a String which defines the password to verify client-server-connections (???).
         */
        public void Start(ushort port, ushort maxConnections, String pw)
        {
            pw = "ver2.07" + pw;
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

        /**
         *   Counts the current amount of client-server-connections and returns them.
         *   Counts the current amount of client-server-connections and returns them.
         *   @return numbers as ushort amount of client-server-connections
         */
        public ushort ConnectionCount()
        {
            SystemAddress[] sa = null;
            ushort numbers = 0;
            server.GetConnectionList(out sa, ref numbers);
            return numbers;
        }

        /**
         *   Game loop which receives data from clients and redirects/reacts accordingly.
         *   In this surrounding loop data is received from individual clients and the server reacts depending 
         *   on the network message types (see class attributes for these types). This is done for each
         *   network message received by individual clients until there is no more (buffered) message
         *   left. Character creation is done here on successful connection as well as the respective 
         *   deletion of disconnect.
         */
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
                                Log.Logger.log(Log.Logger.LOG_INFO, "Disconnected: " + ConnectionCount() + " " + p.guid);
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

                                if (p.data[0] == (byte)DefaultMessageIDTypes.ID_CONNECTION_LOST)
                                {
                                    Scripting.Objects.Character.Player.isOnConnectionLost((Scripting.Objects.Character.Player)pl.ScriptingNPC);
                                }
                            }

                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_NEW_INCOMING_CONNECTION:
                        {
                            Log.Logger.log(Log.Logger.LOG_INFO, "New Connections: " + ConnectionCount() + " " + p.guid+" "+p.systemAddress);
                            break;
                        }
                    case (byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM:

                        receiveBitStream.Reset();
                        receiveBitStream.Write(p.data, p.length);
                        receiveBitStream.IgnoreBytes(2);
                        try
                        {
                            //Console.WriteLine(p.data[1]);
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
