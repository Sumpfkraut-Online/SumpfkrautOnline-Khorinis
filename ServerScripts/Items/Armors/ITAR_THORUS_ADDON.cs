using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_THROUS_ADDON : AbstractArmors
    {
        static ITAR_THROUS_ADDON ii;
        public static ITAR_THROUS_ADDON get()
        {
            if (ii == null)
                ii = new ITAR_THROUS_ADDON();
            return ii;
        }


        protected ITAR_THROUS_ADDON()
            : base("ITAR_THROUS_ADDON")
        {
            Visual = "ItAr_Thorus_ADDON.3ds";
            Visual_Change = "Armor_Thorus_ADDON.asc";
            Name = "Schwere Gardistenrüstung";
            Description = "Rüstung von Raven's Garde";

            ProtectionEdge = 70;
            ProtectionBlunt = 70;
            ProtectionPoint = 70;
            ProtectionFire = 0;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
