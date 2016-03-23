using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects.Cells;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages
{
    static class WorldCellMessage
    {
        public static void Write(NetCell[] newCells, NetCell[] oldCells, int oldVobs, GameClient client)
        {
            if (newCells[0] == null && oldCells[0] == null) return;

            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldCellMessage);

            for (int t = 0; t < (int)VobTypes.Maximum; t++)
            {
                int vobCount = 0;
                for (int i = 0; i < newCells.Length; i++)
                {
                    if (newCells[i] == null)
                        break;

                    vobCount += newCells[i].DynVobs.GetCountOfType((VobTypes)t);
                }

                stream.Write((ushort)vobCount);
                for (int i = 0; i < newCells.Length; i++)
                {
                    if (newCells[i] == null)
                        break;

                    newCells[i].DynVobs.ForEachOfType((VobTypes)t, v => v.WriteStream(stream));
                }
            }

            stream.Write((ushort)oldVobs);
            for (int i = 0; i < oldCells.Length; i++)
            {
                if (oldCells[i] == null)
                    break;
                oldCells[i].DynVobs.ForEach(v => stream.Write((ushort)v.ID));
            }

            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W');
        }
    }
}
