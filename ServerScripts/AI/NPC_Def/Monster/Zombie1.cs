using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public abstract class Zombie : NPC
    {
        public Zombie()
            : base()
        {
            Name = "Zombie";

            Strength = 100;
            Dexterity = 100;
            HPMax = 400;
            HP = 400;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 50);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 50);

            setDamageType(DamageType.DAM_EDGE);






            this.setVisual("Zombie.mds", "Zom_Body", 1, 0, "Zom_Head", 0, 0);
            this.InitNPCAI();

            this.setGuild(Guilds.MON_ZOMBIE);


            this.Equip(this.addItem("ItMw_2H_Sword_M_01", 1));

        }

    }

    public class Zombie1 : Zombie
    {
        public Zombie1()
            : base()
        {
            this.setVisual("Zombie.mds", "Zom_Body", 0, 0, "Zom_Head", 0, 0);
            CreateVob();
        }
    }

    public class Zombie2 : Zombie
    {
        public Zombie2()
            : base()
        {
            this.setVisual("Zombie.mds", "Zom_Body", 0, 0, "Zom_Head", 1, 0);
            CreateVob();
        }
    }

    public class Zombie3 : Zombie
    {
        public Zombie3()
            : base()
        {
            this.setVisual("Zombie.mds", "Zom_Body", 0, 1, "Zom_Head", 0, 0);
            CreateVob();
        }
    }

    public class Zombie4 : Zombie
    {
        public Zombie4()
            : base()
        {
            this.setVisual("Zombie.mds", "Zom_Body", 0, 1, "Zom_Head", 1, 0);
            CreateVob();
        }
    }

}
