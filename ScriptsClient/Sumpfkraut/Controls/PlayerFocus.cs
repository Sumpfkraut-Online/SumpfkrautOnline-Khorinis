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
                currentPlayer.BaseInst.gVob.SetFocusVob(new zCVob(0));
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
                ActivateHandler(npc);
        }

        const long UpdateInterval = 200 * TimeSpan.TicksPerMillisecond;
        static GUCTimer timer = new GUCTimer(UpdateInterval, Update);

        static bool active = false;
        public static bool IsActive { get { return active; } }
        static void ActivateHandler(NPCInst npc)
        {
            if (active) return;
            active = true;
            timer.Start();
        }

        static void DeactivateHandler(NPCInst npc)
        {
            if (!active) return;
            active = false;
            timer.Stop();
            SetFocus(null);
        }

        static void UnconHandler(NPCInst npc)
        {
            if (npc.IsUnconscious) DeactivateHandler(npc);
            else ActivateHandler(npc);
        }

        static BaseVobInst focusVob;
        public static BaseVobInst FocusVob { get { return focusVob; } }
        public static NPCInst GetFocusNPC()
        {
            return focusVob is NPCInst ? (NPCInst)focusVob : null;
        }

        static void Update()
        {
            NPCInst hero = NPCInst.Hero;
            if (hero == null)
                return;

            float maxYaw = Angles.Deg2Rad(60);
            bool fightMode = hero.IsInFightMode;

            ItemInst wep = hero.GetDrawnWeapon();
            float maxRange = (fightMode && wep != null && wep.IsWepRanged) ? 3000 : 300;

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
            if (focusVob == target)
                return;

            focusVob = target;
            NPCInst.Hero.BaseInst.gVob.SetFocusVob(target == null ? new zCVob(0) : target.BaseInst.gVob);
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
    }
}
