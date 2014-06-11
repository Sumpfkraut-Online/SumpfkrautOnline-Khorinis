using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Orc
{
    class OrcElite : NPC
    {
        public OrcElite()
            : base()
        {
            Name = "Ork Elite";

            Strength = 125;
            Dexterity = 225;
            HPMax = 450;
            HP = 450;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 160);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 160);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 160);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 160);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 160);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 100);

            setDamageType(DamageType.DAM_EDGE);

            setHitchances(NPCTalents.H1, 100);
            setHitchances(NPCTalents.H2, 100);
            setHitchances(NPCTalents.Bow, 100);
            setHitchances(NPCTalents.CrossBow, 100);

            WeaponMode = 1;




            this.setVisual("Orc.mds", "Orc_BodyElite", 0, 0, "Orc_HeadWarrior", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.ORC_NONE);
            this.Equip(this.addItem("ItMw_2H_OrcSword_02", 1));
            this.Equip(this.addItem("ItMw_2H_OrcAxe_01", 1));
            CreateVob();
        }
    }
}
