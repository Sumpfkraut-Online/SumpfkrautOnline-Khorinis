using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gothic.mClasses;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GUC.Types;

namespace GUC.Sumpfkraut.GUI
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
                    if (pair.Value != null)
                    {
                        zCVob vob = new zCVob(Process.ThisProcess(), pair.Value.Address);
                        Process.ThisProcess().Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, vob.Address + (int)zCVob.Offsets.lightColorStat);
                        Process.ThisProcess().Write(1.0f, vob.Address + (int)zCVob.Offsets.lightDirectionStat + 8);

                        pair.Value.RenderItem(rndrWorld, pair.Key, 0.0f);

                    }
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "ItemRenderer: OnRender", 0);
            }
            return 0;
        }

        public static GUCMenuText test = null;
        public static Int32 ItemBiasHook(string message)
        {
            Process process = Process.ThisProcess();
            int address = Convert.ToInt32(message);
            zCVob vob = new zCVob(process, process.ReadInt(address));
            zVec3 vec = new zVec3(process, process.ReadInt(address+4));

            if (test == null)
            {
                test = new GUCMenuText("", 10, 10, false);
                test.Show();
            }
            test.Text = vec.X + " " + vec.Y + " " + vec.Z;

            return Process.ThisProcess().THISCALL<IntArg>((uint)vob.Address, (uint)0x0061BB70, new CallValue[] { vec });
        }
    }
}
