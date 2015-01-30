using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SILVERCANDLEHOLDER : AbstractMisc
    {
        static ITMI_SILVERCANDLEHOLDER ii;
        public static ITMI_SILVERCANDLEHOLDER get()
        {
            if (ii == null)
                ii = new ITMI_SILVERCANDLEHOLDER();
            return ii;
        }


        protected ITMI_SILVERCANDLEHOLDER()
            : base("ITMI_SILVERCANDLEHOLDER")
        {
            Name = "Silberner Kerzenständer";
            Visual = "ItMi_SilverCandleHolder.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
