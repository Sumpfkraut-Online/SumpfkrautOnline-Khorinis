using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobSwitchInstance : MobInterInstance
    {
        public partial interface IScriptMobSwitchInstance : IScriptMobInterInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobSwitch;
        public override VobTypes VobType { get { return sVobType; } }
        public static readonly InstanceDictionary MobSwitchInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobSwitchInstance ScriptObj { get; protected set; }

        public MobSwitchInstance(PacketReader stream, IScriptMobSwitchInstance scriptObj) : base(stream, scriptObj)
        {
        }
    }
}
