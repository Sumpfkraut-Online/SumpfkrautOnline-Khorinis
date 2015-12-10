using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.WorldObjects.Instances
{
    public class MobInterInstance : MobInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.MobInter;
        new public readonly static Collections.InstanceDictionary Instances = Network.Server.sInstances.GetDict(sVobType);

        #region Properties

        public string OnTriggerFunc = "";
        public string OnTriggerClientFunc = "";
        public bool IsMulti = false;

        #endregion

        public MobInterInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobInterInstance(ushort ID, string instanceName, object scriptObject) 
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }

        public new static Action<MobInterInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTriggerFunc);
            stream.Write(OnTriggerClientFunc);
            stream.Write(IsMulti);

            if (MobInterInstance.OnWriteProperties != null)
            {
                MobInterInstance.OnWriteProperties(this, stream);
            }
        }
    }
}
