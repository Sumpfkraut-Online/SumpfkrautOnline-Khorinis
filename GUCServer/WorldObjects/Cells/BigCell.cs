using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Cells
{
    /// <summary>
    /// Big cell which includes dynamic vobs and clients. Is used to transmit surrounding vobs to a client.
    /// </summary>
    class BigCell : WorldCell
    {
        public const int CellSize = 4500;

        class ClientInfo
        {
            public readonly GameClient Client;
            public readonly List<GuidableVob> Vobs;
            public int VobNum;

            public ClientInfo(GameClient client)
            {
                this.Client = client;
                this.Vobs = new List<GuidableVob>();
                this.VobNum = 0;
            }
        }

        #region Vob Guiding

        //static List<NetCell> ChangedCells = new List<NetCell>();
        List<GuidableVob> UnguidedVobs = new List<GuidableVob>();

        public void AddGuidableVob(GuidableVob vob)
        {
            if (vob.Guide != null)
                throw new Exception("Vob still has a guide!");

            this.UnguidedVobs.Add(vob);
            /*if (this.UnguidedVobs.Count == 1)
            {
                ChangedCells.Add(this);
            }*/
        }

        public void RemoveGuidableVob(GuidableVob vob)
        {
            if (vob.Guide != null)
                throw new Exception("Vob still has a guide!");

            this.UnguidedVobs.Remove(vob);
            /*if (this.UnguidedVobs.Remove(vob) && this.UnguidedVobs.Count == 0)
            {
                ChangedCells.Remove(this);
            }*/
        }

        static List<BigCell> cellList = new List<BigCell>(100);
        public static void ProcessGuides(IEnumerable<BigCell> cells)
        {
            foreach (BigCell cell in cells)
            {
                if (cell.surroundingClients.Count > 0)
                {
                    if (cell.UnguidedVobs.Count > 0)
                        AssignUnguidedVobs(cell);

                    cellList.Add(cell);
                }
            }

            Flatten(cellList);

            DistributeVobs(cellList);

            cellList.Clear();
        }

        #region First Assignment

        static List<ClientInfo> lowest = new List<ClientInfo>(20);
        static void AssignUnguidedVobs(BigCell cell)
        {
            List<ClientInfo> list = cell.surroundingClients;

            int vobCount = cell.UnguidedVobs.Count;
            while (vobCount > 0) // still vobs to assign
            {
                // put the clients with lowest vob count into a list and return the difference to the next highest vob count
                int difference = CollectLowestAndDiffToNext(list, ref lowest);

                if (difference > vobCount / lowest.Count) // next highest vob count is beyond the cell's possibilities
                {
                    for (int i = 0; i < lowest.Count; i++) // split evenly
                    {
                        ClientInfo ci = lowest[i];
                        ci.VobNum += vobCount / (lowest.Count - i);
                        ci.Client.ProbableVobs += ci.VobNum;
                        vobCount -= ci.VobNum;
                    }
                    lowest.Clear();
                    return;
                }
                else
                {
                    // add as many vobs as the lowest clients need to be even with the next highest
                    for (int i = 0; i < lowest.Count; i++)
                    {
                        ClientInfo ci = lowest[i];
                        ci.VobNum = difference;
                        ci.Client.ProbableVobs += difference;
                        vobCount -= difference;
                    }
                }
                lowest.Clear();
            }
        }

        static int CollectLowestAndDiffToNext(List<ClientInfo> clients, ref List<ClientInfo> output)
        {
            int currentLowest = 0;
            int nextHighest = int.MaxValue;
            for (int i = 0; i < clients.Count; i++)
            {
                ClientInfo ci = clients[i];
                int ciVobCount = ci.Client.GuidedVobs.Count;

                if (output.Count == 0) // no lowest yet, just add it
                {
                    output.Add(ci);
                    currentLowest = ciVobCount;
                }
                else if (currentLowest > ciVobCount) // found a new lowest
                {
                    nextHighest = currentLowest;

                    output.Clear();
                    output.Add(ci);
                    currentLowest = ciVobCount;
                }
                else if (currentLowest == ciVobCount) // same vob count, add
                {
                    output.Add(ci);
                }
            }
            return nextHighest - currentLowest;
        }

        #endregion

        #region Flattening

        /*static List<NetCell> GetCellNetwork(NetCell first)
        {
            List<NetCell> list = new List<NetCell>() { first };

            for (int i = 0; i < list.Count; i++)
            {
                list[i].ForEachSurroundingCell(cell =>
                {
                    if (!list.Contains(cell))
                        list.Add(cell);
                });

                ChangedCells.Remove(list[i]);
            }

            return list;
        }*/

        const int flattenLoops = 5;
        static void Flatten(List<BigCell> cells)
        {
            for (int loop = 0; loop < flattenLoops; loop++)
            {
                bool changed = false;
                for (int i = 0; i < cells.Count; i++)
                {
                    if (AdjustVobs(cells[i]))
                    {
                        changed = true;
                    }
                }

                if (!changed)
                    break;
            }
        }

        static bool AdjustVobs(BigCell cell)
        {
            List<ClientInfo> list = cell.surroundingClients;

            bool changed = false;
            for (int i = 0; i < list.Count; i++)
            {
                var client1 = list[i];
                for (int j = i + 1; j < list.Count; j++) // compare all clients
                {
                    var client2 = list[j];

                    /*float f = (client1.cl.ProbableVobs - client2.cl.ProbableVobs) / 2.0f;
                    if (f > 0) f += 0.5f;
                    else if (f < 0) f -= 0.5f;
                    int moveVobs = (int)f;*/
                    int moveVobs = (client1.Client.ProbableVobs - client2.Client.ProbableVobs) / 2;

                    if (moveVobs > client1.VobNum)
                        moveVobs = client1.VobNum;
                    else if (moveVobs < -client2.VobNum)
                        moveVobs = -client2.VobNum;

                    if (moveVobs != 0)
                    {
                        client2.Client.ProbableVobs += moveVobs;
                        client2.VobNum += moveVobs;
                        client1.Client.ProbableVobs -= moveVobs;
                        client1.VobNum -= moveVobs;
                        //if (moveVobs > 1 || moveVobs < -1)
                        //    changed = true;
                        changed = true;
                    }

                }
            }
            return changed;
        }

        #endregion

        #region Distribute Vobs

        static void DistributeVobs(List<BigCell> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                BigCell cell = list[i];

                for (int c = 0; c < cell.surroundingClients.Count; c++)
                {
                    ClientInfo ci = cell.surroundingClients[c];

                    int diff = ci.VobNum - ci.Vobs.Count;
                    if (diff > 0) // add vobs
                    {

                    }
                    else if (diff < 0) // remove vobs
                    {

                    }
                }
            }
        }

        #endregion

        #endregion

        public readonly VobTypeCollection<BaseVob> DynVobs = new VobTypeCollection<BaseVob>();

        #region Clients

        List<ClientInfo> surroundingClients = new List<ClientInfo>();

        DynamicCollection<GameClient> clients = new DynamicCollection<GameClient>();

        public void AddClient(GameClient client)
        {
            clients.Add(client, ref client.cellID);
            ForEachSurroundingCell(cell =>
            {
                cell.surroundingClients.Add(new ClientInfo(client));
                //ChangedCells.Add(cell);
            });
        }

        public void RemoveClient(GameClient client)
        {
            clients.Remove(ref client.cellID);
            ForEachSurroundingCell(cell =>
            {
                for (int i = cell.surroundingClients.Count - 1; i >= 0; i--)
                    if (cell.surroundingClients[i].Client == client)
                    {
                        cell.surroundingClients.RemoveAt(i);
                        //ChangedCells.Add(cell);
                        return;
                    }
            });
        }

        public void ForEachClient(Action<GameClient> action)
        {
            clients.ForEach(action);
        }

        public void ForEachClient(Predicate<GameClient> action)
        {
            clients.ForEach(action);
        }

        public int ClientCount { get { return this.clients.Count; } }

        public void ForEachSurroundingClient(Action<GameClient> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = this.surroundingClients.Count - 1; i >= 0; i--)
                action(this.surroundingClients[i].Client);

            /*for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    NetCell cell;
                    if (this.world.TryGetCellFromCoords(x, y, out cell))
                    {
                        cell.clients.ForEach(client => action(client));
                    }
                }
            }*/
        }

        #endregion

        public BigCell(World world, int x, int y) : base(world, x, y)
        {
        }

        public void ForEachSurroundingCell(Action<BigCell> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int x = this.x - 1; x <= this.x + 1; x++)
            {
                for (int y = this.y - 1; y <= this.y + 1; y++)
                {
                    BigCell cell;
                    if (this.world.TryGetCellFromCoords(x, y, out cell))
                    {
                        action(cell);
                    }
                }
            }
        }

        public const int NumSurroundingCells = 9;

        public const int Size = 4500;
        public static int[] GetCoords(Vec3f pos)
        {
            return WorldCell.GetCoords(pos, Size);
        }
    }
}
