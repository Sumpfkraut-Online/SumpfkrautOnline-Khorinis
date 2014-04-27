using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Types;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI
{
    public static class AIExtensions
    {
        public static WayNet getWayNet(this NPCProto proto)
        {
            if (AISystem.WayNets.ContainsKey(proto.Map))
                return AISystem.WayNets[proto.Map];
            return null;
        }

        public static NPCAI getAI(this NPCProto proto)
        {
            return (NPCAI)proto.getUserObjects("AI");
        }

        public static void InitNPCAI(this NPCProto proto)
        {
            NPCAI ai = new NPCAI(proto);
            proto.setUserObject("AI", ai);
            ai.init();
        }

        public static void AI_GOTOWP(this NPCProto proto, String waypoint)
        {
            AIStates.AI_GOTOWP g = new AIStates.AI_GOTOWP(proto, waypoint);
            proto.getAI().addState(g);
        }

        public static void AI_ALIGNTOWP(this NPCProto proto, String waypoint)
        {
            AIStates.AI_ALIGNTOWP g = new AIStates.AI_ALIGNTOWP(proto, waypoint);
            proto.getAI().addState(g);
        }

        public static void AI_SETWALKTYPE(this NPCProto proto, WalkTypes wt)
        {
            AIStates.AI_SETWALKTYPE g = new AIStates.AI_SETWALKTYPE(proto, wt);
            proto.getAI().addState(g);
        }

        public static void AI_PLAYANIMATION(this NPCProto proto, String anim)
        {
            AIStates.AI_PLAYANIMATION g = new AIStates.AI_PLAYANIMATION(proto, anim);
            proto.getAI().addState(g);
        }

        public static void standAnim(this NPCProto proto)
        {
            proto.playAnimation("S_RUN");
        }

        public static bool gotoPosition(this NPCProto proto, Vec3f position, float minDistance)
        {
            if (proto is NPC)
            {
                NPC p = (NPC)proto;
                if (p.NPCController == null)//set the position!
                {
                    if (proto.getAI().lastPosUpdate == 0)
                    {
                        proto.getAI().lastPosUpdate = DateTime.Now.Ticks;
                    }
                    else
                    {
                        long now = DateTime.Now.Ticks;
                        long time = now - proto.getAI().lastPosUpdate;

                        float speed = (5 * 100) * (int)(time / 1000.0);

                        Vec3f direction = position - proto.Position;
                        if (direction.Length < speed)
                        {
                            proto.Position = position;
                        }
                        else
                        {
                            direction = direction.normalise();
                            direction *= speed;

                            proto.Position = proto.Position + direction;
                        }
                        proto.getAI().lastPosUpdate = now;
                    }
                }else{
                    proto.getAI().lastPosUpdate = 0;
                }
            }

            if (proto.Position.getDistance(position) < minDistance)
                return true;
            else
            {
                Vec3f p = position - proto.Position;
                p.Y = 0;
                proto.setDirection(p);
                proto.playAnimation(proto.getRunAnimation());
                return false;
            }
        }

        public static bool turnToPosition(this NPCProto proto, Vec3f position)
        {
            Vec3f direction = position - proto.Position;
            direction.Y = 0;
            proto.setDirection(direction);
            return true;
        }

        public static bool turnTo(this NPCProto proto, Vec3f direction)
        {
            proto.setDirection(direction);
            return true;
        }

        public static void ResetAIStates(this NPCProto proto)
        {
            proto.getAI().clearStateList();
        }

        public static String getRunAnimation(this NPCProto proto)
        {
            String anim = "S_WALKL";

            if (proto.getAI().WalkType == Enumeration.WalkTypes.Run)
                anim = "S_RUNL";


            return anim;
        }


        #region Routines

        public static bool RTN_ACTIVE(this NPCProto proto, int startHour, int startMinute, int endHour, int endMinute)
        {
            int day, hour, minute;
            World.getTime(out day, out hour, out minute);

            
            if (endHour == 24 && endMinute == 0)
            {
                endHour = 23;
                endMinute = 59;
                
            }

            int _endHour = endHour;
            int _hournow = hour;
            if (endHour < startHour || (endHour == startHour && endMinute < startMinute))
            {
                endHour += 24;
                day += 1;
                if(startHour > hour)
                    hour += 24;
            }

            if (startHour > hour || (startHour == hour && startMinute > minute))
                return false;
            if (endHour < hour || (endHour == hour && endMinute <= minute))
                return false;

            if (proto.getAI().lastRTNDay == day && proto.getAI().lastRTNHour >= _endHour && proto.getAI().lastRTNMinute >= endMinute)
                return false;

            proto.getAI().lastRTNDay = day;
            proto.getAI().lastRTNHour = _endHour;
            proto.getAI().lastRTNMinute = endMinute;

            return true;
        }

        #endregion


    }
}
