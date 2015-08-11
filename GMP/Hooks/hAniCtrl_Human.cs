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

        public static Int32 hook_StartFallDownAni(String message)
        {
            try
            {
                CheckState(message, NPCState.Fall);
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

        public static Int32 hook_CheckHit(String message)
        {
            try
            {
                aniCtrl.BitField &= ~oCAniCtrl_Human.BitFlag.comboCanHit;

                Vob aVob;
                World.vobAddr.TryGetValue(aniCtrl.NPC.Address, out aVob);
                if (aVob != null && aVob is NPC)
                {
                    NPC attacker = (NPC)aVob;
                    List<NPC> hitlist = new List<NPC>();

                    foreach (NPC npc in World.npcDict.Values)
                    {
                        if (npc.gNpc.AniCtrl.Address == aniCtrl.Address) // self
                        {
                            continue;
                        }                        

                        if (aniCtrl.NPC.IsInFightRange(npc.gVob, 0) &&
                            aniCtrl.NPC.IsInFightFocus(npc.gVob) &&
                            aniCtrl.NPC.IsSameHeight(npc.gVob))
                        {
                            //if (!aniCtrl.NPC.GetDamageByType(16))
                            //FIXME: !Dead
                            if (npc.gNpc.AniCtrl.CanParade(aniCtrl.NPC))
                            {   //attack got parried, no one gets hit
                                aniCtrl.StartParadeEffects(npc.gNpc);
                                Program.Process.Write(new byte[] { 0xF0, 0x01, 0x00, 0x00 }, 0x6B03AA); // parade end
                                return 0;
                            }
                            hitlist.Add(npc);
                        }
                    }

                    if (hitlist.Count > 0)
                    {
                        foreach (NPC target in hitlist)
                        {
                            if (target == Player.Hero)
                            {
                                NPCMessage.WriteSelfHit(attacker);
                            }
                            aniCtrl.CreateHit(target.gVob);
                        }

                        if (attacker == Player.Hero)
                        {
                            NPCMessage.WriteHits(attacker, hitlist);
                        }
                    }
                }
                Program.Process.Write(new byte[] { 0xCF, 0x00, 0x00, 0x00 }, 0x6B03AA); //continue
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
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_StartFallDownAni"), 0x6B5220, 6, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("HitCombo"), (int)oCAniCtrl_Human.FuncOffsets.HitCombo, 6, 2); //entry

            process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0xE9 }, 0x6B03A4); //clear
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_CheckHit"), (int)0x6B03A4, 5, 0);
        }
    }
}
