using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldInst : ExtendedObject, WorldObjects.World.IScriptWorld
    {

        new public static readonly string _staticName = "WorldInst (static)";

        WorldObjects.World baseWorld;
        public WorldObjects.World BaseWorld { get { return baseWorld; } }

        WorldDef definition = null;
        public WorldDef Definition { get { return definition; } }

        ScriptClock clock;
        public ScriptClock Clock { get { return this.clock; } }

        ScriptSkyCtrl skyCtrl;
        public ScriptSkyCtrl SkyCtrl { get { return this.skyCtrl; } }



        public WorldInst()
            : this("WorldInst (default)")
        { }

        public WorldInst(string objName)
        {
            SetObjName(objName);
            this.baseWorld = new WorldObjects.World();
            this.baseWorld.ScriptObject = this;

            this.clock = new ScriptClock(this);
            this.skyCtrl = new ScriptSkyCtrl(this);
        }



        public void OnWriteProperties(PacketWriter stream)
        {
            // write definition id
        }

        public void OnReadProperties(PacketReader stream)
        {
            // read definition id
        }

        partial void pCreate();
        public void Create()
        {
            this.baseWorld.Create();
            pCreate();
        }

        partial void pDelete();
        public void Delete()
        {
            this.baseWorld.Delete();
            pDelete();
        }

    }
}
