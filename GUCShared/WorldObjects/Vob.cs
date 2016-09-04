using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.Models;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class Vob : VobGuiding.GuidedVob
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

        /// <summary> The model of this vob's instance. </summary>
        public Model Model { get { return Instance.Model; } }
        
        public bool CDDyn { get { return Instance.CDDyn; } }
        public bool CDStatic { get { return Instance.CDStatic; } }

        #endregion
    }
}
