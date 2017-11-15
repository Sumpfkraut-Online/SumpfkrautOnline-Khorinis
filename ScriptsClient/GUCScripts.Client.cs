﻿using System;
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

        static Gothic.Objects.zCVob arrow;

        public static Types.Angles AddAngles = new Types.Angles(0, 0, 0);

        public static event Action<long> OnUpdate;
        public void Update(long ticks)
        {
            GUCMenu.UpdateMenus(ticks);
            InputControl.UpdateControls(ticks);
            OnUpdate?.Invoke(ticks);
            CheckMusic();
            CheckPosition();

            var cam = GothicGlobals.Game.GetCameraVob();
            if (!cam.IsNull && NPCInst.Hero != null)
            {
                if (arrow == null)
                {
                    arrow = Gothic.Objects.zCVob.Create();
                    arrow.SetVisual("ITRW_ARROW.3DS");
                    GothicGlobals.Game.GetWorld().AddVob(arrow);
                    arrow.SetPositionWorld(NPCInst.Hero.GetPosition().X, NPCInst.Hero.GetPosition().Y, NPCInst.Hero.GetPosition().Z);
                }

                var rotation = Types.Angles.FromAtVector((Types.Vec3f)cam.Direction);
                var p = rotation.Pitch;
                rotation.Pitch = rotation.Roll;
                rotation.Roll = p;

                rotation += AddAngles;


                GUI.GUCView.DebugText.Text = string.Format("{0:0.00} {1:0.00} {2:0.00}", rotation.Pitch, rotation.Yaw, rotation.Roll);

                rotation.SetMatrix(arrow);
            }
        }

        SoundInstance menuTheme = null;
        public void StartOutgame()
        {
            var theme = new SoundDefinition("INSTALLER_LOOP.WAV");
            theme.zSFX.IsFixed = true;
            theme.zSFX.Volume = 0.5f;
            theme.zSFX.SetLooping(true);
            menuTheme = SoundHandler.PlaySound(theme, 0.5f);

            Arena.Menus.MainMenu.Menu.Open();

            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
            // stop oCAniCtrl_Human::_Stand from canceling the s_bowaim animation
            WinApi.Process.Write(0x006B7772, 0xEB, 0x69);


            Gothic.Objects.oCNpcFocus.SetFocusMode(1);
            GUCMenu.CloseActiveMenus();

            if (menuTheme != null)
            {
                SoundHandler.StopSound(menuTheme);
                menuTheme = null;
            }

            Ingame = true;
            Logger.Log("Ingame started.");
        }

        static void CheckMusic()
        {
            var client = Arena.ArenaClient.Client;
            if (Ingame && client.IsCharacter && !client.Character.IsDead)
            {
                if (Arena.DuelMode.Enemy != null)
                {
                    SoundHandler.CurrentMusicType = SoundHandler.MusicType.Fight;
                    return;
                }
                else if (Arena.TeamMode.TeamDef != null && Arena.TeamMode.Phase == Arena.TOPhases.Battle)
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

        void CheckPosition()
        {
            var hero = Arena.ArenaClient.Client.Character;
            if (hero != null && !hero.IsDead)
            {
                if (Arena.TeamMode.TeamDef == null)
                {
                    if (hero.GetPosition().GetDistancePlanar(Types.Vec3f.Null) > 10000)
                        if (Randomizer.GetInt(2) == 0)
                        {
                            hero.SetPosition(new Types.Vec3f(-1969.563f, -120.6398f, 2707.328f));
                            hero.SetAngles(Types.Angles.Null);
                        }
                        else
                        {
                            hero.SetPosition(new Types.Vec3f(-4550.162f, -98.70279f, 1392.133f));
                            hero.SetAngles(Types.Angles.Null);
                        }
                }
            }
        }
    }
}
