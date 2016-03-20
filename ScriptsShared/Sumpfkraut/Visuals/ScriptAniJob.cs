using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        #region Properties

        public bool IsCreated { get { return this.baseAniJob.IsCreated; } }

        AniJob baseAniJob;
        public AniJob BaseAniJob { get { return this.baseAniJob; } }

        public string AniName { get { return this.baseAniJob.Name; } set { this.baseAniJob.Name = value; } }
        
        public ScriptAni DefaultAni
        {
            get { return (ScriptAni)this.baseAniJob.BaseAni.ScriptObject; }
            set { this.baseAniJob.BaseAni = value == null ? null : value.BaseAni; }
        }

        public bool TryGetOverlayAni(int overlay, out ScriptAni ani)
        {
            for (int i = 0; i < this.baseAniJob.OverlayAnis.Count; i++)
            {
                if (this.baseAniJob.OverlayAnis[i].Overlay == overlay)
                {
                    ani = (ScriptAni)this.baseAniJob.OverlayAnis[overlay].ScriptObject;
                    return true;
                }
            }
            ani = null;
            return false;
        }

        #endregion

        #region Constructors

        public ScriptAniJob(PacketReader stream) : this()
        {
            this.baseAniJob.ReadStream(stream);
        }

        private ScriptAniJob()
        {
            this.baseAniJob = new AniJob();
            this.baseAniJob.ScriptObject = this;
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
