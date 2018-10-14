using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripting;
using Gothic.Objects;
using GUC.Types;
using Gothic.Types;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    static class PlayerFocus
    {
        static NPCInst currentPlayer = null;
        public static void Activate(NPCInst npc)
        {
            if (currentPlayer != null)
            {
                currentPlayer.OnDeath -= DeactivateHandler;
                currentPlayer.OnRevive -= ActivateHandler;
                currentPlayer.OnDespawn -= DeactivateHandler;
                currentPlayer.OnSpawn -= ActivateHandler;
                currentPlayer.OnUnconChange -= UnconHandler;
            }

            currentPlayer = npc;
            if (npc == null)
                return;

            npc.OnDeath += DeactivateHandler;
            npc.OnRevive += ActivateHandler;
            npc.OnDespawn += DeactivateHandler;
            npc.OnSpawn += ActivateHandler;
            npc.OnUnconChange += UnconHandler;

            if (npc.IsSpawned && !npc.IsDead && !npc.IsUnconscious)
                Activate();
        }

        static void ActivateHandler(NPCInst npc) { Activate(); }
        static void DeactivateHandler(NPCInst npc) { Deactivate(); }
        static void UnconHandler(NPCInst npc)
        {
            if (npc.IsUnconscious) Activate();
            else Deactivate();
        }

        const long UpdateInterval = 200 * TimeSpan.TicksPerMillisecond;
        static GUCTimer timer = new GUCTimer(UpdateInterval, Update);

        static bool isActive;
        public static bool IsActive { get { return isActive; } }

        static void Activate()
        {
            if (isActive)
                return;

            isActive = true;
            timer.Start();
        }

        static void Deactivate()
        {
            if (!isActive)
                return;

            isActive = false;
            SetLockedTarget(null);
            timer.Stop();
        }

        static BaseVobInst focusVob;
        public static BaseVobInst FocusVob { get { return focusVob; } }
        public static NPCInst GetFocusNPC()
        {
            return focusVob is NPCInst ? (NPCInst)focusVob : null;
        }

        static void Update()
        {
            NPCInst hero = currentPlayer;
            if (hero == null)
            {
                Deactivate();
                return;
            }

            float maxYaw = Angles.Deg2Rad(60);
            bool fightMode = hero.IsInFightMode;

            ItemInst wep = hero.GetDrawnWeapon();
            float maxRange = (fightMode && wep != null && wep.IsWepRanged) ? 3000 : 400;

            Vec3f heroPos = hero.GetPosition();
            Angles heroAng = hero.GetAngles();

            float bestFit = 2.0f;
            BaseVobInst bestVob = null;
            hero.World.BaseWorld.ForEachVob(v =>
            {
                BaseVobInst vob = (BaseVobInst)v.ScriptObject;
                if (vob == hero)
                    return;

                bool hasPriority = false;
                if (fightMode)
                {
                    if (!(vob is NPCInst npc))
                        return;

                    if (npc.IsDead)
                        return;

                    if (bestVob != null)
                    {
                        NPCInst bestNPC = (NPCInst)bestVob;
                        if (npc.IsUnconscious)
                        {
                            if (!bestNPC.IsUnconscious)
                                return; // alive targets are more important
                        }
                        else
                        {
                            if (bestNPC.IsUnconscious)
                                hasPriority = true;
                        }

                        if (npc.TeamID == hero.TeamID)
                        {
                            if (bestNPC.TeamID != hero.TeamID)
                                return;
                        }
                        else
                        {
                            if (bestNPC.TeamID == hero.TeamID)
                                hasPriority = true;
                        }
                    }
                }

                Vec3f targetPos;
                using (zVec3 z = zVec3.Create())
                {
                    vob.BaseInst.gVob.BBox3D.GetCenter(z);
                    targetPos = (Vec3f)z;
                }

                float distance = heroPos.GetDistance(targetPos);
                if (distance > maxRange)
                    return;

                float yaw = Angles.GetYawFromAtVector(targetPos - heroPos);
                yaw = Math.Abs(Angles.Difference(yaw, heroAng.Yaw));
                if (yaw > maxYaw)
                    return; // target is not in front of hero

                if (!CanSee(heroPos, targetPos, vob))
                    return;

                float fit = distance / maxRange + yaw / maxYaw;
                if (hasPriority || fit < bestFit)
                {
                    bestVob = vob;
                    bestFit = fit;
                }
            });

            SetFocus(bestVob);
        }

        static void SetFocus(BaseVobInst target)
        {
            if (target == focusVob)
                return;

            focusVob = target;
            currentPlayer.BaseInst.gVob.FocusVob = target == null ? new zCVob(0) : target.BaseInst.gVob;
        }

        static bool CanSee(Vec3f start, Vec3f end, BaseVobInst target)
        {
            using (zVec3 zStart = start.CreateGVec())
            using (zVec3 zDir = (end - start).CreateGVec())
            {
                var zWorld = GothicGlobals.Game.GetWorld();
                if (zWorld.TraceRayFirstHit(zStart, zDir, zCWorld.zTraceRay.Ignore_Alpha | zCWorld.zTraceRay.Ignore_NPC | zCWorld.zTraceRay.Ignore_Vob_No_Collision))
                {
                    return zWorld.Raytrace_FoundVob.Address == target.BaseInst.gVob.Address;
                }
            }

            return true;
        }

        static NPCInst lockedTarget;
        public static NPCInst LockedTarget { get { return lockedTarget; } }
        public static void SetLockedTarget(NPCInst target)
        {
            if (!isActive || lockedTarget == target)
                return;

            SetFocus(target);
            lockedTarget = target;
            if (target == null)
            {
                timer.Start();
                oCNpcFocus.StopHighlightingFX();
                Update();
            }
            else
            {
                timer.Stop();
                oCNpcFocus.StartHighlightingFX(target.BaseInst.gVob);
            }
        }
    }
}
