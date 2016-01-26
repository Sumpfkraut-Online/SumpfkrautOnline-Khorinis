using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class Vob : WorldObject, IVobObj<uint>
    {
        public partial interface IScriptVob : IScriptWorldObject
        {
            void OnWriteSpawnProperties(PacketWriter stream);
            void OnReadSpawnProperties(PacketReader stream);
        }

        public const VobTypes sVobType = VobInstance.sVobType;

        public static readonly VobCollection AllVobs = new VobCollection();
        public static readonly VobDictionary StaticVobs = Vob.AllVobs.GetDict(sVobType);


        public uint ID { get; protected set; }
        public VobInstance Instance { get; protected set; }
        new public IScriptVob ScriptObj { get; protected set; }

        public VobTypes VobType { get { return Instance.VobType; } }

        public string Visual { get { return Instance.Visual; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

        public World World { get; internal set; }
        public bool IsSpawned { get { return World != null; } }

        public Vob(VobInstance instance, IScriptVob scriptObj) : base(scriptObj)
        {
            if (instance == null)
                throw new ArgumentNullException("VobInstance can't be null!");

            this.Instance = instance;
        }

        #region Creation

        partial void pCreate();
        public override void Create()
        {
            pCreate();
            Vob.AllVobs.Add(this);
            base.Create();
        }
        
        public override void Delete()
        {
            this.Despawn();
            Vob.AllVobs.Remove(this);
            base.Delete();
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

        internal virtual void WriteSpawnProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Instance.ID);
            stream.Write(this.pos);
            stream.Write(this.dir);

            this.ScriptObj.OnWriteSpawnProperties(stream);
        }

        internal virtual void ReadSpawnProperties(PacketReader stream)
        {
            this.ID = stream.ReadUInt();
            ushort instanceid = stream.ReadUShort();
            this.Instance = VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
            {
                throw new Exception("Vob.ReadSpawnProperties failed: Instance-ID not found!");
            }
            this.pos = stream.ReadVec3f();
            this.dir = stream.ReadVec3f();

            this.ScriptObj.OnReadSpawnProperties(stream);
        }
    }
}
