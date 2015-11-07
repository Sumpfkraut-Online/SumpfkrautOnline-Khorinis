using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using GUC.Client.WorldObjects;
using GUC.Enumeration;
using RakNet;
using Gothic.zTypes;

namespace GUC.Client.Hooks
{
    public class hMob
    {
        public static void AddHooks(Process process)
        {
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMob).GetMethod("hook_MobGetName"), 0x71BC30, 7, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMob).GetMethod("hook_MobGetName_Set"), 0x71BCF6, 6, 0);

            process.Write(new byte[] { 0xE9, 0x7B, 0xD2, 0xFF, 0xFF }, 0x723CC0); // so oCMobLockable doesn't check whether we have the skills
            process.Write(new byte[] { 0xE9, 0x1B, 0xB5, 0xFF, 0xFF }, 0x723CF0); // oCMobLockable::Interact -> oCMobInter::Interact
        }

        #region Mob Focus Name

        //Hooks entry point of oCMob::GetName and saves the current mob object from ecx
        static Mob mob = null;
        public static Int32 hook_MobGetName(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);

                AbstractVob vob = null;
                if (World.vobAddr.TryGetValue(Program.Process.ReadInt(address), out vob) && vob is Mob)
                {
                    mob = (Mob)vob;
                }
                else
                {
                    mob = null;
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', ex.ToString(), 0, "hMob.cs", 0);
            }
            return 0;
        }

        //Hooks the end of oCMob::GetName and writes our own focus name from the current mob object into eax
        static zString mobName = zString.Create(Program.Process, "");
        public static Int32 hook_MobGetName_Set(String message)
        {
            try
            {
                if (mob != null)
                {
                    int address = Convert.ToInt32(message);
                    mobName.Set(mob.FocusName);
                    Program.Process.Write(mobName.Address, address + 4);
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', ex.ToString(), 0, "hMob.cs", 0);
            }
            return 0;
        }
        #endregion
    }
}
