using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class GUCMobInterDef : GUCMobDef
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobInter; } }

        #region ScriptObject

        public partial interface IScriptMobInterInstance : IScriptMobInstance
        {
        }

        public new IScriptMobInterInstance ScriptObject { get { return (IScriptMobInterInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GUCMobInterDef(IScriptMobInterInstance scriptObject) : base(scriptObject)
        {
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
