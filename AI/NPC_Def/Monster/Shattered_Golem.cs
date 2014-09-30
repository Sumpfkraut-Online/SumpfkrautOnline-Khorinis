using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Shattered_Golem: NPC
    {
        public Shattered_Golem()
            : base()
        {
            Name = "";

            Strength = 125;
            Dexterity = 125;
            HPMax = 250;
            HP = 250;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, 100);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Golem.mds", "Gol_Body", 0, 0, "", 0, 0);
            
            this.InitNPCAI();

            this.setGuild(Guilds.MON_STONEGOLEM);

            CreateVob();
        }
    }
}
