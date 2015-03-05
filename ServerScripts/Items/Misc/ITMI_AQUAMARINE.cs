using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_AQUAMARINE : AbstractMisc
    {
        static ITMI_AQUAMARINE ii;
        public static ITMI_AQUAMARINE get()
        {
            if (ii == null)
                ii = new ITMI_AQUAMARINE();
            return ii;
        }


        protected ITMI_AQUAMARINE()
            : base("ITMI_AQUAMARINE")
        {
            Name = "Aquamarin";
            Visual = "ItMi_Aquamarine.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_STONE;


            CreateItemInstance();
        }
    }
}
