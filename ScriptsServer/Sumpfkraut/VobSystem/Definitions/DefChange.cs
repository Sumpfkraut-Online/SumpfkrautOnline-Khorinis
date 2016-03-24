using GUC.Server.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public class DefChange : GUC.Utilities.ExtendedObject
    {

        #region delegates

        protected delegate bool ApplyDefChange_MobDef (ref MobDef vobDef, String param);
        protected delegate bool ApplyDefChange_ItemDef (ref ItemDef vobDef, String param);
        protected delegate bool ApplyDefChange_NpcDef (ref NpcDef vobDef, String param);


        #endregion



        #region dictionaries

        public static readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {"DefChangeId",             SQLiteGetTypeEnum.GetInt32},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

        protected static readonly Dictionary<DefChangeType, ApplyDefChange_MobDef> ApplyDefChangeDict_MobDef =
            new Dictionary<DefChangeType, ApplyDefChange_MobDef>
            {
                {
                    DefChangeType.undefined, delegate(ref MobDef vobDef, String param)
                    {
                        return true;
                    }
                },
            };

        protected static readonly Dictionary<DefChangeType, ApplyDefChange_ItemDef> ApplyDefChangeDict_ItemDef =
            new Dictionary<DefChangeType, ApplyDefChange_ItemDef>
            {
                {
                    DefChangeType.undefined, delegate(ref ItemDef vobDef, String param)
                    {
                        return true;
                    }
                },
            };

        protected static readonly Dictionary<DefChangeType, ApplyDefChange_NpcDef> ApplyDefChangeDict_NpcDef =
            new Dictionary<DefChangeType, ApplyDefChange_NpcDef>
            {
                {
                    DefChangeType.undefined, delegate(ref NpcDef vobDef, String param)
                    {
                        return true;
                    }
                },
            };

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "DefChange (static)";

        public struct DefChangeKeyValPair
        {
            public DefChangeType defChangeType;
            public String param;
        }

        #endregion



        #region constructors

        public DefChange ()
        {
            SetObjName("DefChange (default)");
        }

        #endregion



        #region apply-methods

        public static bool ApplyDefChange (ref MobDef vobDef, DefChangeKeyValPair defChange)
        {
            bool success = false;
            ApplyDefChange_MobDef applyFunc;
            if (ApplyDefChangeDict_MobDef.TryGetValue(defChange.defChangeType, out applyFunc)
                && applyFunc(ref vobDef, defChange.param))
            {
                success = true;
            }
            return success;
        }

        public static bool ApplyDefChange (ref ItemDef vobDef, DefChangeKeyValPair defChange)
        {
            bool success = false;
            ApplyDefChange_ItemDef applyFunc;
            if (ApplyDefChangeDict_ItemDef.TryGetValue(defChange.defChangeType, out applyFunc)
                && applyFunc(ref vobDef, defChange.param))
            {
                success = true;
            }
            return success;
        }

        public static bool ApplyDefChange (ref NpcDef vobDef, DefChangeKeyValPair defChange)
        {
            bool success = false;
            ApplyDefChange_NpcDef applyFunc;
            if (ApplyDefChangeDict_NpcDef.TryGetValue(defChange.defChangeType, out applyFunc)
                && applyFunc(ref vobDef, defChange.param))
            {
                success = true;
            }
            return success;
        }

        #endregion

    }
}
