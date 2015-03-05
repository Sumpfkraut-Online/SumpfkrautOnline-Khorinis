using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_FLASK : AbstractMisc
    {
        static ITMI_FLASK ii;
        public static ITMI_FLASK get()
        {
            if (ii == null)
                ii = new ITMI_FLASK();
            return ii;
        }


        protected ITMI_FLASK()
            : base("ITMI_FLASK")
        {
            Name = "Laborwasserflasche";
            Visual = "ItMi_Flask.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_GLAS;

            CreateItemInstance();
        }
    }
}
