using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    /**
     *   Class from which all spellitems are instatiated (which are handled by the serverscript).
     */
    class SpellDef : VobDef
    {

        #region dictionaries

        private static Dictionary<int, SpellDef> defById = new Dictionary<int, SpellDef>();
        private static Dictionary<string, SpellDef> defByName = new Dictionary<string, SpellDef>();

        #endregion



        #region standard attributes

        protected static String _staticName = "SpellDef (static)";
        protected String _objName = "SpellDef (default)";

        #endregion



        #region constructors

        #endregion

    }
}
