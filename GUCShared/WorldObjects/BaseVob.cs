using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Types;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Log;

namespace GUC.WorldObjects
{
    /// <summary>
    /// The lowermost VobObject.
    /// </summary>
    public abstract partial class BaseVob : GameObject, VobTypeObject
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

        /// <summary>
        /// The Instance of this object.
        /// </summary>
        public BaseVobInstance Instance
        {
            get { return this.instance; }
            set
            {
                if (this.isCreated)
                {
                    throw new Exception("Can't change the Instance of spawned vobs!");
                }
                this.instance = value;
            }
        }
        protected BaseVobInstance instance;

        /// <summary>
        /// The world this Vob is currently in.
        /// </summary>
        public World World
        {
            get { return this.world; }
        }
        protected World world;

        /// <summary>
        /// Checks whether this Vob is spawned.
        /// </summary>
        public bool IsSpawned { get { return this.isCreated; } }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

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
            BaseVobInstance inst;
            if (!BaseVobInstance.TryGet(instanceID, out inst))
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
            if (world == null)
                throw new ArgumentNullException("World is null!");

            if (this.isCreated)
                throw new Exception("Vob is already spawned!");

            this.pos = position;
            this.dir = direction;

            world.AddVob(this);
            this.world = world;

            this.pSpawn();

            this.isCreated = true;
        }

        partial void pSpawn();
        partial void pDespawn();

        /// <summary>
        /// Despawns the Vob.
        /// </summary>
        public virtual void Despawn()
        {
            if (!this.isCreated)
                throw new Exception("Vob isn't spawned!");

            this.world.RemoveVob(this);
            this.world = null;
            pDespawn();

            this.isCreated = false;
        }
        #endregion


        public override string ToString()
        {
            return String.Format("({0}: {1})", this.ID, this.VobType);
        }
    }
}
