using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using GUC.Server.Scripting.Objects;
//using GUC.Server.Scripting.Objects.Character;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
using GUC.Enumeration;
using GUC.Server.Scripts.Sumpfkraut.Database;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{

    /**
     *   Class which initializes all vobs of the indivual types from database information: mobs, items, npcs.
     */
    public class VobHandler : AbstractRunnable
    {

        new public static readonly String _staticName = "VobHandler (static)";
        new protected String _objName = "VobHandler (default)";

        protected bool isInitialized = false;

 

        public VobHandler (String objName, bool startOnCreate)
            : base(startOnCreate)
        {
            this.SetObjName(objName);
        }



        public void Init()
        {
            LoadDefinitions();
            LoadInstances();
        }



        #region Vob-Definitions

        public bool LoadDefinitions()
        {
            return false;
        }

        public void LoadVobDef (VobDefType type, out VobDefLoader vobLoader)
        {
            LoadVobDef(type, null, out vobLoader);
        }

        // idRange must be 
        public void LoadVobDef (VobDefType type, List<Vec2Int> idRanges, 
            out VobDefLoader vobLoader)
        {
            vobLoader = new VobDefLoader(type, idRanges, true);
        }

        public bool LoadDefEffect ()
        {
            return false;
        }
        
        // eventually useless because definitions should not be declared by the running server
        // but from the outside
        public bool SaveDefinitions()
        {
            return false;
        }

        #endregion

        #region Vob-Instances

        public bool LoadInstances()
        {
            return false;
        }

        public bool SaveInstances()
        {
            return false;
        }

        #endregion



        public override void Run()
        {
            if (!isInitialized)
            {
                Init();
            }
        }














        //// stores vob-definitions with their ID-attribute (ID in the VobSystem) as key
        //public static Dictionary<int, MobDef> mobDefDict = new Dictionary<int, MobDef>();
        //public static Dictionary<int, ItemDef> itemDefDict = new Dictionary<int, ItemDef>();
        //public static Dictionary<int, SpellDef> spellDefDict = new Dictionary<int, SpellDef>();
        //public static Dictionary<int, NPCDef> npcDefDict = new Dictionary<int, NPCDef>();

        //// stores the same vob-definition-objects as the DefDicts but using their instance name as key
        //public static Dictionary<string, MobDef> mobDefNameDict = new Dictionary<string, MobDef>();
        //public static Dictionary<string, ItemDef> itemDefNameDict = new Dictionary<string, ItemDef>();
        //public static Dictionary<string, SpellDef> spellDefNameDict = new Dictionary<string, SpellDef>();
        //public static Dictionary<string, NPCDef> npcDefNameDict = new Dictionary<string, NPCDef>();

        //// stores vob-instances with their ID-attribute (ID in the VobSystem) as key
        //public static Dictionary<int, MobInst> mobInstDict = new Dictionary<int, MobInst>();
        //public static Dictionary<int, ItemInst> itemInstDict = new Dictionary<int, ItemInst>();
        //public static Dictionary<int, SpellInst> spellInstDict = new Dictionary<int, SpellInst>();
        //public static Dictionary<int, NPCInst> npcInstDict = new Dictionary<int, NPCInst>();

        //// dictionaries to directly point to the VobInst by knowing the IG-Vob (of the GUC)
        //public static Dictionary<Vob, MobInst> mobInstVobDict = new Dictionary<Vob, MobInst>();
        //public static Dictionary<Vob, ItemInst> itemInstVobDict = new Dictionary<Vob, ItemInst>();
        //public static Dictionary<Vob, SpellInst> spellInstVobDict = new Dictionary<Vob, SpellInst>();
        //public static Dictionary<Vob, NPCInst> npcInstVobDict = new Dictionary<Vob, NPCInst>();

        ///**
        // *   Call this method from outside to create the intial vob definitions
        // *   (spells, items, mobs, npcs).
        // */
        //public static void Init ()
        //{
        //    //LoadDefinitions(DefTableEnum.Spell_def);
        //    LoadDefinitions(DefTableEnum.Item_def);
        //    LoadDefinitions(DefTableEnum.Mob_def);
        //    LoadDefinitions(DefTableEnum.NPC_def);

        //    //LoadInstances(InstTableEnum.World_inst);

        //    //LoadInstances(InstTableEnum.Spell_Effects_inst);
        //    LoadInstances(InstTableEnum.Item_inst);
        //    LoadInstances(InstTableEnum.Mob_inst);
        //    LoadInstances(InstTableEnum.NPC_inst);
        //}



        ///* ---------------------------------------------------------
        //    DATABASE INTERACTION
        //   --------------------------------------------------------- */


        ///**
        // *   Loads the specified type of definitions from their respective datatables.
        // *   The method prepares and executes the sqlite-query and reads the resulting values to create
        // *   vob-definitions.
        // *   @param defTab , the enum-entry which represents the type of definitions to load
        // *   @see DefTableEnum
        // */
        //public static void LoadDefinitions (DefTableEnum defTab)
        //{
        //    // the "1" will ensure, that LoadVobDef is called with sqlWhere=1
        //    // and, thus, tries to load every single definition of the database table (defTab)
        //    LoadDefinitions(defTab, "1");
        //}

        //public static void LoadDefinitions (DefTableEnum defTab, int[] idArr)
        //{
        //    // (does not check if defTab is valid yet, but does it in the subsequent LoadDefinitions-calls)

        //    if ((idArr == null) || (idArr.Length <= 0))
        //    {
        //        // no ids are provided and nothing need to be loaded
        //        return;
        //    }

        //    string sqlWhere;
        //    StringBuilder sqlWhereSB = new StringBuilder();
        //    sqlWhereSB.Append("`ID` IN (");
        //    sqlWhereSB.Append(string.Join(",", idArr.Select(x => x.ToString()).ToArray()));
        //    sqlWhereSB.Append(")");
            
        //    sqlWhere = sqlWhereSB.ToString();

        //    LoadDefinitions(defTab, sqlWhere);
        //}

        //public static void LoadDefinitions (DefTableEnum defTab, string sqlWhere)
        //{
        //    /* -------------------------------------------------------------
        //        getting vob-definitions
        //       ------------------------------------------------------------- */

        //    if (!DBTables.DefTableDict.ContainsKey(defTab))
        //    {
        //        return;
        //    }

        //    string defTabName = null;
        //    DBTables.DefTableNamesDict.TryGetValue(defTab, out defTabName);
        //    if (defTabName == null)
        //    {
        //        // if there is no table of that name
        //        return;
        //    }

        //    ///* try getting the necessary hints on data conversion for each column 
        //    // * of the given defintion table */
        //    Dictionary<String, SQLiteGetTypeEnum> colTypes;
        //    // create new lists to ensure same key-value-order for each row in rdr because the memory
        //    // allocation of the original dictionary and order might be changed during runtime
        //    List<string> colTypesKeys;
        //    List<SQLiteGetTypeEnum> colTypesVals;
        //    if (!DBTables.GetColumnInfo(defTab, out colTypes, out colTypesKeys, out colTypesVals))
        //    {
        //        return;
        //    }
            
        //    /* receive list of vob-definitions here and iterate over it, 
        //     * loading and applying the effec-changes */

        //    // stores the read and converted data of the sql-query
        //    List<List<List<object>>> defList = new List<List<List<object>>>();

        //    //LoadVobDef(defTabName, ref defList, defTab, ref colTypesKeys, ref colTypesVals);
        //    LoadVobDef(defTabName, ref defList, ref colTypesKeys, ref colTypesVals, sqlWhere);

        //    //// for easier list-indexing in list<object> of defList = List<List<object>>
        //    //Dictionary<string, int> colDict = new Dictionary<string, int>();
        //    //for (int i = 0; i < colTypesKeys.Count; i++)
        //    //{
        //    //    colDict.Add(colTypesKeys[i], i);
        //    //}

        //    if ((defList.Count <= 0) || (defList[0].Count <= 0))
        //    {
        //        return;
        //    }

        //    /* -------------------------------------------------------------
        //    getting vob-effects-instances (EI) [for each vob-defintion]
        //    ------------------------------------------------------------- */
            
        //    // try getting the necessary hints on data conversion for each column 
        //    // of the given vob-specific effect-instance table (EI)
        //    Dictionary<string, SQLiteGetTypeEnum> colTypes_EI = null;
        //    Database.InstTableEnum effectsInstTab = 0;
        //    if (!DBTables.Def2EffectsDict.TryGetValue(defTab, out effectsInstTab))
        //    {
        //        return;
        //    }
        //    if (!DBTables.InstTableDict.TryGetValue(effectsInstTab, out colTypes_EI))
        //    {
        //        return;
        //    }
        //    // to lists to ensure same key-value-order for each row in rdr because the memory
        //    // allocation of the original dictionary and order might be changed during runtime
        //    List<string> colTypesKeys_EI = new List<string>(colTypes_EI.Keys);
        //    List<SQLiteGetTypeEnum> colTypesVals_EI = new List<SQLiteGetTypeEnum>();
        //    for (int i = 0; i < colTypesKeys_EI.Count; i++)
        //    {
        //        colTypesVals_EI.Add( colTypes_EI[colTypesKeys_EI[i]] );
        //    }

        //    // filter out the vob-defintiion-ids to look them up in effects-instance-tables later
        //    List<int> defIDs = new List<int>();
        //    int idColIndex = colTypesKeys.IndexOf("ID");
        //    int hasEffectsColIndex = colTypesKeys.IndexOf("HasEffects");
        //    for (int i = 0; i < defList[0].Count; i++)
        //    {
        //        if ((bool)defList[0][i][hasEffectsColIndex])
        //        {
        //            defIDs.Add((int)defList[0][i][idColIndex]);
        //        }
        //    }

        //    // column name and index where the vob-id is
        //    string vobIDColName = null;
        //    if (!DBTables.EffectsTableDict_VobDefID.TryGetValue(effectsInstTab, out vobIDColName))
        //    {
        //        return;
        //    }
        //    int vobIDColIndex = colTypesKeys_EI.IndexOf(vobIDColName);

        //    // use the vob-ids to request all effect-ids which belong to them
        //    List<List<List<object>>> instList_EI = new List<List<List<object>>>();
        //    if (defIDs.Count > 0)
        //    {
        //        LoadEffectsInst(effectsInstTab, ref instList_EI,
        //            ref colTypesKeys_EI, ref colTypesVals_EI, 
        //            vobIDColName + " IN (" + String.Join(",", defIDs.ToArray()) + ")");
        //    }

        //    // filter out the effect-instance-ids to look them up in effects-changes-tables later
        //    // + map vob definition ids to their related effect ids
        //    List<int> effectDefIDs = new List<int>();
        //    int effectIDColIndex = colTypesKeys_EI.IndexOf("EffectDefID");
        //    int eiEffectID = -1;
        //    Dictionary<int, List<int>> vobToEffectsMap = new Dictionary<int, List<int>>();
        //    List<int> tempVobEffectDefIDs;
        //    int eiVobID = -1;
        //    if (instList_EI.Count > 0)
        //    {
        //        for (int i = 0; i < instList_EI[0].Count; i++)
        //        {
        //            eiEffectID = (int) instList_EI[0][i][effectIDColIndex];
        //            eiVobID = (int) instList_EI[0][i][vobIDColIndex];

        //            if (!effectDefIDs.Contains(eiEffectID))
        //            {
        //                effectDefIDs.Add(eiEffectID);
        //            }

        //            if (vobToEffectsMap.TryGetValue(eiVobID, out tempVobEffectDefIDs))
        //            {
        //                if (!tempVobEffectDefIDs.Contains(eiEffectID))
        //                {
        //                    tempVobEffectDefIDs.Add(eiEffectID);
        //                }
        //            }
        //            else
        //            {
        //                tempVobEffectDefIDs = new List<int>() {eiEffectID};
        //                vobToEffectsMap.Add(eiVobID, tempVobEffectDefIDs);
        //            }
        //        }
        //    }    

        //    /* -------------------------------------------------------------
        //        getting effect-changes-defintions (EC) [for each vob-defintion]
        //        ------------------------------------------------------------- */
            
        //    // try getting the necessary hints on data conversion for each column 
        //    // of the given vob-specific effect-instance table (EI)
        //    List<List<List<object>>> defList_EC = new List<List<List<object>>>();
        //    Dictionary<string, SQLiteGetTypeEnum> colTypes_EC = null;
        //    if (!DBTables.DefTableDict.TryGetValue(DefTableEnum.Effect_Changes_def, out colTypes_EC))
        //    {
        //        return;
        //    }

        //    // to lists to ensure same key-value-order for each row in rdr because the memory
        //    // allocation of the original dictionary and order might be changed during runtime
        //    List<string> colTypesKeys_EC = new List<string>(colTypes_EC.Keys);
        //    List<SQLiteGetTypeEnum> colTypesVals_EC = new List<SQLiteGetTypeEnum>();
        //    for (int i = 0; i < colTypesKeys_EC.Count; i++)
        //    {
        //        colTypesVals_EC.Add( colTypes_EC[colTypesKeys_EC[i]] );
        //    }

        //    LoadEffectChangesDef(ref effectDefIDs, ref defList_EC,
        //        ref colTypesKeys_EC, ref colTypesVals_EC);

        //    // remember those EffectChanges for later use (no need for less handy defList_EC from here on)
        //    // + map effectIDs to the ids of EffectChanges (for late use in the vob-instantiation)
        //    int ecIDColIndex = colTypesKeys_EC.IndexOf("ID");
        //    int ecCTColIndex = colTypesKeys_EC.IndexOf("ChangeType");
        //    int ecParamColIndex = colTypesKeys_EC.IndexOf("Parameters");
        //    int ecEffDefIDColIndex = colTypesKeys_EC.IndexOf("EffectDefID");
        //    Dictionary<int, List<int>> effectToChangesMap = new Dictionary<int, List<int>>();
        //    List<int> tempEffectChangesIDs;
        //    int ecEffectID = -1;
        //    int ecEffectChangeID = -1;
        //    if (defList_EC.Count > 0) 
        //    {
        //        for (int i = 0; i < defList_EC[0].Count; i++)
        //        {
        //            ecEffectID = (int) defList_EC[0][i][ecEffDefIDColIndex];
        //            ecEffectChangeID = (int) defList_EC[0][i][ecIDColIndex];

        //            Definitions.EffectChangesDef.Add(ecEffectChangeID, 
        //                (EffectChangesEnum) defList_EC[0][i][ecCTColIndex],
        //                (string) defList_EC[0][i][ecParamColIndex],
        //                true);

        //            if (effectToChangesMap.TryGetValue(ecEffectID, out tempEffectChangesIDs)){
        //                if (!tempEffectChangesIDs.Contains(ecEffectChangeID))
        //                {
        //                    tempEffectChangesIDs.Add(ecEffectChangeID);
        //                }
        //            }
        //            else
        //            {
        //                tempEffectChangesIDs = new List<int>() {ecEffectChangeID};
        //                effectToChangesMap.Add(ecEffectID, tempEffectChangesIDs);
        //            }
        //        }
        //    }
            
        //    /* -------------------------------------------------------------
        //        create actual instances for vob definitions
        //        ------------------------------------------------------------- */

        //    CreateVobDefinitions(defTab, ref defList, ref colTypesKeys, ref colTypesVals, 
        //        ref vobToEffectsMap, ref effectToChangesMap);   
        //}

        //private static void ConvertSQLResult (ref List<List<List<object>>> defList,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    string tabName="/Default defTabName/")
        //{
        //    object tempEntry = null;
        //    int r = 0;
        //    int c = 0;

        //    if ((defList.Count < 1) || (defList[0].Count < 1) || (defList[0][0].Count < 1))
        //    {
        //        return;
        //    }

        //    while (r < defList[0].Count)
        //    {
        //        c = 0;
        //        while (c < defList[0][r].Count)
        //        {
        //            //Console.WriteLine("----------- " + colTypesKeys[c] + "|--|" + defList[0][r][c].GetType() + "|--|" + defList[0][r][c]);
        //            if (defList[0][r][c] == null)
        //            {
        //                // null might be just fine
        //            }
        //            else if (defList[0][r][c].GetType() == typeof(DBNull))
        //            {
        //                // DBNull is aa little unheady because it would need additional type-checks later
        //                // use null instead
        //                defList[0][r][c] = null;
        //            }
        //            else
        //            {
        //                // everything else should be a string and somehow convertable
        //                tempEntry = defList[0][r][c].ToString();
        //                if (DBTables.SqlStringToData((string) tempEntry, colTypesVals[c], ref tempEntry))
        //                {
        //                    defList[0][r][c] = tempEntry;
        //                }
        //                else
        //                {
        //                    defList[0][r][c] = null;
        //                    Log.Logger.logError("LoadVobDef: Could no convert " + tempEntry + " from string to type "
        //                        + colTypesVals[c] + " for loaded database-entry of column "
        //                        + colTypesKeys[c] + " (table = `" + tabName + "`)!");
        //                }
        //            }
        //            //Console.WriteLine("----------- " + colTypesKeys[c] + "|--|" + (defList[0][r][c] == null) + "|--|" + defList[0][r][c]);
        //            c++;
        //        }
        //        r++;
        //    }
        //}

        ///**
        // *   Load vob definitions (without effect changes definitions) as usable data objects from database.
        // *   Requires no colTypes-parameter the data column names and types will be stored in
        // *   colTypesKeys and colTypesVals.
        // */
        //private static void LoadVobDef (string defTabName, ref List<List<List<object>>> defList,
        //    //DefTableEnum defTabEnum,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
        //    string sqlWhere="1")
        //{
        //    // grab from database
        //    DBReader.LoadFromDB(ref defList, 
        //        String.Join(",", colTypesKeys.ToArray()), 
        //        "`" + defTabName + "`", 
        //        sqlWhere, 
        //        "ID ASC");

        //    // convert individual sql-result-strings to usable data of given types
        //    ConvertSQLResult(ref defList, ref colTypesKeys, ref colTypesVals, defTabName);

        //    //Dictionary<String, SQLiteGetTypeEnum> colTypes = null;
        //    //if (DBTables.DefTableDict.TryGetValue(DefTableEnum.Effect_Changes_def, out colTypes))
        //    //{
        //    //    LoadVobDef(defTabName, ref defList, ref colTypes, ref colTypesKeys, 
        //    //    ref colTypesVals, sqlWhere);
        //    //}
        //    //else
        //    //{
        //    //    Log.Logger.logError("LoadVobDef: Cannot load vob definition of vobtype " + defTabEnum);
        //    //}
        //}


        /////**
        //// *   Lower level loading of vob definition that requires colTypes-paramter.
        //// *   @param defTabName is the name of the handled definition table
        //// *   @param defList is used to store the sql-result of your sql-query
        //// *   @param colTypes describes which column (by name) used which data type
        //// *   @param colTypesKeys stores names of colTypes in the order used in the sql-query
        //// *   @param colTypesVals stores the types of colTyes in the order used in the sql-query
        //// */
        ////private static void LoadVobDef (string defTabName, ref List<List<List<object>>> defList, 
        ////    ref Dictionary<String, SQLiteGetTypeEnum> colTypes, 
        ////    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
        ////    string sqlWhere="1")
        ////{
        ////    // grab from database
        ////    DBReader.LoadFromDB(ref defList, 
        ////        String.Join(",", colTypesKeys.ToArray()), 
        ////        "`" + defTabName + "`", 
        ////        sqlWhere, 
        ////        "ID ASC");

        ////    // convert individual sql-result-strings to usable data of given types
        ////    ConvertSQLResult(ref defList, ref colTypesKeys, ref colTypesVals, defTabName);
        ////}


        //private static void LoadEffectsInst (InstTableEnum instTab, ref List<List<List<object>>> defList, 
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
        //    string sqlWhere="1")
        //{
        //    string instTabName = "";
        //    if (!DBTables.InstTableNamesDict.TryGetValue(instTab, out instTabName))
        //    {
        //        return;
        //    }
        //    string orderByID = "";
        //    if (!DBTables.EffectsTableDict_VobDefID.TryGetValue(instTab, out orderByID))
        //    {
        //        return;
        //    }

        //    // grab effect-ids from database
        //    DBReader.LoadFromDB(ref defList, 
        //        String.Join(",", colTypesKeys.ToArray()), 
        //        "`" + instTabName + "`", 
        //        sqlWhere, 
        //        " " + orderByID + " ASC");

        //    if ((defList.Count < 1) || (defList[0].Count < 1) || (defList[0][0].Count < 1))
        //    {
        //        return;
        //    }

        //    // convert individual sql-result-strings to usable data of given types
        //    ConvertSQLResult(ref defList, ref colTypesKeys, ref colTypesVals, instTabName);
        //}


        //private static void LoadEffectChangesDef (ref List<int> effectDefIDs, ref List<List<List<object>>> defList, 
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        //{
        //    if (!(effectDefIDs.Count > 0))
        //    {
        //        Log.Logger.logWarning("LoadEffectChangesDef: Cannot load effect-changes-definitions with 0 effect-definition-ids.");
        //        return;
        //    }

        //    string sqlWhere = "EffectDefID IN (" + Utilities.StringUtil.Concatenate<int>(effectDefIDs.ToArray(), ",") + ")";

        //    LoadEffectChangesDef(ref defList,
        //        ref colTypesKeys, ref colTypesVals,
        //        sqlWhere);
        //}


        //// must be able to read multiple rows 
        //// count of colVals multiple of the count of colKeys to prevent unnecessary repetition?
        //// 1 => object, 2 => object, 3 => object, 4 => object, ... , 1 => object, ...
        //private static void LoadEffectChangesDef (ref List<List<List<object>>> defList, 
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
        //    string sqlWhere="1")
        //{
        //    // grab from database
        //    DBReader.LoadFromDB(ref defList, 
        //        String.Join(",", colTypesKeys.ToArray()), 
        //        "`" + "Effect_Changes_def" + "`", 
        //        sqlWhere, 
        //        "EffectDefID ASC");

        //    // convert individual sql-result-strings to usable data of given types
        //    ConvertSQLResult(ref defList, ref colTypesKeys, ref colTypesVals, "EffectChanges_def");
        //}





        //public static void LoadInstances (InstTableEnum instTab)
        //{
        //    // the "1" will ensure, that LoadVobDef is called with sqlWhere=1
        //    // and, thus, tries to load every single definition of the database table (defTab)
        //    LoadInstances(instTab, "1");
        //}

        //public static void LoadInstances (InstTableEnum instTab, int[] idArr)
        //{
        //    // (does not check if defTab is valid yet, but does it in the subsequent LoadInstances-calls)

        //    if ((idArr == null) || (idArr.Length <= 0))
        //    {
        //        // no ids are provided and nothing need to be loaded
        //        return;
        //    }

        //    string sqlWhere;
        //    StringBuilder sqlWhereSB = new StringBuilder();
        //    sqlWhereSB.Append("`ID` IN (");
        //    sqlWhereSB.Append(string.Join(",", idArr.Select(x => x.ToString()).ToArray()));
        //    sqlWhereSB.Append(")");
            
        //    sqlWhere = sqlWhereSB.ToString();

        //    LoadInstances(instTab, sqlWhere);
        //}

        //public static void LoadInstances (InstTableEnum instTab, string sqlWhere)
        //{
        //    if (!DBTables.InstTableDict.ContainsKey(instTab))
        //    {
        //        return;
        //    }

        //    // ------------------------
        //    // Loading simple vobs
        //    // ------------------------

        //    string instTabName = null;
        //    DBTables.InstTableNamesDict.TryGetValue(instTab, out instTabName);
        //    if (instTabName == null)
        //    {
        //        // if there is no table of that name
        //        return;
        //    }

        //    Dictionary<String, SQLiteGetTypeEnum> colTypes_Vobs;
        //     // create new lists to ensure same key-value-order for each row in rdr because the memory
        //    // allocation of the original dictionary and order might be changed during runtime
        //    List<string> colTypesKeys_Vobs;
        //    List<SQLiteGetTypeEnum> colTypesVals_Vobs;

        //    if (!DBTables.GetColumnInfo(instTab, out colTypes_Vobs, 
        //        out colTypesKeys_Vobs, out colTypesVals_Vobs))
        //    {
        //        return;
        //    }

        //    // receive list of vob-instances here and iterate over it, 
        //    // loading and applying the effect-changes

        //    // stores the read and converted data of the sql-query
        //    List<List<List<object>>> instList_Vobs = new List<List<List<object>>>();

        //    LoadVobInst (instTabName, ref instList_Vobs, ref colTypesKeys_Vobs, ref colTypesVals_Vobs, 
        //        sqlWhere);

        //    if ((instList_Vobs.Count <= 0) || (instList_Vobs[0].Count <= 0))
        //    {
        //        // if nothing was found in the databse and loaded, it does not need
        //        // further initialization --> return prematurely
        //        return;
        //    }

        //    // ------------------------
        //    // Loading vob-effect-changes
        //    // ------------------------

        //    // PLACEHOLDER:
        //    //     - possible EffectDef- and EffectChangesDef-loading

        //    // ------------------------
        //    // Loading vob-locations
        //    // ------------------------

        //    Dictionary<String, SQLiteGetTypeEnum> colTypes_Locs;
        //     // create new lists to ensure same key-value-order for each row in rdr because the memory
        //    // allocation of the original dictionary and order might be changed during runtime
        //    List<string> colTypesKeys_Locs;
        //    List<SQLiteGetTypeEnum> colTypesVals_Locs;

        //    if (!DBTables.GetColumnInfo(instTab, out colTypes_Locs, 
        //        out colTypesKeys_Locs, out colTypesVals_Locs))
        //    {
        //        return;
        //    }

        //    bool inWorldCheck = false; // check for locations in the world
        //    bool inInvCheck = false; // check for locations in npc-inventories
        //    bool inContCheck = false; // check for locations in containers/mob-inventories

        //    switch (instTab)
        //    {
        //        case InstTableEnum.Mob_inst:
        //            inWorldCheck = true;
        //            break;
        //        case InstTableEnum.Item_inst:
        //            inWorldCheck = inInvCheck = inContCheck = true;
        //            break;
        //        case InstTableEnum.NPC_inst:
        //            inWorldCheck = true;
        //            break;
        //    }

        //    LoadVobLocation();

        //    // ------------------------
        //    // Create actual vobs on serverside
        //    // ------------------------

        //    CreateVobInstances(instTab, ref instList_Vobs, ref colTypesKeys_Vobs, ref colTypesVals_Vobs);
        //}

        //public static void LoadVobInst (string instTabName, ref List<List<List<object>>> instList,
        //    //InstTableEnum instTabEnum,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals, 
        //    string sqlWhere="1")
        //{
        //    // grab from database
        //    DBReader.LoadFromDB(ref instList, 
        //        String.Join(",", colTypesKeys.ToArray()), 
        //        "`" + instTabName + "`", 
        //        sqlWhere, 
        //        "ID ASC");

        //    // convert individual sql-result-strings to usable data of given types
        //    ConvertSQLResult(ref instList, ref colTypesKeys, ref colTypesVals, instTabName);
        //}

        //public static void LoadVobLocation (string instTabName, ref List<List<List<object>>> locList,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    string sqlWhere = "1")
        //{
        //    // grab from database
        //    DBReader.LoadFromDB(ref locList, 
        //        String.Join(",", colTypesKeys.ToArray()), 
        //        "``", 
        //        sqlWhere, 
        //        "ID ASC");
        //    //instTabName.StartsWith("");

        //    // convert individual sql-result-strings to usable data of given types
        //    ConvertSQLResult(ref locList, ref colTypesKeys, ref colTypesVals, instTabName);
        //}



        ///* ---------------------------------------------------------
        //    APPLICATION OF RECEIVED DATABASE ENTRIES
        //   --------------------------------------------------------- */


        //private static void CreateVobDefinitions (DefTableEnum defTab, ref List<List<List<object>>> defList,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    ref Dictionary<int, List<int>> mapVDToED,
        //    ref Dictionary<int, List<int>> mapEDToECD)
        //{
        //    List<object> def;
        //    int vobIDColIndex = colTypesKeys.IndexOf("ID");

        //    //for (int r = 0; r < defList[0].Count; r++)
        //    //{
        //    //    def = defList[0][r];
        //    //    CreateVobDefinition(defTab, ref def, 
        //    //        ref colTypesKeys, ref colTypesVals, 
        //    //        ref mapVDToED, 
        //    //        ref mapEDToECD);
        //    //}

        //    switch (defTab)
        //    {
        //        case (DefTableEnum.Mob_def):
        //            for (int r = 0; r < defList[0].Count; r++)
        //            {
        //                def = defList[0][r];
        //                CreateMobDefinition(ref def, 
        //                    ref colTypesKeys, ref colTypesVals, 
        //                    ref mapVDToED, 
        //                    ref mapEDToECD);
        //            }
        //            break;
        //        case (DefTableEnum.Spell_def):
        //            for (int r = 0; r < defList[0].Count; r++)
        //            {
        //                def = defList[0][r];
        //                CreateSpellDefinition(ref def, 
        //                    ref colTypesKeys, ref colTypesVals, 
        //                    ref mapVDToED, 
        //                    ref mapEDToECD);
        //            }
        //            break;
        //        case (DefTableEnum.Item_def):
        //            for (int r = 0; r < defList[0].Count; r++)
        //            {
        //                def = defList[0][r];
        //                CreateItemDefinition(ref def, 
        //                    ref colTypesKeys, ref colTypesVals, 
        //                    ref mapVDToED, 
        //                    ref mapEDToECD);
        //            }
        //            break;
        //        case (DefTableEnum.NPC_def):
        //            for (int r = 0; r < defList[0].Count; r++)
        //            {
        //                def = defList[0][r];
        //                CreateNPCDefinition(ref def, 
        //                    ref colTypesKeys, ref colTypesVals, 
        //                    ref mapVDToED, 
        //                    ref mapEDToECD);
        //            }
        //            break;
        //        default:
        //            Log.Logger.logError("Invalid DefTableEnum " + defTab + " detected in method CreateVobDefinitions"
        //                + ": Terminating vob-definition-creation!");
        //            break;
        //    }
        //}

        ////private static void CreateVobDefinition (DefTableEnum defTab, ref List<object> def,
        ////    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        ////    ref Dictionary<int, List<int>> mapVDToED, 
        ////    ref Dictionary<int, List<int>> mapEDToECD)
        ////{
        ////    switch (defTab)
        ////    {
        ////        case (DefTableEnum.Mob_def):
        ////            CreateMobDefinition(ref def, 
        ////                ref colTypesKeys, ref colTypesVals, 
        ////                ref mapVDToED, 
        ////                ref mapEDToECD);
        ////            break;
        ////        case (DefTableEnum.Spell_def):
        ////            CreateSpellDefinition(ref def, 
        ////                ref colTypesKeys, ref colTypesVals, 
        ////                ref mapVDToED, 
        ////                ref mapEDToECD);
        ////            break;
        ////        case (DefTableEnum.Item_def):
        ////            CreateItemDefinition(ref def, 
        ////                ref colTypesKeys, ref colTypesVals, 
        ////                ref mapVDToED, 
        ////                ref mapEDToECD);
        ////            break;
        ////        case (DefTableEnum.NPC_def):
        ////            CreateNPCDefinition(ref def, 
        ////                ref colTypesKeys, ref colTypesVals, 
        ////                ref mapVDToED, 
        ////                ref mapEDToECD);
        ////            break;
        ////        default:
        ////            Log.Logger.logError("Invalid DefTableEnum " + defTab + " detected in method createVobDefinition"
        ////                + ": Terminating vob-definition-creation!");
        ////            break;
        ////    }
        ////}

        //private static void CreateMobDefinition (ref List<object> def,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    ref Dictionary<int, List<int>> mapVDToED,
        //    ref Dictionary<int, List<int>> mapEDToECD)
        //{
        //    string idColName = "ID";
        //    int vobID = colTypesKeys.IndexOf(idColName);
        //    // list of effect-definition-ids of the the handled vob-definition
        //    List<int> effectDefIDs;
        //    // respective list of effect-changes-definition-ids which is renewed for each effect-def.-id
        //    List<int> effectChangesDefIDs;
        //    // temporarly stores List with {int changeType, string parameters} == the actual single EffectChangeDef
        //    List<object> effectChange;
        //    // temporarly holds the necessary values to instantiate the respective definition-object
        //    DummyMobDef dummyDef = new DummyMobDef();
        //    if (vobID == -1){
        //        throw new Exception("CreateMobDefinition: There is no column for the vob id with the name: " 
        //            + idColName + "!"
        //            + " Correct this malfunction immediately by comparing database tables and their "
        //            + "related Dictionaries in Sumpfkraut.Database.DBTables.");
        //    }
        //    if (!mapVDToED.TryGetValue(vobID, out effectDefIDs))
        //    {
        //        Log.Logger.logWarning("CreateMobDefinition: There are no effect-definitions-ids mapped to by vob-id "
        //            + vobID + "!");
        //    }

        //    /* ---------------------------------------------------
        //        directly accessable attributes from definition table
        //        --------------------------------------------------- */

        //    // temporary used index for more secure code through TryGetValue (see if-blocks below)
        //    int colIndex = -1;

        //    colIndex = colTypesKeys.IndexOf("ID");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setID((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("MobInterType");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setMobInterType((MobInterType)Enum.ToObject(typeof(MobInterType), 
        //            def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("Visual");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setVisual((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("FocusName");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setFocusName((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Items");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        List<ItemDef> Items_List = new List<ItemDef>();

        //        int[] Items_DefIDs = DBReader.ParseParamToIntArray((string) def[colIndex]);
        //        if ((Items_DefIDs != null) && (Items_DefIDs.Length > 0))
        //        {
        //            ItemDef tempContItemDef;
        //            for (int i = 0; i < Items_DefIDs.Length; i++)
        //            {
        //                if (itemDefDict.TryGetValue(Items_DefIDs[i], out tempContItemDef))
        //                {
        //                    Items_List.Add(tempContItemDef);
        //                }
        //                else
        //                {
        //                    Log.Logger.logWarning("CreateMobDefinition: An ItemDef with ID " + (int) def[colIndex]
        //                        + " does not exist (yet). It won't be added to the Items-attribute of its container.");
        //                }
        //            }
        //            dummyDef.setItems(Items_List.ToArray());
        //        }
        //    }

        //    colIndex = colTypesKeys.IndexOf("Amounts");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setAmounts(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("IsLocked");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setIsLocked((bool) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("KeyInstance");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        ItemDef tempKeyItemDef;
        //        if (itemDefDict.TryGetValue((int) def[colIndex], out tempKeyItemDef))
        //        {
        //            dummyDef.setKeyInstance(tempKeyItemDef);
        //        }
        //        else
        //        {
        //            Log.Logger.logWarning("CreateMobDefinition: An ItemDef with ID " + (int) def[colIndex]
        //                + " does not exist (yet). The attribute KeyInstance will be neglected in the further process.");
        //        }
        //    }

        //    colIndex = colTypesKeys.IndexOf("PicklockString");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setPicklockString((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("UseWithItem");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        ItemDef tempUseWithItem;
        //        if (itemDefDict.TryGetValue((int) def[colIndex], out tempUseWithItem))
        //        {
        //            dummyDef.setUseWithItem(tempUseWithItem);
        //        }
        //        else
        //        {
        //            Log.Logger.logWarning("CreateMobDefinition: An ItemDef with ID " + (int) def[colIndex]
        //                + " does not exist (yet). The attribute UseWithItem will be neglected in the further process.");
        //        }
        //    }

        //    colIndex = colTypesKeys.IndexOf("TriggerTarget");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setTriggerTarget((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("CDDyn");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setCDDyn((bool) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("CDStatic");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setCDStatic((bool) def[colIndex]);
        //    }

        //    /* ---------------------------------------------------
        //        attributes which make use of EffectChanges
        //       --------------------------------------------------- */

        //    if (effectDefIDs != null)
        //    {
        //        for (int e = 0; e < effectDefIDs.Count; e++)
        //        {
        //            if (mapEDToECD.TryGetValue(effectDefIDs[e], out effectChangesDefIDs))
        //            {
        //                if (effectChangesDefIDs != null)
        //                {
        //                    for (int ec = 0; ec < effectChangesDefIDs.Count; ec++)
        //                    {
        //                        if (EffectChangesDef.TryGetValue(effectChangesDefIDs[ec], out effectChange))
        //                        {
        //                            //Console.WriteLine("~~~~~~" + effectChange[0] + "|--|" + effectChange[1]);
        //                            EffectChangesDef.ApplyToDummy(ref dummyDef, effectChange);
        //                        }
        //                    } 
        //                }
        //            }
        //            else
        //            {
        //                Log.Logger.logWarning("CreateMobDefinition: No EffectChangesDef provided for EffectDef-id "
        //                    + effectDefIDs[e] + "!");
        //            }
        //        }
        //    }
            
        //    CreateMobDefinition(ref dummyDef);
        //}

        //private static void CreateMobDefinition (ref DummyMobDef dummyDef)
        //{
        //    try
        //    {
        //    //    MobInterType mobInterType, String visual, String focusName, ItemInstance[] items, int[] amounts, 
        //    //bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, 
        //    //String triggerTarget, bool cdDyn, bool cdStatic
        //        MobDef newDef = new MobDef(dummyDef.getMobInterType(), dummyDef.getVisual(), dummyDef.getFocusName(),
        //            dummyDef.getItems(), dummyDef.getAmounts(), dummyDef.getIsLocked(), dummyDef.getKeyInstance(),
        //            dummyDef.getPicklockString(), dummyDef.getUseWithItem(), dummyDef.getTriggerTarget(),
        //            dummyDef.getCDDyn(), dummyDef.getCDStatic());
        //        if (newDef != null)
        //        {
        //            mobDefDict.Add(dummyDef.getID(), newDef);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("CreateMobDefinition: Some necessary attributes of DummyItemDef" 
        //            + " are not valid or missing: ID=" + dummyDef.getID() + ": " + ex);
        //    }
        //}

        //private static void CreateSpellDefinition (ref List<object> def,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    ref Dictionary<int, List<int>> mapVDToED, 
        //    ref Dictionary<int, List<int>> mapEDToECD)
        //{
        //    // !!! TODO ... later because spells are not needed at the moment!!!
        //}


        //private static void CreateItemDefinition (ref List<object> def,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    ref Dictionary<int, List<int>> mapVDToED, 
        //    ref Dictionary<int, List<int>> mapEDToECD)
        //{
        //    string idColName = "ID";
        //    int vobID = colTypesKeys.IndexOf(idColName);
        //    // list of effect-definition-ids of the the handled vob-definition
        //    List<int> effectDefIDs;
        //    // respective list of effect-changes-definition-ids which is renewed for each effect-def.-id
        //    List<int> effectChangesDefIDs;
        //    // temporarly stores List with {int changeType, string parameters} == the actual single EffectChangeDef
        //    List<object> effectChange;
        //    // temporarly holds the necessary values to instantiate the respective definition-object
        //    DummyItemDef dummyDef = new DummyItemDef();
        //    if (vobID == -1){
        //        throw new Exception("CreateItemDefinition: There is no column for the vob id with the name: " 
        //            + idColName + "!"
        //            + " Correct this malfunction immediately by comparing database tables and their "
        //            + "related Dictionaries in Sumpfkraut.Database.DBTables.");
        //    }
        //    if (!mapVDToED.TryGetValue(vobID, out effectDefIDs)){
        //        Log.Logger.logWarning("CreateItemDefinition: There are no effect-definitions-ids mapped to by vob-id "
        //            + vobID + "!");
        //    }


        //    /* ---------------------------------------------------
        //        directly accessable attributes from definition table
        //        --------------------------------------------------- */

        //    // temporary used index for more secure code through TryGetValue (see if-blocks below)
        //    int colIndex = -1;

        //    colIndex = colTypesKeys.IndexOf("ID");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setID((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("InstanceName");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setInstanceName((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Name");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setName((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("ScemeName");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setScemeName((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Protections");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setProtections(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("Damages");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setDamages(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("Value");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setValue((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("MainFlag");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setMainFlag((GUC.Enumeration.MainFlags)Enum.ToObject(typeof(GUC.Enumeration.MainFlags), 
        //            def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("Flag");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setFlag((GUC.Enumeration.Flags)Enum.ToObject(typeof(GUC.Enumeration.Flags), 
        //            def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("ArmorFlag");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setArmorFlag((GUC.Enumeration.ArmorFlags)Enum.ToObject(typeof(GUC.Enumeration.ArmorFlags), 
        //            def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("DamageType");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setDamageType((GUC.Enumeration.DamageTypes)Enum.ToObject(typeof(GUC.Enumeration.DamageTypes), 
        //            def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("TotalDamage");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setTotalDamage((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Range");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setRange((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Visual");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setVisual((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("VisualChange");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setVisualChange((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Effect");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setEffect((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("VisualSkin");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setVisualSkin((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Material");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setMaterial((GUC.Enumeration.MaterialType)Enum.ToObject(typeof(GUC.Enumeration.MaterialType), 
        //            def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("Munition");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        // !!! TO DO !!!
        //    }

        //    colIndex = colTypesKeys.IndexOf("IsKeyInstance");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setIsKeyInstance((bool) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("IsTorch");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setIsTorch((bool) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("IsTorchBurning");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setIsTorchBurning((bool) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("IsTorchBurned");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setIsTorchBurned((bool) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("IsGold");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setIsGold((bool) def[colIndex]);
        //    }

        //    /* ---------------------------------------------------
        //        attributes which make use of EffectChanges
        //       --------------------------------------------------- */

        //    if (effectDefIDs != null)
        //    {
        //        for (int e = 0; e < effectDefIDs.Count; e++)
        //        {
        //            if (mapEDToECD.TryGetValue(effectDefIDs[e], out effectChangesDefIDs))
        //            {
        //                if (effectChangesDefIDs != null)
        //                {
        //                    for (int ec = 0; ec < effectChangesDefIDs.Count; ec++)
        //                    {
        //                        if (EffectChangesDef.TryGetValue(effectChangesDefIDs[ec], out effectChange))
        //                        {
        //                            //Console.WriteLine("~~~~~~" + effectChange[0] + "|--|" + effectChange[1]);
        //                            EffectChangesDef.ApplyToDummy(ref dummyDef, effectChange);
        //                        }
        //                    } 
        //                }
        //            }
        //            else
        //            {
        //                Log.Logger.logWarning("CreateItemDefinition: No EffectChangesDef provided for EffectDef-id "
        //                    + effectDefIDs[e] + "!");
        //            }
        //        }
        //    }
            
        //    CreateItemDefinition(ref dummyDef);
        //}

        //private static void CreateItemDefinition (ref DummyItemDef dummyDef)
        //{
        //    try
        //    {
        //        ItemDef newDef = new ItemDef(dummyDef.getInstanceName(), dummyDef.getName(), dummyDef.getScemeName(), 
        //            dummyDef.getProtections(), dummyDef.getDamages(), dummyDef.getValue(), dummyDef.getMainFlag(), 
        //            dummyDef.getFlag(), dummyDef.getArmorFlag(), dummyDef.getDamageType(), dummyDef.getTotalDamage(), 
        //            dummyDef.getRange(), dummyDef.getVisual(), dummyDef.getVisualChange(), dummyDef.getEffect(), 
        //            dummyDef.getVisualSkin(), dummyDef.getMaterial(), dummyDef.getMunition(), 
        //            dummyDef.getIsKeyInstance(), dummyDef.getIsTorch(), dummyDef.getIsTorchBurning(), 
        //            dummyDef.getIsTorchBurned(), dummyDef.getIsGold());
        //        if (newDef != null)
        //        {
        //            // descriptive texts and numbers
        //            newDef.setDescription(dummyDef.getDescription());
        //            newDef.setText0(dummyDef.getText0());
        //            newDef.setText1(dummyDef.getText1());
        //            newDef.setText2(dummyDef.getText2());
        //            newDef.setText3(dummyDef.getText3());
        //            newDef.setText4(dummyDef.getText4());
        //            newDef.setText5(dummyDef.getText5());
        //            newDef.setCount0(dummyDef.getCount0());
        //            newDef.setCount1(dummyDef.getCount1());
        //            newDef.setCount2(dummyDef.getCount2());
        //            newDef.setCount3(dummyDef.getCount3());
        //            newDef.setCount4(dummyDef.getCount4());
        //            newDef.setCount5(dummyDef.getCount5());

        //            // triggered with OnUse
        //            newDef.setOnUse_HPChange(dummyDef.getOnUse_HPChange());
        //            newDef.setOnUse_HPMaxChange(dummyDef.getOnUse_HPMaxChange());
        //            newDef.setOnUse_MPChange(dummyDef.getOnUse_MPChange());
        //            newDef.setOnUse_MPMaxChange(dummyDef.getOnUse_MPMaxChange());
        //            newDef.setOnUse_HP_Min(dummyDef.getOnUse_HP_Min());
        //            newDef.setOnUse_HPMax_Min(dummyDef.getOnUse_HPMax_Min());
        //            newDef.setOnUse_MP_Min(dummyDef.getOnUse_MP_Min());
        //            newDef.setOnUse_MPMax_Min(dummyDef.getOnUse_MPMax_Min());

        //            // triggered with OnEquip
        //            newDef.setOnEquip_HPChange(dummyDef.getOnEquip_HPChange());
        //            newDef.setOnEquip_HPMaxChange(dummyDef.getOnEquip_HPMaxChange());
        //            newDef.setOnEquip_MPChange(dummyDef.getOnEquip_MPChange());
        //            newDef.setOnEquip_MPMaxChange(dummyDef.getOnEquip_MPMaxChange());
        //            newDef.setOnEquip_HP_Min(dummyDef.getOnEquip_HP_Min());
        //            newDef.setOnEquip_HPMax_Min(dummyDef.getOnEquip_HPMax_Min());
        //            newDef.setOnEquip_MP_Min(dummyDef.getOnEquip_MP_Min());
        //            newDef.setOnEquip_MPMax_Min(dummyDef.getOnEquip_MPMax_Min());

        //            // triggered with OnUnEquip
        //            newDef.setOnUnEquip_HPChange(dummyDef.getOnUnEquip_HPChange());
        //            newDef.setOnUnEquip_HPMaxChange(dummyDef.getOnUnEquip_HPMaxChange());
        //            newDef.setOnUnEquip_MPChange(dummyDef.getOnUnEquip_MPChange());
        //            newDef.setOnUnEquip_MPMaxChange(dummyDef.getOnUnEquip_MPMaxChange());
        //            newDef.setOnUnEquip_HP_Min(dummyDef.getOnUnEquip_HP_Min());
        //            newDef.setOnUnEquip_HPMax_Min(dummyDef.getOnUnEquip_HPMax_Min());
        //            newDef.setOnUnEquip_MP_Min(dummyDef.getOnUnEquip_MP_Min());
        //            newDef.setOnUnEquip_MPMax_Min(dummyDef.getOnUnEquip_MPMax_Min());

        //            itemDefDict.Add(dummyDef.getID(), newDef);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("CreateItemDefinition: Some necessary attributes of DummyItemDef" 
        //            + " are not valid or missing: ID=" + dummyDef.getID() + ": " + ex);
        //    }
        //}

        //// !!! DELETE AFTER TESTING (security risk) !!!
        //public static void CreateItemDefinition_Test (ref DummyItemDef dummyDef){
        //    CreateItemDefinition(ref dummyDef);
        //}

        //private static void CreateNPCDefinition (ref List<object> def,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals,
        //    ref Dictionary<int, List<int>> mapVDToED, 
        //    ref Dictionary<int, List<int>> mapEDToECD)
        //{
        //    string idColName = "ID";
        //    int vobID = colTypesKeys.IndexOf(idColName);
        //    // list of effect-definition-ids of the the handled vob-definition
        //    List<int> effectDefIDs;
        //    // respective list of effect-changes-definition-ids which is renewed for each effect-def.-id
        //    List<int> effectChangesDefIDs;
        //    // temporarly stores List with {int changeType, string parameters} == the actual single EffectChangeDef
        //    List<object> effectChange;
        //    // temporarly holds the necessary values to instantiate the respective definition-object
        //    DummyNPCDef dummyDef = new DummyNPCDef();
        //    if (vobID == -1){
        //        throw new Exception("CreateNPCDefinition: There is no column for the vob id with the name: " 
        //            + idColName + "!"
        //            + " Correct this malfunction immediately by comparing database tables and their "
        //            + "related Dictionaries in Sumpfkraut.Database.DBTables.");
        //    }
        //    if (!mapVDToED.TryGetValue(vobID, out effectDefIDs))
        //    {
        //        Log.Logger.logWarning("CreateNPCDefinition: There are no effect-definitions-ids mapped to by vob-id "
        //            + vobID + "!");
        //    }

        //    /* ---------------------------------------------------
        //        directly accessable attributes from definition table
        //        --------------------------------------------------- */

        //    // temporary used index for more secure code through TryGetValue (see if-blocks below)
        //    int colIndex = -1;

        //    colIndex = colTypesKeys.IndexOf("ID");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setID((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Name");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setName((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Attributes");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setAttributes(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("TalentValues");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setTalentValues(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("TalentSkills");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setTalentSkills(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("HitChances");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setHitChances(DBReader.ParseParamToIntArray((string) def[colIndex]));
        //    }

        //    colIndex = colTypesKeys.IndexOf("Guild");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setGuild((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Voice");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setVoice((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("Visual");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setVisual((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("BodyMesh");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setBodyMesh((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("BodyTex");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setBodyTex((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("SkinColor");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setSkinColor((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("HeadMesh");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setHeadMesh((string) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("HeadTex");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setHeadTex((int) def[colIndex]);
        //    }

        //    colIndex = colTypesKeys.IndexOf("TeethTex");
        //    if ((colIndex != -1) && (def[colIndex] != null))
        //    {
        //        dummyDef.setTeethTex((int) def[colIndex]);
        //    }

        //    /* ---------------------------------------------------
        //        attributes which make use of EffectChanges
        //       --------------------------------------------------- */

        //    if (effectDefIDs != null)
        //    {
        //        for (int e = 0; e < effectDefIDs.Count; e++)
        //        {
        //            if (mapEDToECD.TryGetValue(effectDefIDs[e], out effectChangesDefIDs))
        //            {
        //                if (effectChangesDefIDs != null)
        //                {
        //                    for (int ec = 0; ec < effectChangesDefIDs.Count; ec++)
        //                    {
        //                        if (EffectChangesDef.TryGetValue(effectChangesDefIDs[ec], out effectChange))
        //                        {
        //                            //Console.WriteLine("~~~~~~" + effectChange[0] + "|--|" + effectChange[1]);
        //                            EffectChangesDef.ApplyToDummy(ref dummyDef, effectChange);
        //                        }
        //                    } 
        //                }
        //            }
        //            else
        //            {
        //                Log.Logger.logWarning("CreateNPCDefinition: No EffectChangesDef provided for EffectDef-id "
        //                    + effectDefIDs[e] + "!");
        //            }
        //        }
        //    }
            
        //    CreateNPCDefinition(ref dummyDef);
        //}

        //private static void CreateNPCDefinition (ref DummyNPCDef dummyDef)
        //{
        //    try
        //    {
        //        //(String name, int[] attributes, int[] talentValues, int[] talentSkills, int[] hitChances, 
        //        //int guild, int voice, String visual, String bodyMesh, int bodyTex, int skinColor, 
        //        //String headMesh, int headTex, int teethTex)
        //        NPCDef newDef = new NPCDef(dummyDef.getName(), dummyDef.getAttributes(), dummyDef.getTalentValues(), 
        //            dummyDef.getTalentSkills(), dummyDef.getHitChances(), dummyDef.getGuild(), dummyDef.getVoice(),
        //            dummyDef.getVisual(), dummyDef.getBodyMesh(), dummyDef.getBodyTex(), dummyDef.getSkinColor(),
        //            dummyDef.getHeadMesh(), dummyDef.getHeadTex(), dummyDef.getTeethTex());
        //        if (newDef != null)
        //        {
        //            npcDefDict.Add(dummyDef.getID(), newDef);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("CreateNPCDefinition: Some necessary attributes of DummyItemDef" 
        //            + " are not valid or missing: ID=" + dummyDef.getID() + ": " + ex);
        //    }
        //}





        //private static void CreateVobInstances (InstTableEnum instTab, 
        //    ref List<List<List<object>>> instList,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        //{
        //    List<object> inst;
        //    int vobIDColIndex = colTypesKeys.IndexOf("ID");
            
        //    switch (instTab)
        //    {
        //        case (InstTableEnum.Mob_inst):
        //            for (int r = 0; r < instList[0].Count; r++)
        //            {
        //                inst = instList[0][r];
        //                CreateMobInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        //            }
        //            break;
        //        case (InstTableEnum.Spell_inst):
        //            for (int r = 0; r < instList[0].Count; r++)
        //            {
        //                inst = instList[0][r];
        //                CreateSpellInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        //            }
        //            break;
        //        case (InstTableEnum.Item_inst):
        //            for (int r = 0; r < instList[0].Count; r++)
        //            {
        //                inst = instList[0][r];
        //                CreateItemInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        //            }
        //            break;
        //        case (InstTableEnum.NPC_inst):
        //            for (int r = 0; r < instList[0].Count; r++)
        //            {
        //                inst = instList[0][r];
        //                CreateNPCInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        //            }
        //            break;
        //        default:
        //            Log.Logger.logError("Invalid InstTableEnum " + instTab + " detected in method CreateVobInstances"
        //                + ": Terminating vob-instance-creation!");
        //            break;
        //    }
        //}
        
        ////private static void CreateVobInstance (InstTableEnum instTab, 
        ////    ref List<object> inst,
        ////    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        ////{
        ////    switch (instTab)
        ////    {
        ////        case (InstTableEnum.Mob_inst):
        ////            CreateMobInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        ////            break;
        ////        case (InstTableEnum.Spell_inst):
        ////            CreateSpellInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        ////            break;
        ////        case (InstTableEnum.Item_inst):
        ////            CreateItemInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        ////            break;
        ////        case (InstTableEnum.NPC_inst):
        ////            CreateNPCInstance(ref inst, ref colTypesKeys, ref colTypesVals);
        ////            break;
        ////        default:
        ////            Log.Logger.logError("Invalid InstTableEnum " + instTab + " detected in method createVobInstance"
        ////                + ": Terminating vob-instance-creation!");
        ////            break;
        ////    }
        ////}

        //private static void CreateMobInstance (ref List<object> inst,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        //{
        //    MobDef vobDef;
        //    MobInst oldInst;
        //    MobInst vobInst;            
        //    int colIndex;
            
        //    colIndex = colTypesKeys.IndexOf("ItemDefID");
        //    if ((colIndex != -1) && (inst[colIndex] != null))
        //    {
        //        if (mobDefDict.TryGetValue((int) inst[colIndex], out vobDef))
        //        {
        //            vobInst = new MobInst(vobDef);

        //            // must continue in the if-block because IDE does not understand that vobInst
        //            // is set here and usable afterwards

        //            colIndex = colTypesKeys.IndexOf("ID");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setID((int) inst[colIndex]);
        //            }
                    
        //            colIndex = colTypesKeys.IndexOf("ChangeDate");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setChangeDate((string) inst[colIndex]);
        //            }

        //            colIndex = colTypesKeys.IndexOf("CreationDate");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setCreationDate((string) inst[colIndex]);
        //            }

        //            // try to add entry or update exsiting one in dictionaries
                    
        //            if (vobInst.getID() < 0)
        //            {
        //                // id has not been set properly before --> abort
        //                Log.Logger.logError("CreateItemInstance: No valid id was set for the new instance."
        //                    + " Aborting instantiation.");
        //                return;
        //            }
                    
        //            if (mobInstDict.TryGetValue(vobInst.getID(), out oldInst)) 
        //            {
        //                UpdateMobInstance(ref oldInst, ref vobInst);
        //            }
        //            else
        //            {
        //                mobInstDict.Add(vobInst.getID(), vobInst);
        //                //vobInst.SpawnVob();
        //            }

        //            // do this after everything else to ensure, the vob will be spawned or despawned,
        //            // no matter if the old entry was only preserved/updated or completly replaced
        //            colIndex = colTypesKeys.IndexOf("IsSpawned");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                ItemInst nowInst;
        //                if (itemInstDict.TryGetValue(vobInst.getID(), out nowInst))
        //                {
        //                    nowInst.setIsSpawned((bool) inst[colIndex]);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // not even the ItemDef-object was found
        //            // there is no basis to continue initialization
        //            Log.Logger.logError("CreateItemInstance: Cannot instantiate ItemInst-object"
        //                + " because itemDefDict does not contain an ItemDef with ID=" 
        //                + (int) inst[colIndex]);
        //            return;
        //        }
        //    }
        //}

        //private static void CreateSpellInstance (ref List<object> inst,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        //{

        //}

        //private static void CreateItemInstance (ref List<object> inst,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        //{
        //    ItemDef vobDef;
        //    ItemInst oldInst;
        //    ItemInst vobInst;            
        //    int colIndex;
            
        //    colIndex = colTypesKeys.IndexOf("ItemDefID");
        //    if ((colIndex != -1) && (inst[colIndex] != null))
        //    {
        //        if (itemDefDict.TryGetValue((int) inst[colIndex], out vobDef))
        //        {
        //            vobInst = new ItemInst(vobDef);

        //            // must continue in the if-block because IDE does not understand, vobInst
        //            // is set here and usable afterwards

        //            colIndex = colTypesKeys.IndexOf("ID");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setID((int) inst[colIndex]);
        //            }

        //            colIndex = colTypesKeys.IndexOf("Amount");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setAmount((int) inst[colIndex]);
        //            }

        //            colIndex = colTypesKeys.IndexOf("ChangeDate");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setChangeDate((string) inst[colIndex]);
        //            }

        //            colIndex = colTypesKeys.IndexOf("CreationDate");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                vobInst.setCreationDate((string) inst[colIndex]);
        //            }

        //            // try to add entry or update exsiting one in dictionaries
                    
        //            if (vobInst.getID() < 0)
        //            {
        //                // id has not been set properly before --> abort
        //                Log.Logger.logError("CreateItemInstance: No valid id was set for the new instance."
        //                    + " Aborting instantiation.");
        //                return;
        //            }
                    
        //            if (itemInstDict.TryGetValue(vobInst.getID(), out oldInst)) 
        //            {
        //                UpdateItemInstance(ref oldInst, ref vobInst);
        //            }
        //            else
        //            {
        //                itemInstDict.Add(vobInst.getID(), vobInst);
        //                //vobInst.SpawnVob();
        //            }

        //            // do this after everything else to ensure, the vob will be spawned or despawned,
        //            // no matter if the old entry was only preserved/updated or completely replaced
        //            colIndex = colTypesKeys.IndexOf("IsSpawned");
        //            if ((colIndex != -1) && (inst[colIndex] != null))
        //            {
        //                ItemInst nowInst;
        //                if (itemInstDict.TryGetValue(vobInst.getID(), out nowInst))
        //                {
        //                    nowInst.setIsSpawned((bool) inst[colIndex]);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // not even the ItemDef-object was found
        //            // there is no basis to continue initialization
        //            Log.Logger.logError("CreateItemInstance: Cannot instantiate ItemInst-object"
        //                + " because itemDefDict does not contain an ItemDef with ID=" 
        //                + (int) inst[colIndex]);
        //            return;
        //        }
        //    }
        //}

        //private static void CreateNPCInstance (ref List<object> inst,
        //    ref List<string> colTypesKeys, ref List<SQLiteGetTypeEnum> colTypesVals)
        //{

        //}



        //private static bool UpdateMobInstance (ref MobInst oldInst, ref MobInst newInst)
        //{
        //    bool replace = true;
        //    int newID = newInst.getID();
        //    MobInter newItem = (MobInter) newInst.getVob();
        //    MobInter oldItem = (MobInter) oldInst.getVob();

        //    if (oldInst == newInst)
        //    {
        //        // don't update if nothing would change to be begin with :)
        //        return false;
        //    }

        //    if (oldInst.getID() != newID)
        //    {
        //        // it is forbidden to update an instance with the incorrect id 
        //        // for organizational purposes
        //        Log.Logger.logWarning("UpdateItemInstance: Warning! Updating old instance"
        //            + " with a new one is forbidden if both don't share the same id!");
        //        return false;
        //    }

        //    if (oldInst.getVobDef() == newInst.getVobDef())
        //    {
        //        // replacing the object entirely is not necessary 
        //        // --> simply update the existing one instead
        //        replace = false;
        //    }

        //    if (replace)
        //    {
        //        // carefully replace the instance because it may 
        //        // directly affect the running gameworld
        //        oldInst.DespawnVob();
        //        oldInst.DeleteVob();
        //        oldInst.setVobDef(newInst.getVobDef());
        //    }
        //    else
        //    {
        //        // carefully update the existing instance
        //        oldInst.setInWorld(newInst.getInWorld());
        //        oldInst.setPosition(newInst.getPosition());
        //        oldInst.setChangeDate(newInst.getChangeDate());
        //        oldInst.setCreationDate(newInst.getCreationDate());
        //    }

        //    return true;
        //}

        //private static bool UpdateSpellInstance (ref SpellInst oldInst, ref SpellInst newInst)
        //{
        //    return true;
        //}

        //private static bool UpdateItemInstance (ref ItemInst oldInst, ref ItemInst newInst)
        //{
        //    bool replace = true;
        //    int newID = newInst.getID();
        //    Item newItem = (Item) newInst.getVob();
        //    Item oldItem = (Item) oldInst.getVob();

        //    if (oldInst == newInst)
        //    {
        //        // don't update if nothing would change to be begin with :)
        //        return false;
        //    }

        //    if (oldInst.getID() != newID)
        //    {
        //        // it is forbidden to update an instance with the incorrect id 
        //        // for organizational purposes
        //        Log.Logger.logWarning("UpdateItemInstance: Warning! Updating old instance"
        //            + " with a new one is forbidden if both don't share the same id!");
        //        return false;
        //    }

        //    if (oldInst.getVobDef() == newInst.getVobDef())
        //    {
        //        // replacing the object entirely is not necessary 
        //        // --> simply update the existing one instead
        //        replace = false;
        //    }

        //    if (replace)
        //    {
        //        // carefully replace the instance because it may 
        //        // directly affect the running gameworld
        //        oldInst.DespawnVob();
        //        oldInst.DeleteVob();
        //        oldInst.setVobDef(newInst.getVobDef());
        //    }
        //    else
        //    {
        //        // carefully update the existing instance
        //        oldInst.setAmount(newInst.getAmount());
        //        oldInst.setInWorld(newInst.getInWorld());
        //        oldInst.setPosition(newInst.getPosition());
        //        oldInst.setChangeDate(newInst.getChangeDate());
        //        oldInst.setCreationDate(newInst.getCreationDate());
        //    }

        //    return true;
        //}

        //private static bool UpdateNPCInstance (ref NPCInst oldInst, ref NPCInst newInst)
        //{
        //    return true;
        //}

    }
}
