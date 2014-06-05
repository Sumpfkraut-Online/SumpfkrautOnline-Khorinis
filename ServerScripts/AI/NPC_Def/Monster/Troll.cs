using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Troll: NPC
    {
        public Troll()
            : base()
        {
            Name = "Troll";

            Strength = 100;
            Dexterity = 100;
            HPMax = 500;
            HP = 500;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, -1);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, -1);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 125);
            

            WeaponMode = 1;

            this.setDamageType(DamageType.DAM_FLY);



            this.setVisual("Troll.mds", "Tro_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_TROLL);

            CreateVob();
        }
    }
}
