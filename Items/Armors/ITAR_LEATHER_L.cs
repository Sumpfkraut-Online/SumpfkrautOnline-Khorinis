using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_LEATHER_L : AbstractArmors
    {
        static ITAR_LEATHER_L ii;
        public static ITAR_LEATHER_L get()
        {
            if (ii == null)
                ii = new ITAR_LEATHER_L();
            return ii;
        }


        protected ITAR_LEATHER_L()
            : base("ITAR_LEATHER_L")
        {
            Visual = "ItAr_Leather_L.3ds";
            Visual_Change = "Armor_Leather_L.asc";
            Name = "Lederrüstung";
            Description = Name;

            ProtectionEdge = 25;
            ProtectionBlunt = 25;
            ProtectionPoint = 20;
            ProtectionFire = 5;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
