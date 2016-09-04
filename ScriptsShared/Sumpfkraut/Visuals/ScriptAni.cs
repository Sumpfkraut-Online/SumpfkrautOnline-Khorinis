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

        public int Layer { get { return this.baseAni.Layer; } set { this.baseAni.Layer = value; } }

        /// <summary> Duration of the animation in ticks. (int) </summary>
        public int Duration { get { return this.baseAni.Duration; } set { this.baseAni.Duration = value; } }
        
        public int StartFrame { get { return this.baseAni.StartFrame; } set { this.baseAni.StartFrame = value; } }
        public int EndFrame { get { return this.baseAni.EndFrame; } set { this.baseAni.EndFrame = value; } }

        /// <summary> The overlay number of this animation. </summary>
        public ScriptOverlay Overlay { get { return (ScriptOverlay)this.baseAni.Overlay.ScriptObject; } }
        public ScriptAniJob AniJob { get { return (ScriptAniJob)this.baseAni.AniJob.ScriptObject; } }

        public bool IsCreated { get { return this.baseAni.IsCreated; } }

        #endregion

        #region DrawAni / UndrawAni

        int drawTime = 0;
        public int DrawTime
        {
            get { return this.drawTime; }
            set { this.drawTime = value; }
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
            if (this.AniJob.IsDraw || this.AniJob.IsUndraw)
                this.drawTime = stream.ReadInt();
        }

        public void OnWriteProperties(PacketWriter stream)
        {
            if (this.AniJob.IsDraw || this.AniJob.IsUndraw)
                stream.Write(this.drawTime);
        }

        #endregion
    }
}
