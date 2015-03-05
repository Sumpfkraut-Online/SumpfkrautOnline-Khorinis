using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Bloodfly : NPC
    {
        public Bloodfly()
            : base()
        {
            Name = "Blutfliege";

            Strength = 20;
            Dexterity = 20;
            HPMax = 40;
            HP = 40;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 20);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Bloodfly.mds", "Blo_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_BLOODFLY);

            CreateVob();
        }
    }

    public class YoungBloodfly : NPC
    {
        public YoungBloodfly()
            : base()
        {
            Name = "Kleine Blutfliege";

            Strength = 5;
            Dexterity = 5;
            HPMax = 20;
            HP = 20;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 0);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Bloodfly.mds", "Blo_Body", 0, 0, "", 0, 0);

            this.setScale(new Types.Vec3f(0.9f, 0.9f, 0.9f));

            this.InitNPCAI();

            this.setGuild(Guilds.MON_BLOODFLY);

            CreateVob();
        }
    }
}
