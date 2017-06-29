using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{

    public partial class NamedVobDef : VobDef
    {

        new public static readonly string _staticName = "NamedVobDef (s)";



        protected string name = "";
        /// <summary>The standard name of this named vob.</summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value ?? ""; }
        }



        partial void pConstruct();
        public NamedVobDef()
        {
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.NamedVobInstEffectHandler(null, this);
            pConstruct();
        }

    }

}
