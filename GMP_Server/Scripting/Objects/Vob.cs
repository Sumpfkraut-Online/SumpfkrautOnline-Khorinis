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
using System.Collections;

namespace GUC.Server.Scripting.Objects
{
    public class Vob
    {
        internal GUC.WorldObjects.Vob vob = null;
        protected Dictionary<String, Object> userObjects = new Dictionary<string,object>();

        /**
         * Returns the Vob with the specified ID.
         * To access the ID of a vob, use vob.ID
         * @param id ID of a Vob
         * @return Vob with the ID id or a null-reference
         */
        public static Vob getVob(int id){
            WorldObjects.Vob v;
            sWorld.VobDict.TryGetValue(id, out v);

            if (v == null)
                return null;

            return v.ScriptingVob;
        }


        internal Vob(GUC.WorldObjects.Vob vob)
        {
            this.vob = vob;
        }

        /// <summary>
        /// Creates a new vob.
        /// Do not forget to use the spawn function.
        /// </summary>
        /// <param name="Visual">The visual your vob will get</param>
        public Vob(String Visual) : this(Visual, true, true)
        { 
        }

        /// <summary>
        /// Creates a new vob.
        /// Do not forget to use the spawn function.
        /// </summary>
        /// <param name="Visual">The visual your vob will get</param>
        /// <param name="cdDyn">Dynamic Collisions</param>
        /// <param name="cdStatic">Static Collisions</param>

        public Vob(String visual, bool cdDyn, bool cdStatic) : this(new GUC.WorldObjects.Vob(), visual, cdDyn, cdStatic)
        {
        }

		/**
         * Creates a new vob.
         * @param vob Existing partner-vob in the world
         * @param visual the visual of the Vob to be creaeted.
         * @param cdDyn Dynamic collision
         * @param cdStatic Static collision
         * @param useCreate Call CreateVob() upon creation
         */
        internal Vob(GUC.WorldObjects.Vob vob, String visual, bool cdDyn, bool cdStatic) : this(vob)
        {
            this.vob.ScriptingVob = this;
            this.Visual = visual;
            this.vob.CDDyn = cdDyn;
            this.vob.CDStatic = cdStatic;

            //CreateVobMessage.Write(vob);

            sWorld.addVob(this.vob);
        }

        internal void test()
        {

        }

        public static IEnumerable ToEnumerable()
        {
            foreach (GUC.WorldObjects.Vob item in sWorld.VobList)
            {
                yield return item.ScriptingVob;
            }
        }

        public int ID { get { return vob.ID; } } /**< Internal ID of this vob. Can only be read. */

        public Vec3f Position { get { return vob.PosVec; } set { vob.PosVec = value } }
        public Vec3f Direction { get { return vob.DirVec; } set { vob.DirVec = value } }

        public bool CDDyn { get { return vob.CDDyn; } set { setCDDyn(value); } }
        public bool CDStatic { get { return vob.CDStatic; } set { setCDStatic(value); } }


        public void setCDDyn(bool x)
        {
            vob.CDDyn = x;
            setPropertie(VobChangeID.CDStatic, x);
        }

        public void setCDStatic(bool x)
        {
            vob.CDStatic = x;
            setPropertie(VobChangeID.CDStatic, x);
        }

        #region SetPropertie
        internal void setPropertie(VobChangeID vcID, bool x)
        {
            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobChangeMessage);
            stream.Write((byte)vcID);
            stream.Write(vob.ID);
            stream.Write(x);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        internal void setPropertie(VobChangeID vcID, String x)
        {
            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobChangeMessage);
            stream.Write((byte)vcID);
            stream.Write(vob.ID);
            stream.Write(x);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        internal void setPropertie(VobChangeID vcID, ItemInstance x)
        {
            if (!created)
                return;
            int ii = (x == null) ? 0 : x.ID;
            
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobChangeMessage);
            stream.Write((byte)vcID);
            stream.Write(vob.ID);
            stream.Write(ii);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
        #endregion

        /// <summary>
        /// Spawn your vob into the world.
        /// It will use NewWorld\NewWorld.zen and the setted Position/ Direction as the spawnpoint.
        /// </summary>
        public void Spawn()
        {
            if (this.Map == null)
                this.vob.Map = sWorld.getMapName(@"NewWorld\NewWorld.zen");
            Spawn(this.Map, this.Position, this.Direction);
        }

        /// <summary>
        /// Spawn the vob into the world
        /// </summary>
        /// <param name="world">the world name. Example: NewWorld\NewWorld.zen</param>
        /// <param name="position">The position of the vob in the world</param>
        /// <param name="direction">The direction of the vob.</param>
        public virtual void Spawn(String world, Vec3f position, Vec3f direction)
        {
            
            if (direction == null)
                direction = new Vec3f();
            GUC.WorldObjects.sWorld.getWorld(world).addVob(this.vob);
            //vob.IsSpawned = true;
            this.vob.Position = position;
            this.vob.Direction = direction;


            SpawnVobMessage.Write(vob);
        }

        public virtual void Despawn()
        {
            GUC.WorldObjects.sWorld.getWorld(vob.Map).removeVob(this.vob);
            DespawnVobMessage.Write(vob);
        }

        /// <summary>
        /// Returns the world or null if the vob is not spawned
        /// </summary>
        public virtual World World { 
            get {
                if (this.vob.Map == null)
                    return null;
                return World.getWorld(this.vob.Map);
            } 
        }


        /// <summary>
        /// Returns an array with all near Players in the specified distance
        /// </summary>
        /// <param name="distance">the maximum distance of the player</param>
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

        public float GetDistanceTo(Vob other)
        {
          return (other.Position - this.Position).Length;
          
        }

        /// <summary>
        /// Returns the nearest player to this vob in a specified distance.
        /// </summary>
        /// <param name="distance">Maximum distance of the player.</param>
        /// <returns>Returns the Player or null</returns>
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

                List<WorldObjects.Character.NPC> mobs = sWorld.getWorld(this.vob.Map).PlayerPositionList[key];
                foreach (WorldObjects.Character.NPC m in mobs)
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

        /// <summary>
        /// Returns an array of all Near NPC and Player in a specified distance
        /// </summary>
        /// <param name="distance">The maximum distance the NPCProto can be</param>
        /// <returns></returns>
        public NPC[] getNearNPC(float distance)
        {
            if (this.vob.Map == null || this.vob.Map.Length == 0)
                throw new Exception("The Player has not been spawned! Use Spawn() command first!");

            List<NPC> playerList = new List<NPC>();

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, distance);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).NPCPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.NPC> mobs = sWorld.getWorld(this.vob.Map).NPCPositionList[key];
                foreach (WorldObjects.Character.NPC m in mobs)
                {
                    if (m == this.vob)
                        continue;
                    if (m.ScriptingNPC is Player && !((Player)m.ScriptingVob).IsSpawned())
                        continue;
                    float mD = (m.Position - this.Position).Length;

                    if (mD <= distance)
                    {
                        playerList.Add((NPC)m.ScriptingNPC);
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

        /// <summary>
        /// Returns the nearest npc
        /// </summary>
        /// <returns></returns>
        public NPC getNearestNPC()
        {
            NPC mi = null;

            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, 4000);
            float minDistance = float.MaxValue;
            
            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.vob.Map).MobInterPositionList.ContainsKey(key))
                    continue;
                List<WorldObjects.Character.NPC> mobs = sWorld.getWorld(this.vob.Map).NPCPositionList[key];

                foreach (WorldObjects.Character.NPC m in mobs)
                {
                    if (mi == null)
                    {
                        minDistance = (m.Position - this.Position).Length;
                        mi = (NPC)m.ScriptingVob;
                        continue;
                    }

                    float mD = (m.Position - this.Position).Length;
                    if (mD < minDistance)
                    {
                        mi = (NPC)m.ScriptingVob;
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

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.PlayEffectMessage);
            stream.Write(vob.ID);
            stream.Write(effect);
            stream.Write(targetID);
            stream.Write(effectLevel);
            stream.Write(damage);
            stream.Write(damagetype);
            stream.Write(isprojectile);
            if(player == null)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            else
                using(RakNetGUID guid = player.proto.GUID)
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public virtual void setVisual(String visual)
        {
            vob.Visual = visual;

            if (!created)
                return;
            
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVisualMessage);
            stream.Write(vob.ID);
            stream.Write(visual);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setPosition(Vec3f position)
        {
            vob.Position = position;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobPositionMessage);
            stream.Write(vob.ID);
            stream.Write(vob.Position);

            if(this is NPC)
                ((NPC)this).proto.SendToAreaPlayers(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED);
            else
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setDirection(Vec3f dir)
        {
            if (dir.isNull())
                return;
            dir = dir.normalise();
            vob.Direction = dir;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobDirectionMessage);
            stream.Write(vob.ID);
            stream.Write(vob.Direction);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public virtual void setDirectionFast(Vec3f dir)
        {
            if (dir.isNull())
                return;
            dir = dir.normalise();
            vob.Direction = dir;

            if (!created)
                return;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.SetVobDirectionMessage);
            stream.Write(vob.ID);
            stream.Write(vob.Direction);
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public String Map { get { return vob.Map; } }

        public virtual String Visual
        {
            get { return vob.Visual; }
            protected set { setVisual(value); }
        }


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
