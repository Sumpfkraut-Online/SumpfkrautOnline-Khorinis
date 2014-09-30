using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_PORTALTEMPELWALKTHROUGH_ADDON : AbstractKeys
    {
        static ITKE_PORTALTEMPELWALKTHROUGH_ADDON ii;
        public static ITKE_PORTALTEMPELWALKTHROUGH_ADDON get()
        {
            if (ii == null)
                ii = new ITKE_PORTALTEMPELWALKTHROUGH_ADDON();
            return ii;
        }


        protected ITKE_PORTALTEMPELWALKTHROUGH_ADDON()
            : base("ITKE_PORTALTEMPELWALKTHROUGH_ADDON")
        {
            Name = "Rostiger Schlüssel";
            Visual = "ItKe_Key_01.3ds";
            Description = Name;

            CreateItemInstance();
        }
    }
}
