using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Client.WorldObjects.Instances
{
    public class MobInstance : VobInstance
    {
        new public readonly static Enumeration.VobType sVobType = Enumeration.VobType.Mob;
        new public readonly static Collections.InstanceDictionary Instances = Network.Server.sInstances.GetDict(sVobType);

        #region Properties

        public string FocusName = "";

        #endregion

        public MobInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobInstance(ushort ID, string instanceName, object scriptObject) 
            : base (ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }

        public new static Action<MobInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FocusName);

            if (MobInstance.OnWriteProperties != null)
            {
                MobInstance.OnWriteProperties(this, stream);
            }
        }
    }
}
