using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class World : GameObject
    {
        #region ScriptObject

        public partial interface IScriptWorld : IScriptGameObject
        {
        }

        public new IScriptWorld ScriptObject
        {
            get { return (IScriptWorld)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is always false for Worlds.
        /// </summary>
        public override bool IsStatic
        {
            get
            {
                return false;
            }
            set
            {
                throw new Exception("Worlds can only be dynamic!");
            }
        }

        #endregion

        #region Collection

        #region WorldCollection

        static StaticCollection<World> worldsByID = new StaticCollection<World>();
        static DynamicCollection<World> worlds = new DynamicCollection<World>();

        public bool IsCreated { get { return this.isCreated; } }

        #region Create & Delete

        public void Create()
        {
            if (this.isCreated)
                throw new ArgumentException("World is already in the collection!");

            worldsByID.Add(this);
            worlds.Add(this, ref this.collID);

            this.isCreated = true;
        }

        public void Delete()
        {
            if (!this.isCreated)
                throw new ArgumentException("World is not in the collection!");

            worldsByID.Remove(this);
            worlds.Remove(ref this.collID);

            this.isCreated = false;
        }

        #endregion

        #region Access

        public static bool TryGetWorld(int id, out World world)
        {
            return worldsByID.TryGet(id, out world);
        }

        public static void ForEach(Action<World> action)
        {
            worlds.ForEach(action);
        }

        public static int GetCount()
        {
            return worlds.Count;
        }

        #endregion

        #endregion

        #region VobCollection

        StaticCollection<BaseVob> vobsByID = new StaticCollection<BaseVob>();
        DynamicCollection<BaseVob> vobs = new DynamicCollection<BaseVob>();

        #region Add & Remove

        partial void pAddVob(BaseVob vob);
        internal void AddVob(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (vob.IsSpawned)
                throw new ArgumentException("Vob is already in a world!");

            if (vob.Instance == null)
                throw new ArgumentException("Vob has no instance!");

            vobsByID.Add(vob);
            vobs.Add(vob, ref vob.collID);

            Log.Logger.Log("AddVob " + vob.VobType + ": " + vob.ID);

            pAddVob(vob);
        }

        partial void pRemoveVob(BaseVob vob);
        internal void RemoveVob(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (vob.World != this)
                throw new ArgumentException("Vob is not in this world!");

            Log.Logger.Log("RemoveVob " + vob.VobType + ": " + vob.ID);

            pRemoveVob(vob);

            vobsByID.Remove(vob);
            vobs.Remove(ref vob.collID);
        }

        #endregion

        #region Access

        public bool TryGetVob(int id, out BaseVob vob)
        {
            return vobsByID.TryGet(id, out vob);
        }

        public bool TryGetVob<T>(int id, out T vob) where T : BaseVob
        {
            return vobsByID.TryGet(id, out vob);
        }

        public void ForEachVob(Action<BaseVob> action)
        {
            vobs.ForEach(action);
        }

        public int GetVobCount()
        {
            return vobs.Count;
        }

        #endregion

        #endregion

        #endregion
    }
}
