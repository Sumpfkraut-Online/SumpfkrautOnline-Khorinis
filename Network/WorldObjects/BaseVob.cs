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
        /// The underyling ScriptObject interface for all Vobs.
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
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Instance of this object.
        /// </summary>
        public BaseVobInstance Instance { get { return instance; } }
        BaseVobInstance instance;

        /// <summary>
        /// The world this Vob is currently spawned in.
        /// </summary>
        public World World { get { return this.world; } internal set { this.world = value; } }
        World world;

        /// <summary>
        /// Checks whether this Vob is spawned. (World != null)
        /// </summary>
        public bool IsSpawned { get { return world != null; } }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);
        
        /// <summary>
        /// If the Vob is 'static' it's not communicated via the Cell-System but permanently cached on the Client.
        /// Static Vobs are saved in WorldInstances and will be the same in Worlds with the same WorldInstance.
        /// </summary>
        public bool IsStatic
        {
            get { return isStatic; }
        }
        bool isStatic = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        protected BaseVob(IScriptBaseVob scriptObject, BaseVobInstance instance, int id = -1) : base(scriptObject, id)
        {
            if (instance == null)
                throw new ArgumentNullException("Instance is null!");

            if (InstanceCollection.Get(instance.ID) != instance)
                throw new ArgumentException("Instance is not in the collection!");

            this.instance = instance;
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        protected BaseVob(IScriptBaseVob scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write((ushort)this.Instance.ID); // MAX_ID
            stream.Write(this.pos);
            stream.Write(this.dir);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

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
