using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zStruct;
using Gothic.zClasses;
using GUC.Client.WorldObjects;
using GUC.Enumeration;
using GUC.Client.Network.Messages;

namespace GUC.Client.Hooks
{
    static class hEventManager
    {
        static bool msgHookEnabled = true;
        static bool blocked = false;
        static bool BlockMsg
        {
            get { return blocked; }
            set
            {
                if (blocked != value)
                {
                    blocked = value;
                    if (value)
                    {
                        Program.Process.Write(new byte[] { 0xE9, 0x11, 0x08, 0x00, 0x00, 0x90 }, 0x7863B9); // jmp to end
                    }
                    else
                    {
                        Program.Process.Write(new byte[] { 0x0F, 0x8C, 0xE8, 0x02, 0x00, 0x00 }, 0x7863B9); // original code
                    }
                }
            }
        } //block the incoming EventMessage?

        public static void StartMessage(this zCEventManager em, zCEventMessage msg, zCVob vob)
        {
            msgHookEnabled = false;
            em.OnMessage(msg, vob);
            msgHookEnabled = true;
        }

        public static void AddHooks(Process process)
        {
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hEventManager).GetMethod("hook_OnMessage"), (int)zCEventManager.FuncOffsets.OnMessage, 7, 3); //hook
        }

        public static Int32 hook_OnMessage(String message)
        {
            try
            {
                if (msgHookEnabled)
                {
                    if (Player.Hero != null)
                    {
                        int address = Convert.ToInt32(message);
                        int emAddr = Program.Process.ReadInt(address);
                        int msgAddr = Program.Process.ReadInt(address + 4);
                        int vobAddr = Program.Process.ReadInt(address + 8);

                        int vtbl = Program.Process.ReadInt(msgAddr);

                        switch ((zCObject.VobTypes)vtbl)
                        {
                            case zCObject.VobTypes.oCMsgAttack:
                                OnMsgAttack(emAddr, msgAddr, vobAddr);
                                return 0;
                            case zCObject.VobTypes.oCMsgWeapon:
                                OnMsgWeapon(emAddr, msgAddr, vobAddr);
                                return 0;
                            case zCObject.VobTypes.oCMsgMovement:
                                OnMsgMovement(emAddr, msgAddr, vobAddr);
                                return 0;
                            case zCObject.VobTypes.oCMobMsg:
                                OnMobMsg(emAddr, msgAddr, vobAddr);
                                return 0;
                        }
                    }
                }
                BlockMsg = false;
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "Program.cs", 0);
            }
            return 0;
        }

        static void OnMobMsg(int emAddress, int msgAddress, int vobAddress)
        {
            oCMobMsg msg = new oCMobMsg(Program.Process, msgAddress);

            if (msg.SubType == oCMobMsg.SubTypes.EV_EndInteraction && msg.UserAddress != 0)
            {
                //Turn collision on again, THX to Situ
                (new zCVob(Program.Process, msg.UserAddress)).BitField1 |= (int)zCVob.BitFlag0.collDetectionStatic;
            }

            if (msg.UserAddress == Player.Hero.gVob.Address)
            {
                zCEventManager EM = new zCEventManager(Program.Process, emAddress); //EventManager of the used Mob

                switch (msg.SubType)
                {
                    case oCMobMsg.SubTypes.EV_StartInteraction: //activated a mob
                        Vob vob = null;
                        if (World.vobAddr.TryGetValue(EM.OwnerAddress, out vob) && vob is MobInter)
                        {
                            MobMessage.WriteUseMob((MobInter)vob);
                        }
                        BlockMsg = true;
                        return;
                    case oCMobMsg.SubTypes.EV_EndInteraction:
                        break;
                    case oCMobMsg.SubTypes.EV_StartStateChange:
                        if (msg.StateChangeLeaving) //player wants to stop using the mob
                        {
                            MobMessage.WriteUnUseMob();
                            BlockMsg = true;
                            return;
                        }
                        break;
                    case oCMobMsg.SubTypes.EV_CallScript:
                    case oCMobMsg.SubTypes.EV_Lock:
                    case oCMobMsg.SubTypes.EV_Unlock:
                        BlockMsg = true;
                        return;
                }
            }
            BlockMsg = false;
        }

        static void OnMsgAttack(int emAddress, int msgAddress, int vobAddress)
        {
            if (vobAddress == Player.Hero.gVob.Address)
            {
                oCMsgAttack msg = new oCMsgAttack(Program.Process, msgAddress);

                switch (msg.SubType)
                {
                    case oCMsgAttack.SubTypes.AttackForward:
                        NPCMessage.WriteTargetState(NPCState.AttackForward);
                        break;
                    case oCMsgAttack.SubTypes.AttackLeft:
                        NPCMessage.WriteTargetState(NPCState.AttackLeft);
                        break;
                    case oCMsgAttack.SubTypes.AttackRight:
                        NPCMessage.WriteTargetState(NPCState.AttackRight);
                        break;
                    case oCMsgAttack.SubTypes.AttackRun:
                        NPCMessage.WriteTargetState(NPCState.AttackRun);
                        break;
                    case oCMsgAttack.SubTypes.Parade:
                        if ((msg.Bitfield & oCMsgAttack.BitFlag.Dodge) != 0)
                        {
                            NPCMessage.WriteTargetState(NPCState.DodgeBack);
                        }
                        else
                        {
                            NPCMessage.WriteTargetState(NPCState.Parry);
                        }
                        break;
                }
                BlockMsg = true;
                return;
            }
            BlockMsg = false;
        }

        static void OnMsgWeapon(int emAddress, int msgAddress, int vobAddress)
        {
            if (vobAddress == Player.Hero.gVob.Address)
            {
                oCMsgWeapon msg = new oCMsgWeapon(Program.Process, msgAddress);

                switch (msg.SubType)
                {
                    case oCMsgWeapon.SubTypes.DrawWeapon:
                    case oCMsgWeapon.SubTypes.DrawWeapon1:
                        byte Slot = 0; // = Item.Fists.Slot;
                        if (Player.lastUsedWeapon != null && Player.lastUsedWeapon.Slot != 0)
                        {
                            Slot = Player.lastUsedWeapon.Slot;
                        }
                        else
                        {
                            Slot = Player.Hero.equippedSlots.FirstOrDefault(x => x.Value.IsMeleeWeapon).Key;
                        }
                        NPCMessage.WriteDrawItem(Slot);
                        break;
                    case oCMsgWeapon.SubTypes.RemoveWeapon:
                    case oCMsgWeapon.SubTypes.RemoveWeapon1:
                        NPCMessage.WriteUndrawItem();
                        break;
                    case oCMsgWeapon.SubTypes.DrawWeapon2:
                    case oCMsgWeapon.SubTypes.RemoveWeapon2:
                        BlockMsg = false;
                        return;
                }
                BlockMsg = true;
                return;
            }
            BlockMsg = false;
        }

        static void OnMsgMovement(int emAddress, int msgAddress, int vobAddress)
        {
            if (vobAddress == Player.Hero.gVob.Address)
            {
                oCMsgMovement msg = new oCMsgMovement(Program.Process, msgAddress);
                if (msg.SubType == oCMsgMovement.SubTypes.Strafe)
                {
                    if (msg.Animation == Player.Hero.gVob.AniCtrl._t_strafel)
                    {
                        NPCMessage.WriteTargetState(NPCState.MoveLeft);
                    }
                    else if (msg.Animation == Player.Hero.gVob.AniCtrl._t_strafer)
                    {
                        NPCMessage.WriteTargetState(NPCState.MoveRight);
                    }
                }
                BlockMsg = true;
                return;
            }
            BlockMsg = false;
        }
    }
}
