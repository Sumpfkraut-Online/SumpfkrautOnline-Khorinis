using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class ChangeInitializer : ExtendedObject
    {

        new public static readonly string _staticName = "ChangeInitializer";

        protected ChangeInitializer () { }

        public static void Init ()
        {
            Type t = typeof(ChangeInitializer);

            MakeLogStatic(t, "Initializing... " + ChangeInit_Effect._staticName);
            MakeLogStatic(t, "Initializing... " + ChangeInit_Vob._staticName);
            // ...
        }

    }

}
