using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    /// <summary>
    /// Used to trigger static constructor initialization of ChangeInit-classes, solely.
    /// </summary>
    public partial class ChangeInitializer : ExtendedObject
    {

        protected ChangeInitializer () { }

        /// <summary>
        /// Trigger static constructor initialization of ChangeInit-classes.
        /// </summary>
        public static void Init ()
        {
            Type t = typeof(ChangeInitializer);

            // general
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_Effect).Name);

            // world-wide / global
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_World).Name);

            // vob-specifics
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_Vob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_NamedVob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_Item).Name);
            MakeLogStatic(t, "Initializing... " + typeof(ChangeInit_NPC).Name);

            // ...
        }

    }

}
