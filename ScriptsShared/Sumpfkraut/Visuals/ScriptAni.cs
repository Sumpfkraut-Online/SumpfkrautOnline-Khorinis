using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAni : ExtendedObject, Animation.IScriptAnimation
    {
        #region Constructors

        public ScriptAni()
        {
            this.baseAni = new Animation(this);
        }

        #endregion

        #region Properties

        Animation baseAni;
        public Animation BaseAni { get { return this.baseAni; } }

        /// <summary> Frame speed </summary>
        public float FPS { get { return this.baseAni.FPS; } set { this.baseAni.FPS = value; } }
        
        public float StartFrame { get { return this.baseAni.StartFrame; } set { this.baseAni.StartFrame = value; } }
        public float EndFrame { get { return this.baseAni.EndFrame; } set { this.baseAni.EndFrame = value; } }

        /// <summary> The overlay number of this animation. </summary>
        public ScriptOverlay Overlay { get { return (ScriptOverlay)this.baseAni.Overlay.ScriptObject; } }
        public ScriptAniJob AniJob { get { return (ScriptAniJob)this.baseAni.AniJob.ScriptObject; } }

        public bool IsCreated { get { return this.baseAni.IsCreated; } }

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
