using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Client.WorldObjects;
using Gothic.zClasses;
using GUC.Enumeration;
using GUC.Client.Network.Messages;

namespace GUC.Client.Hooks
{
    static class hAniCtrl_Human
    {
        const int DelayBetweenMessages = 3000000; //300ms

        static bool blockFwd = false;
        static bool _BlockFwd
        {
            get { return blockFwd; }
            set
            {
                if (blockFwd != value)
                {
                    if (value)
                    {
                        Program.Process.Write(new byte[] { 0xE9, 0x99, 0x00, 0x00 /*,0x00*/ }, 0x6B7906);
                    }
                    else
                    {
                        Program.Process.Write(new byte[] { 0x8B, 0x86, 0x4C, 0x01 /*,0x00*/ }, 0x6B7906);
                    }
                    blockFwd = value;
                }
            }
        }

        static bool blockStd = false;
        static bool _BlockStd
        {
            get { return blockStd; }
            set
            {
                if (blockStd != value)
                {
                    if (value)
                    {
                        Program.Process.Write(new byte[] { 0xE9, 0x55, 0x04, 0x00, 0x00 }, 0x6B7495);
                    }
                    else
                    {
                        Program.Process.Write(new byte[] { 0x57, 0x8B, 0x7E, 0x68, 0x85 }, 0x6B7495);
                    }
                    blockStd = value;
                }
            }
        }

        static bool blockBwd = false;
        static bool _BlockBwd
        {
            get { return blockBwd; }
            set
            {
                if (blockBwd != value)
                {
                    if (value)
                    {
                        Program.Process.Write(new byte[] { 0xEB, 0x67 }, 0x6B7BC5);
                    }
                    else
                    {
                        Program.Process.Write(new byte[] { 0x8B, 0x86 }, 0x6B7BC5);
                    }
                    blockBwd = value;
                }
            }
        }

        static bool blockJmp = false;
        static bool _BlockJmp
        {
            get { return blockJmp; }
            set
            {
                if (blockJmp != value)
                {
                    if (value)
                    {
                        Program.Process.Write(new byte[] { 0xE9, 0xE0, 0x01 /*, 0x00, 0x00*/ }, 0x6B21E6);
                    }
                    else
                    {
                        Program.Process.Write(new byte[] { 0x80, 0xA6, 0xB8 /*, 0x00, 0x00*/ }, 0x6B21E6);
                    }
                    blockJmp = value;
                }
            }
        }

        public static Int32 _Forward(String message)
        {
            try
            {
                if (Player.Hero != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Program.Process.ReadInt(address);

                    if (thisAddr == Player.Hero.gAniCtrl.Address)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk*/ && bs <= 7 /*dive*/)
                        {
                            if (Player.Hero.State != NPCState.MoveForward)
                            {
                                if (DateTime.UtcNow.Ticks > Player.Hero.nextForwardUpdate)
                                {
                                    NPCMessage.WriteState(NPCState.MoveForward, Player.Hero);
                                    Player.Hero.nextForwardUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
                                }
                                _BlockFwd = true;
                                return 0;
                            }
                        }
                    }
                }
                _BlockFwd = false;
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        static long count = 0;
        public static Int32 _Stand(String message)
        {
            try
            {
                //zERROR.GetZErr(Program.Process).Report(2, 'G', "Stand " + count++, 0, "hAniCtrl_Human.cs", 0);

                if (Player.Hero != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Program.Process.ReadInt(address);

                    if (thisAddr == Player.Hero.gAniCtrl.Address)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk*/ && bs <= 7 /*dive*/)
                        {
                            if (Player.Hero.State != NPCState.Stand)
                            {
                                if (DateTime.UtcNow.Ticks > Player.Hero.nextStandUpdate)
                                {
                                    NPCMessage.WriteState(NPCState.Stand, Player.Hero);
                                    Player.Hero.nextStandUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
                                }
                                _BlockStd = true;
                                return 0;
                            }
                        }
                    }
                }
                _BlockStd = false;
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static Int32 _Backward(String message)
        {
            try
            {
                if (Player.Hero != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Program.Process.ReadInt(address);

                    if (thisAddr == Player.Hero.gAniCtrl.Address)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk*/ && bs <= 7 /*dive*/)
                        {
                            if (Player.Hero.State != NPCState.MoveBackward)
                            {
                                if (DateTime.UtcNow.Ticks > Player.Hero.nextBackwardUpdate)
                                {
                                    NPCMessage.WriteState(NPCState.MoveBackward, Player.Hero);
                                    Player.Hero.nextBackwardUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
                                }
                                _BlockBwd = true;
                                return 0;
                            }
                        }
                    }
                }
                _BlockBwd = false;
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static Int32 JumpForward(String message)
        {
            try
            {
                if (Player.Hero != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Program.Process.ReadInt(address);

                    if (thisAddr == Player.Hero.gAniCtrl.Address)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk && bs <= 7 /*dive)
                        {
                            if (!Player.Hero.DoJump) //not trying to jump right now
                            {
                                NPCMessage.WriteJump(Player.Hero);
                                _BlockJmp = true;
                                return 0;
                            }
                            Player.Hero.DoJump = false;
                        }
                    }
                }
                _BlockJmp = false;
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static Int32 RunJump(String message)
        {
            try
            {
                if (Player.Hero != null)
                {
                    NPCMessage.WriteJump(Player.Hero);
                }
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        static bool blockCombo = false;
        static bool _BlockCombo
        {
            get { return blockCombo; }
            set
            {
                if (blockCombo != value)
                {
                    if (value)
                    {
                        Program.Process.Write(new byte[] { 0xEB }, 0x6B02B7);
                    }
                    else
                    {
                        Program.Process.Write(new byte[] { 0x74 }, 0x6B02B7);
                    }
                    blockCombo = value;
                }
            }
        }

        public static bool DoCombo = false;
        public static Int32 HitCombo(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                int thisAddr = Program.Process.ReadInt(address);

                if (thisAddr == Player.Hero.gAniCtrl.Address && Program.Process.ReadInt(address + 4) != 0)
                {
                    if (!DoCombo)
                    {
                        NPCMessage.WriteTargetState(NPCState.AttackForward);
                        _BlockCombo = true;
                        return 0;
                    }
                    DoCombo = false;
                }
                _BlockCombo = false;
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static void AddHooks(Process process)
        {
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("_Forward"), (int)oCAniCtrl_Human.FuncOffsets._Forward, 6, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("_Backward"), (int)oCAniCtrl_Human.FuncOffsets._Backward, 5, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("_Stand"), (int)oCAniCtrl_Human.FuncOffsets._Stand, 5, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("JumpForward"), (int)oCAniCtrl_Human.FuncOffsets.JumpForward, 6, 1);

            process.Write(Enumerable.Repeat<byte>(0x90, 25).ToArray(), 0x6B1F13); //erase PC_ForwardJump runjump
            process.Write(Enumerable.Repeat<byte>(0x90, 5).ToArray(), 0x6B1F32);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("RunJump"), 0x6B1F32, 5, 1);

            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_StartFallDownAni"), 0x6B5220, 6, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("HitCombo"), (int)oCAniCtrl_Human.FuncOffsets.HitCombo, 6, 2);
            process.Write(new byte[] { 0xE9, 0x48, 0x01, 0x00 /*, 0x00*/ }, 0x6B0330); //skip hit detection

            //make combos always available:
            process.Write(Enumerable.Repeat<byte>(0x90, 10).ToArray(), 0x6B02B9);
            process.Write(Enumerable.Repeat<byte>(0x90, 16).ToArray(), 0x6B02D7);
        }
    }
}
