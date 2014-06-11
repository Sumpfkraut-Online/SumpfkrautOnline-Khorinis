using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_PIR_H_ADDON : AbstractArmors
    {
        static ITAR_PIR_H_ADDON ii;
        public static ITAR_PIR_H_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_PIR_H_ADDON();
            return ii;
        }


        protected ITAR_PIR_H_ADDON()
            : base("ITAR_PIR_H_ADDON")
        {
            Visual = "ItAr_PIR_H_ADDON.3ds";
            Visual_Change = "Armor_PIR_H_ADDON.asc";
            Name = "Kapitäns Kleidung";
            Description = Name;

            ProtectionEdge = 60;
            ProtectionBlunt = 60;
            ProtectionPoint = 60;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
