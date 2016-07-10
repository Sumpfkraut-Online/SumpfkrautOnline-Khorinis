using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Objects;
using WinApi;

namespace GUC.GUI
{
    class MenuRenderer
    {
        public static Dictionary<int, zCVob> renderList = new Dictionary<int, zCVob>();

        static zCWorld rndrWorld = zCWorld.Create();

        public static Int32 hook_OnDrawItems(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                int viewaddr = Process.ReadInt(address);
                zCVob vob = null;
                renderList.TryGetValue(viewaddr, out vob);
                if (vob != null)
                {
                    zCView view = new zCView(viewaddr);

                    //add some light
                    Process.Write(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }, vob.Address + zCVob.VarOffsets.lightColorStat);
                    Process.Write(1.0f, vob.Address + zCVob.VarOffsets.lightDirectionStat + 8);
                    //draw the vob
                    (new oCItem(vob.Address)).RenderItem(rndrWorld, view, 0);
                }
            }
            catch (Exception e)
            {
                Log.Logger.LogError("Hook DrawItems: " + e.ToString());
            }
            return 0;
        }

        /*public static Int32 ItemBiasHook(string message)
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
        }*/
    }
}
