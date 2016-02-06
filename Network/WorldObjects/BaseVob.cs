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
    public abstract partial class BaseVob : WorldObject
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
        new public BaseVobInstance Instance { get { return (BaseVobInstance)base.Instance; } }
        
        public World World { get; internal set; }
        public bool IsSpawned { get { return World != null; } }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        protected BaseVob(IScriptBaseVob scriptObject, BaseVobInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
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
            stream.Write(this.pos);
            stream.Write(this.dir);
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
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
