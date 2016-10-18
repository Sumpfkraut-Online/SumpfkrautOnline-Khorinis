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
        DrawBow,
        DrawBowRun,
        DrawXBow,
        DrawXBowRun,

        Undraw1H,
        Undraw1HRun,
        Undraw2H,
        Undraw2HRun,
        UndrawBow,
        UndrawBowRun,
        UndrawXBow,
        UndrawXBowRun,

        BowAim,
        BowReload,
        BowLower,

        XBowAim,
        XBowReload,
        XBowLower,

        DropItem
    }
    
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        #region Properties

        public bool IsFightMove { get { return this.ID >= (int)SetAnis.Attack1HFwd1 && this.ID <= (int)SetAnis.Attack2HParry3; } }
        public bool IsAttack { get { return (this.ID >= (int)SetAnis.Attack1HFwd1 && this.ID <= (int)SetAnis.Attack1HRun) || (this.ID >= (int)SetAnis.Attack2HFwd1 && this.ID <= (int)SetAnis.Attack2HRun); } }
        public bool IsAttackCombo { get { return (this.ID >= (int)SetAnis.Attack1HFwd1 && this.ID <= (int)SetAnis.Attack1HFwd4) || (this.ID >= (int)SetAnis.Attack2HFwd1 && this.ID <= (int)SetAnis.Attack2HFwd4); } }
        public bool IsAttackRun { get { return this.ID == (int)SetAnis.Attack1HRun || this.ID == (int)SetAnis.Attack2HRun; } }
        public bool IsParade { get { return (this.ID >= (int)SetAnis.Attack1HParry1 && this.ID <= (int)SetAnis.Attack1HParry3) || (this.ID >= (int)SetAnis.Attack2HParry1 && this.ID <= (int)SetAnis.Attack2HParry3); } }
        public bool IsDodge { get { return this.ID == (int)SetAnis.Attack1HDodge || this.ID == (int)SetAnis.Attack2HDodge; } }
        public bool IsJump { get { return this.ID >= (int)SetAnis.JumpRun && this.ID <= (int)SetAnis.JumpUp; } }
        public bool IsClimb { get { return this.ID >= (int)SetAnis.ClimbLow && this.ID <= (int)SetAnis.ClimbHigh; } }
        public bool IsDraw { get { return this.ID >= (int)SetAnis.Draw1H && this.ID <= (int)SetAnis.DrawXBowRun; } }
        public bool IsUndraw { get { return this.ID >= (int)SetAnis.Undraw1H && this.ID <= (int)SetAnis.UndrawXBowRun; } }

        public bool IsCreated { get { return this.baseAniJob.IsCreated; } }

        AniJob baseAniJob;
        public AniJob BaseAniJob { get { return this.baseAniJob; } }

        public string AniName { get { return this.baseAniJob.Name; } set { this.baseAniJob.Name = value; } }

        public ScriptAni DefaultAni { get { return (ScriptAni)this.baseAniJob.DefaultAni.ScriptObject; } }

        public int ID { get { return this.baseAniJob.ID; } set { this.baseAniJob.ID = value; } }

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
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        #endregion
    }
}
