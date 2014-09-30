using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_CITY_TOWER_02 : AbstractKeys
    {
        static ITKE_CITY_TOWER_02 ii;
        public static ITKE_CITY_TOWER_02 get()
        {
            if (ii == null)
                ii = new ITKE_CITY_TOWER_02();
            return ii;
        }


        protected ITKE_CITY_TOWER_02()
            : base("ITKE_CITY_TOWER_02")
        {
            Visual = "ItKe_Key_01.3ds";

            Name = "Turm Schlüssel";
            Description = Name;

            CreateItemInstance();
        }
    }
}
