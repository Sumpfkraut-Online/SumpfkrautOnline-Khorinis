using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Keiler : NPC
    {
        public Keiler()
            : base()
        {
            Name = "Keiler";

            Strength = 50;
            Dexterity = 50;
            HPMax = 100;
            HP = 100;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 50);

            setDamageType(DamageTypes.DAM_EDGE);






            this.setVisual("Keiler.mds", "Keiler_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WOLF);

            WeaponMode = 1;

            CreateVob();

        }
    }
}
