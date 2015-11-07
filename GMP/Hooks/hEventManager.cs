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
            if (msg.UserAddress == Player.Hero.gVob.Address)
            {
                zCEventManager EM = new zCEventManager(Program.Process, emAddress); //EventManager of the used Mob

                if (msg.SubType == oCMobMsg.SubTypes.EV_StartInteraction) //activated a mob
                {
                    AbstractVob vob = null;
                    if (World.vobAddr.TryGetValue(EM.OwnerAddress, out vob) && vob is MobInter)
                    {
                        MobMessage.WriteUseMob((MobInter)vob);
                    }
                }
                else if (msg.SubType == oCMobMsg.SubTypes.EV_StartStateChange)
                {
                    if (msg.StateChangeLeaving) //player wants to stop using the mob
                    {

                    }
                    else //players
                    {

                    }
                }
                BlockMsg = true;
                //return;
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
                    //FIXME: Magic!
                    case oCMsgWeapon.SubTypes.DrawWeapon:
                    case oCMsgWeapon.SubTypes.DrawWeapon1:
                        if ((msg.WpType == 4 || msg.WpType == 5) && Player.Hero.gNpc.IsInInv(Player.Hero.gNpc.GetEquippedRangedWeapon().Munition, 1).Address != 0) //ranged
                        {
                            NPCMessage.WriteWeaponState(NPCWeaponState.Ranged, false);
                        }
                        else if (Player.Hero.gNpc.GetEquippedMeleeWeapon().Address == 0) //no weapon equipped
                        {
                            NPCMessage.WriteWeaponState(NPCWeaponState.Fists, false);
                        }
                        else// if (Player.Hero.WeaponState != NPCWeaponState.Fists)
                        { ////Don't change the state if we want to get fists while a weapon is equipped
                            NPCMessage.WriteWeaponState(NPCWeaponState.Melee, false);
                        }
                        break;
                    case oCMsgWeapon.SubTypes.RemoveWeapon:
                        NPCMessage.WriteWeaponState(NPCWeaponState.None, false);
                        break;
                    case oCMsgWeapon.SubTypes.RemoveWeapon1:
                        NPCMessage.WriteWeaponState(NPCWeaponState.None, true);
                        break;
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
                    if (msg.Animation == Player.Hero.gNpc.AniCtrl._t_strafel)
                    {
                        NPCMessage.WriteTargetState(NPCState.MoveLeft);
                    }
                    else if (msg.Animation == Player.Hero.gNpc.AniCtrl._t_strafer)
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
