using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BAU_L : AbstractArmors
    {
        static ITAR_BAU_L ii;
        public static ITAR_BAU_L get()
        {
            if (ii == null)
                ii = new ITAR_BAU_L();
            return ii;
        }


        protected ITAR_BAU_L()
            : base("ITAR_BAU_L")
        {
            Visual = "ItAr_Bau_L.3ds";
            Visual_Change = "Armor_Bau_L.asc";
            Name = "Bauernkleidung";
            Description = Name;

            ProtectionEdge = 15;
            ProtectionBlunt = 15;
            ProtectionPoint = 15;


            CreateItemInstance();
        }
    }
}
