using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using GUC.Scripting;
using System.Reflection;
using System.IO;
using GUC.Client.Scripts.Sumpfkraut.Menus;
using GUC.Client;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public static bool Ingame = false;

        public GUCScripts()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            Logger.Log("SumpfkrautOnline ClientScripts loaded!");
        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            Assembly asm = Assembly.LoadFrom(Path.GetFullPath("System\\Multiplayer\\UntoldChapters\\SumpfkrautOnline\\" + name + ".dll"));
            if (asm == null)
            {
                asm = Assembly.LoadFrom(Path.GetFullPath("Multiplayer\\UntoldChapters\\SumpfkrautOnline\\" + name + ".dll"));
            }
            return asm;
        }

        public void Update(long ticks)
        {
            GUCMenu.UpdateMenus(ticks);
            Client.Scripts.TFFA.InputControl.Update(ticks);
            CheckMusic();
        }

        public void StartOutgame()
        {
            WinApi.Process.Write(new byte[] { 0xE9, 0x99, 0x04, 0x00, 0x00 }, 0x0067836C); // always do T_GOTHIT instead of T_STUMBLE/B when getting hit

            Client.Scripts.TFFA.InputControl.Init();
            Client.Scripts.TFFA.MainMenu.Menu.Open();
            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
            Client.Scripts.TFFA.InputControl.Init();
            //GUCMenu.CloseActiveMenus();
            Gothic.Objects.oCNpcFocus.SetFocusMode(1);
            Ingame = true;
            Logger.Log("Ingame started.");
        }

        bool fightMusicEnabled = false;
        void CheckMusic()
        {
            if (!Ingame || TFFA.TFFAClient.Info == null)
                return;

            var heroTeam = TFFA.TFFAClient.Info.Team;
            var hero = TFFA.TFFAClient.Client.Character;

            if (TFFA.TFFAClient.Status != TFFA.TFFAPhase.Fight || heroTeam == TFFA.Team.Spec || hero == null)
            {
                if (fightMusicEnabled)
                {
                    fightMusicEnabled = false;
                    SoundHandler.CurrentMusicType = SoundHandler.MusicType.Normal;
                }
            }
            else
            {
                var gHero = hero.BaseInst.gVob;
                var heroPos = hero.BaseInst.GetPosition();

                float nearestEnemy = float.MaxValue;
                float nearestTeammate = float.MaxValue;
                foreach (TFFA.ClientInfo ci in TFFA.ClientInfo.ClientInfos.Values)
                {
                    if (ci.Team != TFFA.Team.Spec)
                    {
                        WorldObjects.NPC npc;
                        if (WorldObjects.World.Current.TryGetVob(ci.CharID, out npc) && !npc.IsDead)
                        {
                            float distance = npc.GetPosition().GetDistance(heroPos);
                            if (ci.Team == heroTeam)
                            {
                                if (distance < nearestTeammate)
                                    nearestTeammate = distance;
                            }
                            else if (npc.gVob.FreeLineOfSight(gHero))
                            {
                                if (distance < nearestEnemy)
                                    nearestEnemy = distance;
                            }
                        }
                    }
                }

                if (fightMusicEnabled)
                {
                    // enemy is too far away or hero is dead and no teammates are nearby
                    if (nearestEnemy > 1000 || hero.BaseInst.IsDead && nearestTeammate > 1000) 
                    {
                        fightMusicEnabled = false;
                        SoundHandler.CurrentMusicType = SoundHandler.MusicType.Normal;
                    }
                }
                else
                {
                    // enemy is close enough and hero is not dead or teammates are nearby
                    if (nearestEnemy < 500 && (!hero.BaseInst.IsDead || nearestTeammate < 1000))
                    {
                        fightMusicEnabled = true;
                        SoundHandler.CurrentMusicType = SoundHandler.MusicType.Fight;
                    }
                }
            }
        }
    }
}
