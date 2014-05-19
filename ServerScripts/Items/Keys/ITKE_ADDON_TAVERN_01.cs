using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_ADDON_TAVERN_01 : AbstractKeys
    {
        static ITKE_ADDON_TAVERN_01 ii;
        public static ITKE_ADDON_TAVERN_01 get()
        {
            if (ii == null)
                ii = new ITKE_ADDON_TAVERN_01();
            return ii;
        }


        protected ITKE_ADDON_TAVERN_01()
            : base("ITKE_ADDON_TAVERN_01")
        {
            Name = "Schlüssel";
            Visual = "ItKe_Key_01.3ds";
            Description = Name;

            CreateItemInstance();
        }
    }
}
