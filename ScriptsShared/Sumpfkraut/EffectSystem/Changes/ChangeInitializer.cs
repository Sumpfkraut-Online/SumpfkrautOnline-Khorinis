using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public partial class ChangeInitializer : ExtendedObject
    {

        new public static readonly string _staticName = "ChangeInitializer";

        protected ChangeInitializer () { }

        public static void Init ()
        {
            Type t = typeof(ChangeInitializer);

            MakeLogStatic(t, "Initializing... " + ChangeInit_Effect._staticName);

            MakeLogStatic(t, "Initializing... " + ChangeInit_World._staticName);

            MakeLogStatic(t, "Initializing... " + ChangeInit_Vob._staticName);
            MakeLogStatic(t, "Initializing... " + ChangeInit_NamedVob._staticName);
            MakeLogStatic(t, "Initializing... " + ChangeInit_Item._staticName);
            MakeLogStatic(t, "Initializing... " + ChangeInit_NPC._staticName);
            // ...
        }

    }

}
