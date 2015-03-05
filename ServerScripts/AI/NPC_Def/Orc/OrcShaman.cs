using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Orc
{
    class OrcShaman : NPC
    {
        public OrcShaman()
            : base()
        {
            Name = "Ork Schamane";

            Strength = 100;
            Dexterity = 170;
            HPMax = 350;
            HP = 350;
            MPMax = 100;
            MP = 100;
            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 130);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 130);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 130);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 130);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 130);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 65);

            setDamageType(DamageTypes.DAM_EDGE);

            setHitchances(NPCTalent.H1, 60);
            setHitchances(NPCTalent.H2, 60);
            setHitchances(NPCTalent.Bow, 60);
            setHitchances(NPCTalent.CrossBow, 60);

            WeaponMode = 1;




            this.setVisual("Orc.mds", "Orc_BodyShaman", 0, 0, "Orc_HeadShaman", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.ORC_NONE);
            this.Equip(this.addItem("ItMw_2H_OrcAxe_01", 1));

            CreateVob();
        }
    }
}
