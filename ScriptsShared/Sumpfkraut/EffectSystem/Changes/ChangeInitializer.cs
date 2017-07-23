using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInitializer : ExtendedObject
    {

        protected ChangeInitializer () { }

        public static void Init ()
        {
            Type t = typeof(ChangeInitializer);

            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_Effect).Name);

            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_World).Name);

            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_Vob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_NamedVob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_Item).Name);
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_NPC).Name);
            // ...
        }

    }

}
