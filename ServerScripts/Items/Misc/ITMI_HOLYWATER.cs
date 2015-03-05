using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_HOLYWATER : AbstractMisc
    {
        static ITMI_HOLYWATER ii;
        public static ITMI_HOLYWATER get()
        {
            if (ii == null)
                ii = new ITMI_HOLYWATER();
            return ii;
        }


        protected ITMI_HOLYWATER()
            : base("ITMI_HOLYWATER")
        {
            Name = "Geweihtes Wasser";
            Visual = "ItMi_HolyWater.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_GLAS;


            CreateItemInstance();
        }
    }
}
