using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Network;

namespace GUC.Server.Network.Messages
{
    static class ConnectionMessage
    {
        public static void Read(PacketReader stream, Client client, NPC character)
        {
            String driveString = stream.ReadString();
            String macString = stream.ReadString();

            byte[] npcTableHash = new byte[16];
            stream.Read(npcTableHash, 0, 16);

            byte[] itemTableHash = new byte[16];
            stream.Read(itemTableHash, 0, 16);

            byte[] mobTableHash = new byte[16];
            stream.Read(mobTableHash, 0, 16);

            client.CheckValidity(driveString, macString, npcTableHash, itemTableHash, mobTableHash);
        }

        public static void WriteInstanceTables(Client client)
        {
            if (client.instanceNPCNeeded || client.instanceItemNeeded || client.instanceMobNeeded)
            {
                PacketWriter stream = Program.server.SetupStream(NetworkID.ConnectionMessage);

                if (client.instanceNPCNeeded)
                {
                    stream.Write(true);
                    stream.Write(NPCInstance.Table.data.Length);
                    stream.Write(NPCInstance.Table.data, 0, NPCInstance.Table.data.Length);
                }
                else
                {
                    stream.Write(false);
                }

                if (client.instanceItemNeeded)
                {
                    stream.Write(true);
                    stream.Write(ItemInstance.Table.data.Length);
                    stream.Write(ItemInstance.Table.data, 0, ItemInstance.Table.data.Length);
                }
                else
                {
                    stream.Write(false);
                }

                if (client.instanceMobNeeded)
                {
                    stream.Write(true);
                    stream.Write(MobInstance.Table.data.Length);
                    stream.Write(MobInstance.Table.data, 0, MobInstance.Table.data.Length);
                }
                else
                {
                    stream.Write(false);
                }

                client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'G');
            }
        }
    }
}
