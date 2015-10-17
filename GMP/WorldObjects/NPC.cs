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

        public NPCInstance instance;

        public NPC(uint id, ushort instanceID) : base(id)
        {
            instance = NPCInstance.Table.Get(instanceID);
            instance.SetProperties(this);
        }

        public NPC(uint id, ushort instanceID, oCNpc npc) : base(id, npc)
        {
            instance = NPCInstance.Table.Get(instanceID);
            instance.SetProperties(this);
        }

        protected override void CreateVob(bool createNew)
        {
            if (createNew)
            {
                gVob = oCNpc.Create(Program.Process);//oCObjectFactory.GetFactory(Program.Process).CreateNPC("OTHERS_NPC");
                gNpc.InitHumanAI();
            }

            gNpc.Instance = instance.ID;
            gNpc.Name.Set(iName);
            gNpc.SetVisual(visual);
            gNpc.SetAdditionalVisuals(bodyMesh, bodyTex, 0, headMesh, headTex, 0, -1);
            using (zVec3 z = zVec3.Create(Program.Process))
            {
                z.X = iBodyWidth;
                z.Y = iBodyHeight;
                z.Z = iBodyWidth;
                gNpc.SetModelScale(z);
            }
            gNpc.SetFatness(iFatness);

            gNpc.Voice = iVoice;
        }

        protected string iName;
        public string name
        {
            set
            {
                iName = value;
                if (Spawned)
                {
                    using (zString z = zString.Create(Program.Process, value))
                    {
                        gNpc.SetName(z);
                    }
                }

            }
            get
            {
                return iName;
            }
        }

        public oCNpc gNpc
        {
            get
            {
                return new oCNpc(Program.Process, gVob.Address);
            }
        }

        protected int iVoice;
        public int voice
        {
            get { return iVoice; }
            set
            {
                iVoice = value;
                if (Spawned)
                {
                    gNpc.Voice = value;
                }
            }
        }

        public NPCState State = NPCState.Stand;
        public NPCWeaponState WeaponState = NPCWeaponState.None;

        #region Visual
        public string bodyMesh { get; protected set; }
        public int bodyTex { get; protected set; }
        public string headMesh { get; protected set; }
        public int headTex { get; protected set; }

        public void SetBodyVisuals(string bodyMesh, int bodyTex, string headMesh, int headTex)
        {
            this.bodyMesh = bodyMesh;
            this.bodyTex = bodyTex;
            this.headMesh = headMesh;
            this.headTex = headTex;
            if (Spawned)
            {
                gNpc.SetAdditionalVisuals(bodyMesh, bodyTex, 0, headMesh, headTex, 0, -1);
            }
        }

        protected float iFatness;
        public float fatness
        {
            get
            {
                return iFatness;
            }
            set
            {
                iFatness = value;
                if (Spawned)
                {
                    gNpc.SetFatness(value);
                }
            }
        }

        protected float iBodyHeight;
        public float bodyHeight
        {
            get
            {
                return iBodyHeight;
            }
            set
            {
                iBodyHeight = value;
                if (Spawned)
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
        }

        //x & z together
        protected float iBodyWidth;
        public float bodyWidth
        {
            get
            {
                return iBodyWidth;
            }
            set
            {
                iBodyWidth = value;
                if (Spawned)
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
