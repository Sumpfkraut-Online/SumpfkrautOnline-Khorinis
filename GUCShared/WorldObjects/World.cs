using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects.WorldGlobals;
using GUC.GameObjects;
using GUC.GameObjects.Collections;
using GUC.Types;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects
{
    public partial class World : IDObject
    {
        #region ScriptObject

        public partial interface IScriptWorld : IScriptGameObject
        {
        }

        public new IScriptWorld ScriptObject { get { return (IScriptWorld)base.ScriptObject; } }

        #endregion

        #region Constructors

        public World(WorldClock.IScriptWorldClock scriptClock, WeatherController.IScriptWeatherController scriptWeatherController, 
            BarrierController.IScriptBarrierController scriptBarrierController, IScriptWorld scriptObject) : base(scriptObject)
        {
            this.clock = new WorldClock(this, scriptClock);
            this.weatherCtrl = new WeatherController(this, scriptWeatherController);
            this.barrierCtrl = new BarrierController(this, scriptBarrierController);
        }

        #endregion

        #region Properties

        /// <summary> This is always false for Worlds. Will throw a NotSupportedException when set! </summary>
        public override bool IsStatic
        {
            get { return false; }
            set { throw new NotSupportedException("Worlds can only be dynamic!"); }
        }

        WorldClock clock;
        /// <summary> Controls the time. </summary>
        public WorldClock Clock { get { return this.clock; } }

        WeatherController weatherCtrl;
        /// <summary> Controls the wheather. </summary>
        public WeatherController WeatherCtrl { get { return this.weatherCtrl; } }

        BarrierController barrierCtrl;
        /// <summary> Controls the barrier. </summary>
        public BarrierController BarrierCtrl { get { return this.barrierCtrl; } }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            this.Clock.ReadStream(stream);
            this.WeatherCtrl.ReadStream(stream);
            this.BarrierCtrl.ReadStream(stream);
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            this.Clock.WriteStream(stream);
            this.WeatherCtrl.WriteStream(stream);
            this.BarrierCtrl.WriteStream(stream);
        }

        #endregion

        #region Collections

        #region World Collection

        static StaticCollection<World> worldsByID = new StaticCollection<World>(); // all the worlds accessible by ID
        static DynamicCollection<World> worlds = new DynamicCollection<World>(); // all worlds for fast foreach loops

        /// <summary> Checks whether this object is added to the static World collection. </summary>
        public bool IsCreated { get { return this.isCreated; } }

        #region Create & Delete

        /// <summary> Adds this object to the static World collection. </summary>
        public void Create()
        {
            if (this.isCreated)
                throw new ArgumentException("World is already in the collection!");

            worldsByID.Add(this);
            worlds.Add(this, ref this.collID);

            this.isCreated = true;
        }

        partial void pBeforeDelete();
        /// <summary> Removes this object from the static World collection. </summary>
        public void Delete()
        {
            if (!this.isCreated)
                throw new ArgumentException("World is not in the collection!");

            pBeforeDelete();

            this.isCreated = false;

            worldsByID.Remove(this);
            worlds.Remove(ref this.collID);
        }

        #endregion

        #region Access

        /// <summary> Gets a World by ID from the static World collection. </summary>
        public static bool TryGetWorld(int id, out World world)
        {
            return worldsByID.TryGet(id, out world);
        }

        /// <summary> Loops through all Worlds in the static World collection. </summary>
        public static void ForEach(Action<World> action)
        {
            worlds.ForEach(action);
        }

        /// <summary> 
        /// Loops through all Worlds in the static World collection. 
        /// Let the predicate return FALSE to BREAK the loop.
        /// </summary>
        public static void ForEachPredicate(Predicate<World> predicate)
        {
            worlds.ForEachPredicate(predicate);
        }

        /// <summary> Gets the count of Worlds in the static World collection. </summary>
        public static int Count { get { return worlds.Count; } }

        #endregion

        internal static void UpdateWorlds(long now)
        {
            worlds.ForEach(w => w.OnTick(now));
        }

        #endregion

        #region Vob Collection

        StaticCollection<GUCBaseVobInst> vobsByID = new StaticCollection<GUCBaseVobInst>(); // all the vobs accessible by ID
        DynamicCollection<GUCBaseVobInst> vobs = new DynamicCollection<GUCBaseVobInst>(); // all vobs for fast foreach loops

        #region Add & Remove

        partial void pBeforeAddVob(GUCBaseVobInst vob);
        partial void pAfterAddVob(GUCBaseVobInst vob);
        internal void AddVob(GUCBaseVobInst vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (vob.IsSpawned)
                throw new ArgumentException("Vob is already in a world!");

            if (vob.Instance == null)
                throw new ArgumentException("Vob has no instance!");

            pBeforeAddVob(vob);

            vobsByID.Add(vob); // sets or checks the vob ID on the server
            vobs.Add(vob, ref vob.collID);

            pAfterAddVob(vob);
        }

        partial void pBeforeRemoveVob(GUCBaseVobInst vob);
        partial void pAfterRemoveVob(GUCBaseVobInst vob);
        internal void RemoveVob(GUCBaseVobInst vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (vob.World != this)
                throw new ArgumentException("Vob is not in this world!");

            pBeforeRemoveVob(vob);

            vobsByID.Remove(vob); // sets the vob ID to -1 on the server!!!
            vobs.Remove(ref vob.collID);

            pAfterRemoveVob(vob);
        }

        #endregion

        #region Access

        /// <summary> Gets a vob by ID from this world. </summary>
        public bool TryGetVob(int id, out GUCBaseVobInst vob)
        {
            return vobsByID.TryGet(id, out vob);
        }

        /// <summary> Gets a vob of the specific type by ID from this world. </summary>
        public bool TryGetVob<T>(int id, out T vob) where T : GUCBaseVobInst
        {
            return vobsByID.TryGet<T>(id, out vob);
        }

        /// <summary> Loops through all vobs in this world. </summary>
        public void ForEachVob(Action<GUCBaseVobInst> action)
        {
            vobs.ForEach(action);
        }

        /// <summary> Loops through all vobs in this world. The predicate can return FALSE to BREAK the loop. Else return true. </summary>
        public void ForEachVobPredicate(Predicate<GUCBaseVobInst> predicate)
        {
            vobs.ForEachPredicate(predicate);
        }

        /// <summary> Gets the count of vobs in this world. </summary>
        public int VobCount { get { return vobs.Count; } }

        #endregion

        #endregion

        #endregion

        #region OnTick

        partial void pOnTick(long now);
        internal void OnTick(long now)
        {
            this.Clock.UpdateTime();
            this.WeatherCtrl.UpdateWeight();
            this.BarrierCtrl.UpdateWeight();
            this.vobs.ForEach(v => v.OnTick(now));
            pOnTick(now);
        }

        #endregion
    }
}
