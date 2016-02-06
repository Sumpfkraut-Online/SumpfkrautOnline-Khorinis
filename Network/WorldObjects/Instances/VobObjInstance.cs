using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using GUC.Network;
using GUC.Enumeration;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class VobObjInstance : WorldObject<VobObjInstance.IScriptVobObjInstance>, IVobObj<ushort>
    {
        public partial interface IScriptVobObjInstance
        {
            void OnWriteProperties(PacketWriter stream);
            void OnReadProperties(PacketReader stream);
        }

        public readonly static InstanceCollection AllInstances = new InstanceCollection();

        public ushort ID { get; private set; }
        public abstract VobTypes VobType { get; }

        protected VobObjInstance(PacketReader stream, IScriptVobObjInstance scriptObj) : base(scriptObj)
        {
            this.ReadProperties(stream);
        }

        public override void Create()
        {
            base.Create();
            AllInstances.Add(this);
        }

        public override void Delete()
        {
            base.Delete();
            AllInstances.Remove(this);
        }

        internal virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            this.ScriptObj.OnWriteProperties(stream);
        }

        internal virtual void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
            this.ScriptObj.OnReadProperties(stream);
        }
    }



    public abstract partial class VobObjInstance<TVobTypeDummy>
    {
        public readonly static VobTypes sVobType = (VobTypes)typeof(TInstance).GetField("sVobType").GetRawConstantValue();
    }
}
