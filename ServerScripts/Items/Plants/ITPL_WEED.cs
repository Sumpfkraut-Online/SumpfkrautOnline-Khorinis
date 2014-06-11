using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Items.Plants
{
    public class ITPL_WEED : AbstractPlants
    {
        static ITPL_WEED ii;
        public static ITPL_WEED get()
        {
            if (ii == null)
                ii = new ITPL_WEED();
            return ii;
        }


        protected ITPL_WEED()
            : base("ITPL_WEED")
        {
            Name = "Unkraut";
            Visual = "ItPl_Weed.3ds";
            Description = Name;

            CreateItemInstance();
        }
    }
}
