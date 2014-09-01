using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    class GiantRat: NPC
    {
        public GiantRat()
            : base()
        {
            Name = "Riesenratte";

            Strength = 15;
            Dexterity = 15;
            HPMax = 30;
            HP = 30;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 15);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 15);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 15);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 15);

            setDamageType(DamageTypes.DAM_EDGE);






            this.setVisual("Giant_Rat.mds", "Giant_Rat_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GIANT_RAT);

            WeaponMode = 1;

            CreateVob();

        }
    }

    class YoungGiantRat : NPC
    {
        public YoungGiantRat()
            : base()
        {
            Name = "junge Riesenratte";

            Strength = 5;
            Dexterity = 5;
            HPMax = 10;
            HP = 10;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 5);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 5);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 5);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 5);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 5);

            setDamageType(DamageTypes.DAM_EDGE);






            this.setVisual("Giant_Rat.mds", "Giant_Rat_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GIANT_RAT);
            this.setScale(new Types.Vec3f(0.9f, 0.9f, 0.9f));
            WeaponMode = 1;

            CreateVob();

        }
    }
}
