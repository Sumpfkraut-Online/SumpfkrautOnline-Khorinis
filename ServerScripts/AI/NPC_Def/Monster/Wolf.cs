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
    public class Wolf : NPC
    {
        public Wolf()
            : base()
        {
            Name = "Wolf";

            Strength = 30;
            Dexterity = 30;
            HPMax = 60;
            HP = 60;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 30);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 30);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 30);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 30);

            setDamageType(DamageTypes.DAM_EDGE);

            WeaponMode = 1;

            


            this.setVisual("Wolf.mds", "Wol_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WOLF);

            this.addItem("ItFoMuttonRaw", 1);

            CreateVob();
        }
    }

    public class YoungWolf : NPC
    {
        public YoungWolf()
            : base()
        {
            Name = "Junger Wolf";

            Strength = 10;
            Dexterity = 10;
            HPMax = 20;
            HP = 20;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 10);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 10);

            setDamageType(DamageTypes.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Wolf.mds", "Wol_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_WOLF);

            this.addItem("ItFoMuttonRaw", 1);

            CreateVob();
        }
    }
}
