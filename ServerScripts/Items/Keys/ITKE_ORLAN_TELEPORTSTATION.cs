using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_ORLAN_TELEPORTSTATION : AbstractKeys
    {
        static ITKE_ORLAN_TELEPORTSTATION ii;
        public static ITKE_ORLAN_TELEPORTSTATION get()
        {
            if (ii == null)
                ii = new ITKE_ORLAN_TELEPORTSTATION();
            return ii;
        }


        protected ITKE_ORLAN_TELEPORTSTATION()
            : base("ITKE_ORLAN_TELEPORTSTATION")
        {
            Visual = "ItKe_Key_02.3ds";
            Description = "Orlan's Schlüssel";
            CreateItemInstance();
        }
    }
}
