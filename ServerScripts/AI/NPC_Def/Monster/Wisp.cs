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
    public class Wisp : NPC
    {
        public Wisp()
            : base()
        {
            Name = "Irrlicht";

            Strength = 20;
            Dexterity = 20;
            HPMax = 40;
            HP = 40;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 0);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 0);

            setDamageType(DamageTypes.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Irrlicht.mds", "Irrlicht_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_BLOODFLY);

            

            CreateVob();
        }
    }

}
