using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects;
using Gothic.zTypes;

namespace GUC.Hooks
{
    public class hItem
    {
        public static int itemaddr;
        public static Int32 InitByScript(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                itemaddr = Process.ThisProcess().ReadInt(address);

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }

        public static Int32 InitByScript_End(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                oCItem item = new oCItem(process, itemaddr);
                String itemname = item.ObjectName.Value;

                if (itemname.Trim().ToUpper().StartsWith("ITGUC_".ToUpper()))
                {
                    if (!ItemInstance.ItemInstanceDict.ContainsKey(Convert.ToInt32(itemname.Trim().ToUpper().Substring("ITGUC_".Length))))
                        return 0;
                    ItemInstance sitem = (ItemInstance)ItemInstance.ItemInstanceDict[Convert.ToInt32(itemname.Trim().ToUpper().Substring("ITGUC_".Length))];
                    sitem.toItem(item);
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }
    }
}
