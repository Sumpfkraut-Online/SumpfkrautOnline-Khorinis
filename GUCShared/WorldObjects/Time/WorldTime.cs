using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public struct WorldTime
    {
        public static readonly WorldTime Zero = new WorldTime(0);

        public const int SecondsPerDay = 86400;
        public const int SecondsPerHour = 3600;
        public const int SecondsPerMinute = 60;

        int totalSeconds;

        #region Constructors

        public WorldTime(int totalSeconds)
        {
            this.totalSeconds = totalSeconds;
        }

        public WorldTime(int day, int hour, int minute, int second)
        {
            long totalSeconds = (long)second + (long)minute * SecondsPerMinute + (long)hour * SecondsPerHour + (long)day * SecondsPerDay;
            if (totalSeconds > int.MaxValue)
                throw new OverflowException("TotalSeconds is bigger than " + int.MaxValue);
            else if (totalSeconds < int.MinValue)
                throw new OverflowException("TotalSeconds is smaller than " + int.MinValue);

            this.totalSeconds = (int)totalSeconds;
        }

        public WorldTime(int day, int hour)
            : this(day, hour, 0, 0)
        { }

        public WorldTime(int day, int hour, int minute)
            : this(day, hour, minute, 0)
        {
        }

        #endregion

        #region Get Time Methods

        public float GetTotalDays()
        {
            return this.totalSeconds / (float)SecondsPerDay;
        }

        public float GetTotalHours()
        {
            return this.totalSeconds / (float)SecondsPerHour;
        }

        public float GetTotalMinutes()
        {
            return this.totalSeconds / (float)SecondsPerMinute;
        }

        public int GetTotalSeconds()
        {
            return this.totalSeconds;
        }

        public int GetDay()
        {
            return this.totalSeconds / SecondsPerDay;
        }

        public int GetHour()
        {
            return this.totalSeconds % SecondsPerDay / SecondsPerHour;
        }

        public int GetMinute()
        {
            return this.totalSeconds % SecondsPerDay % SecondsPerHour / SecondsPerMinute;
        }

        public int GetSecond()
        {
            return this.totalSeconds % SecondsPerDay % SecondsPerHour % SecondsPerMinute;
        }
        
        #endregion

        #region Operators

        public static WorldTime operator -(WorldTime t)
        {
            return new WorldTime(-t.totalSeconds);
        }

        public static WorldTime operator ++(WorldTime t)
        {
            return new WorldTime(t.totalSeconds + 1);
        }

        public static WorldTime operator --(WorldTime t)
        {
            return new WorldTime(t.totalSeconds - 1);
        }

        public static WorldTime operator +(WorldTime t1, WorldTime t2)
        {
            return new WorldTime(t1.totalSeconds + t2.totalSeconds);
        }

        public static WorldTime operator +(WorldTime t, int min)
        {
            return new WorldTime(t.totalSeconds + min);
        }

        public static WorldTime operator +(int min, WorldTime t)
        {
            return new WorldTime(min + t.totalSeconds);
        }

        public static WorldTime operator -(WorldTime t1, WorldTime t2)
        {
            return new WorldTime(t1.totalSeconds - t2.totalSeconds);
        }

        public static WorldTime operator -(WorldTime t, int min)
        {
            return new WorldTime(t.totalSeconds - min);
        }

        public static WorldTime operator -(int min, WorldTime t)
        {
            return new WorldTime(min - t.totalSeconds);
        }

        public static WorldTime operator *(WorldTime t, int min)
        {
            return new WorldTime(t.totalSeconds * min);
        }

        public static WorldTime operator *(int min, WorldTime t)
        {
            return new WorldTime(min * t.totalSeconds);
        }

        public static WorldTime operator *(WorldTime t, float min)
        {
            return new WorldTime((int)(t.totalSeconds * min));
        }

        public static WorldTime operator *(float min, WorldTime t)
        {
            return new WorldTime((int)(min * t.totalSeconds));
        }

        public static WorldTime operator /(WorldTime t, int min)
        {
            return new WorldTime(t.totalSeconds / min);
        }

        public static WorldTime operator /(int min, WorldTime t)
        {
            return new WorldTime(min / t.totalSeconds);
        }

        public static WorldTime operator /(WorldTime t, float min)
        {
            return new WorldTime((int)(t.totalSeconds / min));
        }

        public static WorldTime operator /(float min, WorldTime t)
        {
            return new WorldTime((int)(min / t.totalSeconds));
        }

        public static bool operator >(WorldTime t1, WorldTime t2)
        {
            return t1.totalSeconds > t2.totalSeconds;
        }

        public static bool operator <(WorldTime t1, WorldTime t2)
        {
            return t1.totalSeconds < t2.totalSeconds;
        }

        public static bool operator ==(WorldTime t1, WorldTime t2)
        {
            return t1.totalSeconds == t2.totalSeconds;
        }

        public static bool operator !=(WorldTime t1, WorldTime t2)
        {
            return t1.totalSeconds != t2.totalSeconds;
        }

        public static bool operator >=(WorldTime t1, WorldTime t2)
        {
            return t1.totalSeconds >= t2.totalSeconds;
        }

        public static bool operator <=(WorldTime t1, WorldTime t2)
        {
            return t1.totalSeconds <= t2.totalSeconds;
        }

        #endregion

        #region

        public static bool TryParse(string s, out WorldTime time)
        {
            int seconds;
            if (int.TryParse(s, out seconds))
            {
                time = new WorldTime(seconds);
                return true;
            }
            time = default(WorldTime);
            return false;
        }

        public override string ToString()
        {
            return this.totalSeconds.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is WorldTime && (WorldTime)obj == this;
        }

        public override int GetHashCode()
        {
            return this.totalSeconds.GetHashCode();
        }

        #endregion
    }
}
