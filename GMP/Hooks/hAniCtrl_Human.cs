using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Objects;
using GUC.Network;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Log;
using GUC.Client.Network.Messages;

namespace GUC.Client.Hooks
{
    static class hAniCtrl_Human
    {
        public static void AddHooks()
        {
            Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("_Forward"), oCAniCtrl_Human.FuncAddresses._Forward, 6, 1);
            Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("_Backward"), oCAniCtrl_Human.FuncAddresses._Backward, 5, 1);
            Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("_Stand"), oCAniCtrl_Human.FuncAddresses._Stand, 5, 1);
            Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("JumpForward"), oCAniCtrl_Human.FuncAddresses.JumpForward, 6, 1);

            // The Runjump is always blocked, we play the animation instead (which is the same) when told to
            Process.Write(Enumerable.Repeat<byte>(0x90, 25).ToArray(), 0x6B1F13); //erase PC_ForwardJump runjump
            Process.Write(Enumerable.Repeat<byte>(0x90, 5).ToArray(), 0x6B1F32);
            Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("RunJump"), 0x6B1F32, 5, 1);

            //Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("hook_StartFallDownAni"), 0x6B5220, 6, 1);

            //Process.Hook(Program.GUCDll, typeof(hAniCtrl_Human).GetMethod("HitCombo"), oCAniCtrl_Human.FuncAddresses.HitCombo, 6, 2);
            //Process.Write(new byte[] { 0xE9, 0x48, 0x01, 0x00 /*, 0x00*/ }, 0x6B0330); //skip hit detection

            //make combos always available:
            // Process.Write(Enumerable.Repeat<byte>(0x90, 10).ToArray(), 0x6B02B9);
            //Process.Write(Enumerable.Repeat<byte>(0x90, 16).ToArray(), 0x6B02D7);

            Logger.Log("Added AniCtrl_Human hooks.");
        }

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
                        Process.Write(new byte[] { 0xE9, 0x99, 0x00, 0x00 /*,0x00*/ }, 0x6B7906);
                    }
                    else
                    {
                        Process.Write(new byte[] { 0x8B, 0x86, 0x4C, 0x01 /*,0x00*/ }, 0x6B7906);
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
                        Process.Write(new byte[] { 0xE9, 0x55, 0x04, 0x00, 0x00 }, 0x6B7495);
                    }
                    else
                    {
                        Process.Write(new byte[] { 0x57, 0x8B, 0x7E, 0x68, 0x85 }, 0x6B7495);
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
                        Process.Write(new byte[] { 0xEB, 0x67 }, 0x6B7BC5);
                    }
                    else
                    {
                        Process.Write(new byte[] { 0x8B, 0x86 }, 0x6B7BC5);
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
                        Process.Write(new byte[] { 0xE9, 0xE0, 0x01 /*, 0x00, 0x00*/ }, 0x6B21E6);
                    }
                    else
                    {
                        Process.Write(new byte[] { 0x80, 0xA6, 0xB8 /*, 0x00, 0x00*/ }, 0x6B21E6);
                    }
                    blockJmp = value;
                }
            }
        }

        public static Int32 _Forward(String message)
        {
            try
            {
                if (GameClient.Client.Character != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Process.ReadInt(address);

                    NPC npc = thisAddr == GameClient.Client.Character.gVob.AniCtrl.Address ? GameClient.Client.Character : null;
                    //NPC npc = Player.Hero.gAniCtrl.Address == thisAddr ? Player.Hero : (NPC)Player.VobControlledList.Find(v => v is NPC && ((NPC)v).gAniCtrl.Address == thisAddr);

                    if (npc != null)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk*/ && bs <= 7 /*dive*/)
                        {
                            if (npc.State != NPCStates.MoveForward)
                            {
                                if (DateTime.UtcNow.Ticks > npc.nextForwardUpdate)
                                {
                                    NPCMessage.WriteState(npc, NPCStates.MoveForward);
                                    npc.nextForwardUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
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
                Logger.LogError(e);
            }
            return 0;
        }
        
        public static Int32 _Stand(String message)
        {
            try
            {
                if (GameClient.Client.Character != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Process.ReadInt(address);

                    NPC npc = thisAddr == GameClient.Client.Character.gVob.AniCtrl.Address ? GameClient.Client.Character : null;
                    //NPC npc = Player.Hero.gAniCtrl.Address == thisAddr ? Player.Hero : (NPC)Player.VobControlledList.Find(v => v is NPC && ((NPC)v).gAniCtrl.Address == thisAddr);

                    if (npc != null)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk*/ && bs <= 7 /*dive*/)
                        {
                            if (npc.State != NPCStates.Stand)
                            {
                                if (DateTime.UtcNow.Ticks > npc.nextStandUpdate)
                                {
                                    NPCMessage.WriteState(npc, NPCStates.Stand);
                                    npc.nextStandUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
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
                Logger.LogError(e);
            }
            return 0;
        }

        static long nextBackwardUpdate = 0;
        public static Int32 _Backward(String message)
        {
            try
            {
                if (GameClient.Client.Character != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Process.ReadInt(address);

                    NPC npc = thisAddr == GameClient.Client.Character.gVob.AniCtrl.Address ? GameClient.Client.Character : null;

                    if (npc != null)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk*/ && bs <= 7 /*dive*/)
                        {
                            if (npc.State != NPCStates.MoveBackward)
                            {
                                if (DateTime.UtcNow.Ticks > nextBackwardUpdate)
                                {
                                    NPCMessage.WriteState(npc, NPCStates.MoveBackward);
                                    nextBackwardUpdate = DateTime.UtcNow.Ticks + DelayBetweenMessages;
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
                Logger.LogError(e);
            }
            return 0;
        }

        public static Int32 JumpForward(String message)
        {
            try
            {
                if (GameClient.Client.Character != null)
                {
                    int address = Convert.ToInt32(message);
                    int thisAddr = Process.ReadInt(address);

                    NPC npc = thisAddr == GameClient.Client.Character.gVob.AniCtrl.Address ? GameClient.Client.Character : null;
                    //NPC npc = Player.Hero.gAniCtrl.Address == thisAddr ? Player.Hero : (NPC)Player.VobControlledList.Find(v => v is NPC && ((NPC)v).gAniCtrl.Address == thisAddr);

                    if (npc != null)
                    {
                        //int bs = Player.Hero.gNpc.GetBodyState();
                        //if (bs >= 1 /*walk && bs <= 7 /*dive)
                        {
                            if (!npc.DoJump) // server didn't tell us to jump right now
                            {
                                NPCMessage.WriteJump(npc);
                                _BlockJmp = true;
                                return 0;
                            }
                            npc.DoJump = false;
                        }
                    }
                }
                _BlockJmp = false;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }

        public static Int32 RunJump(String message)
        {
            try
            {
                if (GameClient.Client.Character != null)
                {
                    NPCMessage.WriteJump(GameClient.Client.Character);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
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
                        Process.Write(new byte[] { 0xEB }, 0x6B02B7);
                    }
                    else
                    {
                        Process.Write(new byte[] { 0x74 }, 0x6B02B7);
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
                int thisAddr = Process.ReadInt(address);

                /*if (thisAddr == Player.Hero.gAniCtrl.Address && Process.ReadInt(address + 4) != 0)
                {
                    if (!DoCombo)
                    {
                        NPCMessage.WriteTargetState(NPCState.AttackForward);
                        _BlockCombo = true;
                        return 0;
                    }
                    DoCombo = false;
                }*/
                _BlockCombo = false;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
    }
}
