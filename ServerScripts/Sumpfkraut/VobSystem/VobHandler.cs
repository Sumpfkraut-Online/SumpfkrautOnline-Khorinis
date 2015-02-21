using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using GUC.Server.Scripting.Objects;
//using GUC.Server.Scripting.Objects.Character;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.Database;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{

    /**
     *   Class which initializes all vobs of the indivual types from database information: mobs, items, npcs.
     */
    class VobHandler
    {

        protected List<MobDef> mobDefList = new List<MobDef>();
        protected List<ItemDef> itemDefList = new List<ItemDef>();
        protected List<SpellDef> spellDefList = new List<SpellDef>();
        protected List<NPCDef> npcDefList = new List<NPCDef>();

        /**
         *   Call this method from outside to create the intiial vob definitions.
         */
        public static void Init ()
        {
            
        }

        /**
         *   Loads the specified type of definitions from their resective datatables.
         *   The method prepares and executes the sqlite-query and reads the resulting values to create
         *   vob-definitions.
         *   @param defTab , the enum-entry which represents the type of definitions to load
         *   @see DefTableEnum
         */
        private static void loadDefinition(DefTableEnum defTab)
        {
            if ((defTab == null) || (!DBTables.DefTableDict.ContainsKey(defTab)))
            {
                return;
            }

            string defTabName = null;
            DBTables.DefTableNames.TryGetValue(defTab, out defTabName);
            if (defTabName == null)
            {
                // if there is no table of that name
                return;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                cmd.CommandText = "SELECT * FROM `" + defTabName + "` WHERE 1";
                SQLiteDataReader rdr = null;
                try
                {
                    rdr = cmd.ExecuteReader();
                    if (!rdr.HasRows)
                    {
                        return;
                    }

                    Dictionary<String, SQLiteGetTypeEnum> colTypes = null;
                    DBTables.DefTableDict.TryGetValue(defTab, out colTypes);
                    //List<string> colKeys = new List<string>(colTypes.Count());
                    string[] colKeys = new string[colTypes.Count()];
                    //List<object> colVals = new List<object>(colTypes.Count());
                    object[] colVals = new object[colTypes.Count()];
                    bool hasEffects = false; 

                    int col = 0;
                    while (rdr.Read())
                    {
                        col = 0;
                        foreach(KeyValuePair<string, SQLiteGetTypeEnum> e in colTypes)
                        {
                            colKeys[col] = e.Key;
                            colVals[col] = DBTables.SqlReadType(ref rdr, col, e.Value);
                            if ((colKeys[col] == "HasEffects") && ((bool) colVals[col]))
                            {
                                hasEffects = true;
                            }
                            col++;
                        }

                        // TO DO: Forward data into functionality which applies changes in various ways
                        // for vob definitions, the effect and effect-changes definitons have to be loaded
                        // directly one after the other!!!
                        if (hasEffects)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not execute SQLiteDataReader during vob-definiton-loading: " + ex);
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
