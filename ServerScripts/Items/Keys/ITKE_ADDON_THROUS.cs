using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_ADDON_THROUS : AbstractKeys
    {
        static ITKE_ADDON_THROUS ii;
        public static ITKE_ADDON_THROUS get()
        {
            if (ii == null)
                ii = new ITKE_ADDON_THROUS();
            return ii;
        }


        protected ITKE_ADDON_THROUS()
            : base("ITKE_ADDON_THROUS")
        {
            Visual = "ItKe_Key_02.3ds";
            Description = "Thorus Schlüssel";

            CreateItemInstance();
        }
    }
}
