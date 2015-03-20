using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

namespace GUC.Server.Scripts.Sumpfkraut.Database
{
    class DBReader
    {

        /**
         *   Executes a defined sql-query and stores the results.
         *   Stores the results for each sql-statement provided, so that after defining 2 statements 
         *   (statement ends with ;) there is a list of 2 results. Each result ist in itself a list of rows.
         *   Per row there can be 0 or more objects/table column entries of data.
         *   @param results stores the results of 1 or multuple provided sql-queries
         *   @param colTypes is the list which provides ways to convert the resulting strings to more defined server data
         *   @param colTypesKeys stores the column names of the data table in a numeric manner (used in cata conversion)
         *   @param colTypesVals stores the hints on data conversion used in a numeric manner (used in cata conversion)
         *   @param convertData must be true if the resultings strings should be converted to data on load
         *   @param select is the optional string after the SELECT-statement in sql
         *   @param from is the optional string after the FROM-statement in sql
         *   @param where is the optional string after the WHERE-statement in sql
         *   @param orderBy is the optional string after the ORDER BY-statement in sql
         *   @param completeQuery is an optional query which can be used to define more than one sql-statement in one shot (if provided, select, from, where, orderBy are ignored)
         */
        public static void LoadFromDB (
            out List<List<List<object>>> results,
            ref Dictionary<String, SQLiteGetTypeEnum> colTypes,
            out List<string> colTypesKeys, out List<SQLiteGetTypeEnum> colTypesVals, 
            bool convertData = true,
            string select="", string from="", string where="", string orderBy="",
            string completeQuery = "")
        {
            // the actual query results
            results = new List<List<List<object>>>();

            // lists to ensure same key-value-order for each row in rdr because, otherwise, the memory
            // adresses of the original dictionary and order might be changed during runtime
            colTypesKeys = new List<string>(colTypes.Keys);
            colTypesVals = new List<SQLiteGetTypeEnum>(colTypes.Values);

            using (SQLiteCommand cmd = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                if (completeQuery.Length <= 0)
                {
                    cmd.CommandText = "SELECT " + select
                    + " FROM " + from
                    + " WHERE " + where
                    + " ORDER BY " + orderBy
                    + ";";
                }
                else
                {
                    cmd.CommandText = completeQuery;
                }

                SQLiteDataReader rdr = null;
                try
                {
                    rdr = cmd.ExecuteReader();
                    if (!rdr.HasRows)
                    {
                        return;
                    }

                    // temporary list to put all data of a row into
                    List<object> rowList = null;

                    do
                    {
                        // add new result-list
                        results.Add(new List<List<object>>());
                        while (rdr.Read())
                        {
                            // create and fill list of the temporary data row
                            rowList = new List<object>();
                            for (int col = 0; col < colTypesKeys.Count; col++)
                            {
                                rowList.Add(DBTables.SqlReadType(ref rdr, col, colTypesVals[col]));
                            }
                            // add the complete temporary data row to the current result-list
                            results[results.Count - 1].Add(rowList);
                        }
                    }
                    while (rdr.NextResult());
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not execute SQLiteDataReader in LoadFromDB: " + ex);
                }
                finally
                {
                    if (rdr != null)
                    {
                        rdr.Close();
                    }
                }
            }
        }

    }
}
