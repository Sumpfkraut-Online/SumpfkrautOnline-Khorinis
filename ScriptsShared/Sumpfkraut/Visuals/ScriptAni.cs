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

        /// <summary> From which percentage the gothic animation should start. 255 = 100% (byte) </summary>
        public int StartPercent { get { return this.baseAni.StartPercent; } set { this.baseAni.StartPercent = value; } }

        /// <summary> The overlay number of this animation. </summary>
        public ScriptOverlay Overlay { get { return (ScriptOverlay)this.baseAni.Overlay.ScriptObject; } }
        public ScriptAniJob AniJob { get { return (ScriptAniJob)this.baseAni.AniJob.ScriptObject; } }

        public bool IsCreated { get { return this.baseAni.IsCreated; } }

        int hitTime = 0;
        public int HitTime
        {
            get { return this.hitTime; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("HitPercent can't be changed when the object is created.");
                this.hitTime = value;
            }
        }

        int comboTime = 0;
        public int ComboTime
        {
            get { return this.comboTime; }
            set
            {
                if (this.IsCreated)
                    throw new Exception("ComboPercent can't be changed when the object is created.");
                this.comboTime = value;
            }
        }

        #endregion

        #region Constructors

        public ScriptAni()
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
