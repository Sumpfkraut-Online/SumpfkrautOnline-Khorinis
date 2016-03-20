using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAni : ScriptObject, Animation.IScriptAnimation
    {
        #region Properties

        Animation baseAni;
        public Animation BaseAni { get { return this.baseAni; } }

        /// <summary> Duration of the animation in ms. (int) </summary>
        public int Duration { get { return this.baseAni.Duration; } set { this.baseAni.Duration = value; } }

        /// <summary> The overlay number of this animation. (byte) </summary>
        public int Overlay { get { return this.baseAni.Overlay; } set { this.baseAni.Overlay = value; } }

        /// <summary> From which percentage the gothic animation should start. 255 = 100% (byte) </summary>
        public int StartPercent { get { return this.baseAni.StartPercent; } set { this.baseAni.StartPercent = value; } }

        #endregion

        #region Constructors

        public ScriptAni(PacketReader stream) : this()
        {
            this.baseAni.ReadStream(stream);
        }

        private ScriptAni()
        {
            this.baseAni = new Animation();
            this.baseAni.ScriptObject = this;
        }

        #endregion

        #region Read & Write

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        #endregion
    }
}
