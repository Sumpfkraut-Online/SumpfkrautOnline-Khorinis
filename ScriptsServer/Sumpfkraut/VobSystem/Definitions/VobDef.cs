using GUC.Server.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public abstract class VobDef : GUC.Utilities.ExtendedObject
    {

        #region dictionaries

        protected static Dictionary<Type, Dictionary<int, VobDef>> allDefById =
            new Dictionary<Type, Dictionary<int, VobDef>>();
        protected static Dictionary<Type, Dictionary<string, VobDef>> allDefByCodeName = 
            new Dictionary<Type, Dictionary<string, VobDef>>();

        // !!! hide them with new-keyworld in child classes !!!
        protected static Dictionary<int, VobDef> defById = new Dictionary<int, VobDef>();
        protected static Dictionary<string, VobDef> defByCodeName = new Dictionary<string, VobDef>();

        // !!! hide it with new-keyworld in child classes !!!
        public static readonly String dbIdColName = "VobDefId";
        public static readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {dbIdColName,                SQLiteGetTypeEnum.GetInt32},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

        #endregion


        #region standard attributes

        new public static readonly String _staticName = "VobDef (static)";

        protected static Type _type = typeof(VobDef);
        public static readonly String dbTable = "VobDef";

        protected int id;
        public int GetId () { return this.id; }
        public void SetId (int id) { this.id = id; }

        protected String codeName;
        public String GetCodeName () { return this.codeName; }
        public void SetCodeName (String codeName) { this.codeName = codeName; }

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
        {
            SetObjName("VobDef (default)");
        }

        #endregion



        #region dictionary-methods

        // checks whether necessary dictionaries are already registeres and 
        // registers them if not done already
        protected static bool RegisterDictionaries(Type type)
        {
            if (type.IsSubclassOf(typeof(VobDef)))
            {
                if (!allDefById.ContainsKey(type))
                {
                    allDefById.Add(type, new Dictionary<int, VobDef>());
                    allDefByCodeName.Add(type, new Dictionary<string, VobDef>());
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

        protected static bool HasDictAttributes(Type type, VobDef def)
        {
            bool check = true;
            int id = def.GetId();
            String codeName = def.GetCodeName();

            if (id < 1)
            {
                MakeLogWarningStatic(type, String.Format("Invalid {0} detected in HasDictAttributes!",
                    "id"));
                check = false;
            }
            if ((codeName == null) || (codeName.Length < 1))
            {
                MakeLogWarningStatic(type, String.Format("Invalid {0} detected in HasDictAttributes!", 
                    "codeName"));
                check = false;
            }

            return check;
        }

        protected static bool Add(Type type, VobDef def)
        {
            if (RegisterDictionaries(type) && HasDictAttributes(type, def))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                Dictionary<String, VobDef> defByCodeName = allDefByCodeName[type];

                int id = def.GetId();
                String codeName = def.GetCodeName();

                if (defById.ContainsKey(id))
                {
                    MakeLogWarningStatic(type,
                        String.Format("Prevented attempt of adding a definition to dictionary:"
                            + " The {0}={1} is already taken!", 
                            "id", id));
                    return false;
                }

                if (defByCodeName.ContainsKey(codeName))
                {
                    MakeLogWarningStatic(type,
                        String.Format("Prevented attempt of adding a definition to dictionary:"
                            + " The {0}={1} is already taken!", 
                            "codeName", codeName));
                    return false;
                }

                defById.Add(id, def);
                defByCodeName.Add(codeName, def);

                return true;
            }
            else
            {
                MakeLogWarningStatic(type, 
                    "Prevented attempt of adding a definition to dictionary");
            }

            return false;
        }

        protected static bool ContainsCodeName(Type type, String codeName)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<String, VobDef> defByCodeName = allDefByCodeName[type];
                return defByCodeName.ContainsKey(codeName);
            }

            return false;
        }

        protected static bool ContainsId(Type type, int id)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                return defById.ContainsKey(id);
            }

            return false;
        }

        protected static bool ContainsDefinition(Type type, VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                return defById.ContainsValue(def);
            }

            return false;
        }

        protected static bool RemoveCodeName(Type type, String codeName)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                Dictionary<String, VobDef> defByCodeName = allDefByCodeName[type];

                if ((defByCodeName.ContainsKey(codeName))
                    && (defById.ContainsKey(defByCodeName[codeName].id)))
                {
                    defById.Remove(defByCodeName[codeName].id);
                    defByCodeName.Remove(codeName);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        protected static bool RemoveId(Type type, int id)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                Dictionary<String, VobDef> defByCodeName = allDefByCodeName[type];

                if ((defById.ContainsKey(id))
                    && (defByCodeName.ContainsKey(defById[id].codeName)))
                {
                    defById.Remove(id);
                    defByCodeName.Remove(defById[id].codeName);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        protected static bool TryGetValueByCodeName(Type type, String codeName, out VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<String, VobDef> defByCodeName = allDefByCodeName[type];
                return defByCodeName.TryGetValue(codeName, out def);
            }

            def = null;
            return false;
        }

        protected static bool TryGetValueById(Type type, int id, out VobDef def)
        {
            if (RegisterDictionaries(type))
            {
                Dictionary<int, VobDef> defById = allDefById[type];
                return defById.TryGetValue(id, out def);
            }
            
            def = null;
            return false;
        }

        #endregion

    }
}
