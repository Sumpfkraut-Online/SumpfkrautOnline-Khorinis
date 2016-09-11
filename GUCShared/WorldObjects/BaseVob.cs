using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Types;
using GUC.GameObjects;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    /// <summary>
    /// The lowermost Vob-Object.
    /// </summary>
    public abstract partial class BaseVob : IDObject, VobTypeObject
    {
        #region ScriptObject

        /// <summary> The underlying ScriptObject interface for all Vobs. </summary>
        public partial interface IScriptBaseVob : IScriptGameObject
        {
            void Spawn(World world);
            void Despawn();
        }
        
        /// <summary> The ScriptObject of this Vob. </summary>
        public new IScriptBaseVob ScriptObject { get { return (IScriptBaseVob)base.ScriptObject; } }

        #endregion

        #region Constructors

        public BaseVob(IScriptBaseVob scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        /// <summary> The VobType of this Vob. </summary>
        public abstract VobTypes VobType { get; }

        #region Vob Instance

        BaseVobInstance instance;
        /// <summary> The Instance of this object. </summary>
        public BaseVobInstance Instance { get { return this.instance; } set { SetInstance(value); } }

        public virtual Type InstanceType { get { return typeof(BaseVobInstance); } }

        protected void SetInstance(BaseVobInstance instance)
        {
            CanChangeNow();

            if (instance == null)
                throw new ArgumentNullException("Instance is null!");

            if (instance.GetType() != InstanceType)
                throw new ArgumentException(string.Format("Instance must be of type {0}! Is {1}.", InstanceType, instance.GetType()));

            if (!instance.IsCreated)
                throw new ArgumentException("Instance is not created!");

            this.instance = instance;
        }

        #endregion

        protected World world;
        /// <summary> The World this Vob is currently in. </summary>
        public World World { get { return this.world; } }

        /// <summary> Checks whether this Vob is spawned. </summary>
        public bool IsSpawned { get { return this.isCreated; } }

        #region Position & Direction

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

        partial void pGetPosition();
        /// <summary> Gets the position of this Vob </summary>
        public Vec3f GetPosition()
        {
            pGetPosition();
            return pos;
        }

        partial void pGetDirection();
        /// <summary> Gets the direction of this Vob </summary>
        public Vec3f GetDirection()
        {
            pGetDirection();
            return dir;
        }

        partial void pSetPosition();
        /// <summary> Sets the position of this Vob </summary>
        public virtual void SetPosition(Vec3f position)
        {
            this.pos = position.CorrectPosition();
            pSetPosition();
        }

        partial void pSetDirection();
        /// <summary> Sets the direction of this Vob </summary>
        public virtual void SetDirection(Vec3f direction)
        {
            this.dir = direction.CorrectDirection();
            pSetDirection();
        }

        #endregion

        #region Environment

        public struct Environment
        {
            bool inAir;
            /// <summary> Whether the vob is in air. </summary>
            public bool InAir { get { return this.inAir; } }
            float waterLevel;
            /// <summary> Returns the vob's relative water level to its height. [0..1] </summary>
            public float WaterLevel { get { return this.waterLevel; } }
            float waterDepth;
            /// <summary> Returns the vob's relative water depth to its height. [0..1] </summary>
            public float WaterDepth { get { return this.waterDepth; } }

            internal Environment(bool inAir, float waterLevel, float waterDepth)
            {
                this.inAir = inAir;
                this.waterLevel = waterLevel;
                this.waterDepth = waterDepth;
            }

            public static bool operator ==(Environment env1, Environment env2)
            {
                if (env1.inAir != env2.inAir)
                    return false;

                if (Math.Abs(env1.waterDepth - env2.waterDepth) >= 0.01f)
                    return false;

                if (Math.Abs(env1.waterLevel - env2.waterLevel) >= 0.01f)
                    return false;

                return true;
            }

            public static bool operator != (Environment env1, Environment env2)
            {
                return !(env1 == env2);
            }
        }

        protected Environment environment;
        partial void pGetEnvironment();
        public virtual Environment GetEnvironment()
        {
            pGetEnvironment();
            return this.environment;
        }

        #endregion

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write((ushort)this.instance.ID);
            stream.Write(this.pos);
            stream.Write(this.dir);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            int instanceID = stream.ReadUShort();
            BaseVobInstance inst;
            if (!BaseVobInstance.TryGet(instanceID, out inst))
            {
                throw new Exception("Instance ID not found! " + instanceID);
            }
            SetInstance(inst);

            this.pos = stream.ReadVec3f();
            this.dir = stream.ReadVec3f();
        }

        #endregion

        #region Spawn

        public delegate void OnSpawnHandler(BaseVob vob, World world, Vec3f pos, Vec3f dir);
        public delegate void OnDespawnHandler(BaseVob vob);

        public static event OnSpawnHandler sOnSpawn = null;
        public static event OnDespawnHandler sOnDespawn = null;

        public event OnSpawnHandler OnSpawn = null;
        public event OnDespawnHandler OnDespawn = null;

        /// <summary>
        /// Spawns the Vob in the given world.
        /// </summary>
        public void Spawn(World world)
        {
            Spawn(world, this.pos, this.dir);
        }

        /// <summary>
        /// Spawns the Vob in the given world at the given position.
        /// </summary>
        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, this.dir);
        }

        partial void pBeforeSpawn(World world, Vec3f position, Vec3f direction);
        partial void pAfterSpawn(World world, Vec3f position, Vec3f direction);
        /// <summary> Spawns the Vob in the given world at the given position & direction. </summary>
        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            if (this.instance == null)
                throw new Exception("Vob has no Instance!");

            if (this.ScriptObject == null)
                throw new Exception("Vob has no ScriptObject!");

            if (this.isCreated)
                throw new Exception("Vob is already spawned!");

            Vec3f spawnPos = position.CorrectPosition();
            Vec3f spawnDir = direction.CorrectDirection();

            this.pBeforeSpawn(world, spawnPos, spawnDir);

            this.pos = spawnPos;
            this.dir = spawnDir;

            world.AddVob(this);
            this.world = world;
            this.isCreated = true;

            this.pAfterSpawn(world, spawnPos, spawnDir);

            if (this.OnSpawn != null)
                this.OnSpawn(this, world, position, direction);
            if (sOnSpawn != null)
                sOnSpawn(this, world, position, direction);
        }


        partial void pBeforeDespawn();
        partial void pAfterDespawn();
        /// <summary>
        /// Despawns the Vob.
        /// </summary>
        public virtual void Despawn()
        {
            if (!this.isCreated)
                throw new Exception("Vob isn't spawned!");

            if (this.OnDespawn != null)
                this.OnDespawn(this);
            if (sOnDespawn != null)
                sOnDespawn(this);

            pBeforeDespawn();

            this.isCreated = false;
            this.world.RemoveVob(this);
            this.world = null;

            pAfterDespawn();
        }
        #endregion

        partial void pOnTick(long now);
        internal virtual void OnTick(long now)
        {
            pOnTick(now);
        }

        public override string ToString()
        {
            return string.Format("({0}: {1})", this.ID, this.GetType());
        }
    }
}
