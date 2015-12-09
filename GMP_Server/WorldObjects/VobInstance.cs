using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Server.Log;
using System.Security.Cryptography;

namespace GUC.Server.WorldObjects
{
    public class VobInstance : ServerObject, IVobClass
    {      
        public readonly static InstanceManager<VobInstance> Table = new InstanceManager<VobInstance>();

        public ushort ID { get; internal set; }
        public string InstanceName { get; internal set; }

        protected VobType vobType;
        public VobType GetVobType() { return vobType; }

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

        public VobInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }


        public VobInstance(ushort ID, string instanceName, object scriptObject) : base (scriptObject)
        {
            this.ID = ID;
            this.InstanceName = instanceName.ToUpper();
            this.vobType = VobType.Vob;
        }

        /// <summary> Use this to send additional information to the clients. The base properties are already written! </summary>
        public static Action<VobInstance, PacketWriter> OnWriteProperties;
        internal virtual void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.ID);
            stream.Write(this.Visual);
            stream.Write(this.CDDyn);
            stream.Write(this.CDStatic);

            if (VobInstance.OnWriteProperties != null)
                VobInstance.OnWriteProperties(this, stream);
        }
    }
}
