using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Controls;

namespace GUC.Scripts.Arena.Controls
{
    partial class ArenaControl
    {
        KeyDictionary playerControls = new KeyDictionary()
        {
            { KeyBind.Jump, d => { if (d) NPCInst.Requests.Jump(ScriptClient.Client.Character); } },
            { KeyBind.DrawFists, d => { if (d) NPCInst.Requests.DrawFists(ScriptClient.Client.Character); } },
            { KeyBind.MoveForward, d => CheckFightMove(d, FightMoves.Fwd) },
            { KeyBind.TurnLeft, d => CheckFightMove(d, FightMoves.Left) },
            { KeyBind.TurnRight, d => CheckFightMove(d, FightMoves.Right) },
            { KeyBind.MoveBack, d => CheckFightMove(d, FightMoves.Parry) },
            { KeyBind.Action, PlayerActionButton },
            { KeyBind.DrawWeapon, DrawWeapon },
            { KeyBind.OpenScoreBoard, ToggleScoreBoard },
            { KeyBind.OpenAllChat, d => { ChatMenu.Menu.OpenAllChat(); } },
            { KeyBind.OpenTeamChat, d => { ChatMenu.Menu.OpenTeamChat(); } },
            { VirtualKeys.P, PrintPosition }
        };

        static void PrintPosition(bool down)
        {
            var hero = NPCInst.Hero;
            if (hero == null)
                return;

            var pos = hero.GetPosition();
            var dir = hero.GetDirection();

            Log.Logger.Log(pos + " " + dir);
            //System.Globalization.CultureInfo
            System.IO.File.AppendAllText("positions.txt", string.Format("{{ new Vec3f({0}, {1}, {2}), new Vec3f({3}, {4}, {5}) }},\n", pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z));
        }

        static void ToggleScoreBoard(bool down)
        {
            if (down)
            {
                if (TeamMode.IsRunning && TeamMode.TeamDef != null)
                {
                    TOBoardScreen.Instance.Open();
                }
                else
                {
                    DuelBoardScreen.Instance.Open();
                }
            }
            else
            {
                DuelBoardScreen.Instance.Close();
                TOBoardScreen.Instance.Close();
            }
        }

        static void DrawWeapon(bool down)
        {
            if (!down) return;

            var hero = ScriptClient.Client.Character;
            if (hero.ModelInst.IsInAnimation())
                return;

            if (hero.MeleeWeapon != null)
                NPCInst.Requests.DrawWeapon(hero, hero.MeleeWeapon);
            else if (hero.DrawnWeapon != null)
                NPCInst.Requests.DrawWeapon(hero, hero.DrawnWeapon);
            else
                NPCInst.Requests.DrawFists(hero);
        }

        static void CheckFightMove(bool down, FightMoves move)
        {
            if (!down || !KeyBind.Action.IsPressed())
                return;

            var hero = ScriptClient.Client.Character;
            if (hero.IsInFightMode)
            {
                NPCInst.Requests.Attack(hero, move);
            }
        }

        static void PlayerActionButton(bool down)
        {
            var hero = ScriptClient.Client.Character;
            if (KeyBind.MoveForward.IsPressed())
            {
                if (down)
                    CheckFightMove(down, FightMoves.Run);
            }
            else if (!hero.IsDead)
            {
                if (hero.IsInFightMode)
                {
                    if (down)
                    {
                        var focus = hero.GetFocusVob();
                        enemy = focus is NPCInst ? (NPCInst)focus : null;
                    }
                    else
                    {
                        enemy = null;
                    }
                    if (enemy == null)
                        Gothic.Objects.oCNpcFocus.StopHighlightingFX();
                    else
                        Gothic.Objects.oCNpcFocus.StartHighlightingFX(enemy.BaseInst.gVob);
                }
                else if (down)
                {
                    var focusVob = hero.GetFocusVob();
                    if (focusVob is NPCInst)
                        DuelMode.SendRequest((NPCInst)focusVob);
                }
            }
        }

        long nextDodgeTime = 0;
        void PlayerUpdate()
        {
            fwdTelHelper.Update(GameTime.Ticks);
            upTelHelper.Update(GameTime.Ticks);

            NPCInst hero = ScriptClient.Client.Character;
            if (hero.IsDead)
                return;

            DoTurning(hero);

            NPCMovement state = NPCMovement.Stand;
            if (KeyBind.MoveLeft.IsPressed()) // strafe left
            {
                state = NPCMovement.Left;
            }
            else if (KeyBind.MoveRight.IsPressed()) // strafe right
            {
                state = NPCMovement.Right;
            }
            else if (!KeyBind.Action.IsPressed())
            {
                hero.BaseInst.gVob.HumanAI.CheckFocusVob(1);
                if (KeyBind.MoveForward.IsPressed()) // move forward
                {
                    state = NPCMovement.Forward;
                }
                else if (KeyBind.MoveBack.IsPressed()) // move backward
                {
                    if (hero.IsInFightMode)
                    {
                        if (nextDodgeTime < GameTime.Ticks) // don't spam
                        {
                            NPCInst.Requests.Attack(hero, FightMoves.Dodge);
                            nextDodgeTime = GameTime.Ticks + 50 * TimeSpan.TicksPerMillisecond;
                        }
                        return;
                    }
                    else
                    {
                        state = NPCMovement.Backward;
                    }
                }
            }
            hero.SetMovement(state);
        }

        static NPCInst enemy;
        static void DoTurning(NPCInst hero)
        {
            if (enemy != null)
            {
                Vec3f heroPos = hero.GetPosition();
                Vec3f heroDir = hero.GetDirection();
                Vec3f enemyPos = enemy.GetPosition();

                Vec3f dir = (new Vec3f(enemyPos.X, 0, enemyPos.Z) - new Vec3f(heroPos.X, 0, heroPos.Z)).Normalise();
                Vec3f diff = new Vec3f(heroDir.X, 0, heroDir.Z) - dir;
                float len = diff.GetLength();
                const float maxSpeed = 0.075f;
                if (len > maxSpeed)
                    diff = new Vec3f(diff.X / len * maxSpeed, 0, diff.Z / len * maxSpeed);

                hero.SetDirection(heroDir - diff);
                return;
            }

            if (hero.ModelInst.GetActiveAniFromLayer(1) == null)
            {
                if (KeyBind.TurnLeft.IsPressed())
                {
                    hero.BaseInst.gVob.AniCtrl.Turn(-2.5f, true);
                    return;
                }
                else if (KeyBind.TurnRight.IsPressed())
                {
                    hero.BaseInst.gVob.AniCtrl.Turn(2.5f, true);
                    return;
                }
            }
            hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
        }

        static KeyHoldHelper fwdTelHelper = new KeyHoldHelper(500, 100)
        {
            { () =>
                {
                    NPCInst hero = ScriptClient.Client.Character;
                    Vec3f pos = hero.GetPosition();
                    Vec3f dir = hero.GetDirection();
                    pos += dir * 125.0f;
                    hero.SetPosition(pos);
                }, VirtualKeys.K
            }
        };

        static KeyHoldHelper upTelHelper = new KeyHoldHelper(500, 100)
        {
            { () =>
                {
                    NPCInst hero = ScriptClient.Client.Character;
                    Vec3f pos = hero.GetPosition();
                    pos.Y += 200.0f;
                    hero.SetPosition(pos);
                    hero.BaseInst.gVob.SetPhysicsEnabled(false);
                }, VirtualKeys.F8
            }
        };
    }

    /*

    public override void KeyDown(VirtualKeys key)
    {
        NPCInst hero = ScriptClient.Client.Character;

        if (key == VirtualKeys.Escape)
        {
            Arena.Menus.MainMenu.Menu.Open();
        }
        else if (KeyBind.Action.Contains(key))
        {
            WorldObjects.BaseVob baseVob;
            if (WorldObjects.World.Current.TryGetVobByAddress(hero.BaseInst.gVob.FocusVob.Address, out baseVob))
            {

            }
        }
        else if (KeyBind.MoveForward.Contains(key))
        {
            if (KeyBind.Action.IsPressed())
            {
                NPCInst.Requests.Attack(hero, FightMoves.Fwd);
            }
        }
        else if(KeyBind.TurnLeft.IsPressed())
        {
            if (KeyBind.Action.IsPressed())
            {
                NPCInst.Requests.Attack(hero, FightMoves.Left);
            }
        }
        else if(KeyBind.TurnRight.IsPressed())
        {
            if (KeyBind.Action.IsPressed())
            {
                NPCInst.Requests.Attack(hero, FightMoves.Right);
            }
        }
        else if(KeyBind.MoveBack.IsPressed())
        {
            if (KeyBind.Action.IsPressed())
            {
                NPCInst.Requests.Attack(hero, FightMoves.Parry);
            }
        }
        else if (KeyBind.Jump.Contains(key))
        {
            NPCInst.Requests.Jump(hero, JumpMoves.Fwd);
        }
        else if (KeyBind.Inventory.Contains(key))
        {
            if (hero.DrawnWeapon != null)
            {
                NPCInst.Requests.DrawWeapon(hero, hero.LastUsedWeapon);
            }
            PlayerInventory.Menu.Open();
        }
        else if (KeyBind.DrawWeapon.Contains(key))
        {
            if (hero.LastUsedWeapon != null)
            {
                NPCInst.Requests.DrawWeapon(hero, hero.LastUsedWeapon);
            }
            else if (hero.MeleeWeapon != null)
            {
                hero.LastUsedWeapon = hero.MeleeWeapon;
                NPCInst.Requests.DrawWeapon(hero, hero.MeleeWeapon);
            }
            else if (hero.RangedWeapon != null)
            {
                hero.LastUsedWeapon = hero.RangedWeapon;
                NPCInst.Requests.DrawWeapon(hero, hero.RangedWeapon);
            }
            else
            {
                NPCInst.Requests.DrawFists(hero);
            }
        }
        else if (KeyBind.DrawFists.Contains(key))
        {
            NPCInst.Requests.DrawFists(hero);
        }
    }

    public override void KeyUp(VirtualKeys key)
    {
        NPCInst hero = ScriptClient.Client.Character;
    }

    public override void KeyUpdate(VirtualKeys key)
    {
        NPCInst hero = ScriptClient.Client.Character;

        if (KeyBind.Action.IsPressed())
        {
            hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
        }
        else
        { 
            hero.BaseInst.gVob.HumanAI.CheckFocusVob(1);
            if (KeyBind.MoveLeft.IsPressed()) // strafe left
            {
                hero.SetMovement(NPCMovement.Left);
            }
            else if (KeyBind.MoveRight.IsPressed()) // strafe right
            {
                hero.SetMovement(NPCMovement.Right);
            }
            else if (KeyBind.MoveForward.IsPressed()) // move forward
            {
                hero.SetMovement(NPCMovement.Forward);
            }
            else if (KeyBind.MoveBack.IsPressed()) // move backward
            {
                hero.SetMovement(NPCMovement.Backward);
            }
            else // not moving
            {
                hero.SetMovement(NPCMovement.Stand);
            }

            // Do turning
            if (KeyBind.TurnLeft.IsPressed())
            {
                hero.BaseInst.gVob.AniCtrl.Turn(-2.5f, true);
            }
            else if (KeyBind.TurnRight.IsPressed())
            {
                hero.BaseInst.gVob.AniCtrl.Turn(2.5f, true);
            }
            else
            {
                hero.BaseInst.gVob.AniCtrl.StopTurnAnis();
            }
        }

        // special keys

        long now = GameTime.Ticks;
        if (InputHandler.IsPressed(VirtualKeys.K))
        {
            if (nextFwdTeleport < now)
            {
                Vec3f pos = hero.GetPosition();
                Vec3f dir = hero.GetDirection();
                pos += dir * 125.0f;
                hero.SetPosition(pos);

                nextFwdTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
            }
        }
        if (InputHandler.IsPressed(VirtualKeys.F8))
        {
            if (nextUpTeleport < now)
            {
                Vec3f pos = hero.GetPosition();
                pos.Y += 200.0f;
                hero.SetPosition(pos);
                hero.BaseInst.gVob.SetPhysicsEnabled(false);

                nextUpTeleport = now + 150 * TimeSpan.TicksPerMillisecond;
            }
        }
    }

    static long nextFwdTeleport = 0;
    static long nextUpTeleport = 0;*/
}
