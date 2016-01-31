using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInterInstance : MobInstance
    {
        public partial interface IScriptMobInterInstance : IScriptMobInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobInter;
        public override VobTypes VobType { get { return sVobType; } }
        public static readonly InstanceDictionary MobInterInstances = VobInstance.AllInstances.GetDict(sVobType);

        public MobInterInstance(PacketReader stream, IScriptMobInterInstance scriptObj) : base(stream, scriptObj)
        {
        }

        #region Properties

        public string OnTriggerClientFunc = "";

        #endregion

        new public IScriptMobInterInstance ScriptObj { get; protected set; }

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTriggerClientFunc = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTriggerClientFunc);
        }
    }
}
