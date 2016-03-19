using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobInterInstance : MobInstance
    {
        public override VobTypes VobType { get { return VobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInterInstance : IScriptMobInstance
        {
        }

        public new IScriptMobInterInstance ScriptObject
        {
            get { return (IScriptMobInterInstance)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        public string OnTriggerClientFunc = "";

        #endregion
        
        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.OnTriggerClientFunc = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(OnTriggerClientFunc);
        }

        #endregion
    }
}
