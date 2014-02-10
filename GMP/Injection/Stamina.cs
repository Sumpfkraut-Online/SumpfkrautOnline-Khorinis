using System;
using System.Collections.Generic;
using System.Text;
using Gothic.mClasses;
using WinApi;
using Gothic.zClasses;
using GMP.Modules;
using WinApi.User.Enumeration;
using Gothic.zTypes;
using Injection;
using GMP.Helper;

namespace GMP.Injection
{
    public class Stamina
    {
        public Bar staminaBar;
        public bool run = false;

        public long ticks = 0;
        public long inputTicks = 0;
        public void update()
        {
            if (staminaBar == null)
            {
                staminaBar = new Bar(Process.ThisProcess());
                int[] sizeB = InputHooked.PixelToVirtual(Process.ThisProcess(), new int[] { 180+40, 30});
                staminaBar.setPosition(sizeB[0], 0x2000 - sizeB[1]);
            }

            

            if (ticks + 10000 * 10 <= DateTime.Now.Ticks)
            {
                String replStr = StaticVars.serverConfig.staminaValue.Trim().ToLower();
                replStr = replStr.Replace("dex", "" + oCNpc.Player(Process.ThisProcess()).Dexterity);
                replStr = replStr.Replace("str", "" + oCNpc.Player(Process.ThisProcess()).Strength);

                int max = (int)MathHelper.Evaluate(replStr);
                if (staminaBar.max != max)
                {
                    staminaBar.max = max;
                    staminaBar.value = max;
                }
                if (run)
                {
                    staminaBar.setValue(staminaBar.value - 1);
                    if (staminaBar.value == 0)
                    {
                        inputTicks = DateTime.Now.Ticks;
                        run = false;

                        zString str = zString.Create(Process.ThisProcess(), "HUMANS_SPRINT.MDS");
                        oCNpc.Player(Process.ThisProcess()).RemoveOverlay(str);
                        str.Dispose();
                    }
                }
                else
                {
                    if (staminaBar.max > staminaBar.value)
                    {
                        staminaBar.setValue(staminaBar.value + 1);
                    }
                }
                ticks = DateTime.Now.Ticks;
            }
        }

        public void updateInput()
        {
            if (StaticVars.Ingame && InputHooked.IsPressed((int)Program.clientOptions.keySprint) && run == false
                && inputTicks + 10000*1000*10 <= DateTime.Now.Ticks)
            {
                zString str = zString.Create(Process.ThisProcess(), "HUMANS_SPRINT.MDS");
                oCNpc.Player(Process.ThisProcess()).ApplyOverlay(str);
                str.Dispose();

                staminaBar.Show();
                run = true;
            }
            else if (run == true && !InputHooked.IsPressed((int)Program.clientOptions.keySprint))
            {

                zString str = zString.Create(Process.ThisProcess(), "HUMANS_SPRINT.MDS");
                oCNpc.Player(Process.ThisProcess()).RemoveOverlay(str);
                str.Dispose();

                
                run = false;
            }
            else if (run == false && staminaBar.value == staminaBar.max)
            {
                staminaBar.Hide();
            }
        }
    }
}
