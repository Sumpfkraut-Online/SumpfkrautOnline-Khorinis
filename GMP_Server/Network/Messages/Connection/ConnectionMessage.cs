using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Enumeration;
using RakNet;
using GUC.Server.Network.Messages.VobCommands;

namespace GUC.Server.Network.Messages.Connection
{
    class ConnectionMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            String heroName = "";
            String driveString = "";
            String macString = "";

            stream.Read(out heroName);
            stream.Read(out driveString);
            stream.Read(out macString);


            Player player = new Player(packet.guid, heroName);
            player.DriveString = driveString;
            player.MacString = macString;
            sWorld.addVob(player);

            Write(player);
            CreateVobMessage.Write(player, packet.guid);

            Scripting.GUI.View.SendToPlayer(player);
            Scripting.Objects.Character.Player.OnPlayerConnect((GUC.Server.Scripting.Objects.Character.Player)player.ScriptingNPC);
        }

        public void Write(Player player)
        {
            RakNet.BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            //stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            //stream.Write((byte)NetworkIDS.ConnectionMessage);

            //Writing Player-Informations:
            //stream.Write(player.ID);


            stream.Write(Player.sSendAllKeys);
            stream.Write(Player.sSendKeys.Count);
            foreach (int k in Player.sSendKeys)
            {
                stream.Write(k);
            }


            stream.Write(sWorld.Day);
            stream.Write(sWorld.Hour);
            stream.Write(sWorld.Minute);

            stream.Write(sWorld.WeatherType);
            stream.Write(sWorld.StartRainHour);
            stream.Write(sWorld.StartRainMinute);
            stream.Write(sWorld.EndRainHour);
            stream.Write(sWorld.EndRainMinute);



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
            foreach (NPCProto proto in sWorld.NpcList)
            {
                proto.Write(stream);
            }
            //PlayerList:
            stream.Write(sWorld.PlayerList.Count);
            foreach (NPCProto proto in sWorld.PlayerList)
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
                stream2.Write((byte)NetworkIDS.ConnectionMessage);

                //Writing Player-Informations:
                stream2.Write(player.ID);
                GUC.Types.Zip.Compress(stream, 0, stream2);
                

                using (RakNetGUID guid = player.GUID)
                    Program.server.server.Send(stream2, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
            }
        }
    }
}
