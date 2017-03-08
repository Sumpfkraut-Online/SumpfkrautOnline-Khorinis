using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
//using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

namespace GUC.Scripts.Sumpfkraut.Database
{

    public class DBTables : GUC.Utilities.ExtendedObject
    {

        public struct ColumnGetTypeInfo
        {
            public string colName;
            public SQLiteGetType getType;

            public ColumnGetTypeInfo (string colName, SQLiteGetType getType)
            {
                this.colName = colName;
                this.getType = getType;
            }

            public override string ToString ()
            {
                return string.Format("Sql-column with colName = {0} and getType = {1}", colName, getType);
            }
        }



        new public static readonly string _staticName = "DBTables (static)";
        new protected string _objName = "DBTables (default)";
        

        /// <summary>
        /// Converts results of a sql-query from string- to their respective datatypes.
        /// </summary>
        /// <param name="sqlResults">the result of the sql-query</param>
        /// <param name="colGetTypeInfo">a predefined list of information concerning the sql-column 
        /// (name + datatype)</param>
        /// <returns></returns>
        public static bool ConvertSQLResults (List<List<List<object>>> sqlResults, 
            List<ColumnGetTypeInfo> colGetTypeInfo)
        {
            List<List<ColumnGetTypeInfo>> _colGetTypeInfo = new List<List<ColumnGetTypeInfo>> { colGetTypeInfo };
            return ConvertSQLResults(sqlResults, _colGetTypeInfo, true);
        }

        public static bool ConvertSQLResults (List<List<List<object>>> sqlResults, 
            List<List<ColumnGetTypeInfo>> colGetTypeInfo, bool noLengthComparison = false)
        {
            bool allConverted = true;
            object tempEntry = null;
            int res = 0;
            int row = 0;
            int col = 0;

            if ((!noLengthComparison) && (sqlResults.Count != colGetTypeInfo.Count))
            {
                MakeLogErrorStatic(typeof(DBTables), "ConvertSQLResults: Lists sqlResults and colGetTypeInfo "
                    + "are not of same length! Aborting conversion.");
            }

            // iterating results (resulted by multiplay statements seperated by ; in sql-command)
            while (res < sqlResults.Count)
            {
                row = 0;
                // iterating data-rows
                while (row < sqlResults[res].Count)
                {
                    // iterating data-columns
                    col = 0;
                    while (col < sqlResults[res][row].Count)
                    {
                        if (sqlResults[res][row][col] == null)
                        {
                            // null might be just fine
                        }
                        else if (sqlResults[res][row][col].GetType() == typeof(DBNull))
                        {
                            // DBNull is a little unheady because it would need additional type-checks later
                            // use null instead
                            sqlResults[res][row][col] = null;
                        }
                        else
                        {
                            // everything else should be a string and somehow convertable
                            tempEntry = sqlResults[res][row][col].ToString();
                            if (TrySqlStringToData((string) tempEntry, 
                                colGetTypeInfo[res][col].getType, 
                                out tempEntry))
                            {
                                sqlResults[res][row][col] = tempEntry;
                            }
                            else
                            {
                                sqlResults[res][row][col] = null;

                                MakeLogErrorStatic(typeof(DBTables), String.Format(
                                    "ConvertSQLResults: Could not convert {0}" 
                                    + "from String to type {1} for column {2}!", 
                                    tempEntry, colGetTypeInfo[res][col].getType, 
                                    colGetTypeInfo[res][col].colName));

                                allConverted = false;
                            }
                        }
 
                        col++;
                    }
                    
                    row++;
                }

                res++;
            }
            
            return allConverted;
        }

        public static void SqlColumnInfo (Dictionary<string, SQLiteGetType> getTypeByColumn,
            out List<ColumnGetTypeInfo> colGetTypes)
        {
            colGetTypes = new List<ColumnGetTypeInfo>();
            foreach (KeyValuePair<string, SQLiteGetType> keyValPair in getTypeByColumn)
            {
                colGetTypes.Add(new ColumnGetTypeInfo(keyValPair.Key, keyValPair.Value));
            }
        }

        public static void SqlColumnInfo (ref Dictionary<string, SQLiteGetType> getTypeByColumn,
            out List<ColumnGetTypeInfo> colGetTypes)
        {
            colGetTypes = new List<ColumnGetTypeInfo>();
            foreach (KeyValuePair<string, SQLiteGetType> keyValPair in getTypeByColumn)
            {
                colGetTypes.Add(new ColumnGetTypeInfo(keyValPair.Key, keyValPair.Value));
            }
        }
        
        /**
         *   Method to convert strings of a sql-result to the hinted datatype.
         *   Return or false if the data conversion succeeds or fails, respectively.
         *   @param sqlString is the string to convert
         *   @param get is the hint on the final data type of the conversion
         *   @param output is the object where the resulting type-converted string will be stored into
         */
        public static bool TrySqlStringToData (string sqlString, SQLiteGetType t, out object output)
        {
            if (sqlString != null)
            {
                switch (t)
                {
                    case (SQLiteGetType.GetBoolean):
                        bool outBool = false;
                        if (bool.TryParse(sqlString, out outBool))
                        {
                            output = outBool;
                            return true;
                        }
                        else if (sqlString.Equals("0"))
                        {
                            output = false;
                            return true;
                        }
                        else if (sqlString.Equals("1"))
                        {
                            output = true;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetByte):
                        byte outByte = 0;
                        if (byte.TryParse(sqlString, out outByte))
                        {
                            output = outByte;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetChar):
                        char outChar = '0';
                        if (char.TryParse(sqlString, out outChar))
                        {
                            output = outChar;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetDateTime):
                        DateTime outDateTime = new DateTime();
                        if (DateTime.TryParse(sqlString, out outDateTime))
                        {
                            output = outDateTime;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetDecimal):
                        decimal outDecimal = 0;
                        if (decimal.TryParse(sqlString, out outDecimal))
                        {
                            output = outDecimal;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetDouble):
                        double outDouble = 0;
                        if (double.TryParse(sqlString, out outDouble))
                        {
                            output = outDouble;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetFloat):
                        float outFloat = 0;
                        if (float.TryParse(sqlString, out outFloat))
                        {
                            output = outFloat;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetGuid):
                        Guid outGuid = Guid.Empty;
                        if (Guid.TryParse(sqlString, out outGuid))
                        {
                            output = outGuid;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetInt16):
                        short outInt16 = 0;
                        if (short.TryParse(sqlString, out outInt16))
                        {
                            output = outInt16;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetInt32):
                        int outInt32 = 0;
                        if (int.TryParse(sqlString, out outInt32))
                        {
                            output = outInt32;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetInt64):
                        long outInt64 = 0;
                        if (long.TryParse(sqlString, out outInt64))
                        {
                            output = outInt64;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetType.GetString):
                        output = sqlString;
                        return true;
                    default:
                        Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData because this datatype is not supported.");
                        break;
                }
            }

            output = null;
            return false;
        }

        public static bool TrySqlStringToData (string sqlString, Type t, out object output)
        {
            if (sqlString != null)
            {
                switch (t.ToString())
                {
                    case ("bool"):
                        bool outBool = false;
                        if (bool.TryParse(sqlString, out outBool))
                        {
                            output = outBool;
                            return true;
                        }
                        else if (sqlString.Equals("0"))
                        {
                            output = false;
                            return true;
                        }
                        else if (sqlString.Equals("1"))
                        {
                            output = true;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("byte"):
                        byte outByte = 0;
                        if (byte.TryParse(sqlString, out outByte))
                        {
                            output = outByte;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("char"):
                        char outChar = '0';
                        if (char.TryParse(sqlString, out outChar))
                        {
                            output = outChar;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("DateTime"):
                        DateTime outDateTime = new DateTime();
                        if (DateTime.TryParse(sqlString, out outDateTime))
                        {
                            output = outDateTime;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("decimal"):
                        decimal outDecimal = 0;
                        if (decimal.TryParse(sqlString, out outDecimal))
                        {
                            output = outDecimal;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("double"):
                        double outDouble = 0;
                        if (double.TryParse(sqlString, out outDouble))
                        {
                            output = outDouble;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("float"):
                        float outFloat = 0;
                        if (float.TryParse(sqlString, out outFloat))
                        {
                            output = outFloat;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("Guid"):
                        Guid outGuid = Guid.Empty;
                        if (Guid.TryParse(sqlString, out outGuid))
                        {
                            output = outGuid;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("short"):
                        short outInt16 = 0;
                        if (short.TryParse(sqlString, out outInt16))
                        {
                            output = outInt16;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("int"):
                        int outInt32 = 0;
                        if (int.TryParse(sqlString, out outInt32))
                        {
                            output = outInt32;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("long"):
                        long outInt64 = 0;
                        if (long.TryParse(sqlString, out outInt64))
                        {
                            output = outInt64;
                            return true;
                        }
                        else
                        {
                            Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData.");
                            break;
                        }
                    case ("System.String"):
                        output = sqlString;
                        return true;
                    default:
                        Log.Logger.LogError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + t + " in SqlStringToData because this datatype is not supported.");
                        break;
                }
            }

            output = null;
            return false;
        }

    }
}
