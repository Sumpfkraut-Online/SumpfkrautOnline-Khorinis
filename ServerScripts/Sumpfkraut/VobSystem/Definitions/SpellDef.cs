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
        //private static Dictionary<string, SpellDef> defByName = new Dictionary<string, SpellDef>();

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "SpellDef (static)";
        new protected String _objName = "SpellDef (default)";

        #endregion



        #region constructors

        #endregion



        #region dictionary-methods

        public static bool Add (SpellDef def)
        {
            int id = def.getId();

            if (id < 1)
            {
                MakeLogWarningStatic(typeof(SpellDef), 
                    "Prevented attempt of adding a definition to to dictionary: "
                     + "An invalid id < 1 was provided!");
                return false;
            }

            if (defById.ContainsKey(id))
            {
                MakeLogWarningStatic(typeof(SpellDef), 
                    String.Format("Prevented attempt of adding a definition to dictionary:"
                        + " The {0}={1} is already taken!", "id", id));
                return false;
            }
            
            defById.Add(id, def);
            return true;
        }

        public static bool ContainsId (int id)
        {
            if (defById.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ContainsDefinition (SpellDef def)
        {
            return defById.ContainsValue(def);
        }

        public static bool Remove (int id)
        {
            SpellDef def;
            defById.TryGetValue(id, out def);

            if (def != null)
            {
                defById.Remove(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryGetValue (int id, out SpellDef def)
        {
            return defById.TryGetValue(id, out def);
        }

        #endregion

    }
}
