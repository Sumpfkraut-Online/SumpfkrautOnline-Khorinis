using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.Scripts.Sumpfkraut.Menus;
using GUC.Enumeration;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Client.Scripts.Sumpfkraut
{
    static class InputControl
    {
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
        }

        static int fwdKeys = 0;
        static int strafeKeys = 0;
        static int turnKeys = 0;

        static long teleportKey = 0;

        static void Fwd(bool inc)
        {
            fwdKeys += inc ? 1 : -1;

            if (fwdKeys > 0)
                GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveForward);
            else if (fwdKeys < 0)
                GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveBackward);
            else if (strafeKeys == 0)
                GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.Stand);
        }

        static void Strafe(bool inc)
        {
            strafeKeys += inc ? 1 : -1;

            if (strafeKeys > 0)
                GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveRight);
            else if (strafeKeys < 0)
                GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.MoveLeft);
            else if (fwdKeys == 0)
                GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.Stand);
        }

        static bool actionMode = false;
        static void KeyDown(VirtualKeys key, long now)
        {
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.KeyDown(key, now);
                return;
            }

            if (GUC.Network.GameClient.Client.Character == null)
                return;

            if (key == VirtualKeys.Control)
            {
                if (ScriptClient.Client.Character.BaseInst.State == NPCStates.MoveForward)
                {

                }
                else
                {
                    actionMode = true;
                    GUC.Network.GameClient.Client.DoSetHeroState(NPCStates.Stand);
                }
            }
            else if (key == VirtualKeys.P)
            {
                using (var str = Gothic.Types.zString.Create("2H"))
                    ScriptClient.Client.Character.BaseInst.gVob.SetWeaponMode2(str);
            }
            else if (key == VirtualKeys.O)
            {
                //var stream = GUC.Network.GameClient.Client.GetIngameMsgStream();
                //GUC.Network.GameClient.Client.SendIngameMsg(stream);

                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.CreateHit(GUC.Network.GameClient.Client.Character.gVob);
            }
            else if (key == VirtualKeys.F1)
            {
                GUC.Network.GameClient.Client.Character.gVob.GetModel().StartAni("S_FALL", 0);
                GUC.Network.GameClient.Client.Character.gVob.SetPhysicsEnabled(true);
            }
            else if (key == VirtualKeys.Up)
            {
                if (actionMode)
                {
                    ScriptAniJob job;
                    if (ScriptClient.Client.Character.CurrentAni != null)
                    {
                        int curID = ScriptClient.Client.Character.CurrentAni.AniJob.ID;
                        if (curID >= (int)SetAnis.Attack2HFwd1 && curID < (int)SetAnis.Attack2HFwd4
                            && ScriptClient.Client.Character.Model.TryGetAniJob(curID + 1, out job))
                        {
                            ScriptClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                            return;
                        }
                    }
                    if (ScriptClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HFwd1, out job))
                    {
                        ScriptClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else
                    Fwd(true);
            }
            else if (key == VirtualKeys.Down)
            {
                if (actionMode)
                {

                }
                else
                    if (fwdKeys == 0) Fwd(false);
            }
            else if (key == VirtualKeys.D)
            {
                if (actionMode)
                {

                }
                else
                    if (strafeKeys == 0) Strafe(true);
            }
            else if (key == VirtualKeys.A)
            {
                if (actionMode)
                {

                }
                else
                    if (strafeKeys == 0) Strafe(false);
            }
            else if (key == VirtualKeys.Right)
            {
                if (actionMode)
                {
                    ScriptAniJob job;
                    if (ScriptClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HRight, out job))
                    {
                        ScriptClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else
                    if (turnKeys == 0) turnKeys++;
            }
            else if (key == VirtualKeys.Left)
            {
                if (actionMode)
                {
                    ScriptAniJob job;
                    if (ScriptClient.Client.Character.Model.TryGetAniJob((int)SetAnis.Attack2HLeft, out job))
                    {
                        ScriptClient.Client.BaseClient.DoStartAni(job.BaseAniJob);
                    }
                }
                else
                    if (turnKeys == 0) turnKeys--;
            }
            else if (key == VirtualKeys.K)
            {
                Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                Vec3f dir = GUC.Network.GameClient.Client.Character.GetDirection();
                pos += dir * 100.0f;
                GUC.Network.GameClient.Client.Character.SetPosition(pos);
                teleportKey = GameTime.Ticks;
            }
            else if (key == VirtualKeys.F8)
            {
                Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                pos.Y += 200.0f;
                GUC.Network.GameClient.Client.Character.SetPosition(pos);
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

            if (GUC.Network.GameClient.Client.Character == null)
                return;

            if (key == VirtualKeys.Control)
            {
                actionMode = false;
            }
            else if (key == VirtualKeys.Up)
            {
                if (actionMode)
                {

                }
                else
                    if (fwdKeys > 0) Fwd(false);
            }
            else if (key == VirtualKeys.Down)
            {
                if (actionMode)
                {

                }
                else
                    if (fwdKeys < 0) Fwd(true);
            }
            else if (key == VirtualKeys.D)
            {
                if (actionMode)
                {

                }
                else
                    if (strafeKeys > 0) Strafe(false);
            }
            else if (key == VirtualKeys.A)
            {
                if (actionMode)
                {

                }
                else
                    if (strafeKeys < 0) Strafe(true);
            }
            else if (key == VirtualKeys.Right)
            {
                if (actionMode)
                {

                }
                else
                    if (turnKeys > 0) turnKeys--;
            }
            else if (key == VirtualKeys.Left)
            {
                if (actionMode)
                {

                }
                else
                    if (turnKeys < 0) turnKeys++;
            }
            else if (key == VirtualKeys.K)
                teleportKey = 0;
        }

        public static void Update(long now)
        {
            if (GUC.Network.GameClient.Client.Character == null)
                return;

            if (turnKeys > 0)
            {
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.Turn(3.0f, true);
            }
            else if (turnKeys < 0)
            {
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.Turn(-3.0f, true);
            }
            else
            {
                GUC.Network.GameClient.Client.Character.gVob.AniCtrl.StopTurnAnis();
            }

            if (teleportKey > 0)
            {
                if (GameTime.Ticks - teleportKey > TimeSpan.TicksPerSecond/2)
                {
                    Vec3f pos = GUC.Network.GameClient.Client.Character.GetPosition();
                    Vec3f dir = GUC.Network.GameClient.Client.Character.GetDirection();
                    pos += dir * 100.0f;
                    GUC.Network.GameClient.Client.Character.SetPosition(pos);

                    teleportKey += 150 * TimeSpan.TicksPerMillisecond;
                }
            }
        }
    }
}
