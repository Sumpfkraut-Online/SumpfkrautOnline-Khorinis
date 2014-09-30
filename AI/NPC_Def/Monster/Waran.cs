using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;
using GUC.Server.Scripts.AI;


namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Waran : NPC
    {
        public Waran()
            : base()
        {
            Name = "Waran";

            Strength = 60;
            Dexterity = 60;
            HPMax = 120;
            HP = 120;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 100);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 100);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Waran.mds", "War_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WARAN);

            CreateVob();
        }
    }

    public class Firewaran : NPC
    {
        public Firewaran()
            : base()
        {
            Name = "Feuerwaran";

            Strength = 150;
            Dexterity = 150;
            HPMax = 300;
            HP = 300;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 150);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 150);

            setDamageType(DamageType.DAM_FIRE);

            WeaponMode = 1;




            this.setVisual("Waran.mds", "War_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WARAN);

            CreateVob();
        }
    }
    
}
