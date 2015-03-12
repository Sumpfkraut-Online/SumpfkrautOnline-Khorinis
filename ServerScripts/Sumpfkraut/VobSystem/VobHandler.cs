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
using GUC.Enumeration;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{

    /**
     *   Class which initializes all vobs of the indivual types from database information: mobs, items, npcs.
     */
    class VobHandler
    {

        //protected List<MobDef> mobDefList = new List<MobDef>();
        //protected List<ItemDef> itemDefList = new List<ItemDef>();
        //protected List<SpellDef> spellDefList = new List<SpellDef>();
        //protected List<NPCDef> npcDefList = new List<NPCDef>();

        private static Dictionary<int, MobDef> mobDefDict = new Dictionary<int, MobDef>();
        private static Dictionary<int, ItemDef> itemDefDict = new Dictionary<int, ItemDef>();
        private static Dictionary<int, SpellDef> spellDefDict = new Dictionary<int, SpellDef>();
        private static Dictionary<int, NPCDef> npcDefDict = new Dictionary<int, NPCDef>();

        /**
         *   Call this method from outside to create the intial vob definitions
         *   (spells, items, mobs, npcs).
         */
        public static void Init ()
        {
            loadDefinitions(DefTableEnum.Spell_def);
            loadDefinitions(DefTableEnum.Item_def);
            loadDefinitions(DefTableEnum.Mob_def);
            loadDefinitions(DefTableEnum.NPC_def);
        }

        /**
         *   Loads the specified type of definitions from their resective datatables.
         *   The method prepares and executes the sqlite-query and reads the resulting values to create
         *   vob-definitions.
         *   @param defTab , the enum-entry which represents the type of definitions to load
         *   @see DefTableEnum
         */
        public static void loadDefinitions (DefTableEnum defTab)
        {
            if (!DBTables.DefTableDict.ContainsKey(defTab))
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

            /* try getting the necessary hints on data conversion for each column 
             * of the given defintion table */
            Dictionary<String, SQLiteGetTypeEnum> colTypes = null;
            if (!DBTables.DefTableDict.TryGetValue(defTab, out colTypes))
            {
                return;
            }

            /* receive list of vob-definitions here and iterate over it, 
             * loading and applying the effec-changes */
            // stores the read and converted data of the sql-query
            List<List<object>> defList = new List<List<object>>();
            // to lists to ensure same key-value-order for each row in rdr because the memory
            // allocation of the original dictionary and order might be changed during runtime
            List<string> colTypesKeys = new List<string>(colTypes.Keys);
            List<SQLiteGetTypeEnum> colTypesVals = new List<SQLiteGetTypeEnum>(colTypes.Values);
            LoadVobDef(defTabName, ref colTypes, out defList, out colTypesKeys, out colTypesVals);

            // !!! continue with loading Effect_Changes
            // !!! apply EffectChanges
            // !!! instantiate definitions objects (results in them being listed)

            // for easier list-indexing in list<object> of defList = List<List<object>>
            Dictionary<string, int> colDict = new Dictionary<string, int>();
            for (int i = 0; i < colTypesKeys.Count; i++)
            {
                colDict.Add(colTypesKeys[i], i);
            }

            // Please forgive me this horrible loop, the G:UC doesn't give me a chance to do it right!
            for (int r = 0; r < defList.Count; r++)
            {
                //instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, visualSkin, types, munition, keyInstance, torch, torchBurning, torchBurned, gold
                String instanceName = "";
                String name = "";
                String scemeName = "";
                int[] protection = null;
                int[] damages = null;
                int value = 0;
                MainFlags mainFlags = 0;
                Flags flags = 0;
                ArmorFlags armorFlags = 0;
                DamageType dmgType = 0;
                int totalDamage = 0;
                int range = 0;
                String visual = "";
                String visual_Change = "";
                String effect = "";
                int visualSkin = 0;
                MaterialTypes types = 0;
                ItemInstance munition = null;
                bool keyInstance = false;
                bool torch = false;
                bool torchBurning = false;
                bool torchBurned = false;
                bool gold = false;

                // temporary used index for more secure code
                int colIndex = -1;

                if (colDict.TryGetValue("InstanceName", out colIndex))
                {
                    instanceName = (String) defList[r][colIndex];
                }

                if (colDict.TryGetValue("Name", out colIndex))
                {
                    name = (String) defList[r][colIndex];
                }

                if (colDict.TryGetValue("ScemeName", out colIndex))
                {
                    name = (String) defList[r][colIndex];
                }

                // TO DO: protection assignment through loaded effect-changes

                if (colDict.TryGetValue("ScemeName", out colIndex))
                {
                    scemeName = (String) defList[r][colIndex];
                }

                // TO DO: damages assignment through loaded effect-changes

                // TO DO: value assignment through loaded effect-changes

                if (colDict.TryGetValue("MainFlag", out colIndex))
                {
                    mainFlags = (MainFlags) defList[r][colIndex];
                }

                // TO DO: value assignment through loaded effect-changes

                // TO DO: armorFlags assignment through loaded effect-changes

                // TO DO: dmgType assignment through loaded effect-changes

                // TO DO: totalDamage assignment through loaded effect-changes

                // TO DO: range assignment through loaded effect-changes

                if (colDict.TryGetValue("Visual", out colIndex))
                {
                    visual = (String) defList[r][colIndex];
                }

                // TO DO: visual_Change assignment through loaded effect-changes

                // TO DO: effect assignment through loaded effect-changes

                if (colDict.TryGetValue("Visual_Skin", out colIndex))
                {
                    visualSkin = (int) defList[r][colIndex];
                }

                if (colDict.TryGetValue("Material", out colIndex))
                {
                    types = (MaterialTypes) defList[r][colIndex];
                }

                // TO DO: munition assignment through loaded effect-changes

                // TO DO: keyInstance assignment through loaded effect-changes

                // TO DO: torch assignment through loaded effect-changes

                // TO DO: torchBurning assignment through loaded effect-changes

                // TO DO: torchBurned assignment through loaded effect-changes

                // TO DO: gold assignment through loaded effect-changes

                // create new ItemInstance to list it internally for the G:UC and list it
                // for the standardized serverscripts too
                ItemDef newDef = new ItemDef(instanceName, name, scemeName, protection, damages, 
                    value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, 
                    visual_Change, effect, visualSkin, types, munition, keyInstance, torch, 
                    torchBurning, torchBurned, gold);
                if (newDef != null)
                {
                    itemDefDict.Add(0, newDef); ;
                }


            }
            

        }

        private static void LoadVobDef (string defTabName, ref Dictionary<String, SQLiteGetTypeEnum> colTypes, 
            out List<List<object>> defList, out List<string> colTypesKeys, 
            out List<SQLiteGetTypeEnum> colTypesVals, string sqlWhere="1")
        {
            // stores the read and converted data of the sql-query
            defList = new List<List<object>>();
            // to lists to ensure same key-value-order for each row in rdr because, otherwise, the memory
            // adresses of the original dictionary and order might be changed during runtime
            colTypesKeys = new List<string>(colTypes.Keys);
            colTypesVals = new List<SQLiteGetTypeEnum>(colTypes.Values);

            using (SQLiteCommand cmd = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                cmd.CommandText = "SELECT (" + String.Join(",", colTypesKeys.ToArray()) + ") FROM `" 
                    + defTabName + "` WHERE " + sqlWhere;
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

                    while (rdr.Read())
                    {
                        rowList = new List<object>();
                        for (int col=0; col<colTypesKeys.Count; col++)
                        {
                            rowList.Add(DBTables.SqlReadType(ref rdr, col, colTypesVals[col]));
                        }
                        defList.Add(rowList);
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

        private static void LoadEffectDef (out List<int> colKeys, out List<object> colVals, 
            ref List<int> effectDefIDs)
        {
            colKeys = new List<int>();
            colVals = new List<object>();

            if ((effectDefIDs == null) || (effectDefIDs.Count <= 0))
            {
                return;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                cmd.CommandText = "SELECT * FROM `Effect_def` WHERE `ID` IN (" 
                    + String.Join(",", effectDefIDs.ToArray()) 
                    + ") ORDER BY `ID` ASC;";

                SQLiteDataReader rdr = null;
                try
                {
                    rdr = cmd.ExecuteReader();
                    if (!rdr.HasRows)
                    {
                        return;
                    }

                    Dictionary<String, SQLiteGetTypeEnum> colTypes = null;
                    if (!DBTables.DefTableDict.TryGetValue(DefTableEnum.Effect_Changes_def, out colTypes))
                    {
                        return;
                    }

                    int col = 0;
                    while(rdr.Read())
                    {
                        col = 0;
                        foreach(KeyValuePair<string, SQLiteGetTypeEnum> e in colTypes)
                        {
                            //colKeys.Add(e.Key);
                            //colVals.Add(DBTables.SqlReadType(ref rdr, col, e.Value));
                            col++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not execute SQLiteDataReader during loading of effect changes definitions: " + ex);
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


        // must be able to read multiple rows 
        // count of colVals multiple of the count of colKeys to prevent unnecessary repetition?
        // 1 => object, 2 => object, 3 => object, 4 => object, ... , 1 => object, ...
        private static void LoadEffectChangesDef (out List<int> colKeys, out List<object> colVals, 
            ref List<int> effectDefIDs)
        {
            colKeys = new List<int>();
            colVals = new List<object>();

            if ((effectDefIDs == null) || (effectDefIDs.Count <= 0))
            {
                return;
            }

            using (SQLiteCommand cmd = new SQLiteCommand(Sqlite.getSqlite().connection))
            {
                cmd.CommandText = "SELECT * FROM `Effect_Changes_def` WHERE `EffectDefID` IN (" 
                    + String.Join(",", effectDefIDs.ToArray()) 
                    + ") ORDER BY `EffectDefID` ASC;";

                SQLiteDataReader rdr = null;
                try
                {
                    rdr = cmd.ExecuteReader();
                    if (!rdr.HasRows)
                    {
                        return;
                    }

                    Dictionary<String, SQLiteGetTypeEnum> colTypes = null;
                    DBTables.DefTableDict.TryGetValue(DefTableEnum.Effect_Changes_def, out colTypes);
                    if (colTypes == null)
                    {
                        return;
                    }

                    

                    int col = 0;
                    while(rdr.Read())
                    {
                        col = 0;
                        foreach(KeyValuePair<string, SQLiteGetTypeEnum> e in colTypes)
                        {
                            //colKeys.Add(e.Key);
                            //colVals.Add(DBTables.SqlReadType(ref rdr, col, e.Value));
                            col++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Could not execute SQLiteDataReader during loading of effect changes definitions: " + ex);
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
