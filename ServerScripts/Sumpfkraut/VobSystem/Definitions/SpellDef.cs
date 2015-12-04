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
    public class SpellDef : VobDef
    {

        #region dictionaries

        protected static Dictionary<int, SpellDef> defById = new Dictionary<int, SpellDef>();
        protected static Dictionary<string, SpellDef> defByCodeName = new Dictionary<string, SpellDef>();

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "SpellDef (static)";
        new protected String _objName = "SpellDef (default)";

        new protected static Type _type = typeof(SpellDef);

        #endregion



        #region constructors

        #endregion



        #region dictionary-methods

        public static bool Add(SpellDef def)
        {
            return Add(_type, def);
        }

        public static bool ContainsCodeName(String codeName)
        {
            return ContainsCodeName(_type, codeName);
        }

        public static bool ContainsId(int id)
        {
            return ContainsId(_type, id);
        }

        public static bool ContainsDefinition(VobDef def)
        {
            return ContainsDefinition(_type, def);
        }

        public static bool RemoveCodeName(String codeName)
        {
            return RemoveCodeName(_type, codeName);
        }

        public static bool RemoveId(int id)
        {
            return RemoveId(_type, id);
        }

        public static bool TryGetValueByCodeName(String codeName, out SpellDef def)
        {
            VobDef tempDef;
            bool result = TryGetValueByCodeName(_type, codeName, out tempDef);
            def = (SpellDef)tempDef;
            return result;
        }

        public static bool TryGetValueById(int id, out SpellDef def)
        {
            VobDef tempDef;
            bool result = TryGetValueById(_type, id, out tempDef);
            def = (SpellDef)tempDef;
            return result;
        }

        #endregion

    }
}
