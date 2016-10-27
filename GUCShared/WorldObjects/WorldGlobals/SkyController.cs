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
            void SetNextWeight(WorldTime time, float weight);
            void OnWriteSetWeight(PacketWriter stream);
            void OnReadSetWeight(PacketReader stream);
        }
        
        /// <summary> The ScriptObject of this GameObject. </summary>
        new public IScriptSkyController ScriptObject { get { return (IScriptSkyController)base.ScriptObject; } }

        #endregion

        #region Constructors

        internal SkyController(World world, IScriptSkyController scriptObject) : base(scriptObject)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            this.world = world;
        }

        #endregion

        #region Properties
        
        World world;
        /// <summary> The World of this SkyController. </summary>
        public World World { get { return this.world; } }
        
        WorldTime startTime;
        /// <summary> The WorldTime from which the interpolation started. </summary>
        public WorldTime StartTime { get { return this.startTime; } }

        float startWeight;
        /// <summary> The weight from which the interpolation started. </summary>
        public float StartWeight { get { return this.startWeight; } }

        WorldTime endTime;
        /// <summary> The WorldTime at which the interpolation reaches its EndWeight. </summary>
        public WorldTime EndTime { get { return this.startTime; } }
        
        float endWeight;
        /// <summary> The weight reached at the EndTime. </summary>
        public float EndWeight { get { return this.endWeight; } }

        float currentWeight = 0;
        /// <summary> The current interpolated weight. </summary>
        public float CurrentWeight { get { return this.currentWeight; } }

        #endregion

        #region Set Weight
        
        /// <summary> Sets the next interpolated weight and time. </summary>
        /// <param name="time"> The point in time when the given weight should be reached. </param>
        /// <param name="weight">[0..1]</param>
        public virtual void SetNextWeight(WorldTime time, float weight)
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
        }

        #endregion

        #region Update Weight
        
        internal virtual void UpdateWeight()
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
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            this.endTime = new WorldTime(stream.ReadInt());
            this.endWeight = stream.ReadFloat();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write(this.endTime.GetTotalSeconds());
            stream.Write(this.endWeight);
        }

        public void WriteNextWeight(PacketWriter stream)
        {
            stream.Write(this.endTime.GetTotalSeconds());
            stream.Write(this.endWeight);
            this.ScriptObject.OnWriteSetWeight(stream);
        }

        public void ReadSetNextWeight(PacketReader stream)
        {
            this.endTime = new WorldTime(stream.ReadInt());
            this.endWeight = stream.ReadFloat();
            this.ScriptObject.OnReadSetWeight(stream);
        }

        #endregion
    }
}
