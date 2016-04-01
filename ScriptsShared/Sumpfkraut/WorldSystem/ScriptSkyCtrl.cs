using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.Weather;
using GUC.Enumeration;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class ScriptSkyCtrl : SkyController.IScriptSkyController
    {
        SkyController baseCtrl;
        public SkyController BaseCtrl { get { return this.baseCtrl; } }

        public WorldInst World { get { return (WorldInst)this.baseCtrl.World.ScriptObject; } }

        // SetTime etc

        partial void pConstruct();
        public ScriptSkyCtrl(WorldInst world)
        {
            this.baseCtrl = world.BaseWorld.SkyCtrl;
            this.baseCtrl.ScriptObject = this;
            pConstruct();
        }

        partial void pSetRainTime(WorldTime time, float weight);
        public void SetRainTime(WorldTime time, float weight)
        {
            this.baseCtrl.SetRainTime(time, weight);
            pSetRainTime(time, weight);
        }

        public void SetWeatherType(WeatherTypes type)
        {
            this.baseCtrl.SetWeatherType(type);
        }

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        public void OnWriteSetRainTime(PacketWriter stream)
        {
        }

        public void OnReadSetRainTime(PacketReader stream)
        {
        }

        public void OnWriteSetWeatherType(PacketWriter stream)
        {
        }

        public void OnReadSetWeatherType(PacketReader stream)
        {
        }
    }
}
