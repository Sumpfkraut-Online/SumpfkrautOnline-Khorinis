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
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hEventManager).GetMethod("hook_OnMessage"), (int)zCEventManager.FuncOffsets.OnMessage, 7, 3); //hook
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
                        int thisAddr = Program.Process.ReadInt(address);

                        if (thisAddr == Player.Hero.gVob.GetEM(0).Address)
                        {
                            int msgAddr = Program.Process.ReadInt(address + 4);
                            int vobAddr = Program.Process.ReadInt(address + 8);

                            zCEventMessage eventMsg = new zCEventMessage(Program.Process, msgAddr);
                            zCVob vob = new zCVob(Program.Process, vobAddr);

                            switch ((zCObject.VobTypes)eventMsg.VTBL)
                            {
                                case zCObject.VobTypes.oCMsgAttack:
                                    OnMsgAttack(new oCMsgAttack(Program.Process, msgAddr));
                                    return 0;
                                case zCObject.VobTypes.oCMsgWeapon:
                                    OnMsgWeapon(new oCMsgWeapon(Program.Process, msgAddr));
                                    break;
                                case zCObject.VobTypes.oCMsgMovement:
                                    OnMsgMovement(new oCMsgMovement(Program.Process, msgAddr));
                                    break;
                            }
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

        static void OnMsgAttack(oCMsgAttack msg)
        {
            BlockMsg = true;
            NPCState state;

            switch (msg.SubType)
            {
                case oCMsgAttack.SubTypes.AttackForward:
                    state = NPCState.AttackForward;
                    break;
                case oCMsgAttack.SubTypes.AttackLeft:
                    state = NPCState.AttackLeft;
                    break;
                case oCMsgAttack.SubTypes.AttackRight:
                    state = NPCState.AttackRight;
                    break;
                case oCMsgAttack.SubTypes.AttackRun:
                    state = NPCState.AttackRun;
                    break;
                case oCMsgAttack.SubTypes.Parade:
                    if ((msg.Bitfield & oCMsgAttack.BitFlag.Dodge) != 0)
                    {
                        state = NPCState.DodgeBack;
                    }
                    else
                    {
                        state = NPCState.Parry;
                    }
                    break;
                default:
                    return;
            }
            NPCMessage.WriteTargetState(state);
        }

        static void OnMsgWeapon(oCMsgWeapon msg)
        {
            BlockMsg = true;

            bool removeType1 = false; //there are 2 animations of undrawing a weapon
            NPCWeaponState state = NPCWeaponState.Melee;

            switch (msg.SubType)
            {
                //FIXME: Magic!
                case oCMsgWeapon.SubTypes.DrawWeapon:
                case oCMsgWeapon.SubTypes.DrawWeapon1:
                    if ((msg.WpType == 4 || msg.WpType == 5) && Player.Hero.gNpc.IsInInv(Player.Hero.gNpc.GetEquippedRangedWeapon().Munition, 1).Address != 0) //ranged
                    {
                        state = NPCWeaponState.Ranged;
                    }
                    else if (Player.Hero.gNpc.GetEquippedMeleeWeapon().Address == 0) //no weapon equipped
                    {
                        state = NPCWeaponState.Fists;
                    }
                    else if (Player.Hero.WeaponState != NPCWeaponState.Fists)
                    { //Don't change the state if we want to get fists while a weapon is equipped
                        state = NPCWeaponState.Melee;
                    }
                    break;
                case oCMsgWeapon.SubTypes.RemoveWeapon:
                    state = NPCWeaponState.None;
                    break;
                case oCMsgWeapon.SubTypes.RemoveWeapon1:
                    state = NPCWeaponState.None;
                    removeType1 = true;
                    break;
                default:
                    return;
            }
            NPCMessage.WriteWeaponState(state, removeType1);
        }

        static void OnMsgMovement(oCMsgMovement msg)
        {
            if (msg.SubType == oCMsgMovement.SubTypes.Strafe)
            {
                NPCState state;
                if (msg.Animation == Player.Hero.gNpc.AniCtrl._t_strafel)
                {
                    state = NPCState.MoveLeft;
                }
                else if (msg.Animation == Player.Hero.gNpc.AniCtrl._t_strafer)
                {
                    state = NPCState.MoveRight;
                }
                else return;
                NPCMessage.WriteTargetState(state);
            }
        }


    }
}
