using GUC.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{
    public partial class VobDefLoader
    {

        protected string dbFilePath = null;
        public string DBFilePath { get { return this.dbFilePath; } }

        protected List<List<List<object>>> sqlResults = null;
        protected bool sqlResultInUse = false;
        public void DropSQLResult () { if (!sqlResultInUse) { sqlResults = null; } }

        public static readonly Dictionary<string, List<DBTables.ColumnGetTypeInfo>> DBStructure =
            new Dictionary<string, List<DBTables.ColumnGetTypeInfo>>()
            {
                {
                    "DefEffect", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "DefChange", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("DefChangeID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("Func", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("Params", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "VobDef", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("VobDefID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("IsStatic", SQLiteGetTypeEnum.GetBoolean),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
                {
                    "VobDefEffect", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("VobDefID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("DefEffectID", SQLiteGetTypeEnum.GetInt32),
                    }
                },
                {
                    "StaticDynamicJob", new List<DBTables.ColumnGetTypeInfo>
                    {
                        new DBTables.ColumnGetTypeInfo("JobID", SQLiteGetTypeEnum.GetInt32),
                        new DBTables.ColumnGetTypeInfo("TableName", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("Task", SQLiteGetTypeEnum.GetString),
                        new DBTables.ColumnGetTypeInfo("ChangeDate", SQLiteGetTypeEnum.GetDateTime),
                        new DBTables.ColumnGetTypeInfo("CreationDate", SQLiteGetTypeEnum.GetDateTime),
                    }
                },
            };

        //public static readonly List<string> DBTableLoadOrder = DBStructure.Keys.ToList();
        public static readonly List<string> DBTableLoadOrder = new List<string>()
        {
            "WorldEffect", "WorldChange", "StaticDynamicJob",
        };

        protected static List<List<DBTables.ColumnGetTypeInfo>> colGetTypeInfo = null;
        public static List<List<DBTables.ColumnGetTypeInfo>> ColGetTypeInfo { get { return colGetTypeInfo; } }



        partial void pLoad ()
        {
            
        }

    }
}
