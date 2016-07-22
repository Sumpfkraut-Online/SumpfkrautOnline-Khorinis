using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Cells;

namespace GUC.Network.Messages
{
    static class SpectatorMessage
    {
        public static void ReadPos(PacketReader stream, GameClient client)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            Vec3f newPos = stream.ReadCompressedPosition();

            float unroundedX = newPos.X / BigCell.Size;
            float unroundedZ = newPos.Z / BigCell.Size;

            // calculate new cell indices
            int x = (int)(newPos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(newPos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (x < short.MinValue || x > short.MaxValue || z < short.MinValue || z > short.MaxValue)
            {
                throw new Exception("Vob position is out of cell range!");
            }

            int coord = (x << 16) | z & 0xFFFF;

            // Spectator moved to a new cell
            if (client.SpecCell.Coord != coord)
            {
                // Check whether we're at least > 15% inside: 0.5f == between 2 cells
                float xdiff = unroundedX - client.SpecCell.X;
                float zdiff = unroundedZ - client.SpecCell.Y;
                if ((xdiff > 0.65f || xdiff < -0.65f) || (zdiff > 0.65f || zdiff < -0.65f))
                {
                    client.SpecCell.RemoveClient(client);
                    if (client.SpecCell.DynVobs.GetCount() <= 0 && client.SpecCell.ClientCount <= 0)
                        client.SpecWorld.netCells.Remove(client.SpecCell.Coord);
                    var newCell = client.SpecWorld.GetCellFromCoords(x, z);
                    client.ChangeCells(client.SpecCell, newCell);
                    client.SpecCell = newCell;
                    newCell.AddClient(client);
                }
            }

            GameClient.ForEach(c =>
            {
                if (c.fakeClient)
                {
                    newPos = Randomizer.GetVec3fRad(c.specPos, 2000);

                    unroundedX = newPos.X / BigCell.Size;
                    unroundedZ = newPos.Z / BigCell.Size;

                    // calculate new cell indices
                    x = (int)(newPos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
                    z = (int)(newPos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

                    if (x < short.MinValue || x > short.MaxValue || z < short.MinValue || z > short.MaxValue)
                    {
                        throw new Exception("Vob position is out of cell range!");
                    }

                    coord = (x << 16) | z & 0xFFFF;

                    // Spectator moved to a new cell
                    if (c.SpecCell.Coord != coord)
                    {
                        // Check whether we're at least > 15% inside: 0.5f == between 2 cells
                        float xdiff = unroundedX - c.SpecCell.X;
                        float zdiff = unroundedZ - c.SpecCell.Y;
                        if ((xdiff > 0.65f || xdiff < -0.65f) || (zdiff > 0.65f || zdiff < -0.65f))
                        {
                            c.SpecCell.RemoveClient(c);
                            if (c.SpecCell.DynVobs.GetCount() <= 0 && c.SpecCell.ClientCount <= 0)
                                c.SpecWorld.netCells.Remove(c.SpecCell.Coord);
                            var newCell = c.SpecWorld.GetCellFromCoords(x, z);
                            c.ChangeCells(c.SpecCell, newCell);
                            c.SpecCell = newCell;
                            newCell.AddClient(c);
                        }
                    }
                }
            });

            watch.Stop();

            Log.Logger.Log(watch.Elapsed.TotalMilliseconds);
        }
    }
}
