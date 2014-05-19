using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_PIR_L_ADDON : AbstractArmors
    {
        static ITAR_PIR_L_ADDON ii;
        public static ITAR_PIR_L_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_PIR_L_ADDON();
            return ii;
        }


        protected ITAR_PIR_L_ADDON()
            : base("ITAR_PIR_L_ADDON")
        {
            Visual = "ItAr_PIR_L_ADDON.3ds";
            Visual_Change = "Armor_Pir_L_Addon.asc";
            Name = "Piratenkleidung";
            Description = Name;

            ProtectionEdge = 40;
            ProtectionBlunt = 40;
            ProtectionPoint = 40;


            CreateItemInstance();
        }
    }
}
