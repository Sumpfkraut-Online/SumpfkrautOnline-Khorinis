using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{
    public class SDestInitializer : ExtendedObject
    {

            protected SDestInitializer () { }

            public static void Init ()
            {
                Type t = typeof(DestInitializer);

                MakeLogStatic(t, "Initializing... " + typeof(SDestInit_Effect).Name);

                MakeLogStatic(t, "Initializing... " + typeof(SDestInit_World).Name);

                MakeLogStatic(t, "Initializing... " + typeof(SDestInit_Vob).Name);
                MakeLogStatic(t, "Initializing... " + typeof(SDestInit_NamedVob).Name);
                MakeLogStatic(t, "Initializing... " + typeof(SDestInit_Item).Name);
                MakeLogStatic(t, "Initializing... " + typeof(SDestInit_NPC).Name);
            }

    }
}
