using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects;
using System.Collections;
using GUC.Server.Network;

namespace GUC.Server.Scripting.Objects.Character
{
    /** 
    * Class from which all player characters/npcs are instantiated.
    * Class which defines RakNet network communication regarding players/player characters.
    * It inherits from NPCProto, a prototype class for all vobs which act as npcs.
    */
    public class Player : NPC
    {
        protected bool isSpawned = false;

        internal Player(GUC.WorldObjects.Character.NPC proto)
            : base(proto)
        {
            this.created = true;
        }

        private WorldObjects.Character.NPC Proto
        {
            get { return (WorldObjects.Character.NPC)this.proto; }
        }

        public static Player[] getAll()
        {
            Player[] protoList = new Player[sWorld.PlayerList.Count];

            for (int i = 0; i < sWorld.PlayerList.Count; i++)
            {
                protoList[i] = (Player)sWorld.PlayerList[i].ScriptingNPC;
            }

            return protoList;
        }

        public static IEnumerable ToEnumerable()
        {
            foreach (GUC.WorldObjects.Character.NPC item in sWorld.PlayerList)
            {
                yield return (Player)item.ScriptingNPC;
            }
        }

        public Vob FocusVob { get { return (proto.FocusVob == null) ? null : proto.FocusVob.ScriptingVob; } }
        public NPC Enemy { get { return (proto.Enemy == null) ? null : proto.Enemy.ScriptingNPC; } }


        public bool IsSpawned()
        {
            return isSpawned;
        }

        public void freeze()
        {
            freeze(true);
        }

        public void unfreeze()
        {
            freeze(false);
        }

        public void freeze(bool freeze)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.PlayerFreezeMessage);
            stream.Write(freeze);
            using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void kick()
        {
            using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
                Server.Program.server.ServerInterface.CloseConnection(guid, true);
        }

        public void ban()
        {
            using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
            {
                Server.Program.server.ServerInterface.CloseConnection(guid, true);
                Server.Program.server.ServerInterface.AddToBanList(this.IP);
            }
        }

        public String IP
        {
            get
            {
                using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
                {
                    return Program.server.ServerInterface.GetSystemAddressFromGuid(guid).ToString();
                }
            }
        }

        public int LastPing
        {
            get
            {
                using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
                {
                    return Program.server.ServerInterface.GetLastPing(guid);
                }
            }
        }

        public int AveragePing
        {
            get
            {
                using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
                {
                    return Program.server.ServerInterface.GetAveragePing(guid);
                }
            }
        }

        public void exitGame()
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ExitGameMessage);

            using (RakNetGUID guid = new RakNetGUID(this.Proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public int ReadIni(String section, String entry)
        {

            int callBackID = sWorld.getNewCallBackID();

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ReadIniEntryMessage);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(section);
            stream.Write(entry);

            using (RakNet.RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            return callBackID;
        }

        public int CheckMD5(String file)
        {

            int callBackID = sWorld.getNewCallBackID();

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ReadMd5Message);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(file);

            using (RakNet.RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            return callBackID;
        }

        public void PlayVideo(String video)
        {
            if (video == null)
                throw new ArgumentNullException();

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.PlayVideo);
            stream.Write(video);

            using (RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void camToVob(Vob vob)
        {
            if (vob == null)
                vob = this;
            if (vob.vob.Map != proto.Map)
                throw new ArgumentException("Vob has to be in the same map!");
            if (vob is Item && !(((Item)vob).ProtoItem.Container is WorldObjects.World))
                throw new ArgumentException("Vob is Item, Item needs to be in a World!");
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CamToVobMessage);
            stream.Write(vob.vob.ID);
            using (RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void camToPlayerFront()
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CamToPlayerFront);

            using (RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void camToPlayer()
        {
            camToVob(this);
        }

        public void setSpawnInfos(String world, Types.Vec3f position, Types.Vec3f direction)
        {
            if (isSpawned)
                return;

            if (world != null)
                Proto.Map = world;
            if (position != null)
                proto.Position = position;
            if (direction != null)
                proto.Direction = direction;
        }

        public override void Spawn(string world, Types.Vec3f position, Types.Vec3f direction)
        {
            if (isSpawned)
                return;

            if (direction == null)
                direction = new Types.Vec3f(0, 0, 1);
            isSpawned = true;



            Proto.Map = world;
            proto.Position = position;
            proto.Direction = direction;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.NPCSpawnMessage);
            stream.Write(proto.ID);
            stream.Write(Proto.Map);

            stream.Write(proto.Position);
            stream.Write(proto.Direction);

            using (RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }


        public void setWorld(String world)
        {
            world = WorldObjects.sWorld.getMapName(world);

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChangeWorldMessage);
            stream.Write(proto.ID);
            stream.Write(world);

            using (RakNetGUID guid = new RakNetGUID(this.proto.client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        #region Events

        #region Callbacks

        #region ReadInit
        public event GUC.Server.Scripting.Events.ReadInitEventHandler OnReadInit;
        internal void iOnReadInit(int callbackID, Player player, String section, String entry, String value)
        {
            if (OnReadInit != null)
                OnReadInit(callbackID, player, section, entry, value);
        }

        public static event GUC.Server.Scripting.Events.ReadInitEventHandler sOnReadInit;
        internal static void isOnReadInit(int callbackID, Player player, String section, String entry, String value)
        {
            player.iOnReadInit(callbackID, player, section, entry, value);
            if (sOnReadInit != null)
                sOnReadInit(callbackID, player, section, entry, value);
        }

        #endregion


        #region OnMd5File
        public event GUC.Server.Scripting.Events.CheckMd5EventHandler OnCheckMd5;
        internal void iOnCheckMd5(int callbackID, Player player, String md5, String value)
        {
            if (OnCheckMd5 != null)
                OnCheckMd5(callbackID, player, md5, value);
        }

        public static event GUC.Server.Scripting.Events.CheckMd5EventHandler sOnCheckMd5;
        internal static void isOnCheckMd5(int callbackID, Player player, String md5, String value)
        {
            player.iOnCheckMd5(callbackID, player, md5, value);
            if (sOnCheckMd5 != null)
                sOnCheckMd5(callbackID, player, md5, value);
        }

        #endregion

        #endregion

        #region Spawn
        public event GUC.Server.Scripting.Events.PlayerEventHandler OnSpawned;
        internal void iOnSpawned(Player pl)
        {
            if (OnSpawned != null)
                OnSpawned(pl);
        }

        public static event Events.PlayerEventHandler sOnPlayerSpawns;
        internal static void isOnPlayerSpawn(Player pl)
        {
            pl.iOnSpawned(pl);
            if (sOnPlayerSpawns != null)
                sOnPlayerSpawns(pl);
        }

        #endregion

        #region Connection
        public static event Events.PlayerEventHandler sOnPlayerConnects;
        internal static void isOnPlayerConnect(Player pl)
        {
            if (sOnPlayerConnects != null)
                sOnPlayerConnects(pl);
        }
        #endregion

        #region Disconnect

        public event GUC.Server.Scripting.Events.PlayerEventHandler OnDisconnected;
        internal void iOnDisconnect(Player pl)
        {
            if (OnDisconnected != null)
                OnDisconnected(pl);
        }


        public static event Events.PlayerEventHandler sOnPlayerDisconnects;
        internal static void isOnPlayerDisconnect(Player pl)
        {
            pl.iOnDisconnect(pl);
            if (sOnPlayerDisconnects != null)
                sOnPlayerDisconnects(pl);
        }

        #endregion

        #region LostConnection

        public event GUC.Server.Scripting.Events.PlayerEventHandler OnConnectionLost;
        internal void iOnConnectionLost(Player pl)
        {
            if (OnConnectionLost != null)
                OnConnectionLost(pl);
        }


        public static event Events.PlayerEventHandler sOnConnectionLost;
        internal static void isOnConnectionLost(Player pl)
        {
            pl.iOnConnectionLost(pl);
            if (sOnConnectionLost != null)
                sOnConnectionLost(pl);


        }

        #endregion


        #endregion
        
        #region ChatMessages
        /*
        public void SendSay(Player from, String text)
        {
            SendText(from, ChatTextType.Say, text);
        }

        public void SendShout(Player from, String text)
        {
            SendText(from, ChatTextType.Shout, text);
        }

        public void SendWhisper(Player from, String text)
        {
            SendText(from, ChatTextType.Whisper, text);
        }

        public void SendAmbient(Player from, String text)
        {
            SendText(from, ChatTextType.Ambient, text);
        }

        public void SendOOC(Player from, String text)
        {
            SendText(from, ChatTextType.OOC, text);
        }

        public void SendPM(Player from, String text)
        {
            SendText(from, ChatTextType.PM, text);
        }

        public void SendOOCEvent(String text)
        {
            SendText(null, ChatTextType.OOCEvent, text);
        }

        public void SendErrorMessage(Player to, String text)
        {
            SendText(null, ChatTextType._Error, text);
        }

        public void SendHintMessage(String text)
        {
            SendText(null, ChatTextType._Hint, text);
        }

        private void SendText(Player from, ChatTextType type, String text)
        {
            if (text == null || text.Length <= 0)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write((byte)type);
            if (type != ChatTextType.OOCEvent && type != ChatTextType._Error && type != ChatTextType._Hint)
            {
                if (from == null)
                    return;
                stream.Write(from.ID);
            }
            stream.Write(text);
            using (RakNetGUID guid = new RakNetGUID(client.guid))
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public static void SendOOCGlobal(Player from, String text)
        {
            SendGlobalText(from, ChatTextType.OOCGlobal, text);
        }

        public static void SendRPGlobal(String text)
        {
            SendGlobalText(null, ChatTextType.RPGlobal, text);
        }

        private static void SendGlobalText(Player from, ChatTextType type, String text)
        {
            if (text == null || text.Length <= 0)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write((byte)type);
            if (type != ChatTextType.RPGlobal)
            {
                if (from == null)
                    return;
                stream.Write(from.ID);
            }
            stream.Write(text);

            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        */
        #endregion
        
    }
}
