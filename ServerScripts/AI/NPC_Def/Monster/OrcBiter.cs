using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class OrcBiter : NPC
    {
        public OrcBiter()
            : base()
        {
            Name = "Beisser";

            Strength = 60;
            Dexterity = 60;
            HPMax = 120;
            HP = 120;


            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 60);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 60);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 60);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 60);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 60);

            setDamageType(DamageTypes.DAM_EDGE);






            this.setVisual("Scavenger.mds", "Sc2_Body", 0, 0, "", 0, 0);
            this.ApplyOverlay("Orcbiter.mds");

            this.InitNPCAI();

            this.setGuild(Guilds.MON_SCAVANGER);

            WeaponMode = 1;

            CreateVob();

        }
    }
}
