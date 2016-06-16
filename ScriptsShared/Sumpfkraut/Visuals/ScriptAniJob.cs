using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public static class SetAnis
    {
        public const int Attack1HFwd1 = 0,
        Attack1HFwd2 = 1,
        Attack1HFwd3 = 2,
        Attack1HFwd4 = 3,
        Attack1HLeft = 4,
        Attack1HRight = 5,
        Attack1HRun = 6,
        Attack1HDodge = 7,
        Attack1HParry1 = 8,
        Attack1HParry2 = 9,
        Attack1HParry3 = 10,

        Attack2HFwd1 = 11,
        Attack2HFwd2 = 12,
        Attack2HFwd3 = 13,
        Attack2HFwd4 = 14,
        Attack2HLeft = 15,
        Attack2HRight = 16,
        Attack2HRun = 17,
        Attack2HDodge = 18,
        Attack2HParry1 = 19,
        Attack2HParry2 = 20,
        Attack2HParry3 = 21,

        JumpRun = 22,
        JumpFwd = 23,
        JumpUp = 24,

        ClimbLow = 25,
        ClimbMid = 26,
        ClimbHigh = 27,

        Draw1H = 28,
        Draw1HRun = 29,
        Draw2H = 30,
        Draw2HRun = 31,

        Undraw1H = 32,
        Undraw1HRun = 33,
        Undraw2H = 34,
        Undraw2HRun = 35;
    }
    
    public partial class ScriptAniJob : ScriptObject, AniJob.IScriptAniJob
    {
        #region Properties

        public bool IsFightMove { get { return this.ID >= SetAnis.Attack1HFwd1 && this.ID <= SetAnis.Attack2HParry3; } }
        public bool IsAttack { get { return (this.ID >= SetAnis.Attack1HFwd1 && this.ID <= SetAnis.Attack1HRun) || (this.ID >= SetAnis.Attack2HFwd1 && this.ID <= SetAnis.Attack2HRun); } }
        public bool IsAttackCombo { get { return (this.ID >= SetAnis.Attack1HFwd1 && this.ID <= SetAnis.Attack1HFwd4) || (this.ID >= SetAnis.Attack2HFwd1 && this.ID <= SetAnis.Attack2HFwd4); } }
        public bool IsAttackRun { get { return this.ID == SetAnis.Attack1HRun || this.ID == SetAnis.Attack2HRun; } }
        public bool IsParade { get { return (this.ID >= SetAnis.Attack1HParry1 && this.ID <= SetAnis.Attack1HParry3) || (this.ID >= SetAnis.Attack2HParry1 && this.ID <= SetAnis.Attack2HParry3); } }
        public bool IsDodge { get { return this.ID == SetAnis.Attack1HDodge || this.ID == SetAnis.Attack2HDodge; } }
        public bool IsJump { get { return this.ID >= SetAnis.JumpRun && this.ID <= SetAnis.JumpUp; } }
        public bool IsClimb { get { return this.ID >= SetAnis.ClimbLow && this.ID <= SetAnis.ClimbHigh; } }
        public bool IsDraw { get { return this.ID >= SetAnis.Draw1H && this.ID <= SetAnis.Draw2HRun; } }
        public bool IsUndraw { get { return this.ID >= SetAnis.Undraw1H && this.ID <= SetAnis.Undraw2HRun; } }

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
