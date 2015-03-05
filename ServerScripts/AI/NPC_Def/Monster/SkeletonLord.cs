using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class SkeletonLord : NPC
    {
        public SkeletonLord()
            : base()
        {
            Name = "Schattenkrieger";

            Strength = 105;
            Dexterity = 100;
            HPMax = 400;
            HP = 400;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 100);

            setDamageType(DamageTypes.DAM_EDGE);






            this.setVisual("HumanS.mds", "hum_body_Naked0", 0, 0, "Ske_Head", 0, 0);
            this.ApplyOverlay("humans_1hST1");
            this.ApplyOverlay("humans_2hST2");
            this.ApplyOverlay("humans_BowT1");
            this.ApplyOverlay("humans_CBowT1");
            this.ApplyOverlay("humans_skeleton");
            this.InitNPCAI();

            this.setGuild(Guilds.MON_SKELETON);


            this.Equip(this.addItem("ItMw_Zweihaender2", 1));
            this.Equip(this.addItem("ITAR_PAL_SKEL", 1));
            CreateVob();

        }

    }


}
