using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;

namespace GUC.Sumpfkraut.Ingame.GUI
{
    class ItemRenderer
    {
        public static Dictionary<zCView, oCItem> renderList = new Dictionary<zCView, oCItem>();

        static zCWorld rndrWorld = null;
        public static Int32 OnRender(String message)
        {
            try
            {
                if (rndrWorld == null)
                {
                    rndrWorld = zCWorld.Create(Process.ThisProcess());
                    rndrWorld.IsInventoryWorld = true;
                }

                foreach (KeyValuePair<zCView, oCItem> pair in renderList)
                {
                    pair.Value.RenderItem(rndrWorld, pair.Key, 0.0f);
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "ItemRenderer: OnRender", 0);
            }
            return 0;
        }
    }
}
