using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_PITCH : AbstractMisc
    {
        static ITMI_PITCH ii;
        public static ITMI_PITCH get()
        {
            if (ii == null)
                ii = new ITMI_PITCH();
            return ii;
        }


        protected ITMI_PITCH()
            : base("ITMI_PITCH")
        {
            Name = "Pech";
            Visual = "ItMi_Pitch.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_GLAS;


            CreateItemInstance();
        }
    }
}
