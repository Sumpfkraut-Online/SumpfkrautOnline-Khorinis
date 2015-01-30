using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Troll_Black: NPC
    {
        public Troll_Black()
            : base()
        {
            Name = "Schwarzer Troll";

            Strength = 200;
            Dexterity = 200;
            HPMax = 1000;
            HP = 1000;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, -1);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, -1);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 300);
            

            WeaponMode = 1;

            this.setDamageType(DamageTypes.DAM_FLY);



            this.setVisual("Troll.mds", "Troll_Black_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_TROLL);

            CreateVob();
        }
    }
}
