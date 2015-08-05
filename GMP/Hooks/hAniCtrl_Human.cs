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
        static void CheckState(String message, NPCState state)
        {
            int thisAddr = Program.Process.ReadInt(Convert.ToInt32(message));

            if (thisAddr != Player.Hero.gNpc.AniCtrl.Address)
                return;

            if (Player.Hero.State != state)
            {
                Player.Hero.State = state;
                NPCMessage.WriteState(Player.Hero);
            }
        }

        public static Int32 _Forward(String message)
        {
            try
            {
                CheckState(message, NPCState.MoveForward);
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static Int32 _Stand(String message)
        {
            try
            {
                CheckState(message, NPCState.Stand);
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
                CheckState(message, NPCState.MoveBackward);
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
                CheckState(message, NPCState.Jump);
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        /* 
         * FIGHTING
         */


        static oCAniCtrl_Human aniCtrl = null;
        public static Int32 HitCombo(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                int thisAddr = Program.Process.ReadInt(address);

                aniCtrl = new oCAniCtrl_Human(Program.Process, thisAddr);
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        static NPC GetNextNPC()
        {
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is NPC && ((NPC)enumerator.Current).gNpc.AniCtrl.Address != aniCtrl.Address && enumerator.Current.gVob.Address != originalAddr)
                {
                    return (NPC)enumerator.Current;
                }
            }
            return null;
        }

        static int originalAddr;
        static IEnumerator<Vob> enumerator = null;
        static NPC current = null;
        static bool done = false;

        public static Int32 CheckHit(String message)
        {
            try
            {
                if (enumerator == null)
                {   //first call
                    Program.Process.Write(new byte[] { 0xE9, 0x22, 0xFF, 0xFF, 0xFF, 0x90 }, 0x6B047D); //write a jmp back
                    Program.Process.Write(new byte[] { 0xE9, 0x35, 0xFF, 0xFF, 0xFF }, 0x6B046A); //Parade return -> jmp back

                    Program.Process.Write(new byte[] { 0x00, 0x24, 0xFB, 0x88, 0x86, 0xB0, 0x01, 0x00, 0x00 }, 0x6B03A9); //something

                    originalAddr = aniCtrl.HitTarget;
                    enumerator = World.AllVobs.GetEnumerator();
                }
                else if (!done)
                {
                    Program.Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }, 0x6B03A9); //something
                    done = true;
                }

                current = GetNextNPC();

                if (current != null)
                {
                    aniCtrl.HitTarget = current.gNpc.Address;
                }
                else
                { // revert changes, last call
                    Program.Process.Write(new byte[] { 0x8B, 0x86, 0xB4, 0x01, 0x00, 0x00 }, 0x6B047D);
                    Program.Process.Write(new byte[] { 0xE9, 0x2F, 0x01, 0x00, 0x00 }, 0x6B046A);

                    aniCtrl.HitTarget = originalAddr;
                    enumerator = null;
                    done = false;
                }
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static Int32 hook_CatchHit(String message)
        {
            try
            {
                if (aniCtrl.HitTarget == Player.Hero.gVob.Address)
                {
                    Vob attacker;
                    World.vobAddr.TryGetValue(aniCtrl.NPC.Address, out attacker);
                    if (attacker != null && attacker is NPC)
                    {
                        NPCMessage.WriteHits((NPC)attacker);
                    }
                }
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "hAniCtrl_Human.cs", 0);
            }
            return 0;
        }

        public static Int32 hook_CatchCombo(String message)
        {
            try
            {
                if (aniCtrl.Address == Player.Hero.gNpc.AniCtrl.Address)
                {
                    NPCMessage.WriteAttack();
                }
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
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("JumpForward"), (int)oCAniCtrl_Human.FuncOffsets.PC_JumpForward, 5, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("HitCombo"), (int)oCAniCtrl_Human.FuncOffsets.HitCombo, 6, 2); //entry
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_CatchCombo"), 0x6B02E7, 8, 0);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("CheckHit"), 0x6B03A4, 6, 0); //loop entry

            process.Write(Enumerable.Repeat<byte>(0x90, 0xE).ToArray(), 0x6B046F); //remove CreateHit call completely
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_CatchHit"), 0x6B046F, 5, 0);
            
            

        }
    }
}
