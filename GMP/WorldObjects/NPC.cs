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
        public const long DirectionUpdateTime = PositionUpdateTime + 200000;

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

        public void StartTurnAni(bool right)
        {
            //FIXME: Swim & Dive anis
            if (right)
            {
                TurnAnimation = gNpc.AniCtrl._t_turnr;
            }
            else
            {
                TurnAnimation = gNpc.AniCtrl._t_turnl;
            }
            gNpc.GetModel().StartAni(TurnAnimation, 0);
        }

        public void StopTurnAnis()
        {
            gNpc.GetModel().FadeOutAni(TurnAnimation);
            lastDir = null;
            nextDir = null;
        }

        public long nextPosUpdate = 0;
        public override void Update(long now)
        {
            if (nextDir != null) //turn!
            {
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

            if (this != Player.Hero)
            {
                switch (State)
                {
                    case NPCState.Stand:
                        gNpc.AniCtrl._Stand();
                        break;
                    case NPCState.MoveForward:
                        gNpc.AniCtrl._Forward();
                        break;
                    case NPCState.MoveBackward:
                        gNpc.AniCtrl._Backward();
                        break;
                    case NPCState.Jump:
                        if (this.gNpc.GetBodyState() != 8) //jumping
                        {
                            this.gNpc.AniCtrl.PC_JumpForward();
                        }
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
