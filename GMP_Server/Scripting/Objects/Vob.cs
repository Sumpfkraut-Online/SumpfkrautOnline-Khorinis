using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Types;
using GUC.Network;
using GUC.Server.Network.Messages.VobCommands;
using GUC.WorldObjects;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.Objects
{
    public class Vob
    {
        internal GUC.WorldObjects.Vob vob = null;
        protected bool created = false;

        protected Dictionary<String, Object> userObjects = new Dictionary<string,object>();

        internal Vob(GUC.WorldObjects.Vob vob)
        {
            this.vob = vob;
        }

        public Vob(String Visual)
            : this(Visual, true, true, true)
        { }

        public Vob(String Visual, bool cdDyn, bool cdStatic)
            : this(Visual, cdDyn, cdStatic, true)
        { }

        private Vob(String visual, bool cdDyn, bool cdStatic, bool useCreate)
            : this(new GUC.WorldObjects.Vob(), visual, cdDyn, cdStatic, useCreate)
        {

        }
        internal Vob(GUC.WorldObjects.Vob vob, String visual, bool cdDyn, bool cdStatic, bool useCreate)
            : this(vob)
        {
            this.vob.ScriptingVob = this;

            this.Visual = visual;
            this.vob.CDDyn = cdDyn;
            this.vob.CDStatic = cdStatic;

            if (useCreate)
                CreateVob();
        }

        public int ID { get { return vob.ID; } }

        public Vec3f Position { get { return vob.Position; } set { setPosition(value); } }
        public Vec3f Direction { get { return vob.Direction; } set { setDirection(value); } }

        public void Spawn()
        {
            if (this.Map == null)
                this.vob.Map = sWorld.getMapName(@"NewWorld\NewWorld.zen");
            Spawn(this.Map, this.Position, this.Direction);
        }

        public virtual void Spawn(String world, Vec3f position, Vec3f direction)
        {
            
            if (direction == null)
                direction = new Vec3f();
            GUC.WorldObjects.sWorld.getWorld(world).addVob(this.vob);
            vob.IsSpawned = true;
            this.vob.Position = position;
            this.vob.Direction = direction;



            SpawnVobMessage.Write(vob);
        }

        protected virtual void CreateVob()
        {
            if (created)
                return;
            CreateVobMessage.Write(vob);

            GUC.WorldObjects.sWorld.addVob(this.vob);
            created = true;
        }

        public virtual World World { 
            get {
                if (this.vob.Map == null)
                    return null;
                return World.getWorld(this.vob.Map);
            } 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Player[] getNearPlayers(float distance)
        {
            if (this.vob.Map == null || this.vob.Map.Length == 0)
                throw new Exception("The Player has not been spawned! Use Spawn() command first!");

            List<Player> playerList = new List<Player>();

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, distance);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).PlayerPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.Player> mobs = sWorld.getWorld(this.vob.Map).PlayerPositionList[key];
                foreach (WorldObjects.Character.Player m in mobs)
                {
                    if (m == this.vob)
                        continue;
                    float mD = (m.Position - this.Position).Length;

                    if (mD <= distance)
                    {
                        playerList.Add((Player)m.ScriptingNPC);
                    }
                }
            }

            return playerList.ToArray();
        }

        public Player getNearestPlayers(float distance)
        {
            if (this.vob.Map == null || this.vob.Map.Length == 0)
                throw new Exception("The Player has not been spawned! Use Spawn() command first!");

            Player lastPlayer = null;
            float lastDistance = 0;

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, distance);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).PlayerPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.Player> mobs = sWorld.getWorld(this.vob.Map).PlayerPositionList[key];
                foreach (WorldObjects.Character.Player m in mobs)
                {
                    if (m == this.vob)
                        continue;
                    float mD = (m.Position - this.Position).Length;

                    if (mD <= distance)
                    {
                        if (lastPlayer == null)
                        {
                            lastPlayer = (Player)m.ScriptingNPC;
                            lastDistance = mD;
                        }
                        else if (mD < lastDistance)
                        {
                            lastPlayer = (Player)m.ScriptingNPC;
                            lastDistance = mD;
                        }
                    }
                }
            }

            return lastPlayer;
        }

        public NPCProto[] getNearNPC(float distance)
        {
            if (this.vob.Map == null || this.vob.Map.Length == 0)
                throw new Exception("The Player has not been spawned! Use Spawn() command first!");

            List<NPCProto> playerList = new List<NPCProto>();

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, distance);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).NPCPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.NPCProto> mobs = sWorld.getWorld(this.vob.Map).NPCPositionList[key];
                foreach (WorldObjects.Character.NPCProto m in mobs)
                {
                    if (m == this.vob)
                        continue;
                    if (m.ScriptingNPC is Player && !((Player)m.ScriptingVob).IsSpawned())
                        continue;
                    float mD = (m.Position - this.Position).Length;

                    if (mD <= distance)
                    {
                        playerList.Add((NPCProto)m.ScriptingNPC);
                    }
                }
            }

            return playerList.ToArray();
        }



        public MobInter getNearestMobInteract()
        {
            MobInter mi = null;

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, 4000);
            float minDistance = float.MaxValue;
            
            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).MobInterPositionList.ContainsKey(key))
                    continue;
                List<WorldObjects.Mobs.MobInter> mobs = sWorld.getWorld(this.vob.Map).MobInterPositionList[key];
                
                foreach (WorldObjects.Mobs.MobInter m in mobs)
                {
                    if (mi == null)
                    {
                        minDistance = (m.Position - this.Position).Length;
                        mi = (MobInter)m.ScriptingVob;
                        continue;
                    }

                    float mD = (m.Position - this.Position).Length;
                    if (mD < minDistance)
                    {
                        mi = (MobInter)m.ScriptingVob;
                        minDistance = mD;
                    }

                }

            }
            
            return mi;
        }

        public NPCProto getNearestNPC()
        {
            NPCProto mi = null;

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, 4000);
            float minDistance = float.MaxValue;
            
            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).MobInterPositionList.ContainsKey(key))
                    continue;
                List<WorldObjects.Character.NPCProto> mobs = sWorld.getWorld(this.vob.Map).NPCPositionList[key];

                foreach (WorldObjects.Character.NPCProto m in mobs)
                {
                    if (mi == null)
                    {
                        minDistance = (m.Position - this.Position).Length;
                        mi = (NPCProto)m.ScriptingVob;
                        continue;
                    }

                    float mD = (m.Position - this.Position).Length;
                    if (mD < minDistance)
                    {
                        mi = (NPCProto)m.ScriptingVob;
                        minDistance = mD;
                    }

                }

            }
            return mi;
        }

        public virtual void PlayEffect(String effect, Vob targetVob, int effectLevel, int damage, int damagetype, bool isprojectile)
        {
            PlayPlayerEffect(null, effect, targetVob, effectLevel, damage, damagetype, isprojectile);
        }

        public virtual void PlayPlayerEffect(Player player, String effect, Vob targetVob, int effectLevel, int damage, int damagetype, bool isprojectile)
        {
            if (!created)
                return;
            if (effect == null || effect.Length == 0)
                return;
            if (targetVob.Map != this.Map)
                throw new Exception("Target-Vob is not in the same map!");


            int targetID = 0;
            if (targetVob != null)
                targetID = targetVob.ID;
            

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.PlayEffectMessage);
            stream.Write(vob.ID);
            stream.Write(effect);
            stream.Write(targetID);
            stream.Write(effectLevel);
            stream.Write(damage);
            stream.Write(damagetype);
            stream.Write(isprojectile);
            if(player == null)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            else
                using(RakNetGUID guid = player.proto.GUID)
                    Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public virtual void setVisual(String visual)
        {
            vob.Visual = visual;

            if (!created)
                return;
            
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVisualMessage);
            stream.Write(vob.ID);
            stream.Write(visual);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setPosition(Vec3f position)
        {
            vob.Position = position;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVobPositionMessage);
            stream.Write(vob.ID);
            stream.Write(vob.Position);

            if(this is NPC)
                ((NPCProto)this).proto.SendToAreaPlayers(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            else
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setDirection(Vec3f dir)
        {
            if (dir.isNull())
                return;
            dir = dir.normalise();
            vob.Direction = dir;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVobDirectionMessage);
            stream.Write(vob.ID);
            stream.Write(vob.Direction);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setDirectionFast(Vec3f dir)
        {
            if (dir.isNull())
                return;
            dir = dir.normalise();
            vob.Direction = dir;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVobDirectionMessage);
            stream.Write(vob.ID);
            stream.Write(vob.Direction);
            Program.server.server.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        #region CreationProperty



        public String Map { get { return vob.Map; } }

        public virtual String Visual
        {
            get { return vob.Visual; }
            protected set { setVisual(value); }
        }

        

        protected void e() { if (created)throw new Exception("Can only be set in constructor/ before creation"); }
        #endregion


        public Object getUserObjects(String userObject)
        {
            if (!userObjects.ContainsKey(userObject))
                return null;
            return userObjects[userObject];
        }

        public void setUserObject(String str, Object obj)
        {
            if (!userObjects.ContainsKey(str))
                userObjects.Add(str, obj);
            userObjects[str] = obj;
        }
    }
}
