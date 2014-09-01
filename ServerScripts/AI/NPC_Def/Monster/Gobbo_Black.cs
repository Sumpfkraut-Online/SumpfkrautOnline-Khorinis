using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Gobbo_Black : NPC
    {
        public Gobbo_Black()
            : base()
        {
            Name = "schwarzer Goblin";

            Strength = 40;
            Dexterity = 40;
            HPMax = 40;
            HP = 40;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 40);

            setDamageType(DamageTypes.DAM_EDGE);

            




            this.setVisual("Gobbo.mds", "Gob_Body", 1, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GOBBO);


            this.Equip(this.addItem("ItMw_1h_Bau_Mace", 1));
            WeaponMode = 1;

            CreateVob();

        }

    }


}
