using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_ADDON_SKINNER : AbstractKeys
    {
        static ITKE_ADDON_SKINNER ii;
        public static ITKE_ADDON_SKINNER get()
        {
            if (ii == null)
                ii = new ITKE_ADDON_SKINNER();
            return ii;
        }


        protected ITKE_ADDON_SKINNER()
            : base("ITKE_ADDON_SKINNER")
        {
            Visual = "ItKe_Key_02.3ds";
            Description = "Truhenschlüssel";

            CreateItemInstance();
        }
    }
}
