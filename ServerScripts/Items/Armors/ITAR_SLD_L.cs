using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_SLD_L : AbstractArmors
    {
        static ITAR_SLD_L ii;
        public static ITAR_SLD_L get()
        {
            if (ii == null)
                ii = new ITAR_SLD_L();
            return ii;
        }


        protected ITAR_SLD_L()
            : base("ITAR_SLD_L")
        {
            Visual = "ItAr_Sld_L.3ds";
            Visual_Change = "Armor_Sld_L.asc";
            Name = "Leichte Söldnerrüstung";
            Description = Name;

            ProtectionEdge = 30;
            ProtectionBlunt = 30;
            ProtectionPoint = 30;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
