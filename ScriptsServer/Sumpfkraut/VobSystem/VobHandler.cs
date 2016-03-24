using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Data.Sqlite;
using SQLiteDataReader = Mono.Data.Sqlite.SqliteDataReader;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
using GUC.Enumeration;
using GUC.Server.Scripts.Sumpfkraut.Database;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Server.WorldObjects;


namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{
    // /**
    // *   Class which initializes all vobs of the indivual types from database information: mobs, items, npcs.
    // */
    //public class VobHandler : GUC.Utilities.Threading.AbstractRunnable
    //{

    //    new public static readonly String _staticName = "VobHandler (static)";

    //    protected bool isInitialized = false;

 

    //    public VobHandler (String objName, bool startOnCreate)
    //        : base(startOnCreate)
    //    {
    //        this.SetObjName(objName);
    //    }



    //    public void Init()
    //    {
    //        LoadDefinitions();
    //        LoadInstances();
    //    }



    //    #region Vob-Definitions

    //    public bool LoadDefinitions()
    //    {
    //        return false;
    //    }

    //    public void LoadVobDef (VobDefType type, out VobDefLoader vobLoader)
    //    {
    //        LoadVobDef(type, null, out vobLoader);
    //    }

    //    // idRange must be 
    //    public void LoadVobDef (VobDefType type, List<Vec2Int> idRanges, 
    //        out VobDefLoader vobLoader)
    //    {
    //        vobLoader = new VobDefLoader(type, idRanges, true);
    //    }

    //    public bool LoadDefEffect ()
    //    {
    //        return false;
    //    }
        
    //    // eventually useless because definitions should not be declared by the running server
    //    // but from the outside
    //    public bool SaveDefinitions ()
    //    {
    //        return false;
    //    }

    //    #endregion

    //    #region Vob-Instances

    //    public bool LoadInstances ()
    //    {
    //        return false;
    //    }

    //    public bool SaveInstances ()
    //    {
    //        return false;
    //    }

    //    #endregion



    //    public override void Run ()
    //    {
    //        if (!isInitialized)
    //        {
    //            Init();
    //        }
    //    }

    //}
}
