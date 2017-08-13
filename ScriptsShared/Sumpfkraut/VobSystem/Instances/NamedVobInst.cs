using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{

    public partial class NamedVobInst : VobInst, Vob.IScriptVob
    {
        #region Constructors

        partial void pConstruct();
        public NamedVobInst()
        {
            pConstruct();
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new NamedVobInstEffectHandler(null, null, this);
        }

        #endregion

        #region Properties
        
        new public NamedVobInstEffectHandler EffectHandler () { return (NamedVobInstEffectHandler)base.EffectHandler; }

        new public NamedVobDef Definition { get { return (NamedVobDef)base.Definition; } set { base.Definition = value; } }

        #endregion
    }

}
