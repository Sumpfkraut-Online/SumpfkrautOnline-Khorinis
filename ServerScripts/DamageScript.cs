using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;

namespace GUC.Server.Scripts
{
    /// <summary>
    /// The npc ai needs this class.
    /// But you can change the damage-function.
    /// </summary>
	public static class DamageScript
	{
		public static void Init()
		{
            Logger.log(Logger.LogLevel.INFO, "################## Initalise DamageScript #################");
            
			NPC.sOnDamage += new Events.PlayerDamageEventHandler(OnDamage);
		}

		
        public delegate void NPCDamgeHandler(NPC victim, NPC attacker, int damage, bool dropUnconscious, bool dropDead);
        public static event NPCDamgeHandler Damages;

		public static void OnDamage(NPC victim, DamageTypes damageMode, Vec3f hitLoc, Vec3f flyDir, Vob aggressor, int weaponMode, Spell spell, Item weapon, float fallDownDistanceY) {
			if(victim.getUserObjects("IMMORTAL") != null && (bool)victim.getUserObjects("IMMORTAL"))//Victim is immortal!
				return;
            NPC attacker = null;
            if (aggressor is NPC)
                attacker = (NPC)aggressor;


			if(attacker != null && attacker.getUserObjects("FRIENDS") != null && ((List<NPC>)attacker.getUserObjects("FRIENDS")).Contains(victim))//Victim is a friend!
				return;
            
            

			int damage = 0;

			Console.WriteLine("OnDamage: "+damageMode+" | "+weaponMode+" | "+spell+" | "+weapon+" | "+attacker);

			if(damageMode == DamageTypes.DAM_FALL) {
                damage = (int)(fallDownDistanceY-500)/100 * 20;
			}

			if(attacker != null) {
				if(weapon == null && weaponMode == 1) {//1 is fist!, 2 => 1h
					damage = attacker.Strength - victim.getProtection(damageMode);
				}else if(weapon != null) {
					damage = attacker.Strength + weapon.TotalDamage - victim.getProtection(weapon.DamageType);
				}
			}

			damage = Math.Max(damage, 5);

			bool toUnconscious = false;
			bool canKill = true;
			if(attacker != null && victim.HP - damage <= 1) {
				if(damageMode == DamageTypes.DAM_BLUNT) {
					canKill = false;
				}

				if(damageMode == DamageTypes.DAM_BLUNT || damageMode == DamageTypes.DAM_EDGE)
					toUnconscious = true;
			}

            if (!victim.IsHuman() || !(attacker != null && attacker.IsHuman()))
            {
                toUnconscious = false;
                canKill = true;
            }

            if (toUnconscious && !victim.isUnconscious)
            {
                damage = victim.HP - 1;
				victim.dropUnconscious(0.0f);

                if (Damages != null)
                    Damages(victim, attacker, damage, true, false);

			}else if(!canKill && victim.HP - damage <= 1) {
				victim.HP = 1;

                if (Damages != null)
                    Damages(victim, attacker, damage, false, false);
			}else{
				victim.HP -= damage;

                if (Damages != null && victim.HP <= 0)
                    Damages(victim, attacker, damage, false, true);
                else if(Damages != null)
                    Damages(victim, attacker, damage, true, false);
			}
		}

		
		
    }
}
