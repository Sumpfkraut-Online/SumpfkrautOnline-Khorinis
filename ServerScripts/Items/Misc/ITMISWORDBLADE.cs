using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMISWORDBLADE : AbstractMisc
    {
        static ITMISWORDBLADE ii;
        public static ITMISWORDBLADE get()
        {
            if (ii == null)
                ii = new ITMISWORDBLADE();
            return ii;
        }


        protected ITMISWORDBLADE()
            : base("ITMISWORDBLADE")
        {
            Name = "Klinge";
            Visual = "ItMiSwordblade.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_METAL;

            CreateItemInstance();
        }
    }
}
