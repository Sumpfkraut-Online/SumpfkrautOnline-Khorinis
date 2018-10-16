using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobInstance : VobInstance
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.Mob; } }

        #region ScriptObject

        public partial interface IScriptMobInstance : IScriptVobInstance
        {
        }

        public new IScriptMobInstance ScriptObject { get { return (IScriptMobInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobInstance(IScriptMobInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        string focusName = "";
        public string FocusName
        {
            get { return this.focusName; }
            set { this.focusName = value ?? ""; }
        }

        #endregion
        
        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FocusName = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FocusName);
        }

        #endregion
    }
}
