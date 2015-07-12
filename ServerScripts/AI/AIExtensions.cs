using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Types;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripts.AI.Enumeration;
using GUC.Enumeration;

namespace GUC.Server.Scripts.AI
{
    public static class AIExtensions
    {
        public static WayNet getWayNet(this NPC proto)
        {
            if (AISystem.WayNets.ContainsKey(proto.Map))
                return AISystem.WayNets[proto.Map];
            return null;
        }

        public static NPCAI getAI(this NPC proto)
        {
            return (NPCAI)proto.getUserObjects("AI");
        }

        public static void setFightTalent(this NPC proto, NPCTalent talent, int percent){
            proto.setHitchances(talent, percent);
            
            if (percent >= 60)
            {
                proto.setTalentSkills(talent, 2);
            }
            else if (percent >= 30)
            {
                proto.setTalentSkills(talent, 1);
            }
            else
            {
                proto.setTalentSkills(talent, 0);
            }
            
        }

        public static void InitNPCAI(this NPC proto)
        {
            NPCAI ai = new NPCAI(proto);
            proto.setUserObject("AI", ai);
            ai.init();
        }

        public static void InitNPCAI(this Player proto)
        {
            NPCAI ai = new NPCAI(proto);
            proto.setUserObject("AI", ai);
        }

        public static void AI_GOTOWP(this NPC proto, String waypoint)
        {
            AIStates.AI_GOTOWP g = new AIStates.AI_GOTOWP(proto, waypoint);
            proto.getAI().addState(g);
        }

        public static void AI_ALIGNTOWP(this NPC proto, String waypoint)
        {
            AIStates.AI_ALIGNTOWP g = new AIStates.AI_ALIGNTOWP(proto, waypoint);
            proto.getAI().addState(g);
        }

        public static void AI_SETWALKTYPE(this NPC proto, WalkTypes wt)
        {
            AIStates.AI_SETWALKTYPE g = new AIStates.AI_SETWALKTYPE(proto, wt);
            proto.getAI().addState(g);
        }

        public static void AI_PLAYANIMATION(this NPC proto, String anim)
        {
            AIStates.AI_PLAYANIMATION g = new AIStates.AI_PLAYANIMATION(proto, anim);
            proto.getAI().addState(g);
        }

        public static void standAnim(this NPC proto)
        {
            if (proto.WeaponMode == 1)
                proto.playAnimation("S_FISTRUN");
            else
                proto.playAnimation("S_RUN");
        }

        public static bool gotoPosition(this NPC proto, Vec3f position, float minDistance)
        {
            if (proto is NPC)
            {
                NPC p = (NPC)proto;
                if (!p.NPCControlled)//set the position!
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

        public static int getAttackRange(this NPC proto)
        {
            return 300;
        }

        public static void setGuild(this NPC proto, Guilds guild)
        {
            proto.getAI().Guild = guild;
        }

        public static Guilds getGuild(this NPC proto)
        {
            return proto.getAI().Guild;
        }

        public static GuildsAttitude getAttitudeToGuild(this NPC proto, Guilds guild)
        {
            return AISystem.getGuildAttitude(proto.getGuild(), guild);
        }

        public static bool turnToPosition(this NPC proto, Vec3f position)
        {
            Vec3f direction = position - proto.Position;
            direction.Y = 0;
            proto.setDirection(direction);
            return true;
        }

        public static bool turnTo(this NPC proto, Vec3f direction)
        {
            proto.setDirection(direction);
            return true;
        }

        public static void ResetAIStates(this NPC proto)
        {
            proto.getAI().clearStateList();
        }

        public static String getRunAnimation(this NPC proto)
        {
            String anim = "S_WALKL";

            if (proto.WeaponMode == 1)
                anim = "S_FISTWALKL";

            if (proto.getAI().WalkType == Enumeration.WalkTypes.Run){
                if (proto.WeaponMode == 0)
                    anim = "S_RUNL";
                else if (proto.WeaponMode == 1)
                    anim = "S_FISTRUNL";
                else if(proto.WeaponMode == 2)
                    anim = "S_1HRUNL";
            }
            return anim;
        }

        public static String getFightAnimation(this NPC proto)
        {
            String anim = "S_RUN";

            if (proto.WeaponMode == 1)
                anim = "T_FISTATTACKMOVE";
            else if (proto.WeaponMode == 2)
                anim = "T_1HATTACKMOVE";
            
            return anim;
        }

        public static String getFightRunAnimation(this NPC proto)
        {
            String anim = "S_RUN";

            if (proto.WeaponMode == 1)
                anim = "S_FISTATTACK";
            else if (proto.WeaponMode == 2)
                anim = "S_1HATTACK";

            return anim;
        }

        public static void hitEnemy(this NPC proto, NPC enemy)
        {
            DamageTypes dt = DamageTypes.DAM_BLUNT;
            Item weaponItem = null;

            if(proto.WeaponMode == 1){
                dt = DamageTypes.DAM_BLUNT;
            }else if(proto.WeaponMode == 2){
                dt = proto.EquippedWeapon.DamageType;
                weaponItem = proto.EquippedWeapon;
            }else if(proto.WeaponMode == (int)FightMode.Far){
                dt = proto.EquippedRangeWeapon.DamageType;
                weaponItem = proto.EquippedRangeWeapon;
            }

            proto.hit(enemy, dt, proto.WeaponMode, weaponItem, null);
        }

        public static bool IsHuman(this NPC proto)
        {
            if (proto.getGuild() <= Guilds.HUM_SPERATOR)
                return true;
            return false;
        }

        public static bool IsMonster(this NPC proto)
        {
            if (proto.getGuild() > Guilds.HUM_SPERATOR && proto.getGuild() <= Guilds.MON_SEPERATOR)
                return true;
            return false;
        }

        public static bool IsOrc(this NPC proto)
        {
            if (proto.getGuild() > Guilds.MON_SEPERATOR && proto.getGuild() <= Guilds.ORC_SEPERATOR)
                return true;
            return false;
        }

        public static void unreadyWeapon(this NPC proto)
        {
            if (proto.IsMonster())
                return;
            if (proto.WeaponMode == 0)
                return;

            if (proto.WeaponMode == (int)FightMode.Fist)//Fist
                proto.WeaponMode = 0;
            else if (proto.WeaponMode == (int)FightMode.Meele)//Sword
            {
                proto.WeaponMode = 0;
                Item i = proto.getSlotItem((int)SlotFlag.SLOT_RIGHTHAND);
                if ((i.ItemInstance.Flags & (Flags.ITEM_2HD_SWD | Flags.ITEM_2HD_AXE)) > 0)
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_RIGHTHAND, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_LONGSWORD, i);
                }
                else
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_RIGHTHAND, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_SWORD, i);
                }
            }
            else if (proto.WeaponMode == (int)FightMode.Far)
            {
                proto.WeaponMode = 0;
                Item i = proto.getSlotItem((int)SlotFlag.SLOT_LEFTHAND);
                if ((i.ItemInstance.Flags & Flags.ITEM_BOW) > 0)
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_LEFTHAND, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_BOW, i);
                }
                else
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_LEFTHAND, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_CROSSBOW, i);
                }
            }

        }

        public static void readyBestWeapon(this NPC proto, NPC enemy)
        {
            if (proto.IsMonster())//Monsters are always in FIST-Mode.
                return;
            if (proto.WeaponMode != 0)
                return;

            //Draw Range-Weapon when Equiped and Distance is over 12 Meters
            if (proto.EquippedRangeWeapon != null && enemy.Position.getDistance(proto.Position) > 1200)
            {
                proto.setWeaponMode((int)FightMode.Far);
                if ((proto.EquippedRangeWeapon.ItemInstance.Flags & Flags.ITEM_BOW) > 0)
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_BOW, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_LEFTHAND, proto.EquippedRangeWeapon);
                }
                else
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_CROSSBOW, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_LEFTHAND, proto.EquippedRangeWeapon);
                }
            }
            else if (proto.EquippedWeapon != null)
            {
                proto.setWeaponMode((int)FightMode.Meele);
                if ((proto.EquippedWeapon.ItemInstance.Flags & (Flags.ITEM_2HD_SWD | Flags.ITEM_2HD_AXE)) > 0)
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_LONGSWORD, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_RIGHTHAND, proto.EquippedWeapon);
                }
                else
                {
                    proto.setSlotItem((int)SlotFlag.SLOT_SWORD, null);
                    proto.setSlotItem((int)SlotFlag.SLOT_RIGHTHAND, proto.EquippedWeapon);
                }
            }
            else
            {
                proto.setWeaponMode((int)FightMode.Fist);
            }
        }


        #region Routines

        public static bool RTN_ACTIVE(this NPC proto, int startHour, int startMinute, int endHour, int endMinute)
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
