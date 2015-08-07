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
        public static Int32 hook_OnMessage(String message)
        {
            try
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
                            break;
                        case zCObject.VobTypes.oCMsgWeapon:
                            OnMsgWeapon(new oCMsgWeapon(Program.Process, msgAddr));
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "Program.cs", 0);
            }
            return 0;
        }

        static void OnMsgAttack(oCMsgAttack msg)
        {
            switch (msg.SubType)
            {
                case oCMsgAttack.SubTypes.AttackForward:
                    Player.Hero.State = NPCState.AttackForward;
                    break;
                case oCMsgAttack.SubTypes.AttackLeft:
                    Player.Hero.State = NPCState.AttackLeft;
                    break;
                case oCMsgAttack.SubTypes.AttackRight:
                    Player.Hero.State = NPCState.AttackRight;
                    break;
                case oCMsgAttack.SubTypes.AttackRun:
                    Player.Hero.State = NPCState.AttackRun;
                    break;
                case oCMsgAttack.SubTypes.Parade:
                    if ((msg.Bitfield & oCMsgAttack.BitFlag.Dodge) != 0)
                    {
                        Player.Hero.State = NPCState.DodgeBack;
                    }
                    else
                    {
                        Player.Hero.State = NPCState.Parry;
                    }
                    break;
                default:
                    return;
            }
            NPCMessage.WriteAttack();
        }

        static void OnMsgWeapon(oCMsgWeapon msg)
        {
            switch (msg.SubType)
            {
                //FIXME: Magic!
                case oCMsgWeapon.SubTypes.DrawWeapon:
                case oCMsgWeapon.SubTypes.DrawWeapon1:
                    if ((msg.WpType == 4 || msg.WpType == 5) && Player.Hero.gNpc.IsInInv(Player.Hero.gNpc.GetEquippedRangedWeapon().Munition, 1).Address != 0) //ranged
                    {
                        Player.Hero.WeaponState = NPCWeaponState.Ranged; 
                    }
                    else if (Player.Hero.gNpc.GetEquippedMeleeWeapon().Address == 0) //no weapon equipped
                    {
                        Player.Hero.WeaponState = NPCWeaponState.Fists;
                    }
                    else if (Player.Hero.WeaponState != NPCWeaponState.Fists)
                    { //Don't change the state if we want to get fists while a weapon is equipped
                        Player.Hero.WeaponState = NPCWeaponState.Melee;
                    }
                    break;
                case oCMsgWeapon.SubTypes.RemoveWeapon:
                case oCMsgWeapon.SubTypes.RemoveWeapon1:
                    Player.Hero.WeaponState = NPCWeaponState.None;
                    break;
                default:
                    return;

            }
            NPCMessage.WriteWeaponState();
        }

        public static void AddHooks(Process process)
        {
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hEventManager).GetMethod("hook_OnMessage"), (int)zCEventManager.FuncOffsets.OnMessage, 7, 3);
        }
    }
}
