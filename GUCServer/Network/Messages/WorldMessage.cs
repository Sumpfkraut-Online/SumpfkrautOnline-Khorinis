using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Cells;
using GUC.WorldObjects.Time;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Weather;

namespace GUC.Network.Messages
{
    static class WorldMessage
    {
        #region Load Message

        public static void WriteLoadMessage(GameClient client, World world)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.LoadWorldMessage);
            world.WriteStream(stream);
            stream.Write(world.Clock.Running);
            client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, '\0');
        }

        #endregion
        
        #region Cells

        public static void WriteCellMessage(BigCell[] newCells, BigCell[] oldCells, int oldVobs, GameClient client)
        {
            if (newCells[0] == null && (oldCells.Length == 0 || oldCells[0] == null)) return;

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

        #endregion

        #region World Clock

        public static void WriteTimeMessage(WorldClock clock)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldTimeMessage);
            clock.WriteStream(stream);

            clock.World.ForEachClient(client => client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W'));
        }

        public static void WriteTimeStartMessage(WorldClock clock, bool start)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldTimeStartMessage);
            stream.Write(start);

            clock.World.ForEachClient(client => client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W'));
        }

        #endregion

        #region Weather

        public static void WriteWeatherMessage(World world, SkyController skyCtrler)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldWeatherMessage);
            skyCtrler.WriteSetRainTime(stream);
            world.ForEachClient(client => client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W'));
        }

        public static void WriteWeatherTypeMessage(World world, SkyController skyCtrler)
        {
            PacketWriter stream = GameServer.SetupStream(NetworkIDs.WorldWeatherTypeMessage);
            skyCtrler.WriteSetWeatherType(stream);
            world.ForEachClient(client => client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W'));
        }

        #endregion
    }
}
