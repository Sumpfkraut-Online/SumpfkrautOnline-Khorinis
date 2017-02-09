using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class DestInitializer : ExtendedObject
    {

        new public static readonly string _staticName = "DestInitializer";

        protected DestInitializer () { }

        public static void Init ()
        {
            Type t = typeof(DestInitializer);

            MakeLogStatic(t, "Initializing... " + DestInit_Effect._staticName);
            MakeLogStatic(t, "Initializing... " + DestInit_Vob._staticName);
            // ...
        }

    }

}
