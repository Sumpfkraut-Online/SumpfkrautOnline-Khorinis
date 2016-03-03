using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Log;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
		public GUCScripts()
		{
            Logger.Log("######## Initalise SumpfkrautOnline ServerScripts #########");

            AddSomeDefs();

            CreateWorld();

            Logger.Log("######################## Finished #########################");
		}

        void AddSomeDefs()
        {
            VobDef vobDef = new VobDef("baum");
            vobDef.Visual = "NW_Nature_BigTree_356P.3ds";
            vobDef.Create();

            ItemDef itemDef = new ItemDef("apple");
            itemDef.Name = "Apfel";
            itemDef.Visual = "ItFo_Apple.3ds";
            itemDef.Create();

            NPCDef npcDef = new NPCDef("player");
            npcDef.Name = "Spieler";
            npcDef.Visual = "humans.mds";
            npcDef.BodyMesh = Enumeration.HumBodyMeshs.HUM_BODY_NAKED0.ToString();
            npcDef.BodyTex = (int)Enumeration.HumBodyTexs.G1Hero;
            npcDef.HeadMesh = Enumeration.HumHeadMeshs.HUM_HEAD_PONY.ToString();
            npcDef.HeadTex = (int)Enumeration.HumHeadTexs.Face_N_Player;
            npcDef.Create();

            npcDef = new NPCDef("scavenger");
            npcDef.Name = "Scavenger";
            npcDef.Visual = "scavenger.mds";
            npcDef.BodyMesh = "Sca_Body";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "";
            npcDef.HeadTex = 0;
            npcDef.Create();
        }


        public static NPCInst testChar;
        void CreateWorld()
        {
            WorldDef wDef = new WorldDef();
            WorldInst.NewWorld = new WorldInst(default(WorldDef));

            VobInst vob = new VobInst(BaseVobDef.Get<VobDef>("baum"));
            vob.BaseInst.Spawn(WorldInst.NewWorld.BaseWorld, new Types.Vec3f(0, 0, 1000));

            testChar = new NPCInst(BaseVobDef.Get<NPCDef>("scavenger"));
            testChar.BaseInst.Spawn(WorldInst.NewWorld.BaseWorld, new Types.Vec3f(0, 0, 500));
        }
    }
}
