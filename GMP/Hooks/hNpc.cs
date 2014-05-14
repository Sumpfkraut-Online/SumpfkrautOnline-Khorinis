using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zStruct;
using GUC.Network.Messages.PlayerCommands;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using Gothic.zTypes;
using GUC.WorldObjects.Mobs;

namespace GUC.Hooks
{
    public class hNpc
    {
        public static bool blockSending = false;
        public static Int32 OnDamage_DD(String message)
        {
            if (blockSending)
            {
                blockSending = false;
                return 0;
            }
            Process Process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);

                oCNpc npc = new oCNpc(Process, Process.ReadInt(address));
                oSDamageDescriptor oDD = new oSDamageDescriptor(Process, Process.ReadInt(address + 4));

                if (oDD.DamageType == oSDamageDescriptor.DamageTypes.DAM_FALL && oCNpc.Player(Process).Address != npc.Address)
                    return 0;

                OnDamageMessage.Write(oDD, npc);

                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "OnDamage: TotalDamage:" + oDD.DamageTotal + " | Damage-Mode: " + oDD.ModeDamage + " | Mode-Weapon: " + oDD.ModeWeapon + " | " + oDD.Damage + " | " + oDD.DamageEffective + " | " + oDD.DamageReal + " | "+npc.HumanAI.FallDownDistanceY+ " | "+oDD.SpellID, 0, "Program.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Exception: "+ex.Message+" "+ex.StackTrace+" "+ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }


        /// <summary>
        /// Not in use anymore!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 EV_CreateInteractItem(String message)
        {
            Process Process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);

                oCNpc npc = new oCNpc(Process, Process.ReadInt(address));
                oCMsgManipulate oDD = new oCMsgManipulate(Process, Process.ReadInt(address + 4));

                oCMobInter mobinter = npc.GetInteractMob();
                MobInter mobInt = null;
                if (sWorld.SpawnedVobDict.ContainsKey(mobinter.Address))
                {
                    mobInt = (MobInter)sWorld.SpawnedVobDict[mobinter.Address];
                    oDD.InstanceName.Set("ITGUC_" + mobInt.UseWithItem.ID);

                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "MobInter-Setted! " + mobInt.UseWithItem.ID, 0, "Program.cs", 0);
                }
                else
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Not founded: MobInter! " + mobinter.Address, 0, "Program.cs", 0);
                }
                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }








        public static bool dontSend = false;
        public static Int32 DoDropVob(String message)
        {
            if (dontSend)
            {
                dontSend = false;
                return 0;
            }
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            try
            {
                if (oCNpc.Player(process).Address == process.ReadInt(address))
                {
                    oCItem vob = new oCItem(process, process.ReadInt(address + 4));

                    if (!sWorld.SpawnedVobDict.ContainsKey(vob.Address))
                        return 0;
                    Vob sWVob = sWorld.SpawnedVobDict[vob.Address];
                    if (!(sWVob is Item))
                        return 0;

                    Item swItem = (Item)sWVob;
                    Player.Hero.DropItem(swItem);

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.DropItemMessage);
                    stream.Write(Player.Hero.ID);
                    stream.Write(swItem.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hNpc.cs", 0);
                
            }
            return 0;
        }

        public static Int32 DoTakeVob(String message)
        {
            try{
                if (dontSend)
                {
                    dontSend = false;
                    return 0;
                }


                int address = Convert.ToInt32(message);
                Process process = Process.ThisProcess();

                if (oCNpc.Player(process).Address == process.ReadInt(address))
                {
                    oCItem vob = new oCItem(process, process.ReadInt(address + 4));

                    if (!sWorld.SpawnedVobDict.ContainsKey(vob.Address))
                        return 0;
                
                    Vob sWVob = sWorld.SpawnedVobDict[vob.Address];
                    if (!(sWVob is Item))
                        return 0;
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "TakeVob: "+sWVob.ID+", "+vob.Address+" | "+sWVob.Address, 0, "Program.cs", 0);

                    Item swItem = (Item)sWVob;
                    Player.Hero.TakeItem(swItem);

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.TakeItemMessage);
                    stream.Write(Player.Hero.ID);
                    stream.Write(swItem.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }



        public static bool blockSendEquip = false;
        public static Int32 oCNpc_EquipItem(String message)
        {
            if (blockSendEquip)
            {
                blockSendEquip = false;
                return 0;
            }
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);
                oCNpc npc = new oCNpc(process, process.ReadInt(address));
                oCItem item = new oCItem(process, process.ReadInt(address + 4));
                
                
                if (npc.Address != Player.Hero.Address)
                    return 0;
                if (item.Address == 0 || !sWorld.SpawnedVobDict.ContainsKey(item.Address))
                    return 0;
                if (npc.Address == 0 || !sWorld.SpawnedVobDict.ContainsKey(npc.Address))
                    return 0;

                NPCProto npcP = (NPCProto)sWorld.SpawnedVobDict[npc.Address];
                Item itemP = (Item)sWorld.SpawnedVobDict[item.Address];

                npcP.EquippedList.Add(itemP);

                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.EquipItemMessage);
                stream.Write(npcP.ID);
                stream.Write(itemP.ID);
                stream.Write(true);
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Equip Item : " + item.Name.Value, 0, "ItemSynchro.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }

        public static bool blockSendUnEquip = false;
        public static Int32 oCNpc_UnEquipItem(String message)
        {
            if (blockSendUnEquip)
            {
                blockSendUnEquip = false;
                return 0;
            }
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);
                oCNpc npc = new oCNpc(process, process.ReadInt(address));
                oCItem item = new oCItem(process, process.ReadInt(address + 4));


                if (npc.Address != Player.Hero.Address)
                    return 0;
                if (item.Address == 0 || !sWorld.SpawnedVobDict.ContainsKey(item.Address))
                    return 0;
                if (npc.Address == 0 || !sWorld.SpawnedVobDict.ContainsKey(npc.Address))
                    return 0;

                NPCProto npcP = (NPCProto)sWorld.SpawnedVobDict[npc.Address];
                Item itemP = (Item)sWorld.SpawnedVobDict[item.Address];

                npcP.EquippedList.Remove(itemP);

                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.EquipItemMessage);
                stream.Write(npcP.ID);
                stream.Write(itemP.ID);
                stream.Write(false);
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "UnEquip Item : " + item.Name.Value, 0, "ItemSynchro.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }

        public static Int32 oCNpc_UseItem(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);
                oCItem item = new oCItem(process, process.ReadInt(address + 4));
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item : " + item.Name.Value, 0, "ItemSynchro.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
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
                oCMsgManipulate manipulation = new oCMsgManipulate(process, ItemMessage);

                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Item UseItem: "+item.Name+" "+manipulation.InstanceName, 0, "ItemSynchro.cs", 0);

                if (item.Address != 0 && item.ObjectName.Address != 0 && item.ObjectName.Value.Trim().Length != 0)
                {
                    
                    
                    //for (int i = 0; i < 200; i++)
                    //{
                    //    int id = process.ReadInt(ItemMessage + i);//0x6C
                    //    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item : " + i + " | :" + id, 0, "ItemSynchro.cs", 0);
                    //}
                }


            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }

        static oCNpc UseItemNPC = null;
        public static Int32 oCNpc_EV_UseItemToState(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                int address = Convert.ToInt32(message);

                int ItemMessage = process.ReadInt(address + 4);

                oCNpc npc = new oCNpc(process, process.ReadInt(address));
                oCItem item = new oCItem(process, process.ReadInt(ItemMessage + 0x6C));
                oCMsgManipulate manipulation = new oCMsgManipulate(process, ItemMessage);


                UseItemNPC = npc;
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }

        public static Int32 oCNpc_EV_UseItemToState_CALLFUNC(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                //bool start = (UseItemNPC.InteractItemState == -1 && UseItemNPC.InteractItemTargetState == 0);
                //bool end = (UseItemNPC.InteractItemState == 0 && UseItemNPC.InteractItemTargetState == -1);

                //if (!start && !end)
                //    return 0;

                if (UseItemNPC.Address != Player.Hero.Address)
                    return 0;
                if (UseItemNPC.InteractItem.Address == 0 || !sWorld.SpawnedVobDict.ContainsKey(UseItemNPC.InteractItem.Address))
                    return 0;
                if (UseItemNPC.Address == 0 || !sWorld.SpawnedVobDict.ContainsKey(UseItemNPC.Address))
                    return 0;

                NPCProto npcP = (NPCProto)sWorld.SpawnedVobDict[UseItemNPC.Address];
                Item itemP = (Item)sWorld.SpawnedVobDict[UseItemNPC.InteractItem.Address];

                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.UseItemMessage);
                stream.Write(npcP.ID);
                stream.Write(itemP.ID);
                stream.Write((short)UseItemNPC.InteractItemState);
                stream.Write((short)UseItemNPC.InteractItemTargetState);
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);




                //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Start-UseItem: " + UseItemNPC.ObjectName.Value + ": " + UseItemNPC.InteractItem.Name.Value + " | " + UseItemNPC.InteractItemState+" | "+UseItemNPC.InteractItemTargetState, 0, "ItemSynchro.cs", 0);
                //if (UseItemNPC.InteractItemState == 1)
                //{
                //    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Start-UseItem: "+UseItemNPC.ObjectName.Value+": "+UseItemNPC.InteractItem.Name.Value, 0, "ItemSynchro.cs", 0);
                //}
                //else if (UseItemNPC.InteractItemState == 2)
                //{
                //    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "End-UseItem: " + UseItemNPC.ObjectName.Value + ": " + UseItemNPC.InteractItem.Name.Value, 0, "ItemSynchro.cs", 0);
                //}
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Use Item failure:" + ex.ToString(), 0, "ItemSynchro.cs", 0);
            }
            return 0;
        }



        public static Int32 DoDie(String message)
        {
            Process Process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);

                oCNpc npc = new oCNpc(Process, Process.ReadInt(address));
                oCNpc killer = new oCNpc(Process, Process.ReadInt(address+4));

                if (killer.Address == Player.Hero.Address)//Send it if killer is hero!
                {
                    if (!sWorld.SpawnedVobDict.ContainsKey(npc.Address))
                        throw new Exception("NPC was not found!");
                    if (!sWorld.SpawnedVobDict.ContainsKey(killer.Address))
                        throw new Exception("Killer was not found!");

                    NPCProto proto = (NPCProto)sWorld.SpawnedVobDict[npc.Address];
                    NPCProto killerproto = (NPCProto)sWorld.SpawnedVobDict[killer.Address];

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.DoDieMessage);
                    stream.Write(proto.ID);
                    stream.Write(killerproto.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                }
                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }



        public static Int32 AniCtrl_InitAnimations(String message)
        {
            Process Process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);

                oCAniCtrl_Human aniCtrl = new oCAniCtrl_Human(Process, Process.ReadInt(address));

                int npcAddress = aniCtrl.NPC.Address;

                Vob vob = null;
                sWorld.SpawnedVobDict.TryGetValue(npcAddress, out vob);

                if (vob == null)
                    return 0;

                aniCtrl.WMode = ((NPCProto)vob).WeaponMode;
                

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }

    }
}
