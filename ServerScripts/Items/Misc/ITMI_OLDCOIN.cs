using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_OLDCOIN : AbstractMisc
    {
        static ITMI_OLDCOIN ii;
        public static ITMI_OLDCOIN get()
        {
            if (ii == null)
                ii = new ITMI_OLDCOIN();
            return ii;
        }


        protected ITMI_OLDCOIN()
            : base("ITMI_OLDCOIN")
        {
            Flags = Enumeration.Flags.ITEM_MULTI | Enumeration.Flags.ITEM_MISSION;
            Name = "Alte Münze";
            Visual = "ItMi_OldCoin.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_METAL;


            CreateItemInstance();
        }
    }
}
