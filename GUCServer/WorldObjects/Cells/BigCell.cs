using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects.Collections;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Cells
{
    /// <summary>
    /// Big cell which includes dynamic vobs and clients. Is used to transmit surrounding vobs to a client.
    /// </summary>
    class BigCell : WorldCell
    {
        public const int CellSize = 2000;

        public BigCell(World world, int x, int y) : base(world, x, y)
        {
        }

        #region Clients

        DynamicCollection<GameClient> clients = new DynamicCollection<GameClient>();

        #region Add & Remove

        public void AddClient(GameClient client)
        {
            clients.Add(client, ref client.cellID);
        }

        public void RemoveClient(GameClient client)
        {
            clients.Remove(ref client.cellID);
        }

        #endregion

        #region Access

        public void ForEachClient(Action<GameClient> action)
        {
            clients.ForEach(action);
        }

        public void ForEachClientPredicate(Predicate<GameClient> predicate)
        {
            clients.ForEachPredicate(predicate);
        }

        public int ClientCount { get { return this.clients.Count; } }

        #endregion

        #endregion

        #region Dynamic vobs

        DynamicCollection<BaseVob> dynVobs = new DynamicCollection<BaseVob>();

        #region Add & Remove

        public void AddDynVob(BaseVob vob)
        {
            dynVobs.Add(vob, ref vob.CellID);
        }

        public void RemoveDynVob(BaseVob vob)
        {
            dynVobs.Remove(ref vob.CellID);
        }

        #endregion

        #region Access

        public void ForEachDynVob(Action<BaseVob> action)
        {
            dynVobs.ForEach(action);
        }

        public void ForEachDynVobPredicate(Predicate<BaseVob> predicate)
        {
            dynVobs.ForEachPredicate(predicate);
        }

        public int DynVobCount { get { return this.dynVobs.Count; } }

        #endregion

        #endregion
                        
        public static Vec2i GetCoords(Vec3f pos)
        {
            return WorldCell.GetCoords(pos, CellSize);
        }
    }
}
