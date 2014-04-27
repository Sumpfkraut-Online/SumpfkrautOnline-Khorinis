using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;

namespace GUC.Server.Scripts
{
	public class DamageScript
	{
		public void Init()
		{
			Console.WriteLine("############## Initalise DamageScript #####################");

			NPCProto.OnDamages += new Events.PlayerDamageEventHandler(OnDamage);
		}

		public int getProtection(NPCProto victim, DamageType damageMode)
		{
			int protection = 0;
			if(victim.EquippedArmor != null) {
				protection += victim.EquippedArmor.getProtection(damageMode);
			}
			return 0;
		}

		public void OnDamage(NPCProto victim, DamageType damageMode, Vec3f hitLoc, Vec3f flyDir, NPCProto attacker, int weaponMode, int spellID, Item weapon) {
			if(victim.getUserObjects("IMMORTAL") != null && (bool)victim.getUserObjects("IMMORTAL"))//Victim is immortal!
				return;
			if(attacker != null && attacker.getUserObjects("FRIENDS") != null && ((List<NPCProto>)attacker.getUserObjects("FRIENDS")).Contains(victim))//Victim is a friend!
				return;
            
			int damage = 0;

			Console.WriteLine("OnDamage: "+damageMode+" | "+weaponMode+" | "+spellID+" | "+weapon+" | "+attacker);

			if(damageMode == DamageType.DAM_FALL) {
				damage = victim.HP;
			}

			if(attacker != null) {
				if(weapon == null && weaponMode == 1) {//1 is fist!, 2 => 1h
					damage = attacker.Strength - getProtection(victim, damageMode);
				}else if(weapon != null) {
					damage = attacker.Strength + weapon.TotalDamage - getProtection(victim, weapon.DamageType);
				}
			}

			damage = Math.Max(damage, 5);

			bool toUnconscious = false;
			bool canKill = true;
			if(attacker != null && victim.HP != 1) {
				if(damageMode == DamageType.DAM_BLUNT) {
					canKill = false;
				}

				if(damageMode == DamageType.DAM_BLUNT || damageMode == DamageType.DAM_EDGE)
					toUnconscious = true;
			}

            if (toUnconscious && !victim.isUnconscious)
            {
				victim.dropUnconscious(0.0f);
			}else if(!canKill && victim.HP - damage <= 1) {
				victim.HP = 1;
			}else{
				victim.HP -= damage;
			}
		}

		
		
    }
}
