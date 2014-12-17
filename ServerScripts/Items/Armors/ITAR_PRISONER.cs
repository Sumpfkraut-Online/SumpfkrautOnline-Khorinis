using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_PRISONER : AbstractArmors
    {
        static ITAR_PRISONER ii;
        public static ITAR_PRISONER get()
        {
            if (ii == null)
                ii = new ITAR_PRISONER();
            return ii;
        }


        protected ITAR_PRISONER()
            : base("ITAR_PRISONER")
        {
            Visual = "ItAr_Prisoner.3ds";
            Visual_Change = "Armor_Prisoner.asc";
            Name = "Sträflings Kleidung";
            Description = Name;

            ProtectionEdge = 20;
            ProtectionBlunt = 20;
            ProtectionPoint = 20;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
