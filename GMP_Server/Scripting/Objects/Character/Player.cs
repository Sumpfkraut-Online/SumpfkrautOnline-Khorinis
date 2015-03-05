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

        public Player[] getAll()
        {
            Player[] protoList = new Player[sWorld.PlayerList.Count()];

            for (int i = 0; i < sWorld.PlayerList.Count(); i++)
            {
                protoList[i] = (Player)sWorld.PlayerList[i].ScriptingNPC;
            }

            return protoList;
        }


        public static void ShowStatusMenu(bool show)
        {
            WorldObjects.Character.Player.EnableStatusMenu = show;
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.InterfaceOptionsMessage);
            stream.Write((byte)0);
            stream.Write(show);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public static void ShowLogMenu(bool show)
        {
            WorldObjects.Character.Player.EnableLogMenu = show;


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.InterfaceOptionsMessage);
            stream.Write((byte)1);
            stream.Write(show);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void showStatusMenu(bool show)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.InterfaceOptionsMessage);
            stream.Write((byte)0);
            stream.Write(show);
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void showLogMenu(bool show)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.InterfaceOptionsMessage);
            stream.Write((byte)1);
            stream.Write(show);
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
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
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.PlayerFreezeMessage);
            stream.Write(freeze);
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
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
                    return Program.server.ServerInterface.GetSystemAddressFromGuid(guid).ToString();
                }
            }
        }

        public int LastPing
        {
            get
            {
                using (RakNet.RakNetGUID guid = Proto.GUID)
                {
                    return Program.server.ServerInterface.GetLastPing(guid);
                }
            }
        }

        public int AveragePing
        {
            get
            {
                using (RakNet.RakNetGUID guid = Proto.GUID)
                {
                    return Program.server.ServerInterface.GetAveragePing(guid);
                }
            }
        }

        public bool IsConnected
        {
            get { return Proto.IsConnected; }
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
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ExitGameMessage);
            
            using (RakNetGUID guid = this.Proto.GUID)
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

            using (RakNet.RakNetGUID guid = new RakNetGUID(this.proto.Guid))
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

            using (RakNet.RakNetGUID guid = new RakNetGUID(this.proto.Guid))
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

            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void camToVob(Vob vob)
        {
            if (vob == null)
                vob = this;
            if (vob.vob.Map != proto.Map)
                throw new ArgumentException("Vob has to be in the same map!");
            if(vob is Item && !(((Item)vob).ProtoItem.Container is WorldObjects.World))
                throw new ArgumentException("Vob is Item, Item needs to be in a World!");
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CamToVobMessage);
            stream.Write(vob.vob.ID);
            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void camToPlayerFront()
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CamToPlayerFront);
            
            using (RakNetGUID guid = this.Proto.GUID)
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

            using (RakNetGUID guid = this.Proto.GUID)
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

            using (RakNetGUID guid = this.Proto.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
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

    }
}
