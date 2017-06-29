using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
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



        new protected NamedVobDefEffectHandler effectHandler;
        new public NamedVobDefEffectHandler GetEffectHandler () { return effectHandler; }

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
            SetObjName("NamedVobDef");
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.NamedVobDefEffectHandler(null, this);
            pConstruct();
        }

    }

}
