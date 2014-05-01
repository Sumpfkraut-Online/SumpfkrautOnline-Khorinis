using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects;

namespace GUC.Server.Scripting.Objects.Character
{
    public class Player : NPCProto
    {
        protected bool isSpawned = false;


        internal Player(GUC.WorldObjects.Character.NPCProto proto)
            : base(proto)
        {
            this.created = true;
        }

        private WorldObjects.Character.Player Proto
        {
            get { return (WorldObjects.Character.Player)this.proto; }
        }


        public Vob FocusVob { get { return (proto.FocusVob == null) ? null : proto.FocusVob.ScriptingVob; } }
        public NPCProto Enemy { get { return (proto.Enemy == null) ? null : proto.Enemy.ScriptingNPC; } }


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
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.PlayerFreezeMessage);
            stream.Write(freeze);
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public String Mac {
            get {
                return Proto.MacString;
            }
        }

        public String Drive
        {
            get
            {
                return Proto.DriveString;
            }
        }

        public String IP
        {
            get
            {
                using(RakNet.RakNetGUID guid = Proto.GUID){
                    return Program.server.server.GetSystemAddressFromGuid(guid).ToString();
                }
            }
        }



        public static void EnableAllPlayerKeys(bool x)
        {
            WorldObjects.Character.Player.sSendAllKeys = x;
        }

        public static void EnablePlayerKey(bool activate, params byte[] keys)
        {
            foreach (byte key in keys)
            {
                if (activate && !WorldObjects.Character.Player.sSendKeys.Contains(key))
                    WorldObjects.Character.Player.sSendKeys.Add(key);
                else if(!activate && WorldObjects.Character.Player.sSendKeys.Contains(key))
                    WorldObjects.Character.Player.sSendKeys.Remove(key);
            }
        }

        public void exitGame()
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ExitGameMessage);
            
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public int ReadIni(String section, String entry)
        {

            int callBackID = sWorld.getNewCallBackID();

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ReadIniEntryMessage);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(section);
            stream.Write(entry);

            using (RakNet.RakNetGUID guid = new RakNetGUID(this.proto.Guid))
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            return callBackID;
        }

        public int CheckMD5(String file)
        {

            int callBackID = sWorld.getNewCallBackID();

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ReadMd5Message);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(file);

            using (RakNet.RakNetGUID guid = new RakNetGUID(this.proto.Guid))
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            return callBackID;
        }

        public void PlayVideo(String video)
        {
            if (video == null)
                throw new ArgumentNullException();
            
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.PlayVideo);
            stream.Write(video);

            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void camToVob(Vob vob)
        {
            if (vob == null)
                vob = this;
            if (vob.vob.Map != proto.Map)
                throw new ArgumentException("Vob has to be in the same map!");
            if(vob is Item && !(((Item)vob).ProtoItem.Container is WorldObjects.World))
                throw new ArgumentException("Vob is Item, Item needs to be in a World!");
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.CamToVobMessage);
            stream.Write(vob.vob.ID);
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
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

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCSpawnMessage);
            stream.Write(proto.ID);
            stream.Write(Proto.Map);
            
            stream.Write(proto.Position);
            stream.Write(proto.Direction);

            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        #region PlayerKeyEvent
        public static event Events.PlayerKeyEventHandler sPlayerKeyEvent;
        public event Events.PlayerKeyEventHandler PlayerKeyEvent;
        
        internal static void sOnPlayerKey(Player pl, Dictionary<byte, byte> keys)
        {
            pl.OnPlayerKey(pl, keys);
            if (sPlayerKeyEvent != null)
                sPlayerKeyEvent(pl, keys);
        }

        internal void OnPlayerKey(Player pl, Dictionary<byte, byte> keys)
        {
            if (pl.PlayerKeyEvent != null)
                pl.PlayerKeyEvent(pl, keys);
        }
        #endregion

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









        public event GUC.Server.Scripting.Events.PlayerEventHandler Disconnected;
        public event GUC.Server.Scripting.Events.PlayerEventHandler Spawned;
        internal void OnDisconnect(Player pl)
        {
            if(Disconnected != null)
                Disconnected(pl);
        }

        internal void OnSpawned(Player pl)
        {
            if (Spawned != null)
                Spawned(pl);
        }
        #endregion
        #region Static Events:

        public static event Events.PlayerEventHandler playerConnects;
        public static event Events.PlayerEventHandler playerDisconnects;
        public static event Events.PlayerEventHandler playerSpawns;
        internal static void OnPlayerConnect(Player pl)
        {
            if (playerConnects != null)
                playerConnects(pl);
        }

        internal static void OnPlayerDisconnect(Player pl)
        {
            pl.OnDisconnect(pl);
            if (playerDisconnects != null)
                playerDisconnects(pl);
        }

        internal static void OnPlayerSpawn(Player pl)
        {
            pl.OnSpawned(pl);
            if (playerSpawns != null)
                playerSpawns(pl);
        }

        #endregion

    }
}
