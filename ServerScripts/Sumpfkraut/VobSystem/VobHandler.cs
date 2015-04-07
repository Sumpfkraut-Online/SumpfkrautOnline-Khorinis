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
            /* -------------------------------------------------------------
                getting vob-definitions
               ------------------------------------------------------------- */

            if (!DBTables.DefTableDict.ContainsKey(defTab))
            {
                return;
            }

            string defTabName = null;
            DBTables.DefTableNamesDict.TryGetValue(defTab, out defTabName);
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
            List<List<List<object>>> defList = new List<List<List<object>>>();
            // to lists to ensure same key-value-order for each row in rdr because the memory
            // allocation of the original dictionary and order might be changed during runtime
            List<string> colTypesKeys = new List<string>(colTypes.Keys);
            List<SQLiteGetTypeEnum> colTypesVals = new List<SQLiteGetTypeEnum>();
            for (int i = 0; i < colTypesKeys.Count; i++)
            {
                colTypesVals.Add( colTypes[colTypesKeys[i]] );
            }

            LoadVobDef(defTabName, ref defList, defTab, ref colTypesKeys, ref colTypesVals);

            // for easier list-indexing in list<object> of defList = List<List<object>>
            Dictionary<string, int> colDict = new Dictionary<string, int>();
            for (int i = 0; i < colTypesKeys.Count; i++)
            {
                colDict.Add(colTypesKeys[i], i);
            }

            // a little unhandy but the instantiation of the ItemDef-Object (which inherits ItemInstance)
            // must be done in one shot 
            // (not all properties can be changed afterwards
            //     + the object is only synchronized when a user performs login 
            //     --> would need to login agan to synchronize)
            for (int r = 0; r < defList[0].Count; r++)
            {
                /* -------------------------------------------------------------
                    getting vob-effects-instances (EI) [for each vob-defintion]
                   ------------------------------------------------------------- */

                // try getting the necessary hints on data conversion for each column 
                // of the given vob-specific effect-instance table (EI)
                List<List<List<object>>> instList_EI = new List<List<List<object>>>();
                Dictionary<string, SQLiteGetTypeEnum> colTypes_EI = null;
                Database.InstTableEnum effectsInstTab = 0;
                if (!DBTables.EffectInstAccesDict.TryGetValue(defTab, out effectsInstTab))
                {
                    return;
                }
                if (!DBTables.InstTableDict.TryGetValue(effectsInstTab, out colTypes_EI))
                {
                    return;
                }
                // to lists to ensure same key-value-order for each row in rdr because the memory
                // allocation of the original dictionary and order might be changed during runtime
                List<string> colTypesKeys_EI = new List<string>(colTypes_EI.Keys);
                List<SQLiteGetTypeEnum> colTypesVals_EI = new List<SQLiteGetTypeEnum>();
                for (int i = 0; i < colTypesKeys_EI.Count; i++)
                {
                    colTypesVals_EI.Add( colTypes_EI[colTypesKeys_EI[i]] );
                }

                // filter out the vob-defintiion-ids to look them up in effects-instance-tables later
                List<int> defIDs = new List<int>();
                int idColIndex = colTypesKeys.IndexOf("ID");
                for (int i = 0; i < defList[0].Count; i++)
                {
                    defIDs.Add((int)defList[0][i][idColIndex]);
                }

                // column name and index where the vob-id is
                string vobIDColName = null;
                if (!DBTables.EffectsInstTableDefIDDict.TryGetValue(effectsInstTab, out vobIDColName))
                {
                    return;
                }
                int vobIDColIndex = colTypesKeys_EI.IndexOf(vobIDColName);

                // use the vob-ids to request all effect-ids which belong to them
                LoadEffectsInst(effectsInstTab, ref instList_EI,
                    ref colTypesKeys_EI, ref colTypesVals_EI, "vobIDColName IN (" + String.Join(",", defIDs.ToArray()) + ")");

                // filter out the effect-instance-ids to look them up in effects-changes-tables later
                List<int> effectDefIDs = new List<int>();
                for (int i = 0; i < instList_EI[0].Count; i++)
                {
                    effectDefIDs.Add((int)instList_EI[0][i][vobIDColIndex]);
                }

                /* -------------------------------------------------------------
                    getting effect-changes-defintions (EC) [for each vob-defintion]
                   ------------------------------------------------------------- */

                // try getting the necessary hints on data conversion for each column 
                // of the given vob-specific effect-instance table (EI)
                List<List<List<object>>> defList_EC = new List<List<List<object>>>();
                Dictionary<string, SQLiteGetTypeEnum> colTypes_EC = null;
                if (!DBTables.DefTableDict.TryGetValue(defTab, out colTypes_EC))
                {
                    return;
                }

                // to lists to ensure same key-value-order for each row in rdr because the memory
                // allocation of the original dictionary and order might be changed during runtime
                List<string> colTypesKeys_EC = new List<string>(colTypes_EC.Keys);
                List<SQLiteGetTypeEnum> colTypesVals_EC = new List<SQLiteGetTypeEnum>();
                for (int i = 0; i < colTypesKeys_EC.Count; i++)
                {
                    colTypesVals_EC.Add( colTypes_EC[colTypesKeys_EC[i]] );
                }

                LoadEffectChangesDef(ref effectDefIDs, ref defList_EC,
                    ref colTypesKeys_EC, ref colTypesVals_EC);

                /* -------------------------------------------------------------
                    create actual instances for vob definitions
                   ------------------------------------------------------------- */

                // !!! mixed up accidentally loading effect changes for all vobs in defList 
                // and the loop over each single vob definition row... MUST BE SOLVED!!!
                //createVobDefinition(defTab, ref defList[0][r], ref defList_EC);
            }
        }



        /* ---------------------------------------------------------
            DATABASE INTERACTION
           --------------------------------------------------------- */


        /**
         *   Load vob definitions (without effect changes definitions) as usable data objects from database.
         *   Requires no colTypes-parameter the data column names and types will be stored in
         *   colTypesKeys and colTypesVals.
         */
        private static void LoadVobDef (string defTabName, ref List<List<List<object>>> defList,
            DefTableEnum defTabEnum,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
            string sqlWhere="1")
        {
            Dictionary<String, SQLiteGetTypeEnum> colTypes = null;
            if (DBTables.DefTableDict.TryGetValue(DefTableEnum.Effect_Changes_def, out colTypes))
            {
                LoadVobDef(defTabName, ref defList, ref colTypes, ref colTypesKeys, 
                ref colTypesVals, sqlWhere);
            }
            else
            {
                throw new Exception("Cannot load vob definition of vobtype " + defTabEnum + ".");
            }  
        }


        /**
         *   Lower level loading of vob definition that requires colTypes-paramter.
         *   @param defTabName is the name of the handled definition table
         *   @param defList is used to store the sql-result of your sql-query
         *   @param colTypes describes which column (by name) used which data type
         *   @param colTypesKeys stores names of colTypes in the order used in the sql-query
         *   @param colTypesVals stores the types of colTyes in the order used in the sql-query
         */
        private static void LoadVobDef (string defTabName, ref List<List<List<object>>> defList, 
            ref Dictionary<String, SQLiteGetTypeEnum> colTypes, 
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
            string sqlWhere="1")
        {
            // grab from database
            DBReader.LoadFromDB(ref defList, 
                "(" + String.Join(",", colTypesKeys.ToArray())  + ")", 
                defTabName, 
                sqlWhere, 
                "ID ASC");

            // convert individual sql-result-strings to usable data of given types
            object tempEntry = null;
            int r = 0;
            int c = 0;
            while (r < defList[1].Count)
            {
                c = 0;
                while (c < defList[1][r].Count)
                {
                    tempEntry = defList[1][r][c];
                    if (DBTables.SqlStringToData((string)tempEntry, colTypesVals[c], ref tempEntry))
                    {
                        defList[1][r][c] = tempEntry;
                    }
                    else
                    {
                        Log.Logger.logError("Could no convert " + tempEntry + " from string to type " 
                            + colTypesVals[c] + " in method LoadVobDef.");
                    }
                    c++;
                }
                r++;
            }
        }


        private static void LoadEffectsInst (InstTableEnum instTab, ref List<List<List<object>>> defList, 
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
            string sqlWhere="1")
        {
            string instTabName = "";
            if (!DBTables.EffectsInstTableNamesDict.TryGetValue(instTab, out instTabName))
            {
                return;
            }
            string orderByID = "";
            if (!DBTables.EffectsInstTableDefIDDict.TryGetValue(instTab, out orderByID))
            {
                return;
            }

            // grab effect-ids from database
            DBReader.LoadFromDB(ref defList, 
                "(" + String.Join(",", colTypesKeys.ToArray())  + ")", 
                instTabName, 
                sqlWhere, 
                " " + orderByID + " ASC");

            // convert individual sql-result-strings to usable data of given types
            object tempEntry = null;
            int r = 0;
            int c = 0;
            while (r < defList[1].Count)
            {
                c = 0;
                while (c < defList[1][r].Count)
                {
                    tempEntry = defList[1][r][c];
                    if (DBTables.SqlStringToData((string)tempEntry, colTypesVals[c], ref tempEntry))
                    {
                        defList[1][r][c] = tempEntry;
                    }
                    else
                    {
                        Log.Logger.logError("Could no convert " + tempEntry + " from string to type " 
                            + colTypesVals[c] + " in method LoadEffectsInst.");
                    }
                    c++;
                }
                r++;
            }
        }


        private static void LoadEffectChangesDef (ref List<int> effectDefIDs, ref List<List<List<object>>> defList, 
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        {
            if (!(effectDefIDs.Count > 0))
            {
                Log.Logger.logError("Cannot load effect-changes-definitions with 0 effect-definition-ids" 
                + " in LoadEffectChangesDef.");
                return;
            }
            string sqlWhere = "EffectDefID IN (" + String.Concat(",", effectDefIDs.ToArray()) + ")";
            LoadEffectChangesDef(ref defList,
                ref colTypesKeys, ref colTypesVals,
                sqlWhere);
        }


        // must be able to read multiple rows 
        // count of colVals multiple of the count of colKeys to prevent unnecessary repetition?
        // 1 => object, 2 => object, 3 => object, 4 => object, ... , 1 => object, ...
        private static void LoadEffectChangesDef (ref List<List<List<object>>> defList, 
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
            string sqlWhere="1")
        {
            // grab from database
            DBReader.LoadFromDB(ref defList, 
                "(" + String.Join(",", colTypesKeys.ToArray())  + ")", 
                "Effect_Changes_def", 
                sqlWhere, 
                "EffectDefID ASC");

            // convert individual sql-result-strings to usable data of given types
            object tempEntry = null;
            int r = 0;
            int c = 0;
            while (r < defList[1].Count)
            {
                c = 0;
                while (c < defList[1][r].Count)
                {
                    tempEntry = defList[1][r][c];
                    if (DBTables.SqlStringToData((string)tempEntry, colTypesVals[c], ref tempEntry))
                    {
                        defList[1][r][c] = tempEntry;
                    }
                    else
                    {
                        Log.Logger.logError("Could no convert " + tempEntry + " from string to type " 
                            + colTypesVals[c] + " in method LoadEffectChangesDef.");
                    }
                    c++;
                }
                r++;
            }
        }



        /* ---------------------------------------------------------
            APPLICATION OF RECEIVED DATABASE ENTRIES
           --------------------------------------------------------- */


        // !!! TO DO: transfer attribute data and the list<list<list<object>>> of the effect-changes-definition !!!
        private static void createVobDefinition (DefTableEnum defTab)
        {
            switch (defTab)
            {
                case (DefTableEnum.Mob_def):
                    createMobDefinition();
                    break;
                case (DefTableEnum.Spell_def):
                    createMobDefinition();
                    break;
                case (DefTableEnum.Item_def):
                    createMobDefinition();
                    break;
                case (DefTableEnum.NPC_def):
                    createMobDefinition();
                    break;
                default:
                    throw new Exception("Invalid DefTableEnum " + defTab + " detected in method createVobDefinition"
                        + ": Terminating vob-definition-creation!");
            }
        }


        private static void createMobDefinition ()
        {

        }


        private static void createSpellDefinition ()
        {

        }


        private static void createItemDefinition ()
        {
            //// these default values are just substitute for the subsequent replacements
            //String instanceName = "";
            //String name = "";
            //String scemeName = "";
            //int[] protection = null;
            //int[] damages = null;
            //int value = 0;
            //MainFlags mainFlags = 0;
            //Flags flags = 0;
            //ArmorFlags armorFlags = 0;
            //DamageTypes dmgType = 0;
            //int totalDamage = 0;
            //int range = 0;
            //String visual = "";
            //String visual_Change = "";
            //String effect = "";
            //int visualSkin = 0;
            //MaterialType types = 0;
            //ItemInstance munition = null;
            //bool keyInstance = false;
            //bool torch = false;
            //bool torchBurning = false;
            //bool torchBurned = false;
            //bool gold = false;

            //// temporary used index for more secure code through TryGetValue (see if-blocks below)
            //int colIndex = -1;

            //if (colDict.TryGetValue("InstanceName", out colIndex))
            //{
            //    instanceName = (String) defList[r][0][colIndex];
            //}

            //if (colDict.TryGetValue("Name", out colIndex))
            //{
            //    name = (String) defList[r][0][colIndex];
            //}

            //if (colDict.TryGetValue("ScemeName", out colIndex))
            //{
            //    name = (String) defList[r][0][colIndex];
            //}

            //// TO DO: protection assignment through loaded effect-changes

            //if (colDict.TryGetValue("ScemeName", out colIndex))
            //{
            //    scemeName = (String) defList[r][0][colIndex];
            //}

            //// TO DO: damages assignment through loaded effect-changes

            //// TO DO: value assignment through loaded effect-changes

            //if (colDict.TryGetValue("MainFlag", out colIndex))
            //{
            //    mainFlags = (MainFlags) defList[r][0][colIndex];
            //}

            //// TO DO: value assignment through loaded effect-changes

            //// TO DO: armorFlags assignment through loaded effect-changes

            //// TO DO: dmgType assignment through loaded effect-changes

            //// TO DO: totalDamage assignment through loaded effect-changes

            //// TO DO: range assignment through loaded effect-changes

            //if (colDict.TryGetValue("Visual", out colIndex))
            //{
            //    visual = (String) defList[r][0][colIndex];
            //}

            //// TO DO: visual_Change assignment through loaded effect-changes

            //// TO DO: effect assignment through loaded effect-changes

            //if (colDict.TryGetValue("Visual_Skin", out colIndex))
            //{
            //    visualSkin = (int) defList[r][0][colIndex];
            //}

            //if (colDict.TryGetValue("Material", out colIndex))
            //{
            //    types = (MaterialType) defList[r][0][colIndex];
            //}

            //// TO DO: munition assignment through loaded effect-changes

            //// TO DO: keyInstance assignment through loaded effect-changes

            //// TO DO: torch assignment through loaded effect-changes

            //// TO DO: torchBurning assignment through loaded effect-changes

            //// TO DO: torchBurned assignment through loaded effect-changes

            //// TO DO: gold assignment through loaded effect-changes

            //// create new ItemInstance to list it internally for the G:UC and list it
            //// for the standardized serverscripts too
            //ItemDef newDef = new ItemDef(instanceName, name, scemeName, protection, damages, 
            //    value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, 
            //    visual_Change, effect, visualSkin, types, munition, keyInstance, torch, 
            //    torchBurning, torchBurned, gold);
            //if (newDef != null)
            //{
            //    itemDefDict.Add(0, newDef); ;
            //}
        }


        private static void createNPCDefinition ()
        {

        }

    }
}
