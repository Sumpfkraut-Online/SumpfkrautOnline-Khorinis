using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_KEY_02 : AbstractKeys
    {
        static ITKE_KEY_02 ii;
        public static ITKE_KEY_02 get()
        {
            if (ii == null)
                ii = new ITKE_KEY_02();
            return ii;
        }


        protected ITKE_KEY_02()
            : base("ITKE_KEY_02")
        {
            Visual = "ItKe_Key_02.3ds";

            CreateItemInstance();
        }
    }
}
