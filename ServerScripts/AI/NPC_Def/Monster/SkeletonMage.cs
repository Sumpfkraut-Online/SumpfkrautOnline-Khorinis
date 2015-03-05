using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class SkeletonMage : NPC
    {
        public SkeletonMage()
            : base()
        {
            Name = "Skelettmagier";

            Strength = 150;
            Dexterity = 150;
            HPMax = 300;
            HP = 300;
            MPMax = 200;
            MP = 200;

            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 175);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 50);

            setDamageType(DamageType.DAM_EDGE);






            this.setVisual("HumanS.mds", "Ske_Fly_Body", 1, 0, "", 1, 0);
            this.ApplyOverlay("humans_skeleton_fly.mds");
            this.InitNPCAI();

            this.setGuild(Guilds.MON_SKELETON);


            this.Equip(this.addItem("ItMw_Zweihaender2", 1));
            this.Equip(this.addItem("ITAR_PAL_SKEL", 1));
            CreateVob();

        }

    }


}
