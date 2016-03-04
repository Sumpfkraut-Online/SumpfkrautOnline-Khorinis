using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.Network;
using GUC.Server.Network.Messages;
using GUC.Types;
using GUC.Network;

namespace GUC.Server.WorldObjects
{

    public class World
    {
        // Worlds, hardcoded but whatever
        // new World("SOK-NEWWORLD.ZEN"); rename for release, so the original 
        // is not replaced and SP is still functional!
        private static World newworld = new World("SOK-NEWWORLD.ZEN");
        public static World NewWorld { get { return newworld; } }

        public string MapName { get; protected set; }

        protected IgTime igTime;
        public IgTime GetIgTime() { return this.igTime; }

        protected float igTimeRate;
        public float GetIgTimeRate () { return this.igTimeRate; }

        protected Object lock_IGTime = new Object();

        protected WeatherEvent weatherEvent;
        public WeatherEvent GetWeatherEvent () { return this.weatherEvent; }
        public void SetWeatherEvent (WeatherEvent weatherEvent) { this.weatherEvent = weatherEvent; }

        protected Object lock_Weather = new Object();

        internal Dictionary<int, Dictionary<int, WorldCell>> Cells = new Dictionary<int, Dictionary<int, WorldCell>>();
        internal Dictionary<int, Dictionary<int, NPCCell>> SmallCells = new Dictionary<int, Dictionary<int, NPCCell>>();

        Dictionary<uint, NPC> playerDict = new Dictionary<uint, NPC>();
        Dictionary<uint, NPC> npcDict = new Dictionary<uint, NPC>();
        Dictionary<uint, Item> itemDict = new Dictionary<uint, Item>();
        Dictionary<uint, Vob> vobDict = new Dictionary<uint, Vob>();

        public Dictionary<uint, NPC> PlayerDict { get { return playerDict; } }
        public Dictionary<uint, NPC> NPCDict { get { return npcDict; } }
        public Dictionary<uint, Item> ItemDict { get { return itemDict; } }
        public Dictionary<uint, Vob> VobDict { get { return vobDict; } }

        public World(string mapname)
        {
            MapName = mapname.ToUpper();
            sWorld.WorldDict.Add(MapName, this);

            //NPC scav = NPC.Create("scavenger");
            //scav.DrawnItem = Item.Fists;
            //scav.Spawn(this);

            igTime = new IgTime(1, 6, 30);
            igTimeRate = 0f;

            this.weatherEvent = new WeatherEvent(WeatherEvent.weatherOverride);

            //Vob mob = Vob.Create("forge");
            //mob.Spawn(this, new Types.Vec3f(-200, -100, 200), new Types.Vec3f(0, 0, 1));

            //mob = Vob.Create("stool");
            //mob.Spawn(this, new Types.Vec3f(400, -160, 458), new Types.Vec3f(0, 0, 1));

            //mob = Vob.Create("latern");
            //mob.Spawn(this, new Types.Vec3f(400, -160, 458), new Types.Vec3f(0, 0, 1));
        }

        public NPC GetNpcOrPlayer(uint id)
        {
            if (id == 0)
            {
                return null;
            }

            NPC npc = null;
            playerDict.TryGetValue(id, out npc);
            if (npc != null) return npc;

            npcDict.TryGetValue(id, out npc);
            return npc;
        }

        internal void AddVob(AbstractVob vob)
        {
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    playerDict.Add(vob.ID, (NPC)vob);
                else
                    npcDict.Add(vob.ID, (NPC)vob);
            }
            else if (vob is Item)
            {
                itemDict.Add(vob.ID, (Item)vob);
            }
            else if (vob is Vob)
            {
                vobDict.Add(vob.ID, (Vob)vob);
            }
            vob.World = this;
        }

        internal void RemoveVob(AbstractVob vob)
        {
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    playerDict.Remove(vob.ID);
                else
                    npcDict.Remove(vob.ID);
            }
            else if (vob is Item)
            {
                itemDict.Remove(vob.ID);
            }
            else
            {
                vobDict.Remove(vob.ID);
            }
            vob.World = null;
        }

        #region Spawn
        public void SpawnVob(AbstractVob vob)
        {
            if (vob.World != null)
            {
                vob.World.DespawnVob(vob);
            }
            AddVob(vob);
            UpdatePosition(vob, vob.ClientOrNull);
        }

        public void DespawnVob(AbstractVob vob)
        {
            if (vob.World != this)
            {
                return;
            }

            RemoveVob(vob);
            if (vob.cell != null)
            {
                vob.WriteDespawn(vob.cell.SurroundingClients());
                vob.cell.RemoveVob(vob);
            }
        }

        #endregion

        #region WorldCells

        internal void UpdatePosition(AbstractVob vob, Client exclude)
        {
            if (vob is NPC)
            {
                UpdateSmallCells((NPC)vob);
            }

            float unroundedX = vob.pos.X / WorldCell.cellSize;
            float unroundedZ = vob.pos.Z / WorldCell.cellSize;

            //calculate new cell indices
            int x = (int)(vob.pos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(vob.pos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (vob.cell == null)
            { //Vob has not been in the world yet
                ChangeCells(vob, x, z, exclude);
            }
            else
            {
                //vob moved to a new cell
                if (vob.cell.x != x || vob.cell.z != z)
                {
                    //check whether we're at least > 15% inside: 0.5f == between 2 cells
                    float xdiff = unroundedX - vob.cell.x;
                    float zdiff = unroundedZ - vob.cell.z;
                    if ((xdiff > 0.65f || xdiff < -0.65f) || (zdiff > 0.65f || zdiff < -0.65f))
                    {
                        ChangeCells(vob, x, z, exclude);
                        return;
                    }
                }

                //still in the old cell, updates for everyone!
                VobMessage.WritePosDir(vob.cell.SurroundingClients(exclude), vob);
            }
        }

        void UpdateSmallCells(NPC vob)
        {
            float unroundedX = vob.pos.X / NPCCell.cellSize;
            float unroundedZ = vob.pos.Z / NPCCell.cellSize;

            //calculate new cell indices
            int x = (int)(vob.pos.X >= 0 ? unroundedX + 0.5f : unroundedX - 0.5f);
            int z = (int)(vob.pos.Z >= 0 ? unroundedZ + 0.5f : unroundedZ - 0.5f);

            if (vob.npcCell != null)
            {
                if (vob.npcCell.x == x && vob.npcCell.z == z)
                    return;

                vob.npcCell.Remove(vob);
            }

            //find new cell
            Dictionary<int, NPCCell> row = null;
            SmallCells.TryGetValue(x, out row);
            if (row == null)
            {
                row = new Dictionary<int, NPCCell>();
                SmallCells.Add(x, row);
            }

            NPCCell cell = null;
            row.TryGetValue(z, out cell);
            if (cell == null)
            {
                cell = new NPCCell(this, x, z);
                row.Add(z, cell);
            }

            cell.Add(vob);
        }

        void ChangeCells(AbstractVob vob, int x, int z, Client exclude)
        {
            //create the new cell
            Dictionary<int, WorldCell> row = null;
            Cells.TryGetValue(x, out row);
            if (row == null)
            {
                row = new Dictionary<int, WorldCell>();
                Cells.Add(x, row);
            }

            WorldCell newCell = null;
            row.TryGetValue(z, out newCell);
            if (newCell == null)
            {
                newCell = new WorldCell(this, x, z);
                row.Add(z, newCell);
            }

            if (vob.cell == null)
            {
                if (exclude != null)
                {
                    VobMessage.WritePosDir(new Client[1] { exclude }, vob);
                }
                //Send creation message to all players in surrounding cells
                vob.WriteSpawn(newCell.SurroundingClients(exclude));

                //send all vobs in surrounding cells to the player
                if (vob is NPC && ((NPC)vob).isPlayer)
                {
                    foreach (WorldCell c in newCell.SurroundingCells())
                    {
                        foreach (AbstractVob v in c.AllVobs())
                        {
                            v.WriteSpawn(new Client[1] { ((NPC)vob).client });
                        }
                    }
                }
            }
            else
            {
                VobChangeDiffCells(vob, vob.cell, newCell, exclude);
                vob.cell.RemoveVob(vob);
            }

            newCell.AddVob(vob);

            #region vob controlling
            if (vob is NPC && ((NPC)vob).isPlayer)
            {
                foreach (AbstractCtrlVob ctrl in new List<AbstractCtrlVob>(((NPC)vob).client.VobControlledList))
                {
                    ctrl.FindNewController();
                }
                foreach (WorldCell c in newCell.SurroundingCells())
                    foreach (AbstractVob v in c.AllVobs())
                        if (v is AbstractCtrlVob)
                        {
                            if (v is NPC && ((NPC)v).isPlayer)
                                continue;

                            if (v is AbstractDropVob && !((AbstractDropVob)v).physicsEnabled)
                                continue;

                            ((AbstractCtrlVob)v).FindNewController();
                        }
            }
            else if (vob is AbstractCtrlVob)
            {
                ((AbstractCtrlVob)vob).FindNewController();
            }
            #endregion
        }

        void VobChangeDiffCells(AbstractVob vob, WorldCell from, WorldCell to, Client exclude)
        {
            int i, j;
            WorldCell cell;
            Dictionary<int, WorldCell> row;

            for (i = from.x - 1; i <= from.x + 1; i++)
            {
                row = null;
                Cells.TryGetValue(i, out row);
                if (row == null) continue;

                for (j = from.z - 1; j <= from.z + 1; j++)
                {
                    cell = null;
                    row.TryGetValue(j, out cell);
                    if (cell == null) continue;

                    if (i <= to.x + 1 && i >= to.x - 1 && j <= to.z + 1 && j >= to.z - 1)
                    {
                        //Position updates in shared cells
                        VobMessage.WritePosDir(cell.GetClients(exclude), vob);
                    }
                    else
                    {
                        //deletion updates in old cells
                        vob.WriteDespawn(cell.GetClients());

                        //deletion updates for the player
                        if (vob is NPC && ((NPC)vob).isPlayer)
                        {
                            foreach (AbstractVob v in cell.AllVobs())
                            {
                                v.WriteDespawn(new Client[1] { ((NPC)vob).client });
                            }
                        }
                    }
                }
            }

            for (i = to.x - 1; i <= to.x + 1; i++)
            {
                row = null;
                Cells.TryGetValue(i, out row);
                if (row == null) continue;

                for (j = to.z - 1; j <= to.z + 1; j++)
                {
                    cell = null;
                    row.TryGetValue(j, out cell);
                    if (cell == null) continue;

                    if (i <= from.x + 1 && i >= from.x - 1 && j <= from.z + 1 && j >= from.z - 1)
                        continue;

                    if (exclude != null)
                    {
                        VobMessage.WritePosDir(new Client[1] { exclude }, vob);
                    }

                    //creation updates in the new cells
                    vob.WriteSpawn(cell.GetClients(exclude));

                    //creation updates for the player
                    if (vob is NPC && ((NPC)vob).isPlayer)
                    {
                        foreach (AbstractVob v in cell.AllVobs())
                        {
                            v.WriteSpawn(new Client[1] { ((NPC)vob).client });
                        }
                    }
                }
            }
        }

        #endregion

        public IEnumerable<NPC> GetNPCs(Vec3f pos, float range)
        {
            float unroundedMinX = (pos.X-range) / NPCCell.cellSize;
            float unroundedMinZ = (pos.Z-range) / NPCCell.cellSize;

            int minX = (int)(pos.X >= 0 ? unroundedMinX + 0.5f : unroundedMinX - 0.5f);
            int minZ = (int)(pos.Z >= 0 ? unroundedMinZ + 0.5f : unroundedMinZ - 0.5f);

            float unroundedMaxX = (pos.X + range) / NPCCell.cellSize;
            float unroundedMaxZ = (pos.Z + range) / NPCCell.cellSize;

            int maxX = (int)(pos.X >= 0 ? unroundedMaxX + 0.5f : unroundedMaxX - 0.5f);
            int maxZ = (int)(pos.Z >= 0 ? unroundedMaxZ + 0.5f : unroundedMaxZ - 0.5f);

            for (int x = minX; x <= maxX; x++)
            {
                Dictionary<int, NPCCell> row = null;
                SmallCells.TryGetValue(x, out row);
                if (row != null)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        NPCCell cell = null;
                        row.TryGetValue(z, out cell);
                        if (cell != null)
                        {
                            NPC npc;
                            for (int i = 0; i < cell.NPCList.Count; i++)
                            {
                                npc = cell.NPCList[i];
                                if (npc.Position.GetDistance(pos) <= range)
                                    yield return npc;
                            }
                        }
                    }
                }
            }
        }

        public void ChangeIgTime (IgTime igTime, float igTimeRate, List<NPC> npcList = null)
        {
            lock (lock_IGTime)
            {
                this.igTime = igTime;
                this.igTimeRate = igTimeRate;

                if ((npcList != null) && (npcList.Count > 0))
                {
                    // send update message to a selection of clients
                    foreach (NPC npc in npcList)
                    {
                        BitStream stream = Program.server.SetupStream(NetworkID.WorldTimeMessage);

                        stream.mWrite(this.igTime.day);
                        stream.mWrite((byte) this.igTime.hour);
                        stream.mWrite((byte) this.igTime.minute);
                        stream.mWrite(this.igTimeRate);

                        Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY,
                            PacketReliability.RELIABLE_ORDERED, 'I', npc.client.guid, false);
                    }
                }
                else
                {
                    // send update message to all clients
                    foreach (KeyValuePair<uint, NPC> keyValPair in NewWorld.PlayerDict)
                    {
                        BitStream stream = Program.server.SetupStream(NetworkID.WorldTimeMessage);

                        stream.mWrite(this.igTime.day);
                        stream.mWrite((byte) this.igTime.hour);
                        stream.mWrite((byte) this.igTime.minute);
                        stream.mWrite(this.igTimeRate);

                        Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY,
                            PacketReliability.RELIABLE_ORDERED, 'I', keyValPair.Value.client.guid,
                            false);
                    }
                }
            }
        }

        public void ChangeIgWeather (WeatherEvent weatherEvent, 
            List<NPC> npcList = null)
        {
            lock (lock_Weather)
            {
                // set world weather in server
                this.weatherEvent = weatherEvent;

                if ((npcList != null) && (npcList.Count > 0))
                {
                    // send update message to a selection of clients
                    foreach (NPC npc in npcList)
                    {
                        BitStream stream = Program.server.SetupStream(NetworkID.WorldWeatherMessage);

                        stream.mWrite((byte) weatherEvent.weatherType);
                        stream.mWrite((byte) weatherEvent.startTime.hour);
                        stream.mWrite((byte) weatherEvent.startTime.minute);
                        stream.mWrite((byte) weatherEvent.endTime.hour);
                        stream.mWrite((byte) weatherEvent.endTime.minute);

                        Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY,
                            PacketReliability.RELIABLE_ORDERED, 'I', npc.client.guid, false);
                    }
                }
                else
                {
                    // send update message to all clients
                    foreach (KeyValuePair<uint, NPC> keyValPair in NewWorld.PlayerDict)
                    {
                        BitStream stream = Program.server.SetupStream(NetworkID.WorldWeatherMessage);

                        stream.mWrite((byte) weatherEvent.weatherType);
                        stream.mWrite((byte) weatherEvent.startTime.hour);
                        stream.mWrite((byte) weatherEvent.startTime.minute);
                        stream.mWrite((byte) weatherEvent.endTime.hour);
                        stream.mWrite((byte) weatherEvent.endTime.minute);

                        Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY,
                            PacketReliability.RELIABLE_ORDERED, 'I', keyValPair.Value.client.guid, false);
                    }
                }
            }
        }
    }
}
