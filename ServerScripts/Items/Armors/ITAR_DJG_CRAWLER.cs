using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DJG_CRAWLER : AbstractArmors
    {
        static ITAR_DJG_CRAWLER ii;
        public static ITAR_DJG_CRAWLER get()
        {
            if (ii == null)
                ii = new ITAR_DJG_CRAWLER();
            return ii;
        }


        protected ITAR_DJG_CRAWLER()
            : base("ITAR_DJG_CRAWLER")
        {
            Visual = "ItAr_Djg_Crawler.3ds";
            Visual_Change = "Armor_Djg_Crawler.asc";
            Name = "Rüstung aus Crawlerplatten";
            Description = Name;

            ProtectionEdge = 70;
            ProtectionBlunt = 70;
            ProtectionPoint = 70;
            ProtectionFire = 15;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
