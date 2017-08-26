using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{

    public partial class NamedVobDef : VobDef
    {
        #region Constructor
        
        partial void pConstruct();
        public NamedVobDef()
        {
            pConstruct();
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new NamedVobDefEffectHandler(null, null, this);
        }

        #endregion

        #region Properties
        
        new public NamedVobDefEffectHandler EffectHandler { get { return (NamedVobDefEffectHandler)base.EffectHandler; } }

        protected string name = "";
        /// <summary>The standard name of this named vob.</summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value ?? ""; }
        }

        #endregion
    }

}
