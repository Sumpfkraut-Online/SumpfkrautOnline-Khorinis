using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON : AbstractKeys
    {
        static ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON ii;
        public static ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON get()
        {
            if (ii == null)
                ii = new ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON();
            return ii;
        }


        protected ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON()
            : base("ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON")
        {
            Visual = "ItKe_Key_01.3ds";
            CreateItemInstance();
        }
    }
}
