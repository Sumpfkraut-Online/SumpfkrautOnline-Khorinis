using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.Client.WorldObjects;
using Gothic.zTypes;

namespace GUC.Client.Hooks
{
    public class hItem
    {
        public static int itemaddr;
        public static Int32 InitByScript(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                itemaddr = Program.Process.ReadInt(address);
                int instanceID = Program.Process.ReadInt(address + 4);
                //int amount = Program.Process.ReadInt(address + 8);

                oCItem item = new oCItem(Program.Process, itemaddr);
                String instanceName = zCParser.getParser(Program.Process).GetSymbol(instanceID).Name.Value.Trim().ToUpper();

                if (instanceName.StartsWith("ITGUC_"))
                {
                    uint gucID = Convert.ToUInt32(instanceName.Substring(6/*"ITGUC_".Length*/));
                    ItemInstance inst;
                    ItemInstance.InstanceDict.TryGetValue(gucID, out inst);
                    if (inst == null) return 0;

                    inst.InitItem(item);
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', "Hook InitByScript: " + ex.ToString(), 0, "Hooks.hItem.cs", 0);
            }
            return 0;
        }

       /* public static Int32 InitByScript_End(String message)
        {
            try
            {
                oCItem item = new oCItem(Program.Process, itemaddr);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', "Hook InitByScript_End: " + ex.ToString(), 0, "Hooks.hItem.cs", 0);
            }
            return 0;
        }*/
    }
}
