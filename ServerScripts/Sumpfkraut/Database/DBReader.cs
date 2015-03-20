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

        public static void LoadFromDB (
            string select, string from, string where, string orderBy,
            out List<List<List<object>>> results,
            //out List<List<object>> defList,
            ref Dictionary<String, SQLiteGetTypeEnum> colTypes,
            out List<string> colTypesKeys, out List<SQLiteGetTypeEnum> colTypesVals, 
            string sqlWhere="1")
        {
            results = new List<List<List<object>>>();
            //// stores the read and converted data of the sql-query
            //defList = new List<List<object>>();

            // lists to ensure same key-value-order for each row in rdr because, otherwise, the memory
            // adresses of the original dictionary and order might be changed during runtime
            colTypesKeys = new List<string>(colTypes.Keys);
            colTypesVals = new List<SQLiteGetTypeEnum>(colTypes.Values);

            using (SQLiteCommand cmd = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                // outputs a row for each effect change definition for the given IDs 
                // and sorts them accordingly in ascending order before returnign the query
                cmd.CommandText = "SELECT " + select
                    + " FROM " + from
                    + " WHERE " + where
                    + " ORDER BY " + orderBy
                    + ";";

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
