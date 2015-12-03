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
using GUC.Client.Hooks;

namespace GUC.Client.WorldObjects
{
    class NPC : AbstractVob
    {
        public const long PositionUpdateTime = 1200000; //120ms
        public const long DirectionUpdateTime = PositionUpdateTime + 100000;

        public NPCInstance instance;

        public MobInter UsedMob;

        public NPC(uint id, ushort instanceID)
            : base(id)
        {
            instance = NPCInstance.Table.Get(instanceID);
            instance.SetProperties(this);
        }

        public NPC(uint id, ushort instanceID, oCNpc npc)
            : base(id, npc)
        {
            instance = NPCInstance.Table.Get(instanceID);
            instance.SetProperties(this);
        }

        protected override void CreateVob(bool createNew)
        {
            if (createNew)
            {
                gVob = oCNpc.Create(Program.Process);
            }

            gNpc.Instance = instance.ID;
            gNpc.Name.Set(Name);
            gNpc.SetVisual(Visual);
            gNpc.SetAdditionalVisuals(BodyMesh, BodyTex, 0, HeadMesh, HeadTex, 0, -1);
            using (zVec3 z = zVec3.Create(Program.Process))
            {
                z.X = BodyWidth;
                z.Y = BodyHeight;
                z.Z = BodyWidth;
                gNpc.SetModelScale(z);
            }
            gNpc.SetFatness(Fatness);

            gNpc.Voice = Voice;

            gNpc.HPMax = HPMax;
            gNpc.HP = HP;

            foreach (Item item in equippedSlots.Values)
            {
                if (item.IsMeleeWeapon)
                    gNpc.EquipWeapon(item.gItem);
                else if (item.IsRangedWeapon)
                    gNpc.EquipFarWeapon(item.gItem);
                else if (item.IsArmor)
                    gNpc.EquipArmor(item.gItem);
                else
                    gNpc.EquipItem(item.gItem);
            }

            gNpc.InitHumanAI();
            gAniCtrl = gNpc.AniCtrl;
        }

        protected ushort hpmax = 100;
        public ushort HPMax
        {
            get { return hpmax; }
            set
            {
                hpmax = value;
                if (Spawned)
                {
                    gNpc.HPMax = value;
                }
            }
        }

        protected ushort hp = 100;
        public ushort HP
        {
            get { return hp; }
            set
            {
                hp = value;
                if (Spawned)
                {
                    gNpc.HP = value;
                }
            }
        }

        protected string name = "";
        public string Name
        {
            set
            {
                name = value;
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
                return name;
            }
        }

        public oCNpc gNpc
        {
            get
            {
                return new oCNpc(Program.Process, gVob.Address);
            }
        }

        public oCAniCtrl_Human gAniCtrl { get; protected set; }

        public bool HasFreeHands
        {
            get
            {
                //return (this.gNpc.BodyState & 65536) != 0;
                return true;
            }
        }

        protected int voice = 0;
        public int Voice
        {
            get { return voice; }
            set
            {
                voice = value;
                if (Spawned)
                {
                    gNpc.Voice = value;
                }
            }
        }

        public NPCState State = NPCState.Stand;

        #region Visual

        public string Visual { get { return instance.visual + ".MDS"; } }
        public string BodyMesh { get { return instance.bodyMesh; } }
        public int BodyTex { get; protected set; }
        public string HeadMesh { get; protected set; }
        public int HeadTex { get; protected set; }

        public void SetBodyVisuals(int bodyTex, string headMesh, int headTex)
        {
            this.BodyTex = bodyTex;
            this.HeadMesh = headMesh;
            this.HeadTex = headTex;
            if (Spawned)
            {
                gNpc.SetAdditionalVisuals(BodyMesh, bodyTex, 0, headMesh, headTex, 0, -1);
            }
        }

        protected float fatness = 0;
        public float Fatness
        {
            get
            {
                return fatness;
            }
            set
            {
                fatness = value;
                if (Spawned)
                {
                    gNpc.SetFatness(value);
                }
            }
        }

        protected float bodyHeight = 1.0f;
        public float BodyHeight
        {
            get
            {
                return bodyHeight;
            }
            set
            {
                bodyHeight = value;
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
        protected float bodyWidth = 1.0f;
        public float BodyWidth
        {
            get
            {
                return bodyWidth;
            }
            set
            {
                bodyWidth = value;
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

        public Item DrawnItem = null;

        #region Equipment

        public Dictionary<byte, Item> equippedSlots = new Dictionary<byte, Item>();

        public void EquipSlot(byte slot, Item item)
        {
            if (item != null && !item.Spawned)
            {
                item.Slot = slot;
                if (UnequipSlot(slot))
                {
                    equippedSlots[slot] = item;
                }
                else
                {
                    equippedSlots.Add(slot, item);
                }

                if (Spawned)
                {
                    if (item.IsMeleeWeapon)
                        gNpc.EquipWeapon(item.gItem);
                    else if (item.IsRangedWeapon)
                        gNpc.EquipFarWeapon(item.gItem);
                    else if (item.IsArmor)
                        gNpc.EquipArmor(item.gItem);
                    else
                        gNpc.EquipItem(item.gItem);
                }
            }
        }

        public bool UnequipSlot(byte slot)
        {
            Item item;
            if (equippedSlots.TryGetValue(slot, out item))
            {
                item.Slot = 0;
                if (Spawned)
                {
                    gNpc.UnequipItem(item.gItem);
                }
                return true;
            }
            return false;
        }

        #endregion

        public Vec3f lastDir;
        public Vec3f nextDir;
        public bool turning = false;
        public long lastDirTime;

        public void StartTurnAni(bool right)
        {
            zCModel model = gNpc.GetModel();

            if (model.IsAniActive(model.GetAniFromAniID(gAniCtrl._s_walk)))
            {
                if (right)
                {
                    TurnAnimation = gAniCtrl._t_turnr;
                }
                else
                {
                    TurnAnimation = gAniCtrl._t_turnl;
                }
            }
            else if (model.IsAniActive(model.GetAniFromAniID(gAniCtrl._s_dive)))
            {
                if (right)
                {
                    TurnAnimation = gAniCtrl._t_diveturnr;
                }
                else
                {
                    TurnAnimation = gAniCtrl._t_diveturnl;
                }
            }
            else if (model.IsAniActive(model.GetAniFromAniID(gAniCtrl._s_swim)))
            {
                if (right)
                {
                    TurnAnimation = gAniCtrl._t_swimturnr;
                }
                else
                {
                    TurnAnimation = gAniCtrl._t_swimturnl;
                }
            }
            else
            {
                return;
            }
                
            model.StartAni(TurnAnimation, 0);
        }

        public void StopTurnAnis()
        {
            if (turning)
            {
                Direction = nextDir;
                gNpc.GetModel().FadeOutAni(TurnAnimation);
                turning = false;
            }
        }

        public long nextForwardUpdate = 0;
        public long nextStandUpdate = 0;
        public long nextBackwardUpdate = 0;
        public long nextJumpUpdate = 0;

        public bool DoJump = false;

        public ControlCmd cmd = ControlCmd.Stop;
        public uint cmdTargetVob;
        public Vec3f cmdTargetPos;
        public float cmdTargetRange;

        public override void Update(long now)
        {
            if (cmd == ControlCmd.GoToVob)
            {
                zCEventMessage activeMsg = gVob.GetEM(0).GetActiveMsg();
                if (activeMsg.Address == 0)
                {
                    AbstractVob target = World.GetVobByID(cmdTargetVob);
                    if (target != null && target.Position.GetDistance(this.Position) > cmdTargetRange)
                    {
                        oCMsgMovement msg = oCMsgMovement.Create(Program.Process, oCMsgMovement.SubTypes.GotoVob, target.gVob);
                        gVob.GetEM(0).StartMessage(msg, gVob);
                    }
                }
                else if (activeMsg.VTBL == (int)zCObject.VobTypes.oCMsgMovement)
                {
                    oCMsgMovement movMsg = new oCMsgMovement(Program.Process, activeMsg.Address);
                    if (movMsg.SubType == oCMsgMovement.SubTypes.GotoPos || movMsg.SubType == oCMsgMovement.SubTypes.GotoVob)
                    {
                        AbstractVob target = World.GetVobByID(cmdTargetVob);
                        if (target != null && target.Position.GetDistance(this.Position) <= cmdTargetRange)
                        {
                            gVob.GetEM(0).KillMessages();
                        }
                    }
                }
            }

            if (this != Player.Hero)
            {
                if (turning) //turn!
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
                }

                switch (State)
                {
                    case NPCState.MoveForward:
                        gAniCtrl._Forward();
                        break;
                    case NPCState.MoveBackward:
                        gAniCtrl._Backward();
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
                        gAniCtrl._Stand();
                        break;
                    default:
                        break;
                }
            }
        }

        public void DrawItem(Item item, bool fast)
        {
            if (item == null) return;

            DrawnItem = item;

            if (Spawned)
            {
                if (item == Item.Fists)
                {
                    if (fast)
                    {
                        gNpc.SetToFistMode();
                    }
                    else
                    {
                        gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 0, 0), gVob);
                    }
                }
                else
                {
                    //FIXME: Sneaky mode

                    switch (item.Type)
                    {
                        case ItemType.Sword_1H:
                        case ItemType.Blunt_1H:
                            if (fast)
                            {
                                gNpc.SetToFightMode(item.gItem, 3);
                                //using (zString z = zString.Create(Program.Process, "1H"))
                                //    gNpc.SetWeaponMode2(z);
                            }
                            else
                            {
                                gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 0, 0), gVob);
                            }
                            break;
                        case ItemType.Sword_2H:
                        case ItemType.Blunt_2H:
                            if (fast)
                            {
                                using (zString z = zString.Create(Program.Process, "2H"))
                                    gNpc.SetWeaponMode2(z);
                            }
                            else
                            {
                                gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 0, 0), gVob);
                            }
                            break;
                        case ItemType.Bow:
                            if (fast)
                            {
                                using (zString z = zString.Create(Program.Process, "BOW"))
                                    gNpc.SetWeaponMode2(z);
                            }
                            else
                            {
                                gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 4, 0), gVob);
                            }
                            break;
                        case ItemType.XBow:
                            if (fast)
                            {
                                using (zString z = zString.Create(Program.Process, "CBOW"))
                                    gNpc.SetWeaponMode2(z);
                            }
                            else
                            {
                                gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.DrawWeapon, 4, 0), gVob);
                            }
                            break;
                        case ItemType.Armor:
                            break;
                        case ItemType.Ring:
                            break;
                        case ItemType.Amulet:
                            break;
                        case ItemType.Belt:
                            break;
                        case ItemType.Food_Huge:
                        case ItemType.Food_Small:
                        case ItemType.Drink:
                        case ItemType.Potions:
                            break;
                        case ItemType.Document:
                        case ItemType.Book:
                            break;
                        case ItemType.Rune:
                        case ItemType.Scroll:
                            break;
                        case ItemType.Misc_Usable:
                            break;
                        case ItemType.Misc:
                            break;
                    }
                }
            }
        }

        public void UndrawItem(bool altRemove, bool fast)
        {
            Item item = DrawnItem;
            if (item == null)
                return;

            if (item == Item.Fists || (item.Type >= ItemType.Sword_1H && item.Type <= ItemType.XBow) || item.Type == ItemType.Scroll || item.Type == ItemType.Rune)
            {
                if (fast)
                {
                    gNpc.SetWeaponMode2(0);
                    if (this == Player.Hero)
                    {
                        oCNpcFocus.SetFocusMode(Program.Process, 0);
                    }
                }
                else
                {
                    if (altRemove && gAniCtrl.IsStanding())
                    {
                        gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon1, gAniCtrl.wmode_last, 0), gVob);
                        if (this != Player.Hero)
                        {
                            gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon2, 0, 0), gVob);
                        }
                    }
                    else
                    {
                        gVob.GetEM(0).StartMessage(oCMsgWeapon.Create(Program.Process, oCMsgWeapon.SubTypes.RemoveWeapon, 0, 0), gVob);
                    }
                }
            }

            DrawnItem = null;
        }

        public void DoMoveTo(Vec3f position, float distance)
        {

        }
    }
}
