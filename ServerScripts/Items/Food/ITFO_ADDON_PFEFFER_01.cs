using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Food
{
    public class ITFO_ADDON_PFEFFER_01 : AbstractFood
    {
        static ITFO_ADDON_PFEFFER_01 ii;
        public static ITFO_ADDON_PFEFFER_01 get()
        {
            if (ii == null)
                ii = new ITFO_ADDON_PFEFFER_01();
            return ii;
        }


        protected ITFO_ADDON_PFEFFER_01()
            : base("ITFO_ADDON_PFEFFER_01")
        {
            Name = "Pfefferbeutel";
            Visual = "ItMi_Pocket.3ds";
            Description = "Roter Tränen- Pfeffer";
            ScemeName = "";

            MainFlags = Enumeration.MainFlags.ITEM_KAT_NONE;

            
            CreateItemInstance();
        }
    }
}
