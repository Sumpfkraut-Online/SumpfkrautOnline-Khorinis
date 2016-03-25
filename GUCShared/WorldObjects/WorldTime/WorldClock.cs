using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.WorldTime
{
    public partial class WorldClock
    {
        #region Properties

        int day = 0;
        int hour = 8;
        int minute = 0;
        float rate = 1.0f;

        public int Day { get { return this.day; } }
        public int Hour { get { return this.hour; } }
        public int Minute { get { return this.minute; } }
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

        public IgTime GetIgTime()
        {
            return new IgTime(this.day, this.hour, this.minute);
        }

        public void SetDay(int day)
        {
            this.SetTime(day, this.hour, this.minute, this.rate);
        }

        public void SetRate(float rate)
        {
            this.SetTime(this.day, this.hour, this.minute, rate);
        }

        public void SetTime(int hour, int minute)
        {
            this.SetTime(this.day, hour, minute, this.rate);
        }

        partial void pSetTime();
        public void SetTime(int day, int hour, int minute, float rate)
        {
            if (day < 0)
                throw new ArgumentOutOfRangeException("Day must be greater or zero! " + day);
            if (hour < 0 || hour > 23)
                throw new ArgumentOutOfRangeException("Hour is out of range! [0..23] <> " + hour);
            if (minute < 0 || minute > 59)
                throw new ArgumentOutOfRangeException("Minute is out of range! [0..59] <> " + minute);
            if (rate <= 0)
                throw new ArgumentOutOfRangeException("Rate must be greater than zero! " + rate);

            this.day = day;
            this.hour = hour;
            this.minute = minute;
            this.rate = rate;

            if (this.running)
            {
                SetStartTicks();
            }

            pSetTime();
        }

        void SetStartTicks()
        {
            long departedTicks = (long)((TimeSpan.TicksPerMinute * (minute + 24L * hour + 1440L * day)) / (double)rate);
            this.startTicks = GameTime.Ticks - departedTicks;
        }

        #endregion

        #region Update

        partial void pUpdateTime();
        internal void UpdateTime()
        {
            if (!running)
                return;

            long d, m, h;
            long departedMinutes = (long)((double)rate * (GameTime.Ticks - startTicks) / TimeSpan.TicksPerMinute);
            d = departedMinutes / 1440;
            departedMinutes %= 1440;
            h = departedMinutes / 60;
            m = departedMinutes % 60;

            if (d >= int.MaxValue)
            {
                Log.Logger.LogWarning("WorldClock reached maximum days! " + int.MaxValue);
                this.day = int.MaxValue;
            }
            else
            {
                this.day = (int)d;
            }

            this.hour = (int)h;
            this.minute = (int)m;
            pUpdateTime();
        }

        #endregion

        #region Read & Write

        public void ReadProperties(PacketReader stream)
        {
            this.day = stream.ReadInt();
            this.hour = stream.ReadByte();
            this.minute = stream.ReadByte();
            this.rate = stream.ReadFloat();
        }

        public void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.day);
            stream.Write((byte)this.hour);
            stream.Write((byte)this.minute);
            stream.Write(this.rate);
        }

        #endregion

        public override string ToString()
        {
            return String.Format("(WorldTime: {0} Days, {1}:{2})", this.day, this.hour, this.minute);
        }
    }
}
