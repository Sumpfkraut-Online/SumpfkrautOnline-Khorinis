using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_MIL_L : AbstractArmors
    {
        static ITAR_MIL_L ii;
        public static ITAR_MIL_L get()
        {
            if (ii == null)
                ii = new ITAR_MIL_L();
            return ii;
        }


        protected ITAR_MIL_L()
            : base("ITAR_MIL_L")
        {
            Visual = "ItAr_MIL_L.3ds";
            Visual_Change = "Armor_Mil_L.asc";
            Name = "Leichte Milizrüstung";
            Description = Name;

            ProtectionEdge = 40;
            ProtectionBlunt = 40;
            ProtectionPoint = 40;


            CreateItemInstance();
        }
    }
}
