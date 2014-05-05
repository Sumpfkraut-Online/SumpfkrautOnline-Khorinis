using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Orc
{
    class OrcWarrior : NPC
    {
        public OrcWarrior()
            : base()
        {
            Name = "Ork Krieger";

            Strength = 100;
            Dexterity = 150;
            HPMax = 300;
            HP = 300;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 20);

            setDamageType(DamageType.DAM_EDGE);

            setHitchances(NPCTalents.H1, 60);
            setHitchances(NPCTalents.H2, 60);
            setHitchances(NPCTalents.Bow, 60);
            setHitchances(NPCTalents.CrossBow, 60);

            WeaponMode = 1;




            this.setVisual("Orc.mds", "Orc_BodyWarrior", 0, 0, "Orc_HeadWarrior", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.ORC_NONE);
            
            CreateVob();
        }
    }
}
