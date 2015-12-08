using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public class MobInstance : VobInstance
    {
        new public readonly static InstanceManager<MobInstance> Table = new InstanceManager<MobInstance>();

        #region Properties

        public string FocusName = "";

        #endregion

        public MobInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobInstance(ushort ID, string instanceName, object scriptObject) 
            : base (ID, instanceName, scriptObject)
        {
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
