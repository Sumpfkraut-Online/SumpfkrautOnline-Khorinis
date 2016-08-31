using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.GUI;

namespace GUC.Scripts.TFFA
{
    static class InputControl
    {
        static void FocusHook(WinApi.Hook hook)
        {
            WorldObjects.NPC self;
            if (WorldObjects.World.Current.TryGetVobByAddress(hook.GetESI(), out self))
            {
                if (self != TFFAClient.Client.Character.BaseInst)
                    return;

                WorldObjects.NPC other;
                if (WorldObjects.World.Current.TryGetVobByAddress(hook.GetEBP(), out other))
                {
                    foreach (ClientInfo ci in ClientInfo.ClientInfos.Values)
                    {
                        if (ci.CharID == other.ID)
                        {
                            if (ci.Team == TFFAClient.Info.Team && (InputHandler.IsPressed(VirtualKeys.Control) || InputHandler.IsPressed(VirtualKeys.LeftButton)))
                            {
                                hook.SetEBX(0); // Some kind of priority value (128, 129, 130)
                            }
                            return;
                        }
                    }
                }
            }
        }

        static bool inited = false;
        public static void Init()
        {
            if (inited)
            {
                return;
            }
            inited = true;

            InputHandler.OnKeyDown += KeyDown;
            InputHandler.OnKeyUp += KeyUp;

            WinApi.Process.AddHook(FocusHook, 0x733FB6, 5);
        }

        static void KeyDown(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyDown(key, now);
                return;
            }

            if (key == VirtualKeys.Escape)
            {
                MainMenu.Menu.Open();
            }
            else if (key == VirtualKeys.Tab)
            {
                Scoreboard.Menu.Open();
            }
            else if (key == VirtualKeys.C)
            {
                ShowClientIDs();
            }
            else if (key == VirtualKeys.T)
            {
                ChatMenu.Menu.TeamChat = false;
                ChatMenu.Menu.Open();
            }
            else if (key == VirtualKeys.Z)
            {
                ChatMenu.Menu.TeamChat = true;
                ChatMenu.Menu.Open();
            }

            NPCInst Hero = TFFAClient.Client.Character;
            if (Hero == null || !Hero.IsSpawned)
                return;

            if (key == VirtualKeys.P)
            {
                if (Hero.IsSpawned)
                {
                    string str = Hero.BaseInst.GetPosition() + " " + Hero.BaseInst.GetDirection() + "\r\n";
                    Log.Logger.Log(str);
                    System.IO.File.AppendAllText(System.IO.Path.Combine(Program.ProjectPath, "SavedLocations.txt"), str);
                }
            }

            if (TFFAClient.Status == TFFAPhase.Waiting)
                return;

            if (key == VirtualKeys.I)
            {
                PlayerInventory.Menu.Open();
            }
            else if (key == VirtualKeys.N1)
            {
                if (Hero.DrawnWeapon == null)
                {
                    WorldObjects.Item wep;
                    ScriptAniJob job;
                    if (Hero.BaseInst.TryGetEquippedItem((int)NPCInst.SlotNums.Sword, out wep) && Hero.Model.TryGetAniJob((int)SetAnis.Draw1H, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, wep.ID);
                    }
                    else if (Hero.BaseInst.TryGetEquippedItem((int)NPCInst.SlotNums.Longsword, out wep) && Hero.Model.TryGetAniJob((int)SetAnis.Draw2H, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, wep.ID);
                    }
                }
                else
                {
                    ScriptAniJob job;
                    if (Hero.TryGetUndrawFromType(Hero.DrawnWeapon.ItemType, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, Hero.DrawnWeapon.ID);
                    }
                }
            }
            else if (key == VirtualKeys.N2)
            {
                if (Hero.DrawnWeapon == null)
                {
                    WorldObjects.Item wep;
                    ScriptAniJob job;
                    if (Hero.BaseInst.TryGetEquippedItem((int)NPCInst.SlotNums.Bow, out wep) && Hero.Model.TryGetAniJob((int)SetAnis.DrawBow, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, wep.ID);
                    }
                    else if (Hero.BaseInst.TryGetEquippedItem((int)NPCInst.SlotNums.XBow, out wep) && Hero.Model.TryGetAniJob((int)SetAnis.DrawXBow, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, wep.ID);
                    }
                }
                else
                {
                    ScriptAniJob job;
                    if (Hero.TryGetUndrawFromType(Hero.DrawnWeapon.ItemType, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, Hero.DrawnWeapon.ID);
                    }
                }
            }
            else if (key == VirtualKeys.Space)
            {

            }
            else if (key == VirtualKeys.Control || key == VirtualKeys.LeftButton)
            {
                // RUN ATTACK
                if (Hero.Movement == MoveState.Forward)
                {
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Run, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (Hero.DrawnWeapon != null && Hero.DrawnWeapon.IsWepRanged && Hero.BaseInst.IsInFightMode)
                {
                    if (Hero.DrawnWeapon.ItemType == ItemTypes.WepBow)
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.BowAim, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                    else if (Hero.DrawnWeapon.ItemType == ItemTypes.WepXBow)
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.XBowAim, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                }
            }
            else if (key == VirtualKeys.Menu) // JUMPING
            {
                if (Hero.GetClimbAni() != null || Hero.GetJumpAni() != null)
                    return;

                var ledge = Hero.BaseInst.DetectClimbingLedge();
                if (ledge != null)
                {
                    float dist = ledge.Location.Y - Hero.BaseInst.gVob.HumanAI.FeetY;

                    if (dist > 205) // CLIMB HIGH
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.ClimbHigh, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, ledge);
                            return;
                        }
                    }
                    else if (dist > 105) // CLIMB MID
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.ClimbMid, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, ledge);
                            return;
                        }
                    }
                    else if (dist > 60) // CLIMB LOW
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.ClimbLow, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, ledge);
                            return;
                        }
                    }
                }

                if (Hero.Movement == MoveState.Forward)
                {
                    ScriptAniJob job;
                    if (Hero.Model.TryGetAniJob((int)SetAnis.JumpRun, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else
                {
                    ScriptAniJob job;
                    if (Hero.Model.TryGetAniJob((int)SetAnis.JumpFwd, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
            }
            else if (InputHandler.IsPressed(VirtualKeys.Control) || InputHandler.IsPressed(VirtualKeys.LeftButton))
            {
                if (key == VirtualKeys.Up || key == VirtualKeys.W)
                {
                    if (Hero.DrawnWeapon != null && Hero.BaseInst.IsInFightMode)
                    {
                        if (Hero.DrawnWeapon.IsWepMelee)
                        {
                            // FORWARD COMBOS
                            ScriptAniJob job;
                            if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd1, out job) && Hero.GetActiveAniFromAniID(job.ID) != null)
                            {
                                if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd2, out job))
                                {
                                    TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                                }
                            }
                            else if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd2, out job) && Hero.GetActiveAniFromAniID(job.ID) != null)
                            {
                                if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd3, out job))
                                {
                                    TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                                }
                            }
                            else if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd3, out job) && Hero.GetActiveAniFromAniID(job.ID) != null)
                            {
                                if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd4, out job))
                                {
                                    TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                                }
                            }
                            else if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Fwd1, out job))
                            {
                                TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                            }
                        }
                        else if (Hero.DrawnWeapon.IsWepRanged && Hero.IsAiming)
                        {
                            if (Hero.DrawnWeapon.ItemType == ItemTypes.WepBow)
                            {
                                ScriptAniJob job;
                                if (Hero.Model.TryGetAniJob((int)SetAnis.BowReload, out job))
                                {
                                    var focusNpc = Hero.BaseInst.gVob.GetFocusNpc();
                                    Vec3f flyDir = focusNpc.Address == 0 ? Hero.BaseInst.GetDirection() : (new Vec3f(focusNpc.Position) - Hero.BaseInst.GetPosition());
                                    flyDir = flyDir.Normalise();
                                    TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, GetFlyDistance(flyDir), flyDir);
                                }
                            }
                            else if (Hero.DrawnWeapon.ItemType == ItemTypes.WepXBow)
                            {
                                ScriptAniJob job;
                                if (Hero.Model.TryGetAniJob((int)SetAnis.XBowReload, out job))
                                {
                                    var focusNpc = Hero.BaseInst.gVob.GetFocusNpc();
                                    Vec3f flyDir = focusNpc.Address == 0 ? Hero.BaseInst.GetDirection() : (new Vec3f(focusNpc.Position) - Hero.BaseInst.GetPosition());
                                    flyDir = flyDir.Normalise();
                                    TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob, GetFlyDistance(flyDir), flyDir);
                                }
                            }
                        }
                    }
                }
                else if (key == VirtualKeys.Left || key == VirtualKeys.A) // LEFT ATTACK
                {
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Left, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Right || key == VirtualKeys.D) // RIGHT ATTACK
                {
                    ScriptAniJob job;
                    if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Right, out job))
                    {
                        TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else if (key == VirtualKeys.Down || key == VirtualKeys.S) // PARADE
                {
                    if (Hero.DrawnWeapon == null || !Hero.DrawnWeapon.IsWepMelee)
                        return;

                    ScriptAniJob job;
                    List<ScriptAniJob> parries = new List<ScriptAniJob>();
                    for (int i = (int)NPCInst.AttackMove.Parry1; i <= (int)NPCInst.AttackMove.Parry3; i++)
                        if (Hero.TryGetAttackFromMove((NPCInst.AttackMove)i, out job))
                            parries.Add(job);

                    TFFAClient.Client.BaseClient.DoStartAni(parries[Randomizer.GetInt(0, parries.Count)].BaseAniJob);
                }
            }
        }

        static void KeyUp(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyUp(key, now);
                return;
            }

            if (key == VirtualKeys.Tab)
            {
                Scoreboard.Menu.Close();
            }
            else if (key == VirtualKeys.C)
            {
                HideClientIDs();
            }
        }

        public static void Update(long now)
        {
            if (TFFAClient.Client != null && TFFAClient.Client.Character != null && TFFAClient.Client.Character.Ammo != null)
            {
                GUCView.DebugText.Text = TFFAClient.Client.Character.Ammo.Definition.Name + ": " + TFFAClient.Client.Character.Ammo.BaseInst.Amount;
            }
            else
            {
                GUCView.DebugText.Text = "";
            }

            Scoreboard.Menu.Update(now);

            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                Scoreboard.Menu.Close();
                return;
            }

            UpdateClientIDs();

            if (GUC.Network.GameClient.Client.IsSpectating)
            {
                if (InputHandler.MouseDistY != 0)
                {
                    Gothic.oCGame.GetCameraVob().RotateLocalX(InputHandler.MouseDistY * 0.2f);
                }
                if (InputHandler.MouseDistX != 0)
                {
                    Gothic.oCGame.GetCameraVob().RotateWorldY(InputHandler.MouseDistX * 0.2f);
                }

                if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(cam.Direction) * 20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(cam.Direction) * -20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }

                if (InputHandler.IsPressed(VirtualKeys.A))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(cam.Direction).Cross(new Vec3f(0, 1, 0));
                    dir *= 20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }
                else if (InputHandler.IsPressed(VirtualKeys.D))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(cam.Direction).Cross(new Vec3f(0, 1, 0));
                    dir *= -20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }

                if (InputHandler.IsPressed(VirtualKeys.Space))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(0, 1, 0);
                    dir *= 20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Control))
                {
                    var cam = Gothic.oCGame.GetCameraVob();
                    var dir = new Vec3f(0, 1, 0);
                    dir *= -20.0f;
                    cam.MoveWorld(dir.X, dir.Y, dir.Z);
                }
                return;
            }

            NPCInst Hero = TFFAClient.Client.Character;

            if (Hero == null || !Hero.IsSpawned || TFFAClient.Status == TFFAPhase.Waiting)
                return;
            
            if (InputHandler.MouseDistY != 0)
            {
                var camAI = Gothic.oCGame.GetCameraAI();

                float cur = WinApi.Process.ReadFloat(camAI.Address + 56);
                WinApi.Process.Write(cur + InputHandler.MouseDistY * 0.035f, camAI.Address + 56);
            }

            if (!InputHandler.IsPressed(VirtualKeys.Control) && !InputHandler.IsPressed(VirtualKeys.LeftButton))
            {
                Gothic.Objects.oCNpcFocus.StopHighlightingFX();

                // Do turning
                bool stopTurning = true;
                if (Hero.GetClimbAni() == null)
                {
                    if (InputHandler.MouseDistX != 0)
                    {
                        float turn = InputHandler.MouseDistX * 0.05f;

                        if (turn > 0.5f || turn < -0.5f)
                        {
                            Hero.BaseInst.gVob.AniCtrl.Turn(turn, true);
                            stopTurning = false;
                        }
                        else
                        {
                            Hero.BaseInst.gVob.AniCtrl.Turn(turn, false);
                        }
                    }

                    if (InputHandler.IsPressed(VirtualKeys.Left) || InputHandler.IsPressed(VirtualKeys.Q))
                    {
                        Hero.BaseInst.gVob.AniCtrl.Turn(-2.5f, true);
                        stopTurning = false;
                    }
                    else if (InputHandler.IsPressed(VirtualKeys.Right) || InputHandler.IsPressed(VirtualKeys.E))
                    {
                        Hero.BaseInst.gVob.AniCtrl.Turn(2.5f, true);
                        stopTurning = false;
                    }
                }

                if (stopTurning)
                {
                    Hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
                }

                TFFAClient.Client.Character.BaseInst.gVob.HumanAI.CheckFocusVob(0);
                if (InputHandler.IsPressed(VirtualKeys.A)) // strafe left
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Left);
                }
                else if (InputHandler.IsPressed(VirtualKeys.D)) // strafe right
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Right);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Up) || InputHandler.IsPressed(VirtualKeys.W)) // move forward
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Forward);
                }
                else if (InputHandler.IsPressed(VirtualKeys.Down) || InputHandler.IsPressed(VirtualKeys.S)) // move backward
                {
                    if (Hero.DrawnWeapon != null && Hero.DrawnWeapon.IsWepMelee && Hero.BaseInst.IsInFightMode)
                    {
                        ScriptAniJob job;
                        if (Hero.TryGetAttackFromMove(NPCInst.AttackMove.Dodge, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                    else
                    {
                        GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Backward);
                    }
                }
                else // not moving
                {
                    GUC.Network.GameClient.Client.DoSetHeroState(MoveState.Stand);
                }

                if (Hero.IsAiming && Hero.DrawnWeapon != null && Hero.DrawnWeapon.IsWepRanged && Hero.BaseInst.IsInFightMode)
                {
                    if (Hero.DrawnWeapon.ItemType == ItemTypes.WepBow)
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.BowLower, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                    else if (Hero.DrawnWeapon.ItemType == ItemTypes.WepXBow)
                    {
                        ScriptAniJob job;
                        if (Hero.Model.TryGetAniJob((int)SetAnis.XBowLower, out job))
                        {
                            TFFAClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                        }
                    }
                }
            }
            else
            {

                Gothic.Objects.oCNpcFocus.StartHighlightingFX(Hero.BaseInst.gVob.GetFocusNpc());

                var fnpc = Hero.BaseInst.gVob.GetFocusNpc();
                if (fnpc.Address == 0 || fnpc.HP <= 0)
                    Hero.BaseInst.gVob.HumanAI.CheckFocusVob(0);
                else
                    TurnToEnemy();
                Hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
            }

            if (InputHandler.IsPressed(VirtualKeys.K))
            {
                if (nextFwdTeleport < now)
                {
                    Vec3f pos = Hero.BaseInst.GetPosition();
                    Vec3f dir = Hero.BaseInst.GetDirection();
                    pos += dir * 125.0f;
                    Hero.BaseInst.SetPosition(pos);

                    nextFwdTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }

            if (InputHandler.IsPressed(VirtualKeys.F8))
            {
                if (nextUpTeleport < now)
                {
                    Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                    pos.Y += 200.0f;
                    Hero.BaseInst.SetPosition(pos);
                    Hero.BaseInst.gVob.SetPhysicsEnabled(false);

                    nextUpTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
                }
            }
        }

        static long nextFwdTeleport = 0;
        static long nextUpTeleport = 0;

        static void TurnToEnemy()
        {
            WorldObjects.NPC e;
            if (WorldObjects.World.Current.TryGetVobByAddress(TFFAClient.Client.Character.BaseInst.gVob.GetFocusNpc().Address, out e))
            {
                NPCInst npc = TFFAClient.Client.Character;
                NPCInst enemy = (NPCInst)e.ScriptObject;
                Vec3f npcDir = npc.BaseInst.GetDirection();
                Vec3f npcPos = npc.BaseInst.GetPosition();
                Vec3f enemyPos = e.GetPosition();

                Vec3f dir = (new Vec3f(enemyPos.X, 0, enemyPos.Z) - new Vec3f(npcPos.X, 0, npcPos.Z)).Normalise();
                Vec3f diff = new Vec3f(npcDir.X, 0, npcDir.Z) - dir;
                float len = diff.GetLength();
                const float maxSpeed = 0.1f;
                if (len > maxSpeed)
                    diff = new Vec3f(diff.X / len * maxSpeed, 0, diff.Z / len * maxSpeed);

                npc.BaseInst.SetDirection(npcDir - diff);
            }
        }

        static bool clientsShown = false;
        public static bool ClientsShown { get { return clientsShown; } }
        static void ShowClientIDs()
        {
            if (clientsShown)
                return;
            clientsShown = true;

            Scoreboard.Menu.UpdateNames();
            ChatMenu.Menu.UpdateLines();
            clientView.Show();

            // Set gothic names
            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values)
            {
                if (ci == TFFAClient.Info || ci.Team == Team.Spec)
                    continue;

                WorldObjects.NPC npc;
                if (WorldObjects.World.Current.TryGetVob(ci.CharID, out npc))
                {
                    npc.gVob.Name.Set(string.Format("({0}){1}", ci.ID, ci.Name));
                }
            }
        }

        static void HideClientIDs()
        {
            if (!clientsShown)
                return;
            clientsShown = false;

            Scoreboard.Menu.UpdateNames();
            ChatMenu.Menu.UpdateLines();
            clientView.Hide();

            // Set gothic names
            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values)
            {
                if (ci == TFFAClient.Info || ci.Team == Team.Spec)
                    continue;

                WorldObjects.NPC npc;
                if (WorldObjects.World.Current.TryGetVob(ci.CharID, out npc))
                {
                    npc.gVob.Name.Set(ci.Name);
                }
            }
        }


        static GUCVisual clientView = new GUCVisual();
        static void UpdateClientIDs()
        {
            if (!clientsShown)
                return;

            var gHero = TFFAClient.Client.Character?.BaseInst.gVob;

            int i = 0;
            foreach (ClientInfo ci in ClientInfo.ClientInfos.Values)
            {
                if (ci == TFFAClient.Info || ci.Team == Team.Spec)
                    continue;

                var cam = Gothic.oCGame.GetCameraVob();
                WorldObjects.NPC npc;
                if (WorldObjects.World.Current.TryGetVob(ci.CharID, out npc))
                {
                    var gNpc = npc.gVob;

                    if (gHero == null || (gNpc.Address != gHero.FocusVob.Address && gNpc.FreeLineOfSight(cam)))
                    {
                        GUCVisualText text;
                        if (i >= clientView.Texts.Count)
                        {
                            text = clientView.CreateText("", 0, 0, true);
                            text.Format = GUCVisualText.TextFormat.Center;
                        }
                        else
                        {
                            text = clientView.Texts[i++];
                        }

                        Vec3f pos = (Vec3f)gNpc.Position;
                        pos.Y += 100;

                        text.Set3DPos(pos);
                        text.Text = string.Format("({0}){1}", ci.ID, ci.Name);
                    }
                }
            }

            for (; i < clientView.Texts.Count; i++)
            {
                clientView.Texts[i].Text = "";
            }
        }

        static int GetFlyDistance(Vec3f dir)
        {
            int distance = ushort.MaxValue;

            var Hero = GUC.Network.GameClient.Client.Character;

            Vec3f start = Hero.GetPosition();
            start.Y += 30;

            Vec3f end = start + dir * ushort.MaxValue;

            using (var zStart = Gothic.Types.zVec3.Create(start.X, start.Y, start.Z))
            using (var zEnd = Gothic.Types.zVec3.Create(end.X, end.Y, end.Z))
            {
                var gWorld = Gothic.oCGame.GetWorld();

                if (gWorld.TraceRayNearestHit(zStart, zEnd, Gothic.Objects.zCWorld.zTraceRay.Ignore_Alpha | Gothic.Objects.zCWorld.zTraceRay.Ignore_Projectiles | Gothic.Objects.zCWorld.zTraceRay.Ignore_Vob_No_Collision | Gothic.Objects.zCWorld.zTraceRay.Ignore_NPC) != 0
                    && gWorld.Raytrace_FoundHit != 0)
                {
                    distance = (int)start.GetDistance((Vec3f)gWorld.Raytrace_FoundIntersection);
                    if (distance > ushort.MaxValue)
                        distance = ushort.MaxValue;
                }
            }

            return distance;
        }
    }
}
