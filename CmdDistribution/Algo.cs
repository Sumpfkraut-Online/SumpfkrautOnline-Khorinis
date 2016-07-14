using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdDistribution
{
    static class Algo
    {
        public static Action<object> Print;
        
        public class ClientInfo
        {
            public Client cl;
            public int num;

            public ClientInfo(Client cl)
            {
                this.cl = cl;
                this.num = 0;
            }
        }

        public class Cell
        {
            // basic info
            public string id;
            public Client client;
            public List<Vob> vobs;

            public List<ClientInfo> AdjoiningClients = new List<ClientInfo>(9);
        }

        public class Client
        {
            public string ClientID;
            public List<Vob> CmdList = new List<Vob>();

            public int ProbableVobs = 0;
        }

        public static void Do(Cell[,] cells)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // START

            // create a list of all cells with multiple available clients and process all other cells
            List<Cell> connectedCells = CreatedConnectedCellList(cells);
            
            // initial vob assignment
            for (int i = 0; i < connectedCells.Count; i++)
                AssignVobs(connectedCells[i]);
            
            // reshift the cells' vobs until there's no need for it anymore
            for (int loop = 0; loop < 10; loop++) // performance tweak: don't shift too often
            {
                bool changed = false;
                for (int i = 0; i < connectedCells.Count; i++)
                {
                    if (AdjustVobs(connectedCells[i]))
                    {
                        changed = true;
                    }
                }

                if (!changed)
                    break;
            }

            // move the cells' vobs to their clients
            for (int i = 0; i < connectedCells.Count; i++)
            {
                Cell cell = connectedCells[i];
                List<ClientInfo> clients = cell.AdjoiningClients;
                int vobIndex = 0;
                for (int c = clients.Count - 1; c >= 0; c--)
                {
                    ClientInfo ci = clients[c];
                    List<Vob> clientVobList = ci.cl.CmdList;
                    clientVobList.Capacity += ci.num;

                    int max = vobIndex + ci.num;
                    while (vobIndex < max)
                    {
                        clientVobList.Add(cell.vobs[vobIndex]);
                        vobIndex++;
                    }
                    clients.RemoveAt(c);
                }
            }

            // END
            watch.Stop();

            // show some results
            Print(watch.ElapsedTicks);
            foreach (Cell cell in cells)
            {
                if (cell.client != null && cell.client.CmdList.Count > 0)
                    Print("Cell " + cell.id + ": Client " + cell.client.ClientID + " takes " + cell.client.CmdList.Count + " Vobs");
            }
        }

        static List<ClientInfo> lowest = new List<ClientInfo>(9);
        static void AssignVobs(Cell cell)
        {
            List<ClientInfo> list = cell.AdjoiningClients;
            
            int vobCount = cell.vobs.Count;
            while (vobCount > 0) // still vobs to assign
            {
                // put the clients with lowest vob count into a list and return the difference to the next highest vob count
                int difference = FindLowestAndNextHighest(list, lowest);
                                
                if (difference > vobCount / lowest.Count) // next highest vob count is beyond the cell's possibilities
                {
                    for (int i = 0; i < lowest.Count; i++) // split evenly
                    {
                        ClientInfo ci = lowest[i];
                        ci.num = vobCount / (lowest.Count - i);
                        ci.cl.ProbableVobs += ci.num;
                        vobCount -= ci.num;
                    }
                    return;
                }
                else
                {
                    // add as many vobs as the lowest clients need to be even with the next highest
                    for (int i = 0; i < lowest.Count; i++)
                    {
                        ClientInfo ci = lowest[i];
                        ci.num = difference;
                        ci.cl.ProbableVobs += difference;
                        vobCount -= difference;
                    }
                }
            }
        }

        static bool AdjustVobs(Cell cell)
        {
            List<ClientInfo> list = cell.AdjoiningClients;

            bool changed = false;
            for (int i = 0; i < list.Count; i++)
            {
                var client1 = list[i];
                for (int j = i + 1; j < list.Count; j++) // compare all cells
                {
                    var client2 = list[j];

                    // makes it much more exact, but needs much more loops too, as there will be shifted single vobs in the end
                    //float f = (info.cl.ProbableVobs - other.cl.ProbableVobs) / 2.0f;
                    //if (f > 0) f += 0.5f; 
                    //else if (f < 0) f -= 0.5f;

                    int moveVobs = (client1.cl.ProbableVobs - client2.cl.ProbableVobs) / 2;
                    
                    if (moveVobs > client1.num)
                        moveVobs = client1.num;
                    else if (moveVobs < -client2.num)
                        moveVobs = -client2.num;

                    if (moveVobs != 0)
                    {
                        client2.cl.ProbableVobs += moveVobs;
                        client2.num += moveVobs;
                        client1.cl.ProbableVobs -= moveVobs;
                        client1.num -= moveVobs;
                        changed = true;
                    }

                }
            }
            return changed;
        }

        static int FindLowestAndNextHighest(List<ClientInfo> clients, List<ClientInfo> output)
        {
            output.Clear();
            int currentLowest = 0;
            int nextHighest = int.MaxValue;
            for (int i = 0; i < clients.Count; i++)
            {
                ClientInfo ci = clients[i];
                if (output.Count == 0) // no lowest yet, just add it
                {
                    output.Add(ci);
                    currentLowest = ci.cl.ProbableVobs;
                }
                else if (currentLowest > ci.cl.ProbableVobs) // found a new lowest
                {
                    nextHighest = currentLowest;

                    output.Clear();
                    output.Add(ci);
                    currentLowest = ci.cl.ProbableVobs;
                }
                else if (currentLowest == ci.cl.ProbableVobs) // same vob count, add
                {
                    output.Add(ci);
                }
            }
            return nextHighest - currentLowest;
        }

        // Creates a list of cells with multiple available clients.
        // Cells without vobs are ignored.
        // If there is only one client available for a cell, this client is directly assigned with the cell's vobs and the cell is dismissed.
        static List<Cell> CreatedConnectedCellList(Cell[,] cells)
        {
            int rows = cells.GetLength(0);
            int cols = cells.GetLength(1);

            List<Cell> ret = new List<Cell>(cells.Length);
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < cols; y++)
                {
                    Cell cell = cells[x, y];
                    if (cell == null) continue;

                    if (cell.vobs.Count == 0) continue;

                    List<ClientInfo> clients = cell.AdjoiningClients;
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        if (i < 0 || i >= rows) continue;
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (j < 0 || j >= cols) continue;

                            Cell other = cells[i, j];
                            if (other == null) continue;

                            if (other.client != null) // cell has a client in it, add to the list
                            {
                                clients.Add(new ClientInfo(other.client));
                            }
                        }
                    }

                    if (clients.Count > 1) // more than one client available, add to the global list
                    {
                        ret.Add(cell);
                    }
                    else if (clients.Count == 1) // only one client available, assign the vobs directly
                    {
                        Client client = clients[0].cl;
                        client.CmdList.AddRange(cell.vobs);
                        client.ProbableVobs += cell.vobs.Count;

                        clients.Clear();
                    }
                }
            }

            return ret;
        }

    }
}
