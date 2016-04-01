using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects.Weather
{
    public partial class SkyController
    {
        #region ScriptObject

        public partial interface IScriptSkyController
        {
            /// <summary>
            /// Can be used to write additional script properties when "WriteStream" is called.
            /// </summary>
            void OnWriteProperties(PacketWriter stream);

            /// <summary>
            /// Can be used to read the additional script properties written when "ReadStream" is called.
            /// </summary>
            void OnReadProperties(PacketReader stream);

            void SetRainTime(WorldTime time, float weight);
        }

        /// <summary>
        /// The ScriptObject of this GameObject.
        /// </summary>
        public IScriptSkyController ScriptObject = null;

        #endregion

        #region Properties

        World world;
        public World World { get { return this.world; } }
        
        WorldTime startTime;
        float startWeight;

        WorldTime endTime;
        float endWeight;
        public WorldTime TargetTime { get { return this.endTime; } }
        public float TargetWeight { get { return this.endWeight; } }

        float currentWeight = 0;
        public float CurrentWeight { get { return this.currentWeight; } }

        #endregion

        internal SkyController(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            this.world = world;
        }

        partial void pSetRainTime();
        public void SetRainTime(WorldTime time, float weight)
        {
            startTime = world.Clock.Time;
            startWeight = currentWeight;

            endTime = time;
            if (weight < 0)
                endWeight = 0;
            else if (weight > 1)
                endWeight = 1;
            else
                endWeight = weight;

            pSetRainTime();
        }

        partial void pUpdateWeather();
        internal void UpdateWeather()
        {
            if (endTime != startTime)
            {
                float percent;

                WorldTime currentTime = world.Clock.Time;
                if (currentTime > this.endTime)
                {
                    percent = 1;
                }
                
                else if (currentTime < this.startTime)
                {
                    percent = 0;
                }
                else
                {
                    percent = (float)((double)(currentTime - this.startTime).GetTotalSeconds() / (endTime - this.startTime).GetTotalSeconds());
                }

                this.currentWeight = startWeight + (endWeight - startWeight) * percent;
            }
            else
            {
                this.currentWeight = endWeight;
            }

            pUpdateWeather();
        }

        #region Read & Write

        public void ReadStream(PacketReader stream)
        {
            this.endTime = new WorldTime(stream.ReadInt());
            this.endWeight = stream.ReadFloat();
            this.ScriptObject.OnReadProperties(stream);
        }

        public void WriteStream(PacketWriter stream)
        {
            stream.Write(this.endTime.GetTotalSeconds());
            stream.Write(this.endWeight);
            this.ScriptObject.OnWriteProperties(stream);
        }

        #endregion
    }
}
