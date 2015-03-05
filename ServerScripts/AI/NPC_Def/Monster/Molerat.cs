using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.NPC_Def.Monster
{
    public class Molerat: NPC
    {
        public Molerat()
            : base()
        {
            Name = "Molerat";

            Strength = 25;
            Dexterity = 25;
            HPMax = 50;
            HP = 50;

            
            setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, 25);
            setProtection(DamageTypeIndex.DAM_INDEX_POINT, 25);
            setProtection(DamageTypeIndex.DAM_INDEX_EDGE, 25);
            setProtection(DamageTypeIndex.DAM_INDEX_FIRE, 25);
            setProtection(DamageTypeIndex.DAM_INDEX_FLY, 25);

            setDamageType(DamageType.DAM_EDGE);

            WeaponMode = 1;




            this.setVisual("Molerat.mds", "Mol_Body", 0, 0, "", 0, 0);

            this.InitNPCAI();

            this.setGuild(Guilds.MON_MOLERAT);

            this.addItem("ItFoMuttonRaw", 1);

            CreateVob();
        }
    }
}
