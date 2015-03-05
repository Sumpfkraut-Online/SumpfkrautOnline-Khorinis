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
    public class Meatbug : NPC
    {
        public Meatbug()
            : base()
        {
            Name = "Fleischwanze";

            Strength = 1;
            Dexterity = 1;
            HPMax = 10;
            HP = 10;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 0);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Meatbug.mds", "Mbg_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_MEATBUG);

            this.addItem("ItFoMuttonRaw", 1);

            CreateVob();
        }
    }

    
}
