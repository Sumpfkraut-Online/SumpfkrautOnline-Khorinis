using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Gobbo_Green : NPC
    {
        public Gobbo_Green()
            : base()
        {
            Name = "Goblin";

            Strength = 20;
            Dexterity = 20;
            HPMax = 20;
            HP = 20;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 20);

            setDamageType(DamageTypes.DAM_EDGE);

            




            this.setVisual("Gobbo.mds", "Gob_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GOBBO);


            this.Equip(this.addItem("ItMw_1h_Bau_Mace", 1));
            WeaponMode = 1;

            CreateVob();

        }
    }

    public class Young_Gobbo_Green : NPC
    {
        public Young_Gobbo_Green()
            : base()
        {
            Name = "Goblin";

            Strength = 5;
            Dexterity = 5;
            HPMax = 20;
            HP = 20;

            setDamageType(DamageTypes.DAM_EDGE);






            this.setVisual("Gobbo.mds", "Gob_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GOBBO);


            this.Equip(this.addItem("ItMw_1h_Bau_Mace", 1));
            WeaponMode = 1;

            CreateVob();

        }
    }
}
