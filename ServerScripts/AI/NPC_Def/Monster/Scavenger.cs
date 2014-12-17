using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Scavenger: NPC
    {
        public Scavenger()
            : base()
        {
            Name = "Scavanger";

            Strength = 35;
            Dexterity = 35;
            HPMax = 70;
            HP = 70;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 35);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 35);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 35);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 35);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Scavenger.mds", "Sca_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_SCAVANGER);

            this.addItem("ItFoMuttonRaw", 1);

            CreateVob();
        }
    }
}
