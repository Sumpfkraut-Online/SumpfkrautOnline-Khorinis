using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Log;
using GUC.Types;
using GUC.Enumeration;

namespace GUC.WorldObjects
{
    public abstract class BaseVob : WorldObject
    {
        public abstract VobTypes VobType { get; }

        #region ScriptObject

        public partial interface IScriptBaseVob : IScriptWorldObject
        {
        }

        public new IScriptBaseVob ScriptObject
        {
            get { return (IScriptBaseVob)base.ScriptObject; }
        }

        #endregion

        #region Properties
        public BaseInstance Instance { get; private set; }

        internal int WorldDictID = -1;
        public int WorldID { get; internal set; }
        public World World { get; internal set; }
        public bool IsSpawned { get { return World != null; } }

        internal BaseVob Next;
        internal BaseVob Last;

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new WorldObject with the given ID or searches a new one when needed.
        /// </summary>
        protected BaseVob(IScriptWorldObject scriptObject, BaseInstance instance, int id = -1) : base(scriptObject, id)
        {
            if (instance == null)
                throw new ArgumentNullException("Instance is null!");

            //if (!InstanceCollection.Contains(instance))
            // throw new Exception

            this.Instance = instance;
        }

        /// <summary>
        /// Creates a new WorldObject by reading a networking stream.
        /// </summary>
        public BaseVob(IScriptWorldObject scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)this.Instance.ID); // MAX_ID
            stream.Write(this.pos);
            stream.Write(this.dir);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            int instanceID = stream.ReadUShort(); // MAX_ID
            //this.Instance = InstanceCollection.Get(instanceID);
            this.pos = stream.ReadVec3f();
            this.dir = stream.ReadVec3f();
        }

        #endregion

        #region Spawn
        public void Spawn(World world)
        {
            Spawn(world, this.pos, this.dir);
        }

        public void Spawn(World world, Vec3f position)
        {
            Spawn(world, position, this.dir);
        }

        public virtual void Spawn(World world, Vec3f position, Vec3f direction)
        {
            this.pos = position;
            this.dir = direction;
            world.SpawnVob(this);
        }

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
