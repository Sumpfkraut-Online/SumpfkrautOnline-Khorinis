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
    public struct VobEnvironment
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

        internal VobEnvironment(bool inAir, float waterLevel, float waterDepth)
        {
            this.inAir = inAir;
            this.waterLevel = waterLevel;
            this.waterDepth = waterDepth;
        }

        public static bool operator ==(VobEnvironment env1, VobEnvironment env2)
        {
            if (env1.inAir != env2.inAir)
                return false;

            if (Math.Abs(env1.waterDepth - env2.waterDepth) >= 0.01f)
                return false;

            if (Math.Abs(env1.waterLevel - env2.waterLevel) >= 0.01f)
                return false;

            return true;
        }

        public static bool operator !=(VobEnvironment env1, VobEnvironment env2)
        {
            return !(env1 == env2);
        }

        public override bool Equals(object obj)
        {
            return obj is VobEnvironment && ((VobEnvironment)obj) == this;
        }

        public override string ToString()
        {
            return string.Format("Environment(InAir {0} WaterLevel {1} WaterDepth {2})", InAir, WaterLevel, WaterDepth);
        }
    }

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
            byte GetVobType();
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
        public abstract GUCVobTypes VobType { get; }

        #region Vob Instance

        GUCBaseVobDef instance;
        /// <summary> The Instance of this object. </summary>
        public GUCBaseVobDef Instance { get { return this.instance; } set { SetInstance(value); } }

        public virtual Type InstanceType { get { return typeof(GUCBaseVobDef); } }

        protected void SetInstance(GUCBaseVobDef instance)
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
        protected Angles ang = new Angles(0, 0, 0);

        /// <summary> Gets the position of this Vob </summary>
        public Vec3f Position { get { return this.pos; } }
        
        /// <summary> Gets the rotational angles of this vob. Pitch (up-down), Yaw (left-right), Roll (sideways). </summary>
        public Angles Angles { get { return this.ang; } }

        /// <summary> Calculates the direction this vob is "heading at" from its angles. </summary>
        public Vec3f GetAtVector()
        {
            return ang.ToAtVector();
        }

        partial void pSetPosition();
        /// <summary> Sets the position of this Vob </summary>
        public void SetPosition(Vec3f position)
        {
            this.pos = position.ClampToWorldLimits();
            pSetPosition();
        }

        partial void pSetAngles();
        /// <summary> Sets the rotational angles of this vob. Pitch (up-down), Yaw (left-right), Roll (sideways). </summary>
        public void SetAngles(Angles angles)
        {
            this.ang = angles.Clamp();
            pSetAngles();
        }

        /// <summary> Sets the angles of this Vob via a directional vector. </summary>
        public void SetAtVector(Vec3f at)
        {
            SetAngles(Angles.FromAtVector(at));
        }

        #endregion

        #region Environment

        protected VobEnvironment environment;
        public VobEnvironment Environment { get { return this.environment; } }

        #endregion

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write((ushort)this.instance.ID);
            stream.Write(this.pos);
            stream.Write(this.ang);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            int instanceID = stream.ReadUShort();
            GUCBaseVobDef inst;
            if (!GUCBaseVobDef.TryGet(instanceID, out inst))
            {
                throw new Exception("Instance ID not found! " + instanceID);
            }
            SetInstance(inst);

            this.pos = stream.ReadVec3f();
            this.ang = stream.ReadAngles();
        }

        #endregion

        #region Spawn

        public delegate void OnSpawnHandler(BaseVob vob, World world, Vec3f pos, Angles ang);
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
            Spawn(world, this.pos, this.ang);
        }

        /// <summary>
        /// Spawns the Vob in the given world at the given position.
        /// </summary>
        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, this.ang);
        }

        partial void pBeforeSpawn(World world, Vec3f position, Angles angles);
        partial void pAfterSpawn(World world, Vec3f position, Angles angles);
        /// <summary> Spawns the Vob in the given world at the given position & direction. </summary>
        public virtual void Spawn(World world, Vec3f position, Angles angles)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            if (this.instance == null)
                throw new Exception("Vob has no Instance!");

            if (this.ScriptObject == null)
                throw new Exception("Vob has no ScriptObject!");

            if (this.isCreated)
                throw new Exception("Vob is already spawned!");

            Vec3f spawnPos = position.ClampToWorldLimits();
            Angles spawnAng = angles.Clamp();

            this.pBeforeSpawn(world, spawnPos, spawnAng);

            this.pos = spawnPos;
            this.ang = spawnAng;

            world.AddVob(this);
            this.world = world;
            this.isCreated = true;

            this.pAfterSpawn(world, spawnPos, spawnAng);

            this.OnSpawn?.Invoke(this, world, position, angles);
            sOnSpawn?.Invoke(this, world, position, angles);
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

            this.OnDespawn?.Invoke(this);
            sOnDespawn?.Invoke(this);

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
