using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_BLOODCUP_MIS : AbstractMisc
    {
        static ITMI_BLOODCUP_MIS ii;
        public static ITMI_BLOODCUP_MIS get()
        {
            if (ii == null)
                ii = new ITMI_BLOODCUP_MIS();
            return ii;
        }


        protected ITMI_BLOODCUP_MIS()
            : base("ITMI_BLOODCUP_MIS")
        {
            Name = "Blutkelch";
            Visual = "ItMi_GoldCup.3ds";
            Description = "Ein Blutkelch";

            Flags = Enumeration.Flags.ITEM_MULTI | Enumeration.Flags.ITEM_MISSION;
            
            Materials = Enumeration.MaterialTypes.MAT_METAL;


            CreateItemInstance();
        }
    }
}
