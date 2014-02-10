using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using AccountModule.Login;
using Gothic.zClasses;
using WinApi;
using Injection;
using Network;
using GMP.Helper;

namespace AccountModule.Ingame
{
    class StartMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, byte type)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xde);
            stream.Write(AccountMessage.AccountID);
            stream.Write(type);

            client.client.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_RAKNET_GUID, true);
        }

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            

            bool saved;
            stream.Read(out saved);
            if (saved)
            {
                int hp, hp_max, mp, mp_max, str, dex; float posX, posY, posZ;
                int[] talents = new int[22]; int[] hitChances = new int[4];

                stream.Read(out hp);
                stream.Read(out hp_max);
                stream.Read(out mp);
                stream.Read(out mp_max);
                stream.Read(out str);
                stream.Read(out dex);
                stream.Read(out posX);
                stream.Read(out posY);
                stream.Read(out posZ);

                for (int i = 0; i < 22; i++)
                    stream.Read(out talents[i]);
                for (int i = 0; i < 4; i++)
                    stream.Read(out hitChances[i]);

                int count = 0; List<item> itemList = new List<item>();
                stream.Read(out count);
                
                for (int i = 0; i < count; i++)
                {
                    item itm = new item();
                    String code;
                    int amount;
                    stream.Read(out code);
                    stream.Read(out amount);
                    itm.code = code;
                    itm.Amount = amount;
                    itemList.Add(itm);
                }

                Process process = Process.ThisProcess();
                oCNpc npc = oCNpc.Player(process);
                npc.HP = hp;
                npc.HPMax = hp_max;
                npc.MP = mp;
                npc.MPMax = mp_max;
                npc.Strength = str;
                npc.Dexterity = dex;
                npc.TrafoObjToWorld.set(3, posX);
                npc.TrafoObjToWorld.set(7, posY);
                npc.TrafoObjToWorld.set(11, posZ);

                for (int i = 0; i < 22; i++)
                    npc.SetTalentSkill(i+1, talents[i]);
                for (int i = 0; i < 4; i++)
                    npc.SetFightSkill(i+1, hitChances[i]);

                InventoryHelper.RemoveInventory(Program.Player);
                InventoryHelper.InsertToInventory(Program.Player, itemList);
            }

            Write(Program.client.sentBitStream, Program.client, 1);
            
        }
    }
}
