using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMISWORDRAWHOT : AbstractMisc
    {
        static ITMISWORDRAWHOT ii;
        public static ITMISWORDRAWHOT get()
        {
            if (ii == null)
                ii = new ITMISWORDRAWHOT();
            return ii;
        }


        protected ITMISWORDRAWHOT()
            : base("ITMISWORDRAWHOT")
        {
            Name = "Glühender Stahl";
            Visual = "ItMiSwordrawhot.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_METAL;

            CreateItemInstance();
        }
    }
}
