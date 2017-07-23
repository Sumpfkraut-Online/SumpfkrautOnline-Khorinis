using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInitializer : ExtendedObject
    {

        protected DestInitializer () { }

        public static void Init ()
        {
            Type t = typeof(DestInitializer);

            MakeLogStatic(t, "Initializing... " + typeof(DestInit_Effect).Name);

            MakeLogStatic(t, "Initializing... " + typeof(DestInit_World).Name);

            MakeLogStatic(t, "Initializing... " + typeof(DestInit_Vob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(DestInit_NamedVob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(DestInit_Item).Name);
            MakeLogStatic(t, "Initializing... " + typeof(DestInit_NPC).Name);
            // ...
        }

    }

}
