using GUC.Server.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    class DefEffect : ScriptObject
    {

        #region dictionaries

        public static readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {"DefEffectId",             SQLiteGetTypeEnum.GetInt32},
                {"Name",                    SQLiteGetTypeEnum.GetString},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

        protected static Dictionary<int, DefEffect> defEffectById = 
            new Dictionary<int, DefEffect>();
        protected static Dictionary<String, DefEffect> defEffectByEffectName = 
            new Dictionary<String, DefEffect>();

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "DefEffect (static)";
        new protected String _objName = "DefEffect (default)";

        protected int id;
        public int GetId () { return this.id; }
        public void SetId(int id) { this.id = id; }

        protected String effectName = "DefEffect (default)";
        public String GetEffectName () { return this.effectName; }
        public void SetEffectName(String effectName) { this.effectName = effectName; }

        protected List<DefChange.DefChangeKeyValPair> changeList = 
            new List<DefChange.DefChangeKeyValPair>();
        public List<DefChange.DefChangeKeyValPair> GetChangeList () { return this.changeList; }

        #endregion



        #region constructors

        public DefEffect (int id, String effectName, 
            List<DefChange.DefChangeKeyValPair> changeList)
        {
            this.id = id;
            this.effectName = effectName;
            this.changeList = changeList;
        }

        #endregion



        #region dictionary-methods

        public static bool Add (DefEffect defEffect)
        {
            int id = defEffect.GetId();
            String effectName = defEffect.GetEffectName();

            if (defEffectById.ContainsKey(id))
                {
                    MakeLogWarningStatic(typeof(DefEffect),
                        String.Format("Prevented attempt of adding a DefEffect to dictionary:"
                            + " The {0}={1} is already taken!", 
                            "id", id));
                    return false;
                }

            if (defEffectByEffectName.ContainsKey(effectName))
                {
                    MakeLogWarningStatic(typeof(DefEffect),
                        String.Format("Prevented attempt of adding a DefEffect to dictionary:"
                            + " The {0}={1} is already taken!", 
                            "effectName", effectName));
                    return false;
                }

            defEffectById.Add(id, defEffect);
            defEffectByEffectName.Add(effectName, defEffect);

            return true;
        }

        public static bool ContainsEffectName (Type type, String effectName)
        {
            return defEffectByEffectName.ContainsKey(effectName);
        }

        public static bool ContainsId (Type type, int id)
        {
            return defEffectById.ContainsKey(id);
        }

        public static bool ContainsDefEffect (DefEffect defEffect)
        {
            return defEffectById.ContainsValue(defEffect);
        }

        public static bool RemoveEffectName (String effectName)
        {
            if ((defEffectByEffectName.ContainsKey(effectName))
                && (defEffectById.ContainsKey(defEffectByEffectName[effectName].id)))
            {
                defEffectByEffectName.Remove(effectName);
                defEffectById.Remove(defEffectByEffectName[effectName].id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool RemoveId (int id)
        {
            if ((defEffectById.ContainsKey(id))
                && (defEffectByEffectName.ContainsKey(defEffectById[id].effectName)))
            {
                defEffectById.Remove(id);
                defEffectByEffectName.Remove(defEffectById[id].effectName);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected static bool TryGetValueByCodeName(String effectName, out DefEffect defEffect)
        {
            return defEffectByEffectName.TryGetValue(effectName, out defEffect);
        }

        protected static bool TryGetValueById(int id, out DefEffect defEffect)
        {
            return defEffectById.TryGetValue(id, out defEffect);
        }

        #endregion



        #region defChangeList-management
        
        // no management yet --> instead assignment only in constructor
 
        #endregion



        public static void ApplyDefEffect (ref MobDef vobDef, DefEffect defEffect, 
            out List<int> failedIndices)
        {
            failedIndices = new List<int>();
            List<DefChange.DefChangeKeyValPair> changeList = defEffect.GetChangeList();

            for (int i = 0; i < changeList.Count; i++)
            {
                if (!DefChange.ApplyDefChange(ref vobDef, changeList[i]))
                {
                    failedIndices.Add(i);
                }
            }
        }

        public static void ApplyDefEffect (ref ItemDef vobDef, DefEffect defEffect, 
            out List<int> failedIndices)
        {
            failedIndices = new List<int>();
            List<DefChange.DefChangeKeyValPair> changeList = defEffect.GetChangeList();

            for (int i = 0; i < changeList.Count; i++)
            {
                if (!DefChange.ApplyDefChange(ref vobDef, changeList[i]))
                {
                    failedIndices.Add(i);
                }
            }
        }

        public static void ApplyDefEffect (ref NpcDef vobDef, DefEffect defEffect, 
            out List<int> failedIndices)
        {
            failedIndices = new List<int>();
            List<DefChange.DefChangeKeyValPair> changeList = defEffect.GetChangeList();

            for (int i = 0; i < changeList.Count; i++)
            {
                if (!DefChange.ApplyDefChange(ref vobDef, changeList[i]))
                {
                    failedIndices.Add(i);
                }
            }
        }

    }
}
