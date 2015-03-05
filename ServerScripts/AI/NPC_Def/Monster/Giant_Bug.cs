using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Giant_Bug : NPC
    {
        public Giant_Bug()
            : base()
        {
            Name = "Feldräuber";

            Strength = 40;
            Dexterity = 40;
            HPMax = 80;
            HP = 80;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 40);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 40);

            setDamageType(DamageType.DAM_EDGE);






            this.setVisual("Giant_Bug.mds", "Giant_Bug_Body", 2, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GIANT_BUG);

            WeaponMode = 1;

            CreateVob();

        }

    }



    public class YoungGiant_Bug : NPC
    {
        public YoungGiant_Bug()
            : base()
        {
            Name = "Junger Feldräuber";

            Strength = 10;
            Dexterity = 10;
            HPMax = 20;
            HP = 20;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 10);

            setDamageType(DamageType.DAM_EDGE);






            this.setVisual("Giant_Bug.mds", "Giant_Bug_Body", 2, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_GIANT_BUG);

            WeaponMode = 1;

            this.setScale(new Types.Vec3f(0.9f, 0.9f, 0.9f));

            CreateVob();

        }

    }

}
