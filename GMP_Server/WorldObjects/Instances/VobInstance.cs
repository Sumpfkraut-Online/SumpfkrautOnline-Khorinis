using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Log;
using GUC.Server.WorldObjects.Collections;

namespace GUC.Server.WorldObjects.Instances
{
    public class VobInstance : ServerObject, IVobObj<ushort>
    {
        public readonly static VobTypes sVobType = VobTypes.Vob;
        public readonly static InstanceDictionary Instances = Network.Server.Instances.GetDict(sVobType);

        public ushort ID { get; internal set; }
        public string InstanceName { get; internal set; }

        public VobTypes VobType { get; protected set; }

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


        public VobInstance(ushort id, string instanceName, object scriptObject) : base (scriptObject)
        {
            this.ID = id;
            this.InstanceName = instanceName.ToUpper();
            this.VobType = sVobType;
        }

        static ushort idCount = 1;
        public override void Create()
        {
            if (ID == 0) //seek a new ID for this instance
            {
                for (int i = 0; i < ushort.MaxValue; i++)
                {
                    if (idCount != 0)
                    {
                        if (Network.Server.Instances.Get(idCount) == null)
                        {
                            ID = idCount++;
                            break;
                        }
                    }
                    idCount++;
                }

                if (ID == 0) //no free id found
                {
                    Logger.LogError("{0} creation failed: Maximum reached: {1}", this.GetType(), ushort.MaxValue);
                    return;
                }
            }

            Network.Server.Instances.Add(this);
            base.Create();
        }

        public override void Delete()
        {
            Network.Server.Instances.Remove(this);
            base.Delete();
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
