using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.Models;

namespace GUC.WorldObjects
{
    public partial class Vob : BaseVob
    {
        public override VobTypes VobType { get { return VobTypes.Vob; } }

        #region ScriptObject

        public partial interface IScriptVob : IScriptBaseVob
        {
        }

        public new IScriptVob ScriptObject
        {
            get { return (IScriptVob)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        new public VobInstance Instance
        {
            get { return (VobInstance)base.Instance; }
            set { base.Instance = value; }
        }

        public Model Model { get { return Instance.Model; } }
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        #endregion
    }
}
