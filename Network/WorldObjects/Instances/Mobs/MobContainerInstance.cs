using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobContainerInstance : MobLockableInstance
    {
        public partial interface IScriptMobContainerInstance : IScriptMobLockableInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobContainer;
        public override VobTypes VobType { get { return sVobType; } }
        public static readonly InstanceDictionary MobContainerInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobContainerInstance ScriptObj { get; protected set; }

        public MobContainerInstance(PacketReader stream, IScriptMobContainerInstance scriptObj) : base(stream, scriptObj)
        {
        }
    }
}
