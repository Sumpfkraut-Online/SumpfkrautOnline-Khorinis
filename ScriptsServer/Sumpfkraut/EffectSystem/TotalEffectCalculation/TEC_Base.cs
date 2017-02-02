using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.TotalEffectCalculation
{

    public abstract class TEC_Base : ExtendedObject
    {

        new public static readonly string _staticName = "TEC_Base (static)";

        protected static List<TEC_Base> tecClasses = new List<TEC_Base>();

        public abstract void Init ();

    }

}
