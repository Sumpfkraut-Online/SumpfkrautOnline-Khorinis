using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Types;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    /// <summary>
    /// The lowermost VobObject.
    /// </summary>
    public abstract partial class BaseVob : GameObject
    {
        /// <summary>
        /// The VobType of this Vob.
        /// </summary>
        public abstract VobTypes VobType { get; }

        #region ScriptObject

        /// <summary>
        /// The underlying ScriptObject interface for all Vobs.
        /// </summary>
        public partial interface IScriptBaseVob : IScriptGameObject
        {
        }

        /// <summary>
        /// The ScriptObject of this Vob.
        /// </summary>
        public new IScriptBaseVob ScriptObject
        {
            get { return (IScriptBaseVob)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        public override int ID
        {
            get {  return base.ID; }
            set
            {
                if (this.IsSpawned)
                {
                    throw new Exception("Can't change the ID of spawned vobs!");
                }
                base.ID = value;
            }
        }

        /// <summary>
        /// The Instance of this object.
        /// </summary>
        public BaseVobInstance Instance
        {
            get { return this.instance; }
            set
            {
                if (this.IsSpawned)
                {
                    throw new Exception("Can't change the Instance of spawned vobs!");
                }
                this.instance = value;
            }
        }
        BaseVobInstance instance;

        /// <summary>
        /// The world this Vob is currently spawned in.
        /// </summary>
        public World World
        {
            get { return this.world; }
            internal set { this.world = value; }
        }
        World world;

        /// <summary>
        /// Checks whether this Vob is spawned. (World != null)
        /// </summary>
        public bool IsSpawned { get { return world != null; } }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);
        
        /// <summary>
        /// If the Vob is 'static' it will not be communicated via the GUC.
        /// </summary>
        public bool IsStatic
        {
            get { return isStatic; }
        }
        bool isStatic = false;

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)this.ID);
            stream.Write((ushort)this.Instance.ID); // MAX_ID
            stream.Write(this.pos);
            stream.Write(this.dir);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();

            int instanceID = stream.ReadUShort(); // MAX_ID
            BaseVobInstance inst = InstanceCollection.Get(instanceID);
            if (inst == null)
            {
                throw new Exception("Instance ID not found! " + instanceID);
            }
            else if (inst.VobType != this.VobType)
            {
                throw new Exception("Instance is for a different VobType!");
            }
            this.instance = inst;
            
            this.pos = stream.ReadVec3f();
            this.dir = stream.ReadVec3f();
        }

        #endregion

        #region Spawn

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

        /// <summary>
        /// Spawns the Vob in the given world at the given position & direction.
        /// </summary>
        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            this.pos = position;
            this.dir = direction;
            world.SpawnVob(this);
        }

        /// <summary>
        /// Despawns the Vob.
        /// </summary>
        public virtual void Despawn()
        {
            if (this.IsSpawned)
            {
                this.World.DespawnVob(this);
            }
        }
        #endregion
    }
}
