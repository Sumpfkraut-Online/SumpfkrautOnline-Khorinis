using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class WeatherController : SkyController
    {
        #region ScriptObject

        public partial interface IScriptWeatherController : IScriptSkyController
        {
            void SetWeatherType(WeatherTypes type);
            void OnWriteSetWeatherType(PacketWriter stream);
            void OnReadSetWeatherType(PacketReader stream);
        }

        /// <summary> The ScriptObject of this GameObject. </summary>
        new public IScriptWeatherController ScriptObject { get { return (IScriptWeatherController)base.ScriptObject; } }


        #endregion

        #region Constructors

        internal WeatherController(World world, IScriptWeatherController scriptObject) : base(world, scriptObject)
        {
        }

        #endregion

        #region Properties

        WeatherTypes type = WeatherTypes.Rain;
        /// <summary> Guess what. The type of weather. (Default: Rain) </summary>
        public WeatherTypes WeatherType { get { return this.type; } }

        #endregion
        
        #region SetWeatherType

        partial void pSetWeatherType();
        /// <summary> Changes the current weather. (Default: Rain) </summary>
        public void SetWeatherType(WeatherTypes type)
        {
            this.type = type;
            pSetWeatherType();
        }

        #endregion

        #region Weight

        partial void pSetNextWeight();
        /// <summary> Sets the next interpolated rainfall weight and time. </summary>
        /// <param name="time"> The point in time when the given rainfall weight should be reached. </param>
        /// <param name="weight">[0..1]</param>
        public override void SetNextWeight(long time, float weight)
        {
            base.SetNextWeight(time, weight);
            pSetNextWeight();
        }

        partial void pUpdateWeight();
        internal override void UpdateWeight()
        {
            base.UpdateWeight();
            pUpdateWeight();
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            this.type = (WeatherTypes)stream.ReadByte();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write((byte)this.type);
        }
        
        public void WriteSetWeatherType(PacketWriter stream)
        {
            stream.Write((byte)this.type);
            this.ScriptObject.OnWriteSetWeatherType(stream);
        }

        public void ReadSetWeatherType(PacketReader stream)
        {
            this.type = (WeatherTypes)stream.ReadByte();
            this.ScriptObject.OnReadSetWeatherType(stream);
        }

        #endregion
    }
}
