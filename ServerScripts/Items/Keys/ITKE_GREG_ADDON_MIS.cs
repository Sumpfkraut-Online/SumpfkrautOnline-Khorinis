using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_GREG_ADDON_MIS : AbstractKeys
    {
        static ITKE_GREG_ADDON_MIS ii;
        public static ITKE_GREG_ADDON_MIS get()
        {
            if (ii == null)
                ii = new ITKE_GREG_ADDON_MIS();
            return ii;
        }


        protected ITKE_GREG_ADDON_MIS()
            : base("ITKE_GREG_ADDON_MIS")
        {
            Name = "Schlüssel";
            Visual = "ItKe_Key_01.3ds";
            Description = Name;

            CreateItemInstance();
        }
    }
}
