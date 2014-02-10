using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GMP.Net;
using Injection;
using GMP.Modules;
using System.Windows.Forms;
using Network;
using GMP.Network.Messages;

namespace GMP.Injection.Synch
{
    public class MobSynch
    {
        public static Int32 oCMobInterByAI(String message)
        {
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "MobInteracted ", 0, "Itemsynchro.cs", 0);
            return 0;
        }

        public static Int32 oCItemContainer_Remove_2(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oCNpc player = oCNpc.Player(process);
            oCNpc stealNPC = oCNpc.StealNPC(process);
            oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
            oCItem item = new oCItem(process, process.ReadInt(address + 4));
            int amount = process.ReadInt(address + 8);

            oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);

            if (oIC.Address == mC.ItemContainer.Address)//Spieler
            {
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 13, new oCMobInter(process, mC.Address), item.ObjectName.Value, amount+"");
            }
            else if (oIC.Address == process.ReadInt(0x00AB27E0) && stealNPC.Address != 0)//Inventar eines anderen Spielers... Stealinventory
            {
                //stealNPC.Name.Value;
                Player npcPlayer = StaticVars.spawnedPlayerDict[stealNPC.Address];
                Player thiefPlayer = StaticVars.spawnedPlayerDict[stealNPC.Address];
                new ItemStealMessage().Write(Program.client.sentBitStream, Program.client, npcPlayer, item.ObjectName.Value, amount);
            }
            //zERROR.GetZErr(process).Report(2, 'G', "Removed Item, Item: " + item.ObjectName.Value + " Amount: " + amount , 0, "Itemsynchro.cs", 0);

            return 0;
        }



        public static Int32 oCItemContainer_Insert(String message)
        {
            
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oCNpc player = oCNpc.Player(process);
            oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
            oCItem item = new oCItem(process, process.ReadInt(address + 4));


            oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);

            if (oIC.Address == mC.ItemContainer.Address)//Spieler
            {
                RenderTasks.RenderTask.add(new RenderTasks.MobSynch_InsertItem(mC.Address, item.ObjectName.Value, item.Amount));
                //new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 14, new oCMobInter(process, mC.Address), item.ObjectName.Value, item.Amount + "");
            }


            return 0;
        }



        public static Int32 OnTrigger(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            zCVob str = new zCVob(process, process.ReadInt(address + 4));
            zCVob str2 = new zCVob(process, process.ReadInt(address + 8));

            
            if (oCNpc.Player(process).Address == str2.Address)
            {
                
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 11, mobInter, str.ObjectName.Value, "");
            }
            return 0;
        }

        public static Int32 OnUnTrigger(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            zCVob str = new zCVob(process, process.ReadInt(address + 4));
            zCVob str2 = new zCVob(process, process.ReadInt(address + 8));


            if (oCNpc.Player(process).Address == str2.Address)
            {

                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 12, mobInter, str.ObjectName.Value, "");
            }
            return 0;
        }

        public static Int32 CallOnStateFunc(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            
            //MessageBox.Show("Start Interaction!");//Ansprechen eines Mobs
            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                String str = mobInter.OnStateFunc.Value;
                //Funktionsname statt id sollte übertragen werden
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 10, mobInter, str, "");
            }
            return 0;
        }


        public static Int32 SetMobBodyState(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();


            //MessageBox.Show("Start Interaction!");//Ansprechen eines Mobs
            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 7, mobInter, "", "" );
            }
            return 0;
        }

        public static Int32 StartInteraction(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();


            //MessageBox.Show("Start Interaction!");//Ansprechen eines Mobs
            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 1, mobInter, "", "");
            }
            return 0;
        }

        public static Int32 StopInteraction(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            
            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            //MessageBox.Show("Stop Interaction!");//Komplettes zurücktreten vom Mob

            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 2, mobInter, "", "");
            }
            return 0;
        }

        public static Int32 StartStateChange(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            
            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));

            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 3, mobInter, "" + process.ReadInt(address + 8), "" + process.ReadInt(address + 12));
            }
            return 0;
        }
        //SetLocked
        public static Int32 PickLock(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            int ch = process.ReadInt(address + 8);

            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 4, mobInter, "" + ch, "");
            }
            return 0;
        }

        //Container -> Open
        public static Int32 Open(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            
            if (oCNpc.Player(process).Address == npc.Address)
            {
                
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 5, mobInter, "", "");
            }
            return 0;
        }

        public static Int32 Close(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
            
            if (oCNpc.Player(process).Address == npc.Address)
            {
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));

                int closefull = 0;
                if (npc.IsInInv(new oCMobLockable(process, mobInter.Address).keyInstance, 1).Address == 0)
                    closefull = 0;
                else
                {
                    new oCMobLockable(process, mobInter.Address).SetLocked(1);
                    closefull = 1;
                }

                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 6, mobInter, closefull+"", "");
            }
            return 0;
        }
    }
}
