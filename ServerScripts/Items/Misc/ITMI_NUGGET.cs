using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_NUGGET : AbstractMisc
    {
        static ITMI_NUGGET ii;
        public static ITMI_NUGGET get()
        {
            if (ii == null)
                ii = new ITMI_NUGGET();
            return ii;
        }


        protected ITMI_NUGGET()
            : base("ITMI_NUGGET")
        {
            Name = "Erzbrocken";
            Visual = "ItMi_Nugget.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_STONE;

            Wear = Enumeration.ArmorFlags.WEAR_EFFECT;
            Effect = "SPELLFX_MANAPOTION";

            CreateItemInstance();
        }
    }
}
