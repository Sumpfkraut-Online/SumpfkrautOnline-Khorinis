using GUC.Server.Scripts.Sumpfkraut.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{
    class VobDefLoader : VobLoader
    {

        protected VobDefType vobDefType = VobDefType.VobDef;
        protected List<Vec2Int> idRanges;



        public VobDefLoader (VobDefType vobDefType, List<Vec2Int> idRanges, bool startOnCreate)
            : base(false)
        {


            if (startOnCreate)
            {
                Start();
            }
        }



        protected override void Load ()
        {
            base.Load();

            // *** correct misleading userinput ***
            bool loadAllVobDef = true;
            if (idRanges != null)
            {
                if (idRanges.Count > 0)
                {
                    loadAllVobDef = false;
                }
                else
                {
                    idRanges = null;
                }
            }

            // *** prepare database-commands **

            String sqlSelect_VobDef = "";
            String sqlFrom_VobDef = "";
            String sqlWhere_VobDef = "";
            String sqlOrderBy_VobDef = "";
            String sqlIdColName_VobDef = "";
            List<DBTables.ColumnGetTypeInfo> sqlSelectColNames = 
                new List<DBTables.ColumnGetTypeInfo>();

            // generating FROM- and ORDER BY-clause and preparing for SELECT-clause
            switch (vobDefType)
            {
                case VobDefType.MobDef:
                    sqlFrom_VobDef = MobDef.dbTable;
                    sqlIdColName_VobDef = MobDef.dbIdColName;
                    sqlOrderBy_VobDef = sqlIdColName_VobDef + " ASC";
                    DBTables.SqlColumnInfo(MobDef.defTab_GetTypeByColumn, out sqlSelectColNames);
                    break;
                case VobDefType.ItemDef:
                    sqlFrom_VobDef = ItemDef.dbTable;
                    sqlIdColName_VobDef = ItemDef.dbIdColName;
                    sqlOrderBy_VobDef = sqlIdColName_VobDef + " ASC";
                    DBTables.SqlColumnInfo(ItemDef.defTab_GetTypeByColumn, out sqlSelectColNames);
                    break;
                case VobDefType.NpcDef:
                    sqlFrom_VobDef = NpcDef.dbTable;
                    sqlIdColName_VobDef = NpcDef.dbIdColName;
                    sqlOrderBy_VobDef = sqlIdColName_VobDef + " ASC";
                    DBTables.SqlColumnInfo(NpcDef.defTab_GetTypeByColumn, out sqlSelectColNames);
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
            StringBuilder sqlSelect_VobDef_SB = new StringBuilder();
            for (int c = 0; c < sqlSelectColNames.Count; c++)
            {
                if (c > 0)
                {
                    sqlSelect_VobDef_SB.Append(",");
                }
                sqlSelect_VobDef_SB.Append(sqlSelectColNames[c].colName);
            }

            // generating WHERE-clause
            if (loadAllVobDef)
            {
                sqlWhere_VobDef = "1";
            }
            else
            {
                StringBuilder sqlWhere_VobDef_SB = new StringBuilder();
                sqlWhere_VobDef_SB.Append(sqlIdColName_VobDef);
                // maybe use <= >= to use the ranges 
                // -->  with lots of ranges this might result in huge overhead in 
                // comparison to using IN-operator
                // --> use Count-threshold to switch between both styles
                for (int r = 0; r < idRanges.Count; r++)
                {
                    if (r > 0)
                    {
                        sqlWhere_VobDef_SB.Append(" OR ");
                    }
                    sqlWhere_VobDef_SB.AppendFormat("({0} >= {1} AND {0} <= {2])", 
                        sqlIdColName_VobDef, idRanges[r].x1, idRanges[r].x2);
                }
                sqlWhere_VobDef = sqlWhere_VobDef_SB.ToString();
            }

            // *** execute sql-command, receive results and convert them to usable datatypes *** 

            DBAgent dbAgent_VobDef = new DBAgent(new List<String> {
                String.Format("SELECT {0} FROM {1} WHERE {2} ORDER BY {3}",
                sqlSelect_VobDef, sqlFrom_VobDef, sqlWhere_VobDef, sqlOrderBy_VobDef) }, false);
            dbAgent_VobDef.waitHandle = new AutoResetEvent(false);
            //dbAgent_VobDef.ReceivedResults;
            dbAgent_VobDef.Start();

            dbAgent_VobDef.waitHandle.WaitOne();
        }

        protected void SqlResultToVobDef (object sender, DBAgent.ReceivedResultsEventArgs e)
        {
            List<List<List<object>>> sqlResults_VobDef  = e.GetResults();
        }

    }
}
