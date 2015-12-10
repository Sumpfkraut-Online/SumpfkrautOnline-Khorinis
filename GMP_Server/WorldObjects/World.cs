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
using GUC.Server.WorldObjects.Collections;

namespace GUC.Server.WorldObjects
{

    public struct IGTime
    {
        public int day;
        public int hour;
        public int minute;

        public IGTime(int hour, int minute)
            : this(0, hour, minute)
        { }

        public IGTime(int day, int hour, int minute)
        {
            this.day = day;
            this.hour = hour;
            this.minute = minute;
        }
    }

    public enum WeatherType : byte
    {
        Normal,
        Rain,
        Snow,
    }

    public class World : ServerObject
    {
        #region Statics

        // All worlds which currently exist on the server
        internal static Dictionary<string, World> sWorldDict = new Dictionary<string, World>();
        public static World GetWorld(string worldName) { World world; sWorldDict.TryGetValue(worldName.ToUpper(), out world); return world; }
        public static IEnumerable<World> GetWorlds() { return sWorldDict.Values; }
        public static int GetWorldCount() { return sWorldDict.Count; }

        #endregion
        
        public string WorldName { get; protected set; }
        public string FileName { get; protected set; }

        public readonly VobCollection Vobs = new VobCollection();

        internal Dictionary<int, Dictionary<int, WorldCell>> netCells = new Dictionary<int, Dictionary<int, WorldCell>>(); // for networking
        internal Dictionary<int, Dictionary<int, NPCCell>> npcCells = new Dictionary<int, Dictionary<int, NPCCell>>(); // small cells containing npcs for distance collections

        // Ingame time
        protected IGTime igTime;
        public IGTime GetIGTime() { return this.igTime; }
        protected Object lock_IGTime = new Object();

        // The weather of the world
        protected WeatherType weatherType;
        public WeatherType GetWeatherType() { return this.weatherType; }
        protected IGTime weatherStartTime;
        public IGTime GetWeatherStartTime() { return this.weatherStartTime; }
        protected IGTime weatherEndTime;
        public IGTime GetWeatherEndTime() { return this.weatherEndTime; }
        protected Object lock_Weather = new Object();

        #region Constructors

        public World(string worldName, string fileName, object scriptObj) : base(scriptObj)
        {
            this.WorldName = worldName.ToUpper();
            this.FileName = fileName.ToUpper();

            igTime = new IGTime();
            igTime.day = 4;
            igTime.hour = 22;
            igTime.minute = 30;

            weatherType = WeatherType.Rain;
            weatherStartTime = new IGTime();
            weatherStartTime.day = 4;
            weatherStartTime.hour = 22;
            weatherStartTime.minute = 30;
            weatherEndTime = new IGTime();
            weatherEndTime.day = 4;
            weatherEndTime.hour = 23;
            weatherEndTime.minute = 30;
        }

        public override void Create()
        {
            if (WorldName == null)
            {
                Log.Logger.logError("World creation failed! WorldName can't be NULL!");
            }
            else if (String.IsNullOrWhiteSpace(FileName))
            {
                Log.Logger.logError("World creation failed! FileName is NULL or empty!");
            }
            else
            {
                World.sWorldDict.Add(this.WorldName, this);
                this.IsCreated = true;
            }
        }

        /// <summary> Deletes all vobs in this world and removes itself from the server. </summary>
        public override void Delete()
        {
            foreach(Vob vob in Vobs.GetAll())
            {
                vob.Delete();
            }
            World.sWorldDict.Remove(this.WorldName);
            this.IsCreated = false;
        }

        #endregion

        #region Spawn

        public void SpawnVob(Vob vob)
        {
            if (vob.IsSpawned)
            {
                if (vob.World != this)
                {
                    vob.Despawn();
                }
            }
            else
            {
                vob.World = this;
                Vobs.Add(vob);
            }
            UpdatePosition(vob, vob is NPC ? ((NPC)vob).client : null);
        }

        public void DespawnVob(Vob vob)
        {
            if (vob.World != this)
                return;

            vob.World = null;
            Vobs.Remove(vob);

            if (vob.cell != null)
            {
                vob.WriteDespawnMessage(vob.cell.SurroundingClients());
                vob.cell.RemoveVob(vob);
            }
        }

        #endregion

        #region WorldCells

        internal void UpdatePosition(Vob vob, Client exclude)
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
            npcCells.TryGetValue(x, out row);
            if (row == null)
            {
                row = new Dictionary<int, NPCCell>();
                npcCells.Add(x, row);
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

        void ChangeCells(Vob vob, int x, int z, Client exclude)
        {
            //create the new cell
            Dictionary<int, WorldCell> row = null;
            netCells.TryGetValue(x, out row);
            if (row == null)
            {
                row = new Dictionary<int, WorldCell>();
                netCells.Add(x, row);
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
                vob.WriteSpawnMessage(newCell.SurroundingClients(exclude));

                //send all vobs in surrounding cells to the player
                if (vob is NPC && ((NPC)vob).isPlayer)
                {
                    foreach (WorldCell c in newCell.SurroundingCells())
                    {
                        foreach (Vob v in c.Vobs.GetAll())
                        {
                            v.WriteSpawnMessage(new Client[1] { ((NPC)vob).client });
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
            /*
            #region vob controlling
            if (vob is NPC && ((NPC)vob).isPlayer)
            {
                foreach (AbstractCtrlVob ctrl in new List<AbstractCtrlVob>(((NPC)vob).client.VobControlledList))
                {
                    ctrl.FindNewController();
                }
                foreach (WorldCell c in newCell.SurroundingCells())
                    foreach (Vob v in c.AllVobs())
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
            */
        }

        void VobChangeDiffCells(Vob vob, WorldCell from, WorldCell to, Client exclude)
        {
            int i, j;
            WorldCell cell;
            Dictionary<int, WorldCell> row;

            for (i = from.x - 1; i <= from.x + 1; i++)
            {
                row = null;
                netCells.TryGetValue(i, out row);
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
                        vob.WriteDespawnMessage(cell.GetClients());

                        //deletion updates for the player
                        if (vob is NPC && ((NPC)vob).isPlayer)
                        {
                            foreach (Vob v in cell.Vobs.GetAll())
                            {
                                v.WriteDespawnMessage(new Client[1] { ((NPC)vob).client });
                            }
                        }
                    }
                }
            }

            for (i = to.x - 1; i <= to.x + 1; i++)
            {
                row = null;
                netCells.TryGetValue(i, out row);
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
                    vob.WriteSpawnMessage(cell.GetClients(exclude));

                    //creation updates for the player
                    if (vob is NPC && ((NPC)vob).isPlayer)
                    {
                        foreach (Vob v in cell.Vobs.GetAll())
                        {
                            v.WriteSpawnMessage(new Client[1] { ((NPC)vob).client });
                        }
                    }
                }
            }
        }

        #endregion

        public IEnumerable<NPC> GetNPCs(Vec3f pos, float range)
        {
            float unroundedMinX = (pos.X - range) / NPCCell.cellSize;
            float unroundedMinZ = (pos.Z - range) / NPCCell.cellSize;

            int minX = (int)(pos.X >= 0 ? unroundedMinX + 0.5f : unroundedMinX - 0.5f);
            int minZ = (int)(pos.Z >= 0 ? unroundedMinZ + 0.5f : unroundedMinZ - 0.5f);

            float unroundedMaxX = (pos.X + range) / NPCCell.cellSize;
            float unroundedMaxZ = (pos.Z + range) / NPCCell.cellSize;

            int maxX = (int)(pos.X >= 0 ? unroundedMaxX + 0.5f : unroundedMaxX - 0.5f);
            int maxZ = (int)(pos.Z >= 0 ? unroundedMaxZ + 0.5f : unroundedMaxZ - 0.5f);

            for (int x = minX; x <= maxX; x++)
            {
                Dictionary<int, NPCCell> row = null;
                npcCells.TryGetValue(x, out row);
                if (row != null)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        NPCCell cell = null;
                        row.TryGetValue(z, out cell);
                        if (cell != null)
                        {
                            foreach (NPC npc in cell.Npcs.GetAll())
                                if (npc.Position.GetDistance(pos) <= range)
                                    yield return npc;
                        }
                    }
                }
            }
        }

        #region World Time

        public void ChangeTime(int day, int hour, int minute)
        {
            ChangeTime(day, hour, minute, true, true, true);
        }

        public void ChangeTime(int day, int hour, int minute,
            bool changeDay, bool changeHour, bool changeMinute)
        {
            /*lock (lock_IGTime)
            {
                // set world time in server
                IGTime newIGTime = new IGTime();
                if (changeDay) { newIGTime.day = day; } else { newIGTime.day = -1; }
                if (changeHour) { newIGTime.hour = hour; } else { newIGTime.hour = -1; }
                if (changeMinute) { newIGTime.minute = minute; } else { newIGTime.minute = -1; }
                this.igTime = newIGTime;
                //Console.WriteLine("++++ " + newIGTime.day + " " + newIGTime.hour + " " + newIGTime.minute);

                // send new world time to clients
                foreach (KeyValuePair<uint, NPC> keyValPair in this.playerDict)
                {
                    Client client = keyValPair.Value.client;
                    PacketWriter stream = Program.server.SetupStream(NetworkID.WorldTimeMessage);

                    stream.Write(this.igTime.day);
                    stream.Write(this.igTime.hour);
                    stream.Write(this.igTime.minute);

                    client.Send(stream, PacketPriority.LOW_PRIORITY,
                        PacketReliability.RELIABLE_ORDERED, 'W');
                }
            }*/
        }

        #endregion

        #region World Weather

        public void ChangeWeather(WeatherType wt, IGTime startTime, IGTime endTime)
        {
            /*lock (lock_Weather)
            {
                // set world weather in server
                this.weatherType = wt;
                this.weatherStartTime = startTime;
                this.weatherEndTime = endTime;
                //Console.WriteLine(String.Format(">>> WT:{0} SD:{1} SH:{2} SM:{3} "
                //    + "ED:{4} EH:{5} EM:{6}", wt, startTime.day, startTime.hour, startTime.minute,
                //    endTime.day, endTime.hour, endTime.minute));

                // send new weather to clients
                foreach (KeyValuePair<uint, NPC> keyValPair in this.playerDict)
                {
                    Client client = keyValPair.Value.client;

                    PacketWriter stream = Program.server.SetupStream(NetworkID.WorldWeatherMessage);
                    stream.Write((byte)wt);
                    //stream.Write(startTime.day);
                    stream.Write((byte)startTime.hour);
                    stream.Write((byte)startTime.minute);
                    //stream.Write(endTime.day);
                    stream.Write((byte)endTime.hour);
                    stream.Write((byte)endTime.minute);

                    client.Send(stream, PacketPriority.LOW_PRIORITY,
                        PacketReliability.RELIABLE_ORDERED, 'W');
                }
            }*/
        }

        #endregion
    }
}
