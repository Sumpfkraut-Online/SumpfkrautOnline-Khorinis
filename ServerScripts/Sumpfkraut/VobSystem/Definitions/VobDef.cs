using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    abstract class VobDef : ScriptObject
    {

        #region dictionaries

        protected static Dictionary<Type, Dictionary<int, VobDef>> allDefById;
        protected static Dictionary<Type, Dictionary<string, VobDef>> allDefByCodename;

        // override them in child classes
        protected static Dictionary<int, VobDef> defById = new Dictionary<int, VobDef>();
        protected static Dictionary<string, VobDef> defByCodename = new Dictionary<string, VobDef>();

        #endregion


        #region standard attributes

        new public static readonly String _staticName = "VobDef (static)";
        new protected String _objName = "VobDef (default)";

        protected static Type _type = typeof(VobDef);

        protected int id;
        public int GetId () { return this.id; }
        public void SetId (int id) { this.id = id; }

        protected String codename;
        public String GetCodeName () { return this.codename; }
        public void SetCodeName (String codename) { this.codename = codename; }

        protected DateTime changeDate;
        public DateTime GetChangeDate () { return this.changeDate; }
        public void SetChangeDate (DateTime changeDate) { this.changeDate = changeDate; }
        public void SetChangeDate (string changeDate)
        {
            DateTime dt;
            if (Sumpfkraut.Utilities.DateTimeUtil.TryStringToDateTime(changeDate, out dt))
            {
                this.changeDate = dt;
            }
        }

        protected DateTime creationDate;
        public DateTime GetCreationDate () { return this.creationDate; }
        public void SetCreationDate (DateTime creationDate) { this.creationDate = creationDate; }
        public void SetCreationDate (string creationDate)
        {
            DateTime dt;
            if (Sumpfkraut.Utilities.DateTimeUtil.TryStringToDateTime(creationDate, out dt))
            {
                this.creationDate = dt;
            }
        }

        #endregion



        #region constructors

        public VobDef ()
            : base("VobDef (default)")
        { }

        #endregion



        #region dictionary-methods

        // checks whether necessary dictionaries are already registeres and 
        // registers them if not done already
        public static bool RegisterDictionaries (Type type)
        {
            if (type.IsSubclassOf(typeof(VobDef)))
            {
                if (!allDefById.ContainsKey(type))
                {
                    allDefById.Add(type, new Dictionary<int, VobDef>());
                    allDefByCodename.Add(type, new Dictionary<string, VobDef>());
                    MakeLogStatic(typeof(VobDef), String.Format("Registered dictionaries for type {0}.",
                        type));
                }
                return true;
            }
            else
            {
                MakeLogWarningStatic(typeof(VobDef), String.Format("Prevented registration " 
                    + "of dictionaries for type {0} because it is no subclass of VobDef!",
                    type));
            }
            
            return false;
        }

        public static bool Add (Type type, VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                Dictionary<String, VobDef> defByCodename = allDefByCodename[type];

                int id = def.GetId();
                String codename = def.GetCodeName();

                if (id < 1)
                {
                    MakeLogWarningStatic(type,
                        "Prevented attempt of adding a definition to to dictionary: "
                         + "An invalid id < 1 was provided!");
                    return false;
                }

                if (defById.ContainsKey(id))
                {
                    MakeLogWarningStatic(type,
                        String.Format("Prevented attempt of adding a definition to dictionary:"
                            + " The {0}={1} is already taken!", "id", id));
                    return false;
                }

                if (defByCodename.ContainsKey(codename))
                {
                    MakeLogWarningStatic(type,
                        String.Format("Prevented attempt of adding a definition to dictionary:"
                            + " The {0}={1} is already taken!", "codename", codename));
                    return false;
                }

                defById.Add(id, def);
                defByCodename.Add(codename, def);
                return true;
            }
            
            return false;
        }

        public static bool ContainsCodename (Type type, String codename)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<String, VobDef> defByCodename = allDefByCodename[type];
                return defByCodename.ContainsKey(codename);
            }

            return false;
        }

        public static bool ContainsId (Type type, int id)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                return defById.ContainsKey(id);
            }
            
            return false;
        }

        public static bool ContainsDefinition (Type type, VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                return defById.ContainsValue(def);
            }
            
            return false;
        }

        public static bool RemoveCodename (Type type, String codename)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                Dictionary<String, VobDef> defByCodename = allDefByCodename[type];

                if ((defByCodename.ContainsKey(codename))
                    && (defById.ContainsKey(defByCodename[codename].id)))
                {
                    defById.Remove(defByCodename[codename].id);
                    defByCodename.Remove(codename);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool RemoveId (Type type, int id)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                Dictionary<String, VobDef> defByCodename = allDefByCodename[type];

                if ((defById.ContainsKey(id)) 
                    && (defByCodename.ContainsKey(defById[id].codename)))
                {
                    defById.Remove(id);
                    defByCodename.Remove(defById[id].codename);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
            return false;
        }

        public static bool TryGetValue (Type type, String codename, out VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                return defByCodename.TryGetValue(codename, out def);
            }

            def = null;
            return false;
        }

        public static bool TryGetValue (Type type, int id, out VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                return defById.TryGetValue(id, out def);
            }
            
            def = null;
            return false;
        }

        #endregion

    }
}
