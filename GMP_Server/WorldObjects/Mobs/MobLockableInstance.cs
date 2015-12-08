using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Mobs
{
    public abstract class MobLockableInstance : MobInterInstance
    {
        #region Properties
        
        public string OnTryOpenClientFunc = "";

        #endregion

        public MobLockableInstance(string instanceName, object scriptObject)
            : base(0, instanceName, scriptObject)
        {
        }

        public MobLockableInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        public new static Action<MobLockableInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTryOpenClientFunc);

            if (MobLockableInstance.OnWriteProperties != null)
            {
                MobLockableInstance.OnWriteProperties(this, stream);
            }
        }
    }
}
