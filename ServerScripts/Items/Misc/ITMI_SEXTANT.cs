using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SEXTANT : AbstractMisc
    {
        static ITMI_SEXTANT ii;
        public static ITMI_SEXTANT get()
        {
            if (ii == null)
                ii = new ITMI_SEXTANT();
            return ii;
        }


        protected ITMI_SEXTANT()
            : base("ITMI_SEXTANT")
        {
            Name = "Sextant";
            Visual = "ItMi_Sextant.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
