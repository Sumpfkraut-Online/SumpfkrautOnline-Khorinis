using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Enumeration;
using Gothic.Objects;

namespace GUC.WorldObjects
{
    public partial class NPC
    {
        public const long PositionUpdateTime = 1200000; //120ms
        public const long DirectionUpdateTime = PositionUpdateTime + 100000;

        new public oCNpc gVob { get { return (oCNpc)base.gVob; } }

        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);

            gVob.HP = this.hp;
            gVob.HPMax = this.hpmax;
            gVob.InitHumanAI();
            gVob.Enable(pos.X, pos.Y, pos.Z);
        }

        public bool HasFreeHands
        {
            get
            {
                //return (this.gNpc.BodyState & 65536) != 0;
                return true;
            }
        }

        #region Animation

        /*public int TurnAnimation = 0;

        public void AnimationStart(Animations ani)
        {
            using (zString z = zString.Create(Program.Process, ani.ToString()))
            {
                gVob.GetModel().StartAnimation(z);
            }
        }

        public void AnimationStop(Animations ani)
        {
            using (zString z = zString.Create(Program.Process, ani.ToString()))
            {
                gVob.GetModel().StopAnimation(z);
            }
        }

        public void AnimationFade(Animations ani)
        {
            using (zString z = zString.Create(Program.Process, ani.ToString()))
            {
                int id = gVob.GetModel().GetAniIDFromAniName(z);
                gVob.GetModel().FadeOutAni(id);
            }
        }*/

        #endregion

        #region Equipment

        /*public void EquipSlot(byte slot, Item item)
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
                        gVob.EquipWeapon(item.gVob);
                    else if (item.IsRangedWeapon)
                        gVob.EquipFarWeapon(item.gVob);
                    else if (item.IsArmor)
                        gVob.EquipArmor(item.gVob);
                    else
                        gVob.EquipItem(item.gVob);
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
                    gVob.UnequipItem(item.gVob);
                }
                return true;
            }
            return false;
        }*/

        #endregion

        /*public Vec3f lastDir;
        public Vec3f nextDir;
        public bool turning = false;
        public long lastDirTime;

        public void StartTurnAni(bool right)
        {
            zCModel model = gVob.GetModel();

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
                gVob.GetModel().FadeOutAni(TurnAnimation);
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
                    Vob target = World.Vobs.Get(cmdTargetVob);
                    if (target != null && target.Position.GetDistance(this.Position) > cmdTargetRange)
                    {
                        oCMsgMovement msg = oCMsgMovement.Create(Program.Process, oCMsgMovement.SubTypes.GotoVob, target.gVob);
                        gVob.GetEM(0).StartMessage(msg, gVob);
                    }
                }
                /*else if (activeMsg.VTBL == (int)zCObject.VobTypes.oCMsgMovement)
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
                }*//*
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
                        gVob.DoStrafe(true);
                        break;
                    case NPCState.MoveLeft:
                        gVob.GetEM(0).KillMessages();
                        gVob.DoStrafe(false);
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
                        gVob.SetToFistMode();
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
                                gVob.SetToFightMode(item.gVob, 3);
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
                                    gVob.SetWeaponMode2(z);
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
                                    gVob.SetWeaponMode2(z);
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
                                    gVob.SetWeaponMode2(z);
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
                    gVob.SetWeaponMode2(0);
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

        }*/
    }
}
