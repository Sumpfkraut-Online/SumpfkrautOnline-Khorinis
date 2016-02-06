using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.Enumeration;
using GUC.Types;

namespace GUC.WorldObjects
{
    public abstract class VobObj : WorldObject<VobObj.IScriptVobObj>, IVobObj<uint>
    {
        public partial interface IScriptVobObj
        {
            void OnWriteSpawnProperties(PacketWriter stream);
            void OnReadSpawnProperties(PacketReader stream);
        }

        public static readonly VobCollection AllVobs = new VobCollection();

        protected VobObj(IScriptVobObj scriptObj) : base(scriptObj)
        {
        }

        protected VobObj(PacketReader stream, IScriptVobObj scriptObj) : base(scriptObj)
        {
            this.ReadSpawnProperties(stream);
        }

        public uint ID { get; private set; }
        public abstract VobTypes VobType { get; }

        protected Vec3f pos = new Vec3f(0, 0, 0);
        protected Vec3f dir = new Vec3f(0, 0, 1);

        public World World { get; internal set; }
        public bool IsSpawned { get { return World != null; } }



        internal virtual void WriteSpawnProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.pos);
            stream.Write(this.dir);

            this.ScriptObj.OnWriteSpawnProperties(stream);
        }

        internal virtual void ReadSpawnProperties(PacketReader stream)
        {
            this.ID = stream.ReadUInt();
            this.pos = stream.ReadVec3f();
            this.dir = stream.ReadVec3f();

            this.ScriptObj.OnReadSpawnProperties(stream);
        }
    }

    public abstract class VobObj<TInstance> : VobObj where TInstance : VobObjInstance
    {
        public TInstance Instance { get; private set; }

        public readonly static VobTypes sVobType = (VobTypes)typeof(TInstance).GetField("sVobType").GetRawConstantValue();
        public readonly static VobDictionary Vobs = AllVobs.GetDict(sVobType);

        internal override void WriteSpawnProperties(PacketWriter stream)
        {
            base.WriteSpawnProperties(stream);

            stream.Write(this.Instance.ID);
        }

        internal override void ReadSpawnProperties(PacketReader stream)
        {
            base.ReadSpawnProperties(stream);

            ushort instanceid = stream.ReadUShort();
            this.Instance = VobInstance.AllInstances.Get(instanceid);
            if (this.Instance == null)
            {
                throw new Exception("Vob.ReadSpawnProperties failed: Instance-ID not found!");
            }
        }
    }
}
