using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Injection;
using GMP.Net.Messages;
using GMP.Modules;
using GMP.Network.Messages;
using Gothic.zTypes;

namespace GMP.Injection.Hooks
{
    public class MagSynch
    {
        public static bool isTransform()
        {
            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);
            int key = player.MagBook.GetKeyByID(player.GetActiveSpellNr());
            oCItem item = player.MagBook.SpellItems.get(key);
            if (item.ObjectName.Value.ToUpper().IndexOf("_Trf".ToUpper()) != -1)
                return true;
            else
                return false;
        }
        public static Int32 MagBookStartCastEffect(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);
            if (player.MagBook.Address == process.ReadInt(address))//Magiebuch des spieler^s
            {
                if (isTransform())
                    return 0;
                zCVob vob = new zCVob(process, process.ReadInt(address + 4));
                zVec3 pos = new zVec3(process, process.ReadInt(address + 8));
                int x,y,z;
                byte[] arr = BitConverter.GetBytes(pos.X);
                x = BitConverter.ToInt32(arr, 0);
                arr = BitConverter.GetBytes(pos.Y);
                y = BitConverter.ToInt32(arr, 0);
                arr = BitConverter.GetBytes(pos.Z);
                z = BitConverter.ToInt32(arr, 0);
                new MagSetupMessage().Write(Program.client.sentBitStream, Program.client, vob, 1, x, y, z);
            }

            //MessageBox.Show("Open...");
            return 0;
        }

        public static Int32 MagBookStartInvestEffect(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);
            if (player.MagBook.Address == process.ReadInt(address))//Magiebuch des spieler^s
            {

                zCVob vob = new zCVob(process, process.ReadInt(address + 4));

                new MagSetupMessage().Write(Program.client.sentBitStream, Program.client, vob, 2, process.ReadInt(address + 8), process.ReadInt(address + 12), process.ReadInt(address + 16));
            }

            //System.Windows.Forms.MessageBox.Show("Open...");
            return 0;
        }

        public static Int32 MagBookOpen(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);
            if (player.MagBook.Address == process.ReadInt(address))//Magiebuch des spieler^s
            {
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 8, Program.Player, process.ReadInt(address+4), 0);
            }

            //MessageBox.Show("Open...");
            return 0;
        }

        public static Int32 MagBookClose(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);
            if (player.MagBook.Address == process.ReadInt(address))//Magiebuch des spieler^s
            {
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 9, Program.Player, process.ReadInt(address + 4), 0);
            }

            //MessageBox.Show("Open...");
            return 0;
        }


        static int activeSpellID;
        static long lastSpellSetup;
        public static Int32 Spell_Setup(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oCNpc player = new oCNpc(process, process.ReadInt(address + 4));
            zCVob vob = new zCVob(process, process.ReadInt(address + 8));
            int x = process.ReadInt(address + 12);

            if (oCNpc.Player(process).MagBook.Address == process.ReadInt(address)
                & lastSpellSetup + 10000*200 < DateTime.Now.Ticks)
            {
                if (isTransform())
                    return 0;
                new MagSetupMessage().Write(Program.client.sentBitStream, Program.client, vob,0, x, 0, 0 );
                lastSpellSetup = DateTime.Now.Ticks;
            }
            return 0;
        }




        public static Int32 SpellCast(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();



            if (oCNpc.Player(process).MagBook.Address == process.ReadInt(address))
            {
                if (isTransform())
                    return 0;
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 7, Program.Player, 0, 0);
            }
            return 0;
        }

        public static Int32 Spell_Invest(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();



            if (oCNpc.Player(process).MagBook.Address == process.ReadInt(address))
            {
                if (isTransform())
                    return 0;
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 10, Program.Player, 0, 0);
            }
            return 0;
        }

        public static Int32 oCSpellCast(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();



            if (oCNpc.Player(process).MagBook.GetSpellByID(oCNpc.Player(process).GetActiveSpellNr()).Address == process.ReadInt(address))
            {
                if (isTransform())
                    return 0;
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 11, Program.Player, 0, 0);
            }
            return 0;
        }
    }
}
