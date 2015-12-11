using GUC.Server.Scripts.Sumpfkraut.Database;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public class VobDefLoader : VobLoader
    {

        protected VobDefType vobDefType = VobDefType.VobDef;

        protected List<Vec2Int> idRanges_VobDef;
        protected List<Vec2Int> idRanges_DefEffect;
        protected List<Vec2Int> idRanges_DefChange;

        protected List<DBTables.ColumnGetTypeInfo> sqlColInfo_VobDef;
        protected List<DBTables.ColumnGetTypeInfo> sqlColInfo_DefEffect;
        protected List<DBTables.ColumnGetTypeInfo> sqlColInfo_DefChange;

        protected List<List<List<object>>> sqlResults_VobDef;
        protected List<List<List<object>>> sqlResults_DefEffect;
        protected List<List<List<object>>> sqlResults_DefChange;



        public VobDefLoader (VobDefType vobDefType, List<Vec2Int> idRanges, bool startOnCreate)
        {
            SetObjName("VobDefLoader (default)");

            if (startOnCreate)
            {
                Start();
            }
        }



        protected override void Load ()
        {
            base.Load();

            // load VobDef-database-entries
            List<DBTables.ColumnGetTypeInfo> sqlColInfo_VobDef;
            List<List<List<object>>> sqlResults_VobDef;
            LoadVobDef(out sqlColInfo_VobDef, out sqlResults_VobDef);

            // load necessary DefEffect
            List<DBTables.ColumnGetTypeInfo> sqlColInfo_DefEffect;
            List<List<List<object>>> sqlResults_DefEffect;
            LoadVobDef(out sqlColInfo_DefEffect, out sqlResults_DefEffect);
        }

        protected bool LoadVobDef (out List<DBTables.ColumnGetTypeInfo> sqlColInfo,
            out List<List<List<object>>> sqlResults)
        {
            // *** correct misleading userinput ***
            bool loadAllVobDef = true;
            if (idRanges_VobDef != null)
            {
                if (idRanges_VobDef.Count > 0)
                {
                    loadAllVobDef = false;
                }
                else
                {
                    idRanges_VobDef = null;
                }
            }

            // *** prepare database-commands **

            String sqlSelect = null;
            String sqlFrom = null;
            String sqlWhere = null;
            String sqlOrderBy = null;
            String sqlIdColName = null;
            Dictionary<String, SQLiteGetTypeEnum> sqlGetTypeByCol = null;
            sqlColInfo = null;

            // generating FROM- and ORDER BY-clause and preparing for SELECT-clause
            switch (vobDefType)
            {
                case VobDefType.MobDef:
                    sqlGetTypeByCol = MobDef.defTab_GetTypeByColumn; // SELECT
                    sqlFrom = MobDef.dbTable; // FROM
                    sqlIdColName = MobDef.dbIdColName; // WHERE
                    sqlOrderBy = sqlIdColName + " ASC"; // ORDER BY
                    break;
                case VobDefType.ItemDef:
                    sqlGetTypeByCol = ItemDef.defTab_GetTypeByColumn;
                    sqlFrom = ItemDef.dbTable;
                    sqlIdColName = ItemDef.dbIdColName;
                    sqlOrderBy = sqlIdColName + " ASC";
                    break;
                case VobDefType.NpcDef:
                    sqlGetTypeByCol = NpcDef.defTab_GetTypeByColumn;
                    sqlFrom = NpcDef.dbTable;
                    sqlIdColName = NpcDef.dbIdColName;
                    sqlOrderBy = sqlIdColName + " ASC";
                    break;
                case VobDefType.VobDef:
                    MakeLogError(String.Format("LoadVobDef: Unknown VobDef-Type {0} detected! "
                        + "Cancelling loading process.", vobDefType));
                    break;
                default:
                    MakeLogError(String.Format("LoadVobDef: Unknown VobDef-Type {0} detected! "
                        + "Cancelling loading process.", vobDefType));
                    break;
            }

            // generate SELECT-clause
            DBTables.SqlColumnInfo(sqlGetTypeByCol, out sqlColInfo);

            StringBuilder sqlSelect_VobDef_SB = new StringBuilder();
            for (int c = 0; c < sqlColInfo.Count; c++)
            {
                if (c > 0)
                {
                    sqlSelect_VobDef_SB.Append(",");
                }
                sqlSelect_VobDef_SB.Append(sqlColInfo[c].colName);
            }

            // generating WHERE-clause
            if (loadAllVobDef)
            {
                // selects everything from the table
                sqlWhere = "1";
            }
            else
            {
                StringBuilder sqlWhere_VobDef_SB = new StringBuilder();
                sqlWhere_VobDef_SB.Append(sqlIdColName);
                // maybe use <= >= to use the ranges 
                // -->  with lots of ranges this might result in huge overhead in 
                // comparison to using IN-operator
                // --> use Count-threshold to switch between both styles
                for (int r = 0; r < idRanges_VobDef.Count; r++)
                {
                    if (r > 0)
                    {
                        sqlWhere_VobDef_SB.Append(" OR ");
                    }
                    sqlWhere_VobDef_SB.AppendFormat("({0} >= {1} AND {0} <= {2])", 
                        sqlIdColName, idRanges_VobDef[r].x1, idRanges_VobDef[r].x2);
                }
                sqlWhere = sqlWhere_VobDef_SB.ToString();
            }

            // *** execute sql-command, receive results and convert them to usable datatypes *** 

            List<List<List<object>>> tempSqlResults = new List<List<List<object>>>();

            DBAgent dbAgent_VobDef = new DBAgent(new List<String> {
                String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}",
                sqlSelect, sqlFrom, sqlWhere, sqlOrderBy) }, false);

            dbAgent_VobDef.waitHandle = new AutoResetEvent(false);
            dbAgent_VobDef.ReceivedResults += delegate (Runnable sender, DBAgent.ReceivedResultsEventArgs e) 
            {
                tempSqlResults = e.GetResults();
                sender.waitHandle.Set();
            };
            dbAgent_VobDef.Start();

            // wait for the results of the query to arise
            dbAgent_VobDef.waitHandle.WaitOne();

            if ((tempSqlResults == null) || (tempSqlResults.Count < 1) 
                || (tempSqlResults[0].Count < 1))
            {
                // return when there are no database-results to work with
                sqlResults = new List<List<List<object>>>();
                return false;
            }

            DBTables.ConvertSQLResults(ref tempSqlResults, ref sqlColInfo);
            sqlResults = tempSqlResults;

            return true;
        }

        protected bool LoadDefEffect (out List<DBTables.ColumnGetTypeInfo> sqlColInfo,
            out List<List<List<object>>> sqlResults)
        {
            // *** correct misleading userinput ***
            bool loadAllVobDef = true;
            if (idRanges_VobDef != null)
            {
                if (idRanges_VobDef.Count > 0)
                {
                    loadAllVobDef = false;
                }
                else
                {
                    idRanges_VobDef = null;
                }
            }

            // *** prepare database-commands **

            String sqlSelect = null;
            String sqlFrom = null;
            String sqlWhere = null;
            String sqlOrderBy = null;
            String sqlIdColName = null;
            Dictionary<String, SQLiteGetTypeEnum> sqlGetTypeByCol = null;
            sqlColInfo = null;

            // generating FROM- and ORDER BY-clause and preparing for SELECT-clause
            switch (vobDefType)
            {
                case VobDefType.MobDef:
                    sqlGetTypeByCol = MobDef.defTab_GetTypeByColumn; // SELECT
                    sqlFrom = MobDef.dbTable; // FROM
                    sqlIdColName = MobDef.dbIdColName; // WHERE
                    sqlOrderBy = sqlIdColName + " ASC"; // ORDER BY
                    break;
                case VobDefType.ItemDef:
                    sqlGetTypeByCol = ItemDef.defTab_GetTypeByColumn;
                    sqlFrom = ItemDef.dbTable;
                    sqlIdColName = ItemDef.dbIdColName;
                    sqlOrderBy = sqlIdColName + " ASC";
                    break;
                case VobDefType.NpcDef:
                    sqlGetTypeByCol = NpcDef.defTab_GetTypeByColumn;
                    sqlFrom = NpcDef.dbTable;
                    sqlIdColName = NpcDef.dbIdColName;
                    sqlOrderBy = sqlIdColName + " ASC";
                    break;
                case VobDefType.VobDef:
                    MakeLogError(String.Format("LoadDefEffect: Unknown Type {0} detected! "
                        + "Cancelling loading process.", vobDefType));
                    break;
                default:
                    MakeLogError(String.Format("LoadVobDef: Unknown Type {0} detected! "
                        + "Cancelling loading process.", vobDefType));
                    break;
            }

            // generate SELECT-clause
            DBTables.SqlColumnInfo(sqlGetTypeByCol, out sqlColInfo);

            StringBuilder sqlSelect_VobDef_SB = new StringBuilder();
            for (int c = 0; c < sqlColInfo.Count; c++)
            {
                if (c > 0)
                {
                    sqlSelect_VobDef_SB.Append(",");
                }
                sqlSelect_VobDef_SB.Append(sqlColInfo[c].colName);
            }

            // generating WHERE-clause
            if (loadAllVobDef)
            {
                // selects everything from the table
                sqlWhere = "1";
            }
            else
            {
                StringBuilder sqlWhere_VobDef_SB = new StringBuilder();
                sqlWhere_VobDef_SB.Append(sqlIdColName);
                // maybe use <= >= to use the ranges 
                // -->  with lots of ranges this might result in huge overhead in 
                // comparison to using IN-operator
                // --> use Count-threshold to switch between both styles
                for (int r = 0; r < idRanges_VobDef.Count; r++)
                {
                    if (r > 0)
                    {
                        sqlWhere_VobDef_SB.Append(" OR ");
                    }
                    sqlWhere_VobDef_SB.AppendFormat("({0} >= {1} AND {0} <= {2])", 
                        sqlIdColName, idRanges_VobDef[r].x1, idRanges_VobDef[r].x2);
                }
                sqlWhere = sqlWhere_VobDef_SB.ToString();
            }

            // *** execute sql-command, receive results and convert them to usable datatypes *** 

            List<List<List<object>>> tempSqlResults = new List<List<List<object>>>();

            DBAgent dbAgent_VobDef = new DBAgent(new List<String> {
                String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}",
                sqlSelect, sqlFrom, sqlWhere, sqlOrderBy) }, false);

            dbAgent_VobDef.waitHandle = new AutoResetEvent(false);
            dbAgent_VobDef.ReceivedResults += delegate (Runnable sender, DBAgent.ReceivedResultsEventArgs e) 
            {
                tempSqlResults = e.GetResults();
                sender.waitHandle.Set();
            };

            dbAgent_VobDef.Start();

            // wait for the results of the query to arise
            dbAgent_VobDef.waitHandle.WaitOne();

            if ((tempSqlResults == null) || (tempSqlResults.Count < 1) 
                || (tempSqlResults[0].Count < 1))
            {
                // return when there are no database-results to work with
                sqlResults = new List<List<List<object>>>();
                return false;
            }

            DBTables.ConvertSQLResults(ref tempSqlResults, ref sqlColInfo);
            sqlResults = tempSqlResults;

            return true;
        }

        protected bool LoadDefChange (out List<DBTables.ColumnGetTypeInfo> sqlColInfo,
            out List<List<List<object>>> sqlResults)
        {
            // TO DO: WRITE THE ACTUAL METHOD --> used delcarations to prevent compile error for the moment
            sqlColInfo = new List<DBTables.ColumnGetTypeInfo>();
            sqlResults = new List<List<List<object>>>();
            return true;
        }

    }
}
