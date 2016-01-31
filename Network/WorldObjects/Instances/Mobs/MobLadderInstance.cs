using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public partial class MobLadderInstance : MobInterInstance
    {
        public partial interface IScriptMobLadderInstance : IScriptMobInterInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobLadder;
        public override VobTypes VobType { get { return sVobType; } }
        public static readonly InstanceDictionary MobLadderInstances = VobInstance.AllInstances.GetDict(sVobType);

        new public IScriptMobLadderInstance ScriptObj { get; protected set; }

        public MobLadderInstance(PacketReader stream, IScriptMobLadderInstance scriptObj) : base(stream, scriptObj)
        {
        }
    }
}
