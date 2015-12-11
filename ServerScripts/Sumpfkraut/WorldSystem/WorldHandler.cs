using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.WorldSystem
{
    public class WorldHandler : ScriptObject
    {

        new protected static readonly string _staticName = "WorldHandler (static)";

        public static readonly string defaultWorldName = @"NEWWORLD\NEWWORLD.ZEN";

        public static Dictionary<int, WorldInst> worldInstDict = new Dictionary<int, WorldInst>();
        public static Dictionary<string, WorldInst> worldInstNameDict = new Dictionary<string, WorldInst>();

        
        
        public WorldHandler ()
        {
            SetObjName("WorldHandler");
        }
        
        
        
        // hardcoded loading of the one and only world yet... load the few infos 
        // from database in the future
        public static void LoadWorlds ()
        {
            WorldInst worldInst;
            if (!worldInstDict.TryGetValue(0, out worldInst))
            {
                worldInst = new WorldInst();
                worldInstDict.Add(worldInst.getID(), worldInst);
                worldInstNameDict.Add(worldInst.getName(), worldInst);
            }
        }

    }
}
