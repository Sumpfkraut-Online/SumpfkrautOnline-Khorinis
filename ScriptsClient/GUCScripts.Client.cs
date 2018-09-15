using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using GUC.Scripting;
using System.Reflection;
using System.IO;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Scripts.Sumpfkraut.Controls;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Arena.Duel;
using GUC.Types;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public static bool Ingame = false;

        partial void pConstruct()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            InputControl.Active = new Arena.Controls.ArenaControl();

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

        public static event Action<long> OnUpdate;
        public void Update(long ticks)
        {
            GUCMenu.UpdateMenus(ticks);
            InputControl.UpdateControls(ticks);
            OnUpdate?.Invoke(ticks);
            CheckMusic();
            CheckPosition();

            //if (Sumpfkraut.WorldSystem.WorldInst.Current != null)
            //    Sumpfkraut.WorldSystem.WorldInst.Current.Barrier.SetNextWeight(Sumpfkraut.WorldSystem.WorldInst.Current.Clock.Time, 1);
        }

        SoundInstance menuTheme = null;
        public static event Action OnOutgame;
        public void StartOutgame()
        {
            var theme = new SoundDefinition("INSTALLER_LOOP.WAV");
            theme.zSFX.IsFixed = true;
            theme.zSFX.Volume = 0.5f;
            theme.zSFX.SetLooping(true);
            menuTheme = SoundHandler.PlaySound(theme, 0.5f);

            Arena.Menus.MainMenu.Menu.Open();

            OnOutgame?.Invoke();
            Logger.Log("Outgame started.");
        }

        public static event Action OnIngame;
        public void StartIngame()
        {
            // stop oCAniCtrl_Human::_Stand from canceling the s_bowaim animation
            WinApi.Process.Write(0x006B7772, 0xEB, 0x69);

            // remove SetTorchAni
            WinApi.Process.Write(0x0073B410, 0xC2, 0x08, 0x00);
            Gothic.Objects.oCNpcFocus.SetFocusMode(1);

            if (menuTheme != null)
            {
                SoundHandler.StopSound(menuTheme);
                menuTheme = null;
            }

            Ingame = true;

            OnIngame?.Invoke();
            Logger.Log("Ingame started.");
        }

        static void CheckMusic()
        {
            if (Arena.ArenaClient.GMJoined && Arena.GameModes.GameMode.IsActive)
            {
                var phase = Arena.GameModes.GameMode.ActiveMode.Phase;
                if (phase == Arena.GameModes.GamePhase.WarmUp)
                {
                    SoundHandler.CurrentMusicType = SoundHandler.MusicType.Threat;
                    return;
                }
                else if (phase == Arena.GameModes.GamePhase.FadeOut)
                {
                    SoundHandler.CurrentMusicType = SoundHandler.MusicType.Normal;
                    return;
                }
                if (Arena.GameModes.Horde.HordeMode.IsActive && phase > Arena.GameModes.GamePhase.Fight)
                {
                    SoundHandler.CurrentMusicType = SoundHandler.MusicType.Fight;
                    return;
                }
            }

            var client = Arena.ArenaClient.Client;
            if (Ingame && client.IsCharacter && !client.Character.IsDead)
            {
                if (DuelMode.Enemy != null)
                {
                    SoundHandler.CurrentMusicType = SoundHandler.MusicType.Fight;
                    return;
                }
                else if (client.Character.TeamID >= 0)
                {
                    bool enemyCloseBy = false;
                    var heroPos = client.Character.GetPosition();
                    int distance = SoundHandler.CurrentMusicType == SoundHandler.MusicType.Fight ? 3000 : 1000;
                    WorldObjects.World.Current.ForEachVobPredicate(v =>
                    {
                        if (Cast.Try(v.ScriptObject, out NPCInst npc) && !npc.IsDead
                            && npc.TeamID != -1 && npc.TeamID != client.Character.TeamID)
                        {
                            if (heroPos.GetDistance(npc.GetPosition()) < distance)
                            {
                                enemyCloseBy = true;
                                return false;
                            }
                        }
                        return true;
                    });

                    SoundHandler.CurrentMusicType = enemyCloseBy ? SoundHandler.MusicType.Fight : SoundHandler.MusicType.Normal;
                    return;
                }
            }
            SoundHandler.CurrentMusicType = SoundHandler.MusicType.Normal;
        }

        Vec3f lastValidPos = Vec3f.Null;
        bool doneUncon = false;
        Utilities.LockTimer swimTimer = new Utilities.LockTimer(3000);

        void DoUnconstuff(NPCInst hero)
        {
            var env = hero.Environment;
            if (env.WaterLevel < 0.2f)
            {
                if (env.InAir) return;
                var gAI = hero.BaseInst.gAI;
                if (!gAI.CheckEnoughSpaceMoveForward(false)) return;
                if (!gAI.CheckEnoughSpaceMoveBackward(false)) return;
                if (!gAI.CheckEnoughSpaceMoveLeft(false)) return;
                if (!gAI.CheckEnoughSpaceMoveRight(false)) return;

                lastValidPos = hero.GetPosition();
            }
            else
            {
                if (!hero.IsUnconscious && env.WaterLevel > 0.3f && swimTimer.IsReady)
                {
                    ScreenScrollText.AddText("Du kannst ja gar nicht schwimmen!?!");
                }

                if (!hero.IsUnconscious)
                    doneUncon = false;

                if (!doneUncon && hero.IsUnconscious)
                {
                    hero.BaseInst.SetPhysics(false);
                    var rb = WinApi.Process.ReadInt(hero.BaseInst.gVob.Address + 224);
                    using (var vec = Vec3f.Null.CreateGVec())
                        WinApi.Process.THISCALL<WinApi.NullReturnCall>(rb, 0x5B66D0, vec);
                    Vec3f.Null.SetGVec(hero.BaseInst.gAI.Velocity);
                    hero.SetPosition(lastValidPos);
                    doneUncon = true;
                }
            }
        }

        void CheckPosition()
        {
            var hero = Arena.ArenaClient.Client.Character;
            if (hero != null && !hero.IsDead)
            {
                DoUnconstuff(hero);
            }
        }

        public static event Action OnWorldEnter;
        public void FirstWorldRender()
        {
            OnWorldEnter?.Invoke();
        }
    }
}
