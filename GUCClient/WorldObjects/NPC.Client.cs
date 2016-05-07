using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;
using Gothic.Objects;
using GUC.Enumeration;
using GUC.Animations;

namespace GUC.WorldObjects
{
    public partial class NPC
    {
        public partial interface IScriptNPC : IScriptVob
        {
            void OnTick(long now);
        }

        public const long PositionUpdateTime = 1200000; //120ms
        public const long DirectionUpdateTime = PositionUpdateTime + 100000;

        new public oCNpc gVob { get { return (oCNpc)base.gVob; } }

        internal long nextStateUpdate = 0;

        partial void pJump()
        {
            if (state == MoveState.Forward)
            {
                gVob.GetModel().StartAni(gVob.AniCtrl._t_runr_2_jump, 0);

                //set some flags, see 0x6B1F1D: LOBYTE(aniCtrl->_zCAIPlayer_bitfield[0]) &= 0xF7u;
                gVob.SetBodyState(8);
            }
            else if (state == MoveState.Stand)
            {
                //Just in case the npc is turning
                //StopTurnAnis();

                gVob.AniCtrl.JumpForward();
            }
            gVob.SetPhysicsEnabled(true);

            var ai = gVob.HumanAI;

            var vec = new Gothic.Types.zVec3(ai.Address + 0x90);
            var dir = GetDirection();
            vec.X = 0;
            vec.Y = 300.0f;
            vec.Z = 0;
            var rb = WinApi.Process.ReadInt(gvob.Address + 224);
            WinApi.Process.THISCALL<WinApi.NullReturnCall>(rb, 0x5B66D0, vec);
        }

        #region Animations

        #region Overlays

        partial void pAddOverlay(Overlay overlay)
        {
            if (this.gvob != null)
                this.gVob.ApplyOverlay(overlay.Name);
        }

        partial void pRemoveOverlay(Overlay overlay)
        {
            if (this.gvob != null)
                this.gVob.ApplyOverlay(overlay.Name);
        }

        #endregion

        partial void pStartAnimation(Animation ani)
        {
            if (this.gvob != null)
            {
                var gModel = this.gVob.GetModel();
                int aniID = gModel.GetAniIDFromAniName(ani.AniJob.Name);
                if (aniID > 0)
                {
                    gModel.StartAni(aniID, 0);
                    gModel.GetActiveAni(aniID).SetActFrame(ani.StartFrame);
                }
            }
        }

        partial void pStopAnimation(bool fadeOut)
        {
            if (this.gvob != null)
            {
                var gModel = gVob.GetModel();
                int id = gModel.GetAniIDFromAniName(currentAni.AniJob.Name);
                var activeAni = gModel.GetActiveAni(id);

                if (fadeOut)
                {
                    gModel.StopAni(activeAni);
                }
                else
                {
                    gModel.FadeOutAni(activeAni);
                }
            }
        }

        partial void pEndAni()
        {
            if (this.state == MoveState.Forward)
                this.gVob.GetModel().StartAni(this.gVob.AniCtrl._s_walkl, 0);
            else
                this.gVob.GetModel().StartAni(this.gVob.AniCtrl._s_walk, 0);
        }

        #endregion

        #region Spawn

        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);

            gVob.HP = this.hp;
            gVob.HPMax = this.hpmax;
            gVob.InitHumanAI();
            gVob.Enable(pos.X, pos.Y, pos.Z);
            if (this.Name == "Scavenger") gVob.SetToFistMode();
            if (overlays != null)
                for (int i = 0; i < overlays.Count; i++)
                    this.gVob.ApplyOverlay(overlays[i].Name);
            gVob.Name.Set(this.Name);
        }

        partial void pDespawn()
        {
            gVob.Disable();
        }

        #endregion

        partial void pSetHealth()
        {
            if (this.gVob == null)
                return;

            var gModel = gVob.GetModel();
            var gAniCtrl = gVob.AniCtrl;

            gModel.StopAni(gAniCtrl._t_strafel);
            gModel.StopAni(gAniCtrl._t_strafer);

            this.gVob.HPMax = this.hpmax;
            this.gVob.HP = this.hp;
        }

        partial void pSetState(MoveState state)
        {
            if (this.gVob == null)
                return;

            if (!this.IsInAnimation)
                if (this.state == MoveState.Right || this.state == MoveState.Left)
                {
                    if (state == MoveState.Forward)
                        this.gVob.GetModel().StartAni(this.gVob.AniCtrl._s_walkl, 0);
                    else
                        this.gVob.GetModel().StartAni(this.gVob.AniCtrl._s_walk, 0);
                }

            this.Update(GameTime.Ticks);
        }
        

        internal void Update(long now)
        {
            /*if (turning) //turn!
            {
                float diff = (float)(DateTime.UtcNow.Ticks - lastDirTime) / (float)DirectionUpdateTime;

                if (diff < 1.0f)
                {
                    Direction = lastDir + (nextDir - lastDir) * diff;
                }
                else
                {
                    StopTurnAnis();
                }
            }*/

            this.ScriptObject.OnTick(now);

            if (this.IsDead)
                return;

            if (gVob.GetBodyState() == 8 || gVob.HumanAI.AboveFloor > 20)
                return;

            if (this.IsInAnimation)
            {
                return;
            }

            switch (State)
            {
                case MoveState.Forward:
                    gVob.AniCtrl._Forward();
                    break;
                case MoveState.Backward:
                    gVob.AniCtrl._Backward();
                    break;
                case MoveState.Right:
                    if (!this.IsInAnimation && !gVob.GetModel().IsAniActive(gVob.GetModel().GetAniFromAniID(gVob.AniCtrl._t_strafer)))
                    {
                        gVob.GetModel().StartAni(gVob.AniCtrl._t_strafer, 0);
                    }
                    break;
                case MoveState.Left:
                    if (!this.IsInAnimation && !gVob.GetModel().IsAniActive(gVob.GetModel().GetAniFromAniID(gVob.AniCtrl._t_strafel)))
                    {
                        gVob.GetModel().StartAni(gVob.AniCtrl._t_strafel, 0);
                    }
                    break;
                case MoveState.Stand:
                    gVob.AniCtrl._Stand();
                    break;
                default:
                    break;
            }
        }
        
    }
}
