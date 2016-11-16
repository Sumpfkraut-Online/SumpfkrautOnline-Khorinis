using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.WorldGlobals;


namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class ScriptWeatherCtrl : WeatherController.IScriptWeatherController
    {
        #region Constructors

        partial void pConstruct();
        public ScriptWeatherCtrl(WorldInst world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");
            this.world = world;
            pConstruct();
        }

        #endregion

        #region Properties

        WorldInst world;
        public WorldInst World { get { return this.world; } }
        public WeatherController BaseWeather { get { return this.world.BaseWorld.WeatherCtrl; } }

        #endregion

        partial void pSetNextWeight(WorldTime time, float weight);
        public void SetNextWeight(WorldTime time, float weight)
        {
            BaseWeather.SetNextWeight(time, weight);
            pSetNextWeight(time, weight);
        }

        public void SetWeatherType(WeatherTypes type)
        {
            BaseWeather.SetWeatherType(type);
        }

        #region Read & Write

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        public void OnWriteSetWeight(PacketWriter stream)
        {
        }

        public void OnReadSetWeight(PacketReader stream)
        {
        }

        public void OnWriteSetWeatherType(PacketWriter stream)
        {
        }

        public void OnReadSetWeatherType(PacketReader stream)
        {
        }

        #endregion
    }
}
