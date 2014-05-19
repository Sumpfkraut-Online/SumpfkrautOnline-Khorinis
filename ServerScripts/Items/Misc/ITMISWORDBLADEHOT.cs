using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMISWORDBLADEHOT : AbstractMisc
    {
        static ITMISWORDBLADEHOT ii;
        public static ITMISWORDBLADEHOT get()
        {
            if (ii == null)
                ii = new ITMISWORDBLADEHOT();
            return ii;
        }


        protected ITMISWORDBLADEHOT()
            : base("ITMISWORDBLADEHOT")
        {
            Name = "Glühende Klinge";
            Visual = "ItMiSwordbladehot.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_METAL;

            CreateItemInstance();
        }
    }
}
