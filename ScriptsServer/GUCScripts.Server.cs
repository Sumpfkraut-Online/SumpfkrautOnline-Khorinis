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
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public GUCScripts()
        {
            Logger.Log("######## Initalise SumpfkrautOnline ServerScripts #########");

            AddSomeDefs();

            CreateTestWorld();

            Logger.Log("######################## Finished #########################");
        }

        void AddSomeDefs()
        {
            ModelDef m = new ModelDef("baum", "NW_Nature_BigTree_356P.3ds");
            m.Create();
            VobDef vobDef = new VobDef("baum");
            vobDef.Model = m;
            vobDef.Create();

            m = new ModelDef("zweihander", "ItMw_040_2h_PAL_Sword_03.3DS");
            m.Create();
            ItemDef itemDef = new ItemDef("zweihander");
            itemDef.Name = "Paladin Zweihänder";
            itemDef.ItemType = ItemTypes.Wep2H;
            itemDef.Model = m;
            itemDef.Create();

            m = new ModelDef("human", "humans.mds");

            ScriptOverlay overlay = new ScriptOverlay("Humans_1hST1");
            m.AddOverlay(overlay);
            
            ScriptAniJob aniJob = new ScriptAniJob("FistAttack", new ScriptAni(505, 127));
            aniJob.AniName = "s_FistAttack";
            m.AddAniJob(aniJob);

            aniJob = new ScriptAniJob("FistRunAttack", new ScriptAni(1100));
            aniJob.AniName = "t_FistAttackMove";
            m.AddAniJob(aniJob);

            m.Create();

            NPCDef npcDef = new NPCDef("player");
            npcDef.Name = "Spieler";
            npcDef.Model = m;
            npcDef.BodyMesh = Enumeration.HumBodyMeshs.HUM_BODY_NAKED0.ToString();
            npcDef.BodyTex = (int)Enumeration.HumBodyTexs.G1Hero;
            npcDef.HeadMesh = Enumeration.HumHeadMeshs.HUM_HEAD_PONY.ToString();
            npcDef.HeadTex = (int)Enumeration.HumHeadTexs.Face_N_Player;
            npcDef.Create();

            m = new ModelDef("scavenger", "scavenger.mds");
            m.Create();
            npcDef = new NPCDef("scavenger");
            npcDef.Name = "Scavenger";
            npcDef.Model = m;
            npcDef.BodyMesh = "Sca_Body";
            npcDef.BodyTex = 0;
            npcDef.HeadMesh = "";
            npcDef.HeadTex = 0;
            npcDef.Create();
        }

        void CreateTestWorld()
        {
            WorldDef wDef = new WorldDef();
            WorldInst.NewWorld = new WorldInst(default(WorldDef));

            VobInst vob = new VobInst(BaseVobDef.Get<VobDef>("baum"));
            vob.BaseInst.Spawn(WorldInst.NewWorld.BaseWorld, new Types.Vec3f(0, 0, 1000));

            var npc = new NPCInst(BaseVobDef.Get<NPCDef>("scavenger"));
            npc.BaseInst.Spawn(WorldInst.NewWorld.BaseWorld, new Types.Vec3f(0, 0, 500));

            WorldInst.NewWorld.Create();
            WorldInst.NewWorld.Clock.SetTime(new Types.WorldTime(0, 8), 3.0f);
            WorldInst.NewWorld.Clock.Start();
        }
    }
}
