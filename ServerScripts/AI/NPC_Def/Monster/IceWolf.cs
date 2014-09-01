using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;
using GUC.Server.Scripts.AI;


namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Icewolf : NPC
    {
        public Icewolf()
            : base()
        {
            Name = "Eiswolf";

            Strength = 150;
            Dexterity = 150;
            HPMax = 300;
            HP = 300;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 150);

            setDamageType(DamageTypes.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Wolf.mds", "SnoWol_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WOLF);

            this.addItem("ItFoMuttonRaw", 1);

            CreateVob();
        }
    }

    
}
