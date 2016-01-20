using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Server.WorldObjects.Instances
{
    public class MobFireInstance : MobInterInstance
    {
        new public readonly static Enumeration.VobTypes sVobType = Enumeration.VobTypes.MobFire;
        new public readonly static Collections.InstanceDictionary Instances = Network.Server.Instances.GetDict(sVobType);

        #region Properties

        public string FireVobTree = "";

        #endregion

        public MobFireInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public MobFireInstance(ushort ID, string instanceName, object scriptObject) 
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }

        public new static Action<MobFireInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FireVobTree);

            if (MobFireInstance.OnWriteProperties != null)
            {
                MobFireInstance.OnWriteProperties(this, stream);
            }
        }
    }
}
