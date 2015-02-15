using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
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
        Effect_Changes_def
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
        NPCInWorld_inst
    }

    /**
    *   Enum of the supported SQLite-Get-Types, used internally 
    */
    enum SQLiteGetTypeEnum
    {
        GetBoolean,
        GetByte,
        GetChar,
        GetDateTime,
        GetDecimal,
        GetDouble,
        GetFloat,
        GetGuid,
        GetInt16,
        GetInt32,
        GetInt64,
        GetString,
    }

    class DBTables
    {

        public static readonly Dictionary<DefTableEnum, string>
            DefTableNames = new Dictionary<DefTableEnum, string>
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
        //public static Dictionary<DefTableEnum, SQLiteGetTypeEnum[]> DefTableDict = new Dictionary<DefTableEnum, SQLiteGetTypeEnum[]>();
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

        /**
        *   Method that is mainly used internally to read data from SQLiteDataReader given the type.
        *   It is used as more flexible "interface" to the standart Get-methods of the SQLiteDataReader
        *   by other methods. Extra care should be taken when processing the return value, because
        *   it is of variable type (although generalized as object-type).
        */
        public static object SqlReadType(ref SQLiteDataReader rdr, int col, SQLiteGetTypeEnum get)
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
