using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    class Giant_DesertRat: NPC
    {
        public Giant_DesertRat()
            : base()
        {
            Name = "Wüstenratte";

            Strength = 75;
            Dexterity = 75;
            HPMax = 75;
            HP = 75;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 25);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 75);

            setDamageType(DamageType.DAM_EDGE);






            this.setVisual("Giant_Rat.mds", "Giant_DesertRat_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GIANT_RAT);
            this.setScale(new Types.Vec3f(1.3f, 1.3f, 1f));

            WeaponMode = 1;

            CreateVob();

        }
    }
}
