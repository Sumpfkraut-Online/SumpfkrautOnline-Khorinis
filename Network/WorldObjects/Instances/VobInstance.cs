using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class VobInstance : WorldObject, IVobObj<ushort>
    {
        public partial interface IScriptVobInstance : IScriptWorldObject
        {
            void OnWriteProperties(PacketWriter stream);
            void OnReadProperties(PacketReader stream);
        }

        public const VobTypes sVobType = VobTypes.Vob;
        public readonly static InstanceCollection AllInstances = new InstanceCollection();
        public readonly static InstanceDictionary VobInstances = AllInstances.GetDict(sVobType);

        public ushort ID { get; protected set; }
        public virtual VobTypes VobType { get { return sVobType; } }
        new public IScriptVobInstance ScriptObj { get; protected set; }

        public VobInstance(PacketReader stream, IScriptVobInstance scriptObj) : base(scriptObj)
        {
            this.ScriptObj = scriptObj;
            ReadProperties(stream);
        }

        #region Properties

        protected string visual = "";
        public virtual string Visual
        {
            get { return visual; }
            set { visual = value.Trim().ToUpper(); }
        }

        protected bool cddyn = true;
        public virtual bool CDDyn
        {
            get { return cddyn; }
            set { cddyn = value; }
        }

        protected bool cdstatic = true;
        public virtual bool CDStatic
        {
            get { return cdstatic; }
            set { cdstatic = value; }
        }

        #endregion
        
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
            stream.Write(this.Visual);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);

            this.ScriptObj.OnWriteProperties(stream);
        }

        internal virtual void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
            this.Visual = stream.ReadString();
            this.CDDyn = stream.ReadBit();
            this.CDStatic = stream.ReadBit();

            this.ScriptObj.OnReadProperties(stream);
        }
    }
}
