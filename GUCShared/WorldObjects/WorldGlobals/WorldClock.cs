using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GameObjects;
using GUC.Types;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class WorldClock : GameObject
    {
        #region ScriptObject
        
        public partial interface IScriptWorldClock : IScriptGameObject
        {
            void SetTime(WorldTime time, float rate);
            void Start();
            void Stop();
        }

        /// <summary> The ScriptObject of this GameObject. </summary>
        new public IScriptWorldClock ScriptObject { get { return (IScriptWorldClock)base.ScriptObject; } }

        #endregion

        #region Constructors

        internal WorldClock(World world, IScriptWorldClock scriptObject) : base(scriptObject)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            this.world = world;
        }

        #endregion

        #region Properties

        long startTicks;

        WorldTime time = WorldTime.Zero;
        public WorldTime Time { get { return this.time; } }

        float rate = 1.0f;
        public float Rate { get { return this.rate; } }

        World world;
        public World World { get { return this.world; } }

        bool isRunning = false;
        public bool IsRunning { get { return this.isRunning; } }

        #endregion
        
        #region Start & Stop

        partial void pStart();
        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                SetStartTicks();
                pStart();
            }
        }

        partial void pStop();
        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                pStop();
            }
        }

        #endregion

        #region SetTime

        partial void pSetTime();

        public void SetTime(WorldTime time)
        {
            this.SetTime(time, this.rate);
        }

        public void SetTime(WorldTime time, float rate)
        {
            if (rate <= 0)
            {
                throw new ArgumentOutOfRangeException("Rate must be greater than zero! " + rate);
            }

            this.time = time;
            this.rate = rate;

            if (this.isRunning)
            {
                SetStartTicks();
            }

            pSetTime();
        }

        void SetStartTicks()
        {
            long departedTicks = (long)((TimeSpan.TicksPerSecond * this.time.GetTotalSeconds()) / (double)rate);
            this.startTicks = GameTime.Ticks - departedTicks;
        }

        #endregion

        #region Update

        partial void pUpdateTime();
        internal void UpdateTime()
        {
            if (!isRunning)
                return;
            
            long departedSeconds = (long)((GameTime.Ticks - startTicks) / TimeSpan.TicksPerSecond * (double)rate);
            this.time = new WorldTime((int)departedSeconds);

            pUpdateTime();
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            this.time = new WorldTime(stream.ReadInt());
            this.rate = stream.ReadFloat();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.time.GetTotalSeconds());
            stream.Write(this.rate);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("(WorldClock: Day {0}, {1}:{2}:{3}, Rate: {4})", this.time.GetDay(), this.time.GetHour(), this.time.GetMinute(), this.time.GetSecond(), this.rate);
        }
    }
}
