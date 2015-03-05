using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMISWORDRAW : AbstractMisc
    {
        static ITMISWORDRAW ii;
        public static ITMISWORDRAW get()
        {
            if (ii == null)
                ii = new ITMISWORDRAW();
            return ii;
        }


        protected ITMISWORDRAW()
            : base("ITMISWORDRAW")
        {
            Name = "Rohstahl";
            Visual = "ItMiSwordraw.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_METAL;

            CreateItemInstance();
        }
    }
}
