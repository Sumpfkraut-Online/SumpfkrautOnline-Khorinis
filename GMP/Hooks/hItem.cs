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
        static zCWorld rndrWorld = null;
        public static Int32 ViewDraw_DrawChildren(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                return 0;


                GUC.WorldObjects.Character.Player pl = GUC.WorldObjects.Character.Player.Hero;
                if (pl == null)
                    return 0;
                
                if (pl.ItemList.Count == 0)
                    return 0;
                
                if (pl.ItemList[0].Address == 0)
                    return 0;
                if (oCGame.Game(process).World.Address == 0)
                    return 0;

                if (rndrWorld == null)
                {
                    rndrWorld = zCWorld.Create(process);
                    rndrWorld.IsInventoryWorld = true;
                }
                oCItem item = new oCItem(process, pl.ItemList[0].Address);

                zCView zV = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
                zV.FillZ = true;
                zV.Blit();

                zCView.GetScreen(Process.ThisProcess()).InsertItem(zV, 0);
                item.RenderItem(rndrWorld, zV, 0.0f);
                zCView.GetScreen(Process.ThisProcess()).RemoveItem(zV);
                zV.Dispose();

                

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "ViewDraw_DrawChildren", 0);
            }
            return 0;
        }



        public static int itemaddr;
        public static Int32 InitByScript(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);
                itemaddr = process.ReadInt(address);
                int parserID = process.ReadInt(address + 4);
                int anz = process.ReadInt(address + 8);

                
                oCItem item = new oCItem(process, itemaddr);
                String itemname = zCParser.getParser(process).GetSymbol(parserID).Name.Value;

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
