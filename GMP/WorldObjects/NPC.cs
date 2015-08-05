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
        public const long DirectionUpdateTime = PositionUpdateTime + 300000;

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
        public List<String> Overlays = new List<string>();
        public short Animation = short.MaxValue;
        public long AnimationStartTime = 0;
        #endregion

        public void DrawFists()
        {
            oCItem w = gNpc.GetEquippedMeleeWeapon();
            gNpc.UnequipItem(w);
            gNpc.DrawMeleeWeapon();
            gNpc.GetEM(0).DoFrameActivity();
            gNpc.EquipWeapon(w);
        }

        public ItemInstance EquippedMeleeWeapon;
        public ItemInstance EquippedRangedWeapon;
        public ItemInstance EquippedArmor;

        public Vec3f lastDir;
        public Vec3f nextDir;
        public long lastDirTime;

        public void StartTurnAni(bool right)
        {
            if (right)
            {
                Animation = (short)gNpc.AniCtrl._t_turnr;
            }
            else
            {
                Animation = (short)gNpc.AniCtrl._t_turnl;
            }
            gNpc.GetModel().StartAni(Animation, 0);
        }

        public void StopTurnAnis()
        {
            gNpc.GetModel().FadeOutAni(Animation);
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
