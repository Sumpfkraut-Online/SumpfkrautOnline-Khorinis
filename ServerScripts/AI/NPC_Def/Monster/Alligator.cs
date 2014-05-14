using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Alligator : NPC
    {
        public Alligator()
            : base()
        {
            Name = "Alligator";

            Strength = 40;
            Dexterity = 40;
            HPMax = 80;
            HP = 80;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 20);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 20);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Alligator.mds", "KRO_BODY", 0, 0, "", 0, 0);

            this.setScale(new Types.Vec3f(0.9f, 0.9f, 0.9f));

            this.InitNPCAI();

            this.setGuild(Guilds.MON_ALLIGATOR);

            CreateVob();
        }
    }
}
