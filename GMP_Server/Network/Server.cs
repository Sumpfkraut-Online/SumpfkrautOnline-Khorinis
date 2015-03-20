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


        /** 
        * Server-class which defines and manages network transfer as well as the game loop.
        * It uses RakNet network messages to establish communication channels between server and clients.
        * It further counts the connections and initialiszes the overall game loop.
        */
        public Server()
        {
            ServerInterface = RakPeer.GetInstance(); /**< Instance of RakNets main interface for network communication. */
            MessageListener = new Dictionary<byte,IMessage>();
            ReceiveBitStream = new BitStream();
            SendBitStream = new BitStream();


            initMessageListener();
        }

		protected void initMessageListener()
		{
			MessageListener.Add((byte)NetworkID.ConnectionMessage, new ConnectionMessage()); /**< Network messages concerning player connection state. */
			MessageListener.Add((byte)NetworkID.NPCSpawnMessage, new NPCSpawnMessage()); /**< Network messages concerning spawning npcs. */
			MessageListener.Add((byte)NetworkID.GuiMessage, new GuiMessage()); /**< Network messages concerning GUI events. */
			MessageListener.Add((byte)NetworkID.OnDamageMessage, new OnDamageMessage()); /**< Network messages concerning damage events. */

			MessageListener.Add((byte)NetworkID.TakeItemMessage, new TakeItemMessage()); /**< Network messages concerning events which are triggered when world-items are taken. */
			MessageListener.Add((byte)NetworkID.DropItemMessage, new DropItemMessage()); /**< Network messages concerning events which are triggered when items are dropped into the world. */

			MessageListener.Add((byte)NetworkID.SetVobPosDirMessage, new SetVobPosDirMessage()); /**< Network messages concerning forced vob positioning (in world). */
			MessageListener.Add((byte)NetworkID.AnimationUpdateMessage, new AnimationUpdateMessage()); /**< Network messages concerning npc animations. */

			MessageListener.Add((byte)NetworkID.NPCUpdateMessage, new NPCUpdateMessage()); /**< Network messages concerning npc-status-updates. */
			MessageListener.Add((byte)NetworkID.MobInterMessage, new MobInterMessage()); /**< Network messages concerning mob interaction (e.g. using an avil). */

			MessageListener.Add((byte)NetworkID.ItemRemovedByUsing, new ItemRemovedByUsing()); /**< Network messages concerning item removable following item use events. */
			MessageListener.Add((byte)NetworkID.ContainerItemChangedMessage, new GUC.Server.Network.Messages.ContainerCommands.ItemChangedMessage()); /**< Network messages concerning container exchange (chests, inventory, etc.). */

			MessageListener.Add((byte)NetworkID.CallbackNPCCanSee, new CallbackNPCCanSee()); /**< Network messages concerning visibility by npcs. */
			MessageListener.Add((byte)NetworkID.DoDieMessage, new DoDieMessage()); /**< Network messages concerning object/npc/player death. */

			MessageListener.Add((byte)NetworkID.ReadIniEntryMessage, new ReadIniEntryMessage()); /**< Network messages concerning ???. */
			MessageListener.Add((byte)NetworkID.ReadMd5Message, new ReadMd5Message()); /**< Network messages concerning MD5 data encryption. */

			MessageListener.Add((byte)NetworkID.EquipItemMessage, new EquipItemMessage()); /**< Network messages concerning item equipping. */

			MessageListener.Add((byte)NetworkID.ChangeWorldMessage, new ChangeWorldMessage()); /**< Network messages concerning switching the world (go to another world-instance) */

			MessageListener.Add((byte)NetworkID.PlayerKeyMessage, new PlayerKeyMessage()); /**< Network messages concerning keys triggered/pressed by players. */
			MessageListener.Add((byte)NetworkID.UseItemMessage, new UseItemMessage()); /**< Network messages concerning item use. */

			MessageListener.Add((byte)NetworkID.CastSpell, new CastSpell()); /**< Network messages concerning cast spells. */
			MessageListener.Add((byte)NetworkID.SpellInvestMessage, new SpellInvestMessage()); /**< Network messages concerning investments of an npcs to cast a spell (e.g. mana use). */


			MessageListener.Add((byte)NetworkID.PlayerOpenInventoryMessage, new OpenInventoryMessage());
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

        /**
         *   Counts the current amount of client-server-connections and returns them.
         *   Counts the current amount of client-server-connections and returns them.
         *   @return numbers as ushort amount of client-server-connections
         */
        public ushort ConnectionCount()
        {
            SystemAddress[] sa = null;
            ushort numbers = 0;
            ServerInterface.GetConnectionList(out sa, ref numbers);
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
