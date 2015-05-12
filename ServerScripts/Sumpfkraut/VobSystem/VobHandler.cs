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
        public static Dictionary<int, ItemDef> itemDefDict = new Dictionary<int, ItemDef>();
        private static Dictionary<int, SpellDef> spellDefDict = new Dictionary<int, SpellDef>();
        private static Dictionary<int, NPCDef> npcDefDict = new Dictionary<int, NPCDef>();



        /**
         *   Call this method from outside to create the intial vob definitions
         *   (spells, items, mobs, npcs).
         */
        public static void Init ()
        {
            //LoadDefinitions(DefTableEnum.Spell_def);
            LoadDefinitions(DefTableEnum.Item_def);
            //LoadDefinitions(DefTableEnum.Mob_def);
            //LoadDefinitions(DefTableEnum.NPC_def);
        }


        /**
         *   Loads the specified type of definitions from their resective datatables.
         *   The method prepares and executes the sqlite-query and reads the resulting values to create
         *   vob-definitions.
         *   @param defTab , the enum-entry which represents the type of definitions to load
         *   @see DefTableEnum
         */
        public static void LoadDefinitions (DefTableEnum defTab)
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
            int hasEffectsColIndex = colTypesKeys.IndexOf("HasEffects");
            for (int i = 0; i < defList[0].Count; i++)
            {
                if ((bool)defList[0][i][hasEffectsColIndex])
                {
                    defIDs.Add((int)defList[0][i][idColIndex]);
                }
            }

            // column name and index where the vob-id is
            string vobIDColName = null;
            if (!DBTables.EffectsInstTableDict_VobDefID.TryGetValue(effectsInstTab, out vobIDColName))
            {
                return;
            }
            int vobIDColIndex = colTypesKeys_EI.IndexOf(vobIDColName);

            // use the vob-ids to request all effect-ids which belong to them
            if (defIDs.Count > 0)
            {
                LoadEffectsInst(effectsInstTab, ref instList_EI,
                    ref colTypesKeys_EI, ref colTypesVals_EI, 
                    vobIDColName + " IN (" + String.Join(",", defIDs.ToArray()) + ")");
            }

            // filter out the effect-instance-ids to look them up in effects-changes-tables later
            // + map vob definition ids to their related effect ids
            List<int> effectDefIDs = new List<int>();
            int effectIDColIndex = colTypesKeys_EI.IndexOf("EffectDefID");
            int eiEffectID = -1;
            Dictionary<int, List<int>> vobToEffectsMap = new Dictionary<int, List<int>>();
            List<int> tempVobEffectDefIDs;
            int eiVobID = -1;
            if (instList_EI.Count > 0)
            {
                for (int i = 0; i < instList_EI[0].Count; i++)
                {
                    eiEffectID = (int) instList_EI[0][i][effectIDColIndex];
                    eiVobID = (int) instList_EI[0][i][vobIDColIndex];

                    if (!effectDefIDs.Contains(eiEffectID))
                    {
                        effectDefIDs.Add(eiEffectID);
                    }

                    if (vobToEffectsMap.TryGetValue(eiVobID, out tempVobEffectDefIDs))
                    {
                        if (!tempVobEffectDefIDs.Contains(eiEffectID))
                        {
                            tempVobEffectDefIDs.Add(eiEffectID);
                        }
                    }
                    else
                    {
                        tempVobEffectDefIDs = new List<int>() {eiEffectID};
                        vobToEffectsMap.Add(eiVobID, tempVobEffectDefIDs);
                    }
                }
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

            // remember those EffectChanges for later use (no need for less handy defList_EC from here on)
            // + map effectIDs to the ids of EffectChanges (for late use in the vob-instantiation)
            int ecIDColIndex = colTypesKeys_EC.IndexOf("ID");
            int ecCTColIndex = colTypesKeys_EC.IndexOf("ChangeType");
            int ecParamColIndex = colTypesKeys_EC.IndexOf("Parameters");
            int ecEffDefIDColIndex = colTypesKeys_EC.IndexOf("EffectDefID");
            Dictionary<int, List<int>> effectToChangesMap = new Dictionary<int, List<int>>();
            List<int> tempEffectChangesIDs;
            int ecEffectID = -1;
            int ecEffectChangeID = -1;
            if (defList_EC.Count > 0) 
            {
                for (int i = 0; i < defList_EC[0].Count; i++)
                {
                    ecEffectID = (int) defList_EC[0][i][ecEffDefIDColIndex];
                    ecEffectChangeID = (int) defList_EC[0][i][ecIDColIndex];

                    Definitions.EffectChangesDef.Add(ecEffectChangeID, 
                        (EffectChangesEnum) defList_EC[0][i][ecCTColIndex],
                        (string) defList_EC[0][i][ecParamColIndex],
                        true);

                    if (effectToChangesMap.TryGetValue(ecEffectID, out tempEffectChangesIDs)){
                        if (!tempEffectChangesIDs.Contains(ecEffectChangeID))
                        {
                            tempEffectChangesIDs.Add(ecEffectChangeID);
                        }
                    }
                    else
                    {
                        tempEffectChangesIDs = new List<int>() {ecEffectChangeID};
                        effectToChangesMap.Add(ecEffectID, tempEffectChangesIDs);
                    }
                }
            }
            
            /* -------------------------------------------------------------
                create actual instances for vob definitions
                ------------------------------------------------------------- */

            CreateVobDefinitions(defTab, ref defList, ref colTypesKeys, ref colTypesVals, 
                ref vobToEffectsMap, ref effectToChangesMap);
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
                Log.Logger.logError("LoadVobDef: Cannot load vob definition of vobtype " + defTabEnum);
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
                String.Join(",", colTypesKeys.ToArray()), 
                "`" + defTabName + "`", 
                sqlWhere, 
                "ID ASC");

            // convert individual sql-result-strings to usable data of given types
            object tempEntry = null;
            int r = 0;
            int c = 0;
            while (r < defList[0].Count)
            {
                c = 0;
                while (c < defList[0][r].Count)
                {
                    Console.WriteLine("----------- " + colTypesKeys[c] + "|--|" + defList[0][r][c].GetType() + "|--|" + defList[0][r][c]);
                    if (defList[0][r][c] == null)
                    {
                        // null might be just fine
                    }
                    else if (defList[0][r][c].GetType() == typeof(DBNull))
                    {
                        // DBNull is aa little unheady because it would need additional type-checks later
                        // use null instead
                        defList[0][r][c] = null;
                    }
                    else
                    {
                        // everything else should be a string and somehow convertable
                        tempEntry = defList[0][r][c].ToString();
                        if (DBTables.SqlStringToData((string)tempEntry, colTypesVals[c], ref tempEntry))
                        {
                            defList[0][r][c] = tempEntry;
                        }
                        else
                        {
                            defList[0][r][c] = null;
                            Log.Logger.logError("Could no convert " + tempEntry + " from string to type " 
                                + colTypesVals[c] + " in method LoadVobDef for loaded database-entry of column "
                                + colTypesKeys[c]);
                        }
                    }
                     Console.WriteLine("----------- " + colTypesKeys[c] + "|--|" + (defList[0][r][c] == null) + "|--|" + defList[0][r][c]);
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
            if (!DBTables.EffectsInstTableDict_VobDefID.TryGetValue(instTab, out orderByID))
            {
                return;
            }

            // grab effect-ids from database
            DBReader.LoadFromDB(ref defList, 
                String.Join(",", colTypesKeys.ToArray()), 
                "`" + instTabName + "`", 
                sqlWhere, 
                " " + orderByID + " ASC");

            if (defList.Count < 1)
            {
                return;
            }

            // convert individual sql-result-strings to usable data of given types
            object tempEntry = null;
            int r = 0;
            int c = 0;
            while (r < defList[0].Count)
            {
                c = 0;
                while (c < defList[0][r].Count)
                {
                    tempEntry = defList[0][r][c];
                    if (DBTables.SqlStringToData((string)tempEntry, colTypesVals[c], ref tempEntry))
                    {
                        defList[0][r][c] = tempEntry;
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
                Log.Logger.logWarning("LoadEffectChangesDef: Cannot load effect-changes-definitions with 0 effect-definition-ids.");
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
                String.Join(",", colTypesKeys.ToArray()), 
                "`" + "Effect_Changes_def" + "`", 
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


        private static void CreateVobDefinitions (DefTableEnum defTab, ref List<List<List<object>>> defList,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
            ref Dictionary<int, List<int>> mapVDToED,
            ref Dictionary<int, List<int>> mapEDToECD)
        {
            List<object> def;
            int vobIDColIndex = colTypesKeys.IndexOf("ID");
            for (int r = 0; r < defList[0].Count; r++)
            {
                def = defList[0][r];
                CreateVobDefinition(defTab, ref def, 
                    ref colTypesKeys, ref colTypesVals, 
                    ref mapVDToED, 
                    ref mapEDToECD);
            }
        }

        private static void CreateVobDefinition (DefTableEnum defTab, ref List<object> def,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
            ref Dictionary<int, List<int>> mapVDToED, 
            ref Dictionary<int, List<int>> mapEDToECD)
        {
            switch (defTab)
            {
                case (DefTableEnum.Mob_def):
                    CreateMobDefinition(defTab, ref def, 
                        ref colTypesKeys, ref colTypesVals, 
                        ref mapVDToED, 
                        ref mapEDToECD);
                    break;
                case (DefTableEnum.Spell_def):
                    CreateSpellDefinition(defTab, ref def, 
                        ref colTypesKeys, ref colTypesVals, 
                        ref mapVDToED, 
                        ref mapEDToECD);
                    break;
                case (DefTableEnum.Item_def):
                    CreateItemDefinition(defTab, ref def, 
                        ref colTypesKeys, ref colTypesVals, 
                        ref mapVDToED, 
                        ref mapEDToECD);
                    break;
                case (DefTableEnum.NPC_def):
                    CreateNPCDefinition(defTab, ref def, 
                        ref colTypesKeys, ref colTypesVals, 
                        ref mapVDToED, 
                        ref mapEDToECD);
                    break;
                default:
                    throw new Exception("Invalid DefTableEnum " + defTab + " detected in method createVobDefinition"
                        + ": Terminating vob-definition-creation!");
            }
        }

        private static void CreateMobDefinition (DefTableEnum defTab, ref List<object> def,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
            ref Dictionary<int, List<int>> mapVDToED,
            ref Dictionary<int, List<int>> mapEDToECD)
        {

        }

        private static void CreateMobDefinition (ref DummyMobDef dummyDef)
        {
            try
            {
                MobDef newDef = new MobDef();
                if (newDef != null)
                {
                    //itemDefDict.Add(dummyDef.getID(), newDef);
                }
            }
            catch
            {
                throw new Exception("CreateItemDefinition: Some necessary attributes of DummyItemDef" 
                    + " are not valid or missing:" + dummyDef);
            }
        }


        private static void CreateSpellDefinition (DefTableEnum defTab, ref List<object> def,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
            ref Dictionary<int, List<int>> mapVDToED, 
            ref Dictionary<int, List<int>> mapEDToECD)
        {
            // !!! TODO ... later because spells are not needed at the moment!!!
        }


        private static void CreateItemDefinition (DefTableEnum defTab, ref List<object> def,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
            ref Dictionary<int, List<int>> mapVDToED, 
            ref Dictionary<int, List<int>> mapEDToECD)
        {
            string idColName = "ID";
            int vobID = colTypesKeys.IndexOf(idColName);
            // list of effect-definition-ids of the the handled vob-definition
            List<int> effectDefIDs;
            // respective list of effect-changes-definition-ids which is renewed for each effect-def.-id
            List<int> effectChangesDefIDs;
            // temporarly stores List with {int changeType, string parameters} == the actual single EffectChangeDef
            List<object> effectChange;
            // temporarly holds the necessary values to instantiate the respective definition-object
            DummyItemDef dummyDef = new DummyItemDef();
            if (vobID == -1){
                throw new Exception("CreateItemDefinition: There is no column for the vob id with the name: " 
                    + idColName + "!"
                    + " Correct this malfunction immediately by comparing database tables and their "
                    + "related Dictionaries in Sumpfkraut.Database.DBTables.");
            }
            if (!mapVDToED.TryGetValue(vobID, out effectDefIDs)){
                Log.Logger.logWarning("CreateItemDefinition: There are no effect-definitions-ids mapped to by vob-id "
                    + vobID + "!");
            }


            /* ---------------------------------------------------
                directly accessable attributes from definition table
                --------------------------------------------------- */

            // temporary used index for more secure code through TryGetValue (see if-blocks below)
            int colIndex = -1;

            colIndex = colTypesKeys.IndexOf("ID");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setID((int) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("InstanceName");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setInstanceName((string) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("Name");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setName((string) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("ScemeName");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setScemeName((string) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("Protections");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setProtections(DBReader.ParseParamToIntArray((string) def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("Damages");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setDamages(DBReader.ParseParamToIntArray((string) def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("Value");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setValue((int) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("MainFlag");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setMainFlag((GUC.Enumeration.MainFlags)Enum.ToObject(typeof(GUC.Enumeration.MainFlags), 
                    def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("Flag");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setFlag((GUC.Enumeration.Flags)Enum.ToObject(typeof(GUC.Enumeration.Flags), 
                    def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("ArmorFlag");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setArmorFlag((GUC.Enumeration.ArmorFlags)Enum.ToObject(typeof(GUC.Enumeration.ArmorFlags), 
                    def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("DamageType");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setDamageType((GUC.Enumeration.DamageTypes)Enum.ToObject(typeof(GUC.Enumeration.DamageTypes), 
                    def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("TotalDamage");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setTotalDamage((int) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("Range");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setRange((int) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("Visual");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setVisual((string) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("VisualChange");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setVisualChange((string) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("Effect");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setEffect((string) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("VisualSkin");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setVisualSkin((int) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("Material");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setMaterial((GUC.Enumeration.MaterialType)Enum.ToObject(typeof(GUC.Enumeration.MaterialType), 
                    def[colIndex]));
            }

            colIndex = colTypesKeys.IndexOf("Munition");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                // !!! TO DO !!!
            }

            colIndex = colTypesKeys.IndexOf("IsKeyInstance");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setIsKeyInstance((bool) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("IsTorch");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setIsTorch((bool) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("IsTorchBurning");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setIsTorchBurning((bool) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("IsTorchBurned");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setIsTorchBurned((bool) def[colIndex]);
            }

            colIndex = colTypesKeys.IndexOf("IsGold");
            if ((colIndex != -1) && (def[colIndex] != null))
            {
                dummyDef.setIsGold((bool) def[colIndex]);
            }

            /* ---------------------------------------------------
                attributes which make use of EffectChanges
               --------------------------------------------------- */

            if (effectDefIDs != null)
            {
                for (int e = 0; e < effectDefIDs.Count; e++)
                {
                    if (mapEDToECD.TryGetValue(effectDefIDs[e], out effectChangesDefIDs))
                    {
                        if (effectChangesDefIDs != null)
                        {
                            for (int ec = 0; ec < effectChangesDefIDs.Count; ec++)
                            {
                                if (EffectChangesDef.TryGetValue(effectChangesDefIDs[ec], out effectChange))
                                {
                                    EffectChangesDef.ApplyToDummy(ref dummyDef, effectChange);
                                }
                            } 
                        }
                    }
                    else
                    {
                        Log.Logger.logWarning("CreateItemDefinition: No EffectChangesDef provided for EffectDef-id "
                            + effectDefIDs[e] + "!");
                    }
                }
            }
            
            CreateItemDefinition(ref dummyDef);
        }

        private static void CreateItemDefinition (ref DummyItemDef dummyDef)
        {
            try
            {
                ItemDef newDef = new ItemDef(dummyDef.getInstanceName(), dummyDef.getName(), dummyDef.getScemeName(), 
                    dummyDef.getProtections(), dummyDef.getDamages(), dummyDef.getValue(), dummyDef.getMainFlag(), 
                    dummyDef.getFlag(), dummyDef.getArmorFlag(), dummyDef.getDamageType(), dummyDef.getTotalDamage(), 
                    dummyDef.getRange(), dummyDef.getVisual(), dummyDef.getVisualChange(), dummyDef.getEffect(), 
                    dummyDef.getVisualSkin(), dummyDef.getMaterial(), dummyDef.getMunition(), 
                    dummyDef.getIsKeyInstance(), dummyDef.getIsTorch(), dummyDef.getIsTorchBurning(), 
                    dummyDef.getIsTorchBurned(), dummyDef.getIsGold());
                if (newDef != null)
                {
                    itemDefDict.Add(dummyDef.getID(), newDef);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateItemDefinition: Some necessary attributes of DummyItemDef" 
                    + " are not valid or missing: ID=" + dummyDef.getID() + ": " + ex);
            }
        }

        public static void CreateItemDefinition_Test (ref DummyItemDef dummyDef){
            CreateItemDefinition(ref dummyDef);
        }

        private static void CreateNPCDefinition (DefTableEnum defTab, ref List<object> def,
            ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
            ref Dictionary<int, List<int>> mapVDToED, 
            ref Dictionary<int, List<int>> mapEDToECD)
        {
            // !!! TODO !!!
        }

    }
}
