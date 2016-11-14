using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Menus;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    static class PlayerControl
    {
        public static void KeyDown(NPCInst hero, VirtualKeys key, long now)
        {
            if (key == VirtualKeys.Escape)
            {
                MainMenu.Menu.Open();
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
                    hero.RequestAttack(ScriptRequestMessageIDs.AttackForward);
                }
            }
            else if(KeyBind.TurnLeft.IsPressed())
            {
                if (KeyBind.Action.IsPressed())
                {
                    hero.RequestAttack(ScriptRequestMessageIDs.AttackLeft);
                }
            }
            else if(KeyBind.TurnRight.IsPressed())
            {
                if (KeyBind.Action.IsPressed())
                {
                    hero.RequestAttack(ScriptRequestMessageIDs.AttackRight);
                }
            }
            else if(KeyBind.MoveBack.IsPressed())
            {
                if (KeyBind.Action.IsPressed())
                {
                    hero.RequestAttack(ScriptRequestMessageIDs.Parry);
                }
            }
            else if (KeyBind.Jump.Contains(key))
            {
                hero.SendCommand(ScriptRequestMessageIDs.JumpFwd);
            }
            else if (KeyBind.Inventory.Contains(key))
            {
                if (hero.DrawnWeapon != null)
                {
                    hero.RequestDrawWeapon(hero.LastUsedWeapon);
                }
                PlayerInventory.Menu.Open();
            }
            else if (KeyBind.DrawWeapon.Contains(key))
            {
                if (hero.LastUsedWeapon != null)
                {
                    hero.RequestDrawWeapon(hero.LastUsedWeapon);
                }
                else if (hero.MeleeWeapon != null)
                {
                    hero.LastUsedWeapon = hero.MeleeWeapon;
                    hero.RequestDrawWeapon(hero.MeleeWeapon);
                }
                else if (hero.RangedWeapon != null)
                {
                    hero.LastUsedWeapon = hero.RangedWeapon;
                    hero.RequestDrawWeapon(hero.RangedWeapon);
                }
                else
                {
                    hero.RequestDrawFists();
                }
            }
            else if (KeyBind.DrawFists.Contains(key))
            {
                hero.RequestDrawFists();
            }
        }

        public static void KeyUp(NPCInst hero, VirtualKeys key, long now)
        {

        }

        public static void Update(NPCInst hero, long now)
        {
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
        static long nextUpTeleport = 0;
    }
}
