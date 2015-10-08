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
        public static Int32 _Forward(String message)
        {
            try
            {
                if (Program.Process.ReadInt(Convert.ToInt32(message)) != Player.Hero.gNpc.AniCtrl.Address)
                    return 0;

                int bs = Player.Hero.gNpc.GetBodyState();
                if (bs < 1 /*walk*/ || bs > 7 /*dive*/)
                    return 0;

                if (Player.Hero.State != NPCState.MoveForward)
                {
                    Player.Hero.State = NPCState.MoveForward;
                    NPCMessage.WriteState(Player.Hero);
                }
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
                if (Program.Process.ReadInt(Convert.ToInt32(message)) != Player.Hero.gNpc.AniCtrl.Address)
                    return 0;

                if (Player.Hero.gNpc.GetBodyState() != 0 /*stand*/)
                    return 0;

                if (Player.Hero.State != NPCState.Stand)
                {
                    Player.Hero.State = NPCState.Stand;
                    NPCMessage.WriteState(Player.Hero);
                }
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
                if (Program.Process.ReadInt(Convert.ToInt32(message)) != Player.Hero.gNpc.AniCtrl.Address)
                    return 0;

                int bs = Player.Hero.gNpc.GetBodyState();
                if (bs < 1 /*walk*/ || bs > 6 /*crawl, wat*/)
                    return 0;

                if (Player.Hero.State != NPCState.MoveBackward)
                {
                    Player.Hero.State = NPCState.MoveBackward;
                    NPCMessage.WriteState(Player.Hero);
                }
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
                if (Program.Process.ReadInt(Convert.ToInt32(message)) != Player.Hero.gNpc.AniCtrl.Address)
                    return 0;

                //FIXME und so

                if (Player.Hero.State != NPCState.Jump)
                {
                    Player.Hero.State = NPCState.Jump;
                    NPCMessage.WriteState(Player.Hero);
                }
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
                if (Program.Process.ReadInt(Convert.ToInt32(message)) != Player.Hero.gNpc.AniCtrl.Address)
                    return 0;

                if (Player.Hero.State != NPCState.Fall)
                {
                    Player.Hero.State = NPCState.Fall;
                    NPCMessage.WriteState(Player.Hero);
                }
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

        public static Int32 DoCombo(String message)
        {
            try
            {
                if (aniCtrl.Address == Player.Hero.gNpc.AniCtrl.Address)
                {
                    Player.Hero.State = NPCState.AttackForward;
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
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_StartFallDownAni"), 0x6B5220, 6, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("HitCombo"), (int)oCAniCtrl_Human.FuncOffsets.HitCombo, 6, 2); //entry

            process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0xE9 }, 0x6B03A4); //clear
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("hook_CheckHit"), (int)0x6B03A4, 5, 0);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hAniCtrl_Human).GetMethod("DoCombo"), (int)0x6B02E7, 8, 0); //entry
        }
    }
}
