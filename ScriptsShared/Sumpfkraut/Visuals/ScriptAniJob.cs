using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Visuals
{   
    public enum SetAnis
    {
        Attack1HFwd1,
        Attack1HFwd2,
        Attack1HFwd3,
        Attack1HFwd4,
        Attack1HLeft,
        Attack1HRight,
        Attack1HRun,
        Attack1HDodge,
        Attack1HParry1,
        Attack1HParry2,
        Attack1HParry3,

        Attack2HFwd1,
        Attack2HFwd2,
        Attack2HFwd3,
        Attack2HFwd4,
        Attack2HLeft,
        Attack2HRight,
        Attack2HRun,
        Attack2HDodge,
        Attack2HParry1,
        Attack2HParry2,
        Attack2HParry3,

        JumpRun,
        JumpFwd,
        JumpUp,

        ClimbLow,
        ClimbMid,
        ClimbHigh,

        Draw1H,
        Draw1HRun,
        Draw2H,
        Draw2HRun,

        Undraw1H,
        Undraw1HRun,
        Undraw2H,
        Undraw2HRun,
    }

    public enum AniType
    {
        Normal,
        Fight,
        FightAttack,
        FightAttackCombo,
        FightAttackRun,
        FightParade,
        FightDodge,
        Jump,
        Climb,
        Draw,
        Undraw
    }

    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        #region Properties

        public bool IsFightMove { get { return this.Type >= AniType.Fight && this.Type <= AniType.FightDodge; } }
        public bool IsAttack { get { return this.Type >= AniType.FightAttack && this.Type <= AniType.FightAttackRun; } }
        public bool IsAttackCombo { get { return this.Type == AniType.FightAttackCombo; } }
        public bool IsAttackRun { get { return this.Type == AniType.FightAttackRun; } }
        public bool IsParade { get { return this.Type == AniType.FightParade; } }
        public bool IsDodge { get { return this.Type == AniType.FightDodge; } }
        public bool IsJump { get { return this.Type == AniType.Jump; } }
        public bool IsClimb { get { return this.Type == AniType.Climb; } }
        public bool IsDraw { get { return this.Type == AniType.Draw; } }
        public bool IsUndraw { get { return this.Type == AniType.Undraw; } }

        public bool IsCreated { get { return this.baseAniJob.IsCreated; } }

        AniJob baseAniJob;
        public AniJob BaseAniJob { get { return this.baseAniJob; } }

        public string AniName { get { return this.baseAniJob.Name; } set { this.baseAniJob.Name = value; } }
        
        public ScriptAni DefaultAni { get { return (ScriptAni)this.baseAniJob.DefaultAni.ScriptObject; } }

        public int ID { get { return this.baseAniJob.ID; } set { this.baseAniJob.ID = value; } }

        public AniType Type = AniType.Normal;

        #endregion
        
        public void SetDefaultAni(Animation ani)
        {
            this.SetDefaultAni((ScriptAni)ani.ScriptObject);
        }

        public virtual void SetDefaultAni(ScriptAni ani)
        {
            this.baseAniJob.SetDefaultAni(ani.BaseAni);
        }

        public void AddOverlayAni(Animation ani, Overlay overlay)
        {
            this.AddOverlayAni((ScriptAni)ani.ScriptObject, (ScriptOverlay)overlay.ScriptObject);
        }

        public virtual void AddOverlayAni(ScriptAni ani, ScriptOverlay ov)
        {
            this.baseAniJob.AddOverlayAni(ani.BaseAni, ov.BaseOverlay);
        }

        public void RemoveOverlayAni(Animation ani)
        {
            this.RemoveOverlayAni((ScriptAni)ani.ScriptObject);
        }

        public void RemoveOverlayAni(ScriptAni ani)
        {
            this.baseAniJob.RemoveOverlayAni(ani.BaseAni);
        }

        #region Constructors

        public ScriptAniJob()
        {
            this.baseAniJob = new AniJob();
            this.baseAniJob.ScriptObject = this;
        }

        #endregion

        #region Read & Write

        public void OnReadProperties(PacketReader stream)
        {
            this.Type = (AniType)stream.ReadByte();
        }

        public void OnWriteProperties(PacketWriter stream)
        {
            stream.Write((byte)this.Type);
        }

        #endregion
    }
}
