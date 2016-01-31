using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobWheelInstance : MobInterInstance
    {
        public partial interface IScriptMobWheelInstance : IScriptMobInterInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobWheel;
        public override VobTypes VobType { get { return sVobType; } }
        public static readonly InstanceDictionary MobWheelInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobWheelInstance ScriptObj { get; protected set; }

        public MobWheelInstance(PacketReader stream, IScriptMobWheelInstance scriptObj) : base(stream, scriptObj)
        {
        }
    }
}
