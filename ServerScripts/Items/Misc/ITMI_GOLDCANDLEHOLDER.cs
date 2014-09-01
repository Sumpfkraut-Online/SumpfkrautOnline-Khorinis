using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDCANDLEHOLDER : AbstractMisc
    {
        static ITMI_GOLDCANDLEHOLDER ii;
        public static ITMI_GOLDCANDLEHOLDER get()
        {
            if (ii == null)
                ii = new ITMI_GOLDCANDLEHOLDER();
            return ii;
        }


        protected ITMI_GOLDCANDLEHOLDER()
            : base("ITMI_GOLDCANDLEHOLDER")
        {
            Name = "Goldener Kerzenständer";
            Visual = "ItMi_GoldCandleHolder.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
