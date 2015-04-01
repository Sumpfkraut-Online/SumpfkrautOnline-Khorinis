using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
//using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;

namespace GUC.Server.Scripts.Sumpfkraut.Database
{

    enum DefTableEnum
    {
        Mob_def,
        Item_def,
        Spell_def,
        NPC_def,
        Effect_def,
        Effect_Changes_def,
    }

    enum InstTableEnum
    {
        World_inst,
        Account_inst,

        MobDef_Effects_inst,
        ItemDef_Effects_inst,
        NPCDef_Effects_inst,

        Mob_inst,
        Item_inst,
        NPC_inst,

        ItemInInventory_inst,
        MobInWorld_inst,
        ItemInWorld_inst,
        NPCInWorld_inst,
    }

    enum EffectsInstTableEnum
    {
        Mob_Effects_inst,
        Spell_Effects_inst,
        Item_Effects_inst,
        NPC_Effects_inst
    }

    /**
    *   Enum of the supported SQLite-Get-Types, used internally 
    */
    enum SQLiteGetTypeEnum
    {
        GetBoolean          = 0,
        GetByte             = 1,
        GetChar             = 2,
        GetDateTime         = 3,
        GetDecimal          = 4,
        GetDouble           = 5,
        GetFloat            = 6,
        GetGuid             = 7,
        GetInt16            = 8,
        GetInt32            = 9,
        GetInt64            = 10,
        GetString           = 11,
    }

    class DBTables
    {

        public static readonly Dictionary<DefTableEnum, string>
            DefTableNamesDict = new Dictionary<DefTableEnum, string>
        {
            {DefTableEnum.Mob_def, "Mob_def"},
            {DefTableEnum.Item_def, "Item_def"},
            {DefTableEnum.Spell_def, "Spell_def"},
            {DefTableEnum.NPC_def, "NPC_def"},
            {DefTableEnum.Effect_def, "Effect_def"},
            {DefTableEnum.Effect_Changes_def, "Effect_Changes_def"}
        };

        /**
        *   Definition datatable enum entries mapped to their respective access-functions 
        *   (further strcutured in a dictionary).
        *   This dictionary is mainly used to allow parts of the program to know what datatype should be read
        *   from the underlying SQLite-datatable via sqlReadType and SQLiteDataReader.
        *   @see sqlReadType
        *   @see SQLiteDataReader
        */
        public static readonly Dictionary<DefTableEnum, Dictionary<String, SQLiteGetTypeEnum>>
            DefTableDict = new Dictionary<DefTableEnum, Dictionary<String, SQLiteGetTypeEnum>> 
        {
            {
                DefTableEnum.Mob_def, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"ID",                      SQLiteGetTypeEnum.GetInt32},
                    {"VobType",                 SQLiteGetTypeEnum.GetInt32},
                    {"Visual",                  SQLiteGetTypeEnum.GetString},
                    {"Material",                SQLiteGetTypeEnum.GetInt32},
                    {"HasEffect",               SQLiteGetTypeEnum.GetBoolean}
                }
            },
            {
                DefTableEnum.Item_def, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"ID",                      SQLiteGetTypeEnum.GetInt32},
                    {"VobType",                 SQLiteGetTypeEnum.GetInt32},
                    {"Visual",                  SQLiteGetTypeEnum.GetString},
                    {"Visual_Skin",             SQLiteGetTypeEnum.GetInt32},
                    {"InstanceName",            SQLiteGetTypeEnum.GetString},
                    {"Name",                    SQLiteGetTypeEnum.GetString},
                    {"Description",             SQLiteGetTypeEnum.GetString},
                    {"ScemeName",               SQLiteGetTypeEnum.GetString},
                    {"MainFlag",                SQLiteGetTypeEnum.GetInt32},
                    {"Flags",                   SQLiteGetTypeEnum.GetInt32},
                    {"Material",                SQLiteGetTypeEnum.GetInt32},
                    {"HasEffect",               SQLiteGetTypeEnum.GetBoolean},
                    // better define them as effects due them being quite custom attributes/features
                    // which aren't shared by many most items
                    //{"IsGold",                  SQLiteGetTypeEnum.GetBoolean},
                    //{"IsKeyInstance",           SQLiteGetTypeEnum.GetBoolean},
                    //{"IsTorch",                 SQLiteGetTypeEnum.GetBoolean},
                    //{"IsTorchBurned",           SQLiteGetTypeEnum.GetBoolean},
                    //{"IsTorchBurned",           SQLiteGetTypeEnum.GetBoolean},
                }
            },
            {
                DefTableEnum.Spell_def, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"ID",                      SQLiteGetTypeEnum.GetInt32},
                    {"Name",                    SQLiteGetTypeEnum.GetString},
                    {"FXName",                  SQLiteGetTypeEnum.GetString},
                    {"AniName",                 SQLiteGetTypeEnum.GetString},
                    {"TimePerMana",             SQLiteGetTypeEnum.GetInt32},
                    {"DamagePerLevel",          SQLiteGetTypeEnum.GetInt32},
                    {"DamageType",              SQLiteGetTypeEnum.GetInt32},
                    {"SpellType",               SQLiteGetTypeEnum.GetInt32},
                    {"CanTurnDuringInvest",     SQLiteGetTypeEnum.GetBoolean},
                    {"CanChangeDuringInvest",   SQLiteGetTypeEnum.GetBoolean},
                    {"isMultiEffect",           SQLiteGetTypeEnum.GetBoolean},
                    {"TargetCollectionAlgo",    SQLiteGetTypeEnum.GetInt32},
                    {"TargetCollectType",       SQLiteGetTypeEnum.GetInt32},
                    {"TargetCollectRange",      SQLiteGetTypeEnum.GetInt32},
                    {"TargetCollectAzi",        SQLiteGetTypeEnum.GetInt32},
                    {"TargetCollectElev",       SQLiteGetTypeEnum.GetInt32},
                    {"HasEffect",               SQLiteGetTypeEnum.GetBoolean}
                }
            },
            {
                DefTableEnum.NPC_def, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"ID",                      SQLiteGetTypeEnum.GetInt32},
                    {"VobType",                 SQLiteGetTypeEnum.GetInt32},
                    {"Visual",                  SQLiteGetTypeEnum.GetString},
                    {"Visual_Skin",             SQLiteGetTypeEnum.GetInt32},
                    {"HasEffect",               SQLiteGetTypeEnum.GetBoolean}
                }
            },
            {
                DefTableEnum.Effect_def, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"ID",                      SQLiteGetTypeEnum.GetInt32},
                    {"Name",                    SQLiteGetTypeEnum.GetString}
                }
            },
            {
                DefTableEnum.Effect_Changes_def, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"Event",                   SQLiteGetTypeEnum.GetInt32},
                    {"EffectDefID",             SQLiteGetTypeEnum.GetInt32},
                    {"Change",                  SQLiteGetTypeEnum.GetInt32},
                    {"Parameters",              SQLiteGetTypeEnum.GetString}
                }
            }
        };



        public static readonly Dictionary<DefTableEnum, EffectsInstTableEnum>
            EffectInstAccesDict = new Dictionary<DefTableEnum, EffectsInstTableEnum>()
        {
            {DefTableEnum.Mob_def, EffectsInstTableEnum.Mob_Effects_inst},
            {DefTableEnum.Spell_def, EffectsInstTableEnum.Spell_Effects_inst},
            {DefTableEnum.Item_def, EffectsInstTableEnum.Item_Effects_inst},
            {DefTableEnum.NPC_def, EffectsInstTableEnum.NPC_Effects_inst},
        };

        public static readonly Dictionary<EffectsInstTableEnum, string>
            EffectsInstTableNamesDict = new Dictionary<EffectsInstTableEnum, string>()
        {
            {EffectsInstTableEnum.Mob_Effects_inst, "Mob_Effects_inst"},
            {EffectsInstTableEnum.Spell_Effects_inst, "Spell_Effects_inst"},
            {EffectsInstTableEnum.Item_Effects_inst, "Item_Effects_inst"},
            {EffectsInstTableEnum.NPC_Effects_inst, "NPC_Effects_inst"},
        };

        public static readonly Dictionary<EffectsInstTableEnum, Dictionary<String, SQLiteGetTypeEnum>>
            EffectsInstTableDict = new Dictionary<EffectsInstTableEnum, Dictionary<String, SQLiteGetTypeEnum>> 
        {
            {
                EffectsInstTableEnum.Mob_Effects_inst, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"MobDefID",                      SQLiteGetTypeEnum.GetInt32},
                    {"EffectDefID",                      SQLiteGetTypeEnum.GetInt32},
                }
            },
            {
                EffectsInstTableEnum.Spell_Effects_inst, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"SpellDefID",                      SQLiteGetTypeEnum.GetInt32},
                    {"EffectDefID",                      SQLiteGetTypeEnum.GetInt32},
                }
            },
            {
                EffectsInstTableEnum.Item_Effects_inst, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"ItemDefID",                      SQLiteGetTypeEnum.GetInt32},
                    {"EffectDefID",                      SQLiteGetTypeEnum.GetInt32},
                }
            },
            {
                EffectsInstTableEnum.NPC_Effects_inst, new Dictionary<String, SQLiteGetTypeEnum>
                {
                    {"NPCDefID",                      SQLiteGetTypeEnum.GetInt32},
                    {"EffectDefID",                      SQLiteGetTypeEnum.GetInt32},
                }
            },
        };



        /**
         *   Method to convert strings of a sql-result to the hinted datatype.
         *   Return or false if the data conversion succeeds or fails, respectively.
         *   @param sqlString is the string to convert
         *   @param get is the hint on the final data type of the conversion
         *   @param output is the object where the resulting type-converted string will be stored into
         */
        public static bool SqlStringToData (string sqlString, SQLiteGetTypeEnum get, ref object output)
        {
            if (sqlString != null)
            {
                switch (get)
                {
                    case (SQLiteGetTypeEnum.GetBoolean):
                        bool outBool = false;
                        if (bool.TryParse(sqlString, out outBool))
                        {
                            output = outBool;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetByte):
                        byte outByte = 0;
                        if (byte.TryParse(sqlString, out outByte))
                        {
                            output = outByte;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetChar):
                        char outChar = '0';
                        if (char.TryParse(sqlString, out outChar))
                        {
                            output = outChar;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetDateTime):
                        DateTime outDateTime = new DateTime();
                        if (DateTime.TryParse(sqlString, out outDateTime))
                        {
                            output = outDateTime;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetDecimal):
                        decimal outDecimal = 0;
                        if (decimal.TryParse(sqlString, out outDecimal))
                        {
                            output = outDecimal;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetDouble):
                        double outDouble = 0;
                        if (double.TryParse(sqlString, out outDouble))
                        {
                            output = outDouble;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetFloat):
                        float outFloat = 0;
                        if (float.TryParse(sqlString, out outFloat))
                        {
                            output = outFloat;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetGuid):
                        Guid outGuid = Guid.Empty;
                        if (Guid.TryParse(sqlString, out outGuid))
                        {
                            output = outGuid;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetInt16):
                        Int16 outInt16 = 0;
                        if (Int16.TryParse(sqlString, out outInt16))
                        {
                            output = outInt16;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetInt32):
                        Int32 outInt32 = 0;
                        if (Int32.TryParse(sqlString, out outInt32))
                        {
                            output = outInt32;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetInt64):
                        Int64 outInt64 = 0;
                        if (Int64.TryParse(sqlString, out outInt64))
                        {
                            output = outInt64;
                            return true;
                        }
                        else
                        {
                            Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData.");
                            break;
                        }
                    case (SQLiteGetTypeEnum.GetString):
                        output = sqlString;
                        return true;
                    default:
                        Log.Logger.logError("Could not convert sql-result-string '" + sqlString 
                                + "' with " + get + " in SqlStringToData because this datatype is not supported.");
                        break;
                }
            }

            return false;
        }
        
        /**
         *   Method that is mainly used internally to read data from SQLiteDataReader given the type.
         *   It is used as more flexible "interface" to the standart Get-methods of the SQLiteDataReader
         *   by other methods. Extra care should be taken when processing the return value, because
         *   it is of variable type (although generalized as object-type).
         *   @param rdr is the SQLiteDataReader which has access to the sql query result
         *   @param col is the column of the current row of the sql result which should be read
         *   @param get provides the hint which get-method of the SQLiteDataReader should be used (type conversionon read)
         */
        public static object SqlReadType (ref SQLiteDataReader rdr, int col, SQLiteGetTypeEnum get)
        {
            try
            {
                switch (get)
                {
                    case (SQLiteGetTypeEnum.GetBoolean):
                        return (bool)rdr.GetBoolean(col);
                    case (SQLiteGetTypeEnum.GetByte):
                        return (byte)rdr.GetByte(col);
                    case (SQLiteGetTypeEnum.GetDateTime):
                        return (DateTime)rdr.GetDateTime(col);
                    case (SQLiteGetTypeEnum.GetDecimal):
                        return (decimal)rdr.GetDecimal(col);
                    case (SQLiteGetTypeEnum.GetDouble):
                        return (double)rdr.GetDouble(col);
                    case (SQLiteGetTypeEnum.GetFloat):
                        return (float)rdr.GetFloat(col);
                    case (SQLiteGetTypeEnum.GetGuid):
                        return (Guid)rdr.GetGuid(col);
                    case (SQLiteGetTypeEnum.GetInt16):
                        return (short)rdr.GetInt16(col);
                    case (SQLiteGetTypeEnum.GetInt32):
                        return (int)rdr.GetInt32(col);
                    case (SQLiteGetTypeEnum.GetInt64):
                        return (long)rdr.GetInt64(col);
                    case (SQLiteGetTypeEnum.GetString):
                        return (String)rdr.GetString(col);
                    default:
                        throw new Exception("SQLiteDataReader has no Get-method for SQLiteGetTypeEnum " 
                            + get + ".");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Couldn't get data from SQLiteDataReader from db-column " + col 
                    + " and with SQLiteGetTypeEnum get=" + get + ": " + ex);
            }
        }

    }
}
