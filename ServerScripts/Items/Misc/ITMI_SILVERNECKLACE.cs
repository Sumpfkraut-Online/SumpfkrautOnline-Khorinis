using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SILVERNECKLACE : AbstractMisc
    {
        static ITMI_SILVERNECKLACE ii;
        public static ITMI_SILVERNECKLACE get()
        {
            if (ii == null)
                ii = new ITMI_SILVERNECKLACE();
            return ii;
        }


        protected ITMI_SILVERNECKLACE()
            : base("ITMI_SILVERNECKLACE")
        {
            Name = "Silberkette";
            Visual = "ItMi_SilverNecklace.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialTypes.MAT_METAL;
            Flags = Enumeration.Flags.ITEM_MULTI | Enumeration.Flags.ITEM_AMULET;

            CreateItemInstance();
        }
    }
}
