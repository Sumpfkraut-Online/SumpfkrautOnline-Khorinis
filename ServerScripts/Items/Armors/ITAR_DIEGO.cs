using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DIEGO : AbstractArmors
    {
        static ITAR_DIEGO ii;
        public static ITAR_DIEGO get()
        {
            if (ii == null)
                ii = new ITAR_DIEGO();
            return ii;
        }


        protected ITAR_DIEGO()
            : base("ITAR_DIEGO")
        {
            Visual = "ItAr_Diego.3ds";
            Visual_Change = "Armor_Diego.asc";
            Name = "Diegos Rüstung";
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
