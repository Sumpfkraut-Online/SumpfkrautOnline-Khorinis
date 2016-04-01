using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.Time
{
    public partial class WorldClock
    {
        #region ScriptObject
        
        public partial interface IScriptWorldClock
        {
            /// <summary>
            /// Can be used to write additional script properties when "WriteStream" is called.
            /// </summary>
            void OnWriteProperties(PacketWriter stream);

            /// <summary>
            /// Can be used to read the additional script properties written when "ReadStream" is called.
            /// </summary>
            void OnReadProperties(PacketReader stream);

            void SetTime(WorldTime time, float rate);
            void Start();
            void Stop();
        }

        /// <summary>
        /// The ScriptObject of this GameObject.
        /// </summary>
        public IScriptWorldClock ScriptObject = null;

        #endregion

        #region Properties

        WorldTime time = WorldTime.Zero;
        float rate = 1.0f;

        public WorldTime Time { get { return this.time; } }
        public float Rate { get { return this.rate; } }

        long startTicks;

        World world;
        public World World { get { return this.world; } }

        bool running = false;
        public bool Running { get { return this.running; } }

        #endregion

        #region Constructors

        internal WorldClock(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");
            this.world = world;
        }

        #endregion

        #region Start & Stop

        partial void pStart();
        public void Start()
        {
            if (!running)
            {
                running = true;
                SetStartTicks();
                pStart();
            }
        }

        partial void pStop();
        public void Stop()
        {
            if (running)
            {
                running = false;
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

            if (this.running)
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
            if (!running)
                return;
            
            long departedSeconds = (long)((GameTime.Ticks - startTicks) / TimeSpan.TicksPerSecond * (double)rate);
            this.time = new WorldTime((int)departedSeconds);

            pUpdateTime();
        }

        #endregion

        #region Read & Write

        public void ReadStream(PacketReader stream)
        {
            this.time = new WorldTime(stream.ReadInt());
            this.rate = stream.ReadFloat();
            this.ScriptObject.OnReadProperties(stream);
        }

        public void WriteStream(PacketWriter stream)
        {
            stream.Write(this.time.GetTotalSeconds());
            stream.Write(this.rate);
            this.ScriptObject.OnWriteProperties(stream);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("(WorldClock: Day {0}, {1}:{2}:{3}, Rate: {4})", this.time.GetDay(), this.time.GetHour(), this.time.GetMinute(), this.time.GetSecond(), this.rate);
        }
    }
}
