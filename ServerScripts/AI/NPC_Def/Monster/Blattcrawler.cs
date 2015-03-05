using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Blattcrawler : NPC
    {
        public Blattcrawler()
            : base()
        {
            Name = "Fangheuschrecke";

            Strength = 75;
            Dexterity = 75;
            HPMax = 150;
            HP = 150;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 75);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 75);

            setDamageType(DamageTypes.DAM_EDGE);

            




            this.setVisual("Blattcrawler.mds", "BlattCrawler_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_BLATTCRAWLER);

            WeaponMode = 1;

            CreateVob();

        }
    }
}
