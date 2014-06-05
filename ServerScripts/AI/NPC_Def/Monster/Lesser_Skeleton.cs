using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Lesser_Skeleton : NPC
    {
        public Lesser_Skeleton()
            : base()
        {
            Name = "Niederes Skelett";

            Strength = 45;
            Dexterity = 75;
            HPMax = 150;
            HP = 150;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 80);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 80);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 130);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 80);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 80);

            setDamageType(DamageType.DAM_EDGE);






            this.setVisual("HumanS.mds", "Ske_Body", 1, 0, "", 0, 0);
            this.ApplyOverlay("humans_1hST1");
            this.ApplyOverlay("humans_2hST2");
            this.ApplyOverlay("humans_BowT1");
            this.ApplyOverlay("humans_CBowT1");
            this.ApplyOverlay("humans_skeleton");
            this.InitNPCAI();

            this.setGuild(Guilds.MON_SKELETON);


            this.Equip(this.addItem("ItMw_1h_MISC_Sword", 1));

            CreateVob();

        }

    }


}
