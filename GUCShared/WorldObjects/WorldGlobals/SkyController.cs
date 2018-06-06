using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GameObjects;
using GUC.Types;

namespace GUC.WorldObjects.WorldGlobals
{
    public abstract partial class SkyController : GameObject
    {
        #region ScriptObject

        public partial interface IScriptSkyController : IScriptGameObject
        {
            void SetNextWeight(long time, float weight);
            void OnWriteSetWeight(PacketWriter stream);
            void OnReadSetWeight(PacketReader stream);
        }
        
        /// <summary> The ScriptObject of this GameObject. </summary>
        new public IScriptSkyController ScriptObject { get { return (IScriptSkyController)base.ScriptObject; } }

        #endregion

        #region Constructors

        internal SkyController(World world, IScriptSkyController scriptObject) : base(scriptObject)
        {
            this.world = world;
        }

        #endregion

        #region Properties

        World world;
        public World World { get { return world; } }

        long startTime;
        /// <summary> The time in ticks from which the interpolation started. </summary>
        public long StartTime { get { return this.startTime; } }

        float startWeight;
        /// <summary> The weight from which the interpolation started. </summary>
        public float StartWeight { get { return this.startWeight; } }

        long endTime;
        /// <summary> The time in ticks at which the interpolation reaches its EndWeight. </summary>
        public long EndTime { get { return this.endTime; } }
        
        float endWeight;
        /// <summary> The weight reached at the EndTime. </summary>
        public float EndWeight { get { return this.endWeight; } }

        float currentWeight = 0;
        /// <summary> The current interpolated weight. </summary>
        public float CurrentWeight { get { return this.currentWeight; } }

        #endregion

        #region Set Weight
        
        /// <summary> Sets the next interpolated weight and time. </summary>
        /// <param name="ticks"> The point in time when the given weight should be reached. </param>
        /// <param name="weight">[0..1]</param>
        public virtual void SetNextWeight(long ticks, float weight)
        {
            startTime = GameTime.Ticks;
            startWeight = currentWeight;

            endTime = ticks;
            if (weight < 0)
                endWeight = 0;
            else if (weight > 1)
                endWeight = 1;
            else
                endWeight = weight;
        }

        #endregion

        #region Update Weight
        
        internal virtual void UpdateWeight()
        {
            if (endTime != startTime)
            {
                float percent;

                long currentTime = GameTime.Ticks;
                                
                if (currentTime >= endTime)
                {
                    percent = 1;
                }
                else if (currentTime <= startTime)
                {
                    percent = 0;
                } 
                else
                {
                    percent = (float)((double)(currentTime - startTime) / (endTime - startTime));
                }

                this.currentWeight = startWeight + (endWeight - startWeight) * percent;
            }
            else
            {
                this.currentWeight = endWeight;
            }
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            this.endTime = GameTime.Ticks + stream.ReadUInt() * TimeSpan.TicksPerMillisecond;
            this.endWeight = stream.ReadFloat();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write(GetMsecToEnd());
            stream.Write(this.endWeight);
        }

        protected uint GetMsecToEnd()
        {
            long current = GameTime.Ticks;
            if (current >= endTime)
            {
                return 0;
            }
            else
            {
                return (uint)((this.EndTime - current) / TimeSpan.TicksPerMillisecond);
            }
        }

        public void WriteNextWeight(PacketWriter stream)
        {
            stream.Write(GetMsecToEnd());
            stream.Write(this.endWeight);
            this.ScriptObject.OnWriteSetWeight(stream);
        }

        public void ReadSetNextWeight(PacketReader stream)
        {
            this.endTime = GameTime.Ticks + stream.ReadUInt() * TimeSpan.TicksPerMillisecond;
            this.endWeight = stream.ReadFloat();
            this.ScriptObject.OnReadSetWeight(stream);
        }

        #endregion
    }
}
