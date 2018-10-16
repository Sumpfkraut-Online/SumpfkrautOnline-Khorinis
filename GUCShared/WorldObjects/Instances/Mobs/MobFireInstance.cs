using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobFireInstance : MobInterInstance
    {
        public override GUCVobTypes VobType { get { return GUCVobTypes.MobFire; } }

        #region ScriptObject

        public partial interface IScriptMobFireInstance : IScriptMobInterInstance
        {
        }

        public new IScriptMobFireInstance ScriptObject { get { return (IScriptMobFireInstance)base.ScriptObject; } }

        #endregion

        #region Constructors

        public MobFireInstance(IScriptMobFireInstance scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        public string FireVobTree = "";

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.FireVobTree = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(FireVobTree);
        }

        #endregion
    }
}
