using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_PACKET : AbstractMisc
    {
        static ITMI_PACKET ii;
        public static ITMI_PACKET get()
        {
            if (ii == null)
                ii = new ITMI_PACKET();
            return ii;
        }


        protected ITMI_PACKET()
            : base("ITMI_PACKET")
        {
            Flags = 0;
            Name = "Päckchen";
            Visual = "ItMi_Packet.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_LEATHER;
            ScemeName = "MAPSEALED";

            CreateItemInstance();
        }
    }
}
