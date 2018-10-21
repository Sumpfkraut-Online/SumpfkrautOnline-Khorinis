using GUC.Utilities;
using System;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Class with the sole purpose to initialize registration
    /// of ChangeDestinations by triggering static constructors
    /// of destination classes.
    /// </summary>
    public class SDestInitializer : ExtendedObject
    {

        /// <summary>
        /// Dummy.
        /// </summary>
        protected SDestInitializer () { }

        /// <summary>
        /// Initialize registration process for ChangeDestinations.
        /// This triggers static constructors of DestInit-classes.
        /// </summary>
        public static void Init ()
        {
            Type t = typeof(SDestInitializer);

            MakeLogStatic(t, "Initializing... " + typeof(SDestInit_Effect).Name);

            MakeLogStatic(t, "Initializing... " + typeof(SDestInit_World).Name);

            MakeLogStatic(t, "Initializing... " + typeof(SDestInit_Vob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(SDestInit_NamedVob).Name);
            MakeLogStatic(t, "Initializing... " + typeof(SDestInit_Item).Name);
            MakeLogStatic(t, "Initializing... " + typeof(SDestInit_NPC).Name);
        }

    }
}
