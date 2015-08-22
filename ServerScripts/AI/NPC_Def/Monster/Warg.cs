using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Warg: NPC
    {
        public Warg()
            : base()
        {
            Name = "Warg";

            Strength = 150;
            Dexterity = 150;
            HPMax = 300;
            HP = 300;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 125);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 125);

            setDamageType(DamageTypes.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Wolf.mds", "Warg_Body2", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WARG);

            CreateVob();
        }
    }
}
