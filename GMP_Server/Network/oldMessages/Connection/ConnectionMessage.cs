using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Enumeration;
using RakNet;
using GUC.Server.Network.Messages.VobCommands;
using GUC.Types;

using GUC.Network;

namespace GUC.Server.Network.Messages.Connection
{
    class ConnectionMessage : IMessage
    {
        public void Read(BitStream stream, Client client)
        {
            String driveString = "";
            String macString = "";

            stream.Read(out driveString);
            stream.Read(out macString);

            client.CheckValidity(driveString, macString);
        }

        public static void Write(Client client)
        {
            RakNet.BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            //stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            //stream.Write((byte)NetworkIDS.ConnectionMessage);

            //Writing Player-Informations:
            //stream.Write(player.ID);
            
            stream.Write(sWorld.Day);
            stream.Write(sWorld.Hour);
            stream.Write(sWorld.Minute);

            stream.Write(sWorld.WeatherType);
            stream.Write(sWorld.StartRainHour);
            stream.Write(sWorld.StartRainMinute);
            stream.Write(sWorld.EndRainHour);
            stream.Write(sWorld.EndRainMinute);


            //Writing Spells:
            stream.Write((short)Spell.SpellList.Count);
            foreach (Spell spell in Spell.SpellList)
            {
                spell.Write(stream);
            }

            //Writing created item instances:
            stream.Write((short)ItemInstance.ItemInstanceList.Count);
            foreach (ItemInstance ii in ItemInstance.ItemInstanceList)
            {
                ii.Write(stream);
            }

            //ItemList:
            stream.Write(sWorld.ItemList.Count);
            foreach (Item item in sWorld.ItemList)
            {
                item.Write(stream);
            }

            //VobList:
            stream.Write(sWorld.VobList.Count);
            foreach (Vob vob in sWorld.VobList)
            {
                stream.Write((int)vob.VobType);
                vob.Write(stream);
            }

            //NPCList:
            stream.Write(sWorld.NpcList.Count);
            foreach (NPC proto in sWorld.NpcList)
            {
                proto.Write(stream);
            }
            //PlayerList:
            stream.Write(sWorld.PlayerList.Count);
            foreach (NPC proto in sWorld.PlayerList)
            {
                proto.Write(stream);
            }

            //World-SpawnList:
            stream.Write(sWorld.WorldDict.Count);
            foreach (KeyValuePair<String, World> worldPair in sWorld.WorldDict)
            {
                worldPair.Value.Write(stream);
            }

            //System.IO.File.WriteAllBytes("test.stream", stream.GetData());

            using (BitStream stream2 = new BitStream())
            {
                stream2.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream2.Write((byte)NetworkID.ConnectionMessage);

                GUC.Types.Zip.Compress(stream, 0, stream2);          

                using (RakNetGUID guid = new RakNetGUID(client.guid))
                    Program.server.ServerInterface.Send(stream2, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
            }
        }
    }
}
