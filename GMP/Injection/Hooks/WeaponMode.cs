using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Injection;
using Gothic.zTypes;
using Gothic.zClasses;
using GMP.Net.Messages;
using System.Windows.Forms;
using GMP.Modules;

namespace GMP.Injection.Synch
{
    public class WeaponMode
    {
        public static Int32 SetWeaponMode2_Str(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                zString str = new zString(process, process.ReadInt(address + 4));
                new WeaponModeMessage().Write(Program.client.sentBitStream, Program.client,1,str.Value);
            }
            return 0;
        }

        public static Int32 SetWeaponMode2_Int(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                new WeaponModeMessage().Write(Program.client.sentBitStream, Program.client, 2, ""+process.ReadInt(address + 4));
            }
            return 0;
        }

        public static Int32 SetWeaponMode(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                new WeaponModeMessage().Write(Program.client.sentBitStream, Program.client, 3, "" + process.ReadInt(address + 4));
            }
            return 0;
        }
    }
}
