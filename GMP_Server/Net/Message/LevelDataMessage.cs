using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class LevelDataMessage : Message
    {
        public void Write(RakNet.BitStream stream, Server server, Player pl)
        {
            Console.WriteLine("Level-Data-Message sent");
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.LevelDataMessage);
            stream.Write(Program.World.getItemCount(pl.actualMap));
            stream.Write(Program.World.getContainerCount(pl.actualMap));
            stream.Write(Program.World.getMobCount(pl.actualMap));

            for (int i = 0; i < Program.World.Items.Count; i++)
            {
                if (Player.isSameMap(Program.World.Items[i].world, pl.actualMap))
                {
                    for (int p = 0; p < 3; p++ )
                        stream.Write(Program.World.Items[i].pos[p]);
                    stream.Write(Program.World.Items[i].itm.code);
                    stream.Write(Program.World.Items[i].itm.Amount);
                }
            }
            for (int i = 0; i < Program.World.container.Count; i++)
            {
                if (Player.isSameMap(Program.World.container[i].world, pl.actualMap))
                {
                    stream.Write(Program.World.container[i].name);
                    for (int p = 0; p < 3; p++)
                        stream.Write(Program.World.container[i].pos[p]);
                    stream.Write(Program.World.container[i].itemList.Count);
                    for (int cItm = 0; cItm < Program.World.container[i].itemList.Count; cItm++)
                    {
                        stream.Write(Program.World.container[i].itemList[cItm].code);
                        stream.Write(Program.World.container[i].itemList[cItm].Amount);
                    }
                }
            }

            for (int i = 0; i < Program.World.mobInter.Count; i++)
            {
                if (Player.isSameMap(Program.World.mobInter[i].world, pl.actualMap))
                {
                    stream.Write(Program.World.mobInter[i].name);
                    stream.Write(Program.World.mobInter[i].vobType);
                    for (int p = 0; p < 3; p++)
                        stream.Write(Program.World.mobInter[i].pos[p]);
                    stream.Write(Program.World.mobInter[i].triggered);
                }
            }


            server.server.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, pl.guid, false);

        }

        public override void Read(BitStream stream, Packet packet, Server server)
        {
            Write(stream, server, Player.getPlayerByGuid(packet.guid, Program.playerList));
        }
    }
}
