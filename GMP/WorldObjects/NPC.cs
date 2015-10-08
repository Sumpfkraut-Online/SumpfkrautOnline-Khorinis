using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Types;
using Gothic.zTypes;
using RakNet;
using GUC.Network;
using Gothic.zStruct;

namespace GUC.Client.WorldObjects
{
    class NPC : Vob
    {
        public const long PositionUpdateTime = 1200000; //120ms
        public const long DirectionUpdateTime = PositionUpdateTime + 100000;

        public NPC(uint id)
            : base(id, oCObjectFactory.GetFactory(Program.Process).CreateNPC("OTHERS_NPC"))
        {

        }

        public NPC(uint id, oCNpc npc)
            : base(id, npc)
        {
        }

        public NPCState State = NPCState.Stand;
        public NPCWeaponState WeaponState = NPCWeaponState.None;

        public string Name
        {
            set
            {
                using (zString z = zString.Create(Program.Process, value))
                    gNpc.SetName(z);
            }
            get
            {
                return gNpc.Name.Value;
            }
        }

        public oCNpc gNpc
        {
            get
            {
                return new oCNpc(Program.Process, gVob.Address);
            }
        }

        #region Visual
        public string BodyMesh
        {
            get
            {
                return gNpc.BodyVisualString.ToString();
            }
        }

        public int BodyTex
        {
            get
            {
                return gNpc.BodyTex;
            }
        }

        public string HeadMesh
        {
            get
            {
                return gNpc.HeadVisualString.ToString();
            }
        }

        public int HeadTex
        {
            get
            {
                return gNpc.HeadTex;
            }
        }

        public float Fatness
        {
            get
            {
                return gNpc.Fatness;
            }
            set
            {
                gNpc.SetFatness(value);
            }
        }

        public float BodyHeight
        {
            get
            {
                return gNpc.Scale.Y;
            }
            set
            {
                using (zVec3 scale = zVec3.Create(Program.Process))
                {
                    scale.X = gNpc.Scale.X;
                    scale.Y = value;
                    scale.Z = gNpc.Scale.Z;
                    gNpc.SetModelScale(scale);
                }
            }
        }

        //x & z together
        public float BodyWidth
        {
            get
            {
                return gNpc.Scale.X;
            }
            set
            {
                using (zVec3 scale = zVec3.Create(Program.Process))
                {
                    scale.X = value;
                    scale.Y = gNpc.Scale.Y;
                    scale.Z = value;
                    gNpc.SetModelScale(scale);
                }
            }
        }
        #endregion

        #region Animation

        public int TurnAnimation = 0;

        public void AnimationStart(Animations ani)
        {
            using (zString z = zString.Create(Program.Process, ani.ToString()))
            {
                gNpc.GetModel().StartAnimation(z);
            }
        }

        public void AnimationStop(Animations ani)
        {
            using (zString z = zString.Create(Program.Process, ani.ToString()))
            {
                gNpc.GetModel().StopAnimation(z);
            }
        }

        public void AnimationFade(Animations ani)
        {
            using (zString z = zString.Create(Program.Process, ani.ToString()))
            {
                int id = gNpc.GetModel().GetAniIDFromAniName(z);
                gNpc.GetModel().FadeOutAni(id);
            }
        }

        #endregion

        public void DrawFists()
        {
            WeaponState = NPCWeaponState.Fists;
            gNpc.DrawMeleeWeapon();
        }

        public ItemInstance EquippedMeleeWeapon;
        public ItemInstance EquippedRangedWeapon;
        public ItemInstance EquippedArmor;

        public Vec3f lastDir;
        public Vec3f nextDir;
        public long lastDirTime;
        public int turn;

        public void StartTurnAni(bool right)
        {
            turn = right ? 1 : -1;
            DoTurnAni();
        }

        void DoTurnAni()
        {
            TurnAnimation = 0;
            zCModel model = gNpc.GetModel();

            if (model.IsAniActive(model.GetAniFromAniID(gNpc.AniCtrl._s_walk)))
            {
                if (turn > 0)
                {
                    TurnAnimation = gNpc.AniCtrl._t_turnr;
                }
                else if (turn < 0)
                {
                    TurnAnimation = gNpc.AniCtrl._t_turnl;
                }
            }
            else if (model.IsAniActive(model.GetAniFromAniID(gNpc.AniCtrl._s_dive)))
            {
                if (turn > 0)
                {
                    TurnAnimation = gNpc.AniCtrl._t_diveturnr;
                }
                else if (turn < 0)
                {
                    TurnAnimation = gNpc.AniCtrl._t_diveturnl;
                }
            }
            else if (model.IsAniActive(model.GetAniFromAniID(gNpc.AniCtrl._s_swim)))
            {
                if (turn > 0)
                {
                    TurnAnimation = gNpc.AniCtrl._t_swimturnr;
                }
                else if (turn < 0)
                {
                    TurnAnimation = gNpc.AniCtrl._t_swimturnl;
                }
            }

            if (TurnAnimation != 0)
            {
                gNpc.GetModel().StartAni(TurnAnimation, 0);
                turn = 0;
            }
        }

        public void StopTurnAnis()
        {
            gNpc.GetModel().FadeOutAni(TurnAnimation);
            TurnAnimation = 0;
            lastDir = null;
            nextDir = null;
            turn = 0;
        }

        public long nextPosUpdate = 0;
        public override void Update(long now)
        {
            if (this != Player.Hero)
            {
                if (nextDir != null) //turn!
                {
                    if (turn != 0)
                    {
                        DoTurnAni();
                    }

                    float diff = (float)(DateTime.Now.Ticks - lastDirTime) / (float)DirectionUpdateTime;

                    if (diff < 1.0f)
                    {
                        this.Direction = lastDir + (nextDir - lastDir) * diff;
                    }
                    else
                    {
                        this.Direction = nextDir;
                        StopTurnAnis();
                    }
                }

                switch (State)
                {
                    case NPCState.MoveForward:
                        gNpc.AniCtrl._Forward();
                        break;
                    case NPCState.MoveBackward:
                        gNpc.AniCtrl._Backward();
                        break;
                    case NPCState.MoveRight:
                        gVob.GetEM(0).KillMessages();
                        gNpc.DoStrafe(true);
                        break;
                    case NPCState.MoveLeft:
                        gVob.GetEM(0).KillMessages();
                        gNpc.DoStrafe(false);
                        break;
                    case NPCState.Stand:
                        gNpc.AniCtrl._Stand();
                        break;
                    default:
                        //gNpc.AniCtrl._Stand();
                        break;
                }
            }
            else
            {
                if (nextPosUpdate < DateTime.Now.Ticks)
                {
                    Network.Messages.VobMessage.WritePosDir(this);
                    nextPosUpdate = DateTime.Now.Ticks + PositionUpdateTime;
                }
            }
        }
    }
}
