using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using GMP.Net.Messages;
using Injection;
using WinApi;
using System.Windows.Forms;
using GMP.Modules;
using GMP.Injection.Hooks;

namespace GMP.Injection.Synch
{
    public class ItemSynchro
    {
        #region HookEvents
        public delegate void UseItemEventHandler(oCItem e);
        public static event UseItemEventHandler OnUseItem;

        public static Int32 oCNpc_UseItem(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);
                oCItem item = new oCItem(process, process.ReadInt(address + 4));
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item : "+item.ObjectName.Value, 0, "ItemSynchro.cs", 0);

                if (ItemSynchro.OnUseItem != null)
                {
                    
                    //Process process = Process.ThisProcess();

                    //int address = Convert.ToInt32(message);
                    //oCItem item = new oCItem(process, process.ReadInt(address + 4));
                    //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item : ", 0, "ItemSynchro.cs", 0);

                    ItemSynchro.OnUseItem(item);
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:"+ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }


        public static Int32 oCNpc_EV_UseItem(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);

                int ItemMessage = process.ReadInt(address + 4);


                oCItem item = new oCItem(process, process.ReadInt(ItemMessage + 0x6C));

                if (item.Address != 0 && item.ObjectName.Address != 0 && item.ObjectName.Value.Trim().Length != 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        int id =  process.ReadInt(ItemMessage + i);//0x6C
                        zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item : " + i + " | :" + id, 0, "ItemSynchro.cs", 0);
                    }

                    //for (int i = 0; i < 200; i++)
                    //{
                    //    oCItem item = new oCItem(process, process.ReadInt(ItemMessage + i));//0x6C
                    //    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item : " + i + " | :" + item.ObjectName.Value, 0, "ItemSynchro.cs", 0);
                    //}
                }
                
                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }

        public static oCItem lastItem;
        public static int i = 0;
        public static Int32 oCNpc_SetInteractItem(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);
                oCNpc npc = new oCNpc(process, process.ReadInt(address));

                if (npc.Address != oCNpc.Player(process).Address)
                    return 0;

                oCItem item = new oCItem(process, process.ReadInt(address + 4));
                
                if (item.Address == 0 || item.ObjectName.Address == 0)
                {
                    i++;



                    lastItem = null;
                }else if (lastItem == null)
                {
                    lastItem = item;
                    //UseItem?
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Set Interact item:" + item.ObjectName.Value, 0, "ItemSynchro.cs", 0);
                }
                
                

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }
        #endregion


        /// <summary>
        /// Hook, wenn Item aus Inventar entfernt wird, z.b. Beim Stehlen oder beim Ausnehmen.
        /// Oder auch wenn Item aus einer Truhe entfernt wird ! 
        /// 
        /// Jetzt in MobSynch.cs
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 oCItemContainer_Remove_2(String message)
        {
            Process process = Process.ThisProcess();
            
            int address = Convert.ToInt32(message);
            oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
            oCItem item = new oCItem(process, process.ReadInt(address + 4));
            int amount = process.ReadInt(address + 8);

            oCNpc player = oCNpc.Player(process);//Spieler
            oCNpc stealNPC = oCNpc.StealNPC(process);//Steal-NPC: Eventuell null bzw. Address == 0
            oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);//Ist es ne Truhe?



            byte type = 0;
            String other = "";

            if (mC.ItemContainer.Address == oIC.Address)
                type = 2;//z.B. Truhe
            if (oIC.Address == process.ReadInt(0x00AB27E0) && stealNPC.Address != 0)//Inventar eines anderen Spielers... Stealinventory
            {
                type = 3;
                other = "Inventar von: " + stealNPC.Name.Value;
            }

            

            zERROR.GetZErr(process).Report(2, 'G', "Removed Item, type:"+type+" Item: "+item.ObjectName.Value+" Amount: "+amount+" Anderes: "+other, 0, "Itemsynchro.cs", 0);

            return 0;
        }



        /// <summary>
        /// Truhe Inhalt hinzufügen!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 oCItemContainer_Insert(String message)
        {
            Process process = Process.ThisProcess();
            
            int address = Convert.ToInt32(message);
            oCItemContainer oIC = new oCItemContainer(process, process.ReadInt(address));
            oCItem item = new oCItem(process, process.ReadInt(address + 4));

            oCNpc player = oCNpc.Player(process);//Spieler
            oCNpc stealNPC = oCNpc.StealNPC(process);//Steal-NPC: Eventuell null bzw. Address == 0
            oCMobContainer mC = new oCMobContainer(process, player.GetInteractMob().Address);//Ist es ne Truhe?



            byte type = 0;
            String other = "";

            if (mC.ItemContainer.Address == oIC.Address)
                type = 2;//z.B. Truhe

            

            zERROR.GetZErr(process).Report(2, 'G', "Insert Item, type:"+type+" Item: "+item.ObjectName.Value+" Anderes: "+other, 0, "Itemsynchro.cs", 0);

            return 0;
        }
        




        public static Int32 DoDropVob(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                oCItem vob = new oCItem(process, process.ReadInt(address + 4));
                new ItemSynchro_DoDrop().Write(Program.client.sentBitStream, Program.client, vob.ObjectName.Value, vob.Amount, 1);
            }
            return 0;
        }

        static bool DoTakeVobIsBlocked = false;
        public static Int32 DoTakeVob(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                if (StaticVars.serverConfig.InventoryLimit != -1)
                {
                    if (oCNpc.Player(process).Inventory.ItemList.size() > StaticVars.serverConfig.InventoryLimit)
                    {
                        BlockDoTakeVob();
                    }
                    else
                    {
                        oCItem vob = new oCItem(process, process.ReadInt(address + 4));
                        new ItemSynchro_DoDrop().Write(Program.client.sentBitStream, Program.client, vob.ObjectName.Value, vob.Amount, 2);
                        UnBlockDoTakeVob();
                    }
                }
                else
                {
                    oCItem vob = new oCItem(process, process.ReadInt(address + 4));
                    new ItemSynchro_DoDrop().Write(Program.client.sentBitStream, Program.client, vob.ObjectName.Value, vob.Amount, 2);
                }


                
            }
            else
            {
                UnBlockDoTakeVob();
            }
            return 0;
        }

        public static void BlockDoTakeVob()
        {
            if (DoTakeVobIsBlocked)
                return;

            byte[] data = new byte[] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90, 0x90 };//68 B8 50 82 00
            Process.ThisProcess().Write(data, 0x007449C6);


            DoTakeVobIsBlocked = true;
        }

        public static void UnBlockDoTakeVob()
        {
            if (!DoTakeVobIsBlocked)
                return;

            byte[] data = new byte[] { 0x6A, 0xFF, 0x68, 0xB8, 0x50, 0x82, 0x00 };//68 B8 50 82 00
            Process.ThisProcess().Write(data, 0x007449C6);

            DoTakeVobIsBlocked = false;
        }




    }
}
