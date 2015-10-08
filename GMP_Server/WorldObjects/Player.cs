﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Server.Network;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.WorldObjects
{
    public static class Player
    {
        public delegate void OnEnterWorldHandler(NPC player);
        public static event OnEnterWorldHandler OnEnterWorld; 

        internal static void WriteControl(Client client, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.PlayerControlMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(npc.World.MapName);
            stream.mWrite(npc.pos);
            stream.mWrite(npc.dir);
            //write stats
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'G', client.guid, false);
        }

        internal static void ReadControl(BitStream stream, Client client)
        {
            if (client.mainChar == null) // coming from the log-in menus, first spawn
            {
                client.mainChar = client.character;
                Network.Messages.ConnectionMessage.WriteInstanceTables(client);

                if (OnEnterWorld != null)
                    OnEnterWorld(client.mainChar);
            }

            if (!client.character.Spawned)
                client.character.Spawn(client.character.World);
        }

        internal static void ReadPickUpItem(BitStream stream, Client client)
        {
            Item item;
            client.character.World.ItemDict.TryGetValue(stream.mReadUInt(), out item);
            if (item == null) return;
            
            if (client.character.AddItem(item.Instance,item.Amount))
            {
                item.RemoveFromServer();
            }
        }
    }
}
