using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInstance : VobInstance
    {
        public partial interface IScriptMobInstance : IScriptVobInstance
        {
        }

        new public const VobTypes sVobType = VobTypes.Mob;
        public static readonly InstanceDictionary MobInstances = VobInstance.AllInstances.GetDict(sVobType);

        #region Properties

        public string FocusName = "";

        #endregion

        new public IScriptMobInstance ScriptObj { get; protected set; }

        internal override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FocusName = stream.ReadString();
        }

        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FocusName);
        }
    }
}
