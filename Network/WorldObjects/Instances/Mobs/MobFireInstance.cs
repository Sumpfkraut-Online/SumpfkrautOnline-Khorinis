using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobFireInstance : MobInterInstance
    {
        public partial interface IScriptMobFireInstance : IScriptMobInterInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.MobFire;
        public override VobTypes VobType { get { return sVobType; } }
        public static readonly InstanceDictionary MobFireInstances = VobInstance.AllInstances.GetDict(sVobType);

        public MobFireInstance(PacketReader stream, IScriptMobFireInstance scriptObj) : base(stream, scriptObj)
        {
        }

        #region Properties

        public string FireVobTree = "";

        #endregion

        new public IScriptMobFireInstance ScriptObj { get; protected set; }

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FireVobTree = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FireVobTree);
        }
    }
}
