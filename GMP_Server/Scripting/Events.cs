using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;

namespace GUC.Server.Scripting
{
    public static class Events
    {
        public delegate void AttributeChangedEventHandler(NPC proto, NPCAttribute attrib, int oldValue, int newValue);
        public delegate void TalentChangedEventHandler(NPC proto, NPCTalent talent, int oldValue, int newValue);
        public delegate void CastSpell(NPC caster, Spell spell, Vob target );

        public delegate void ContainerItemMessage(MobContainer container, Player pl, Item item, int amount);

        public delegate void PlayerEventHandler(Player sender);
        public delegate void PlayerItemEventHandler(NPC player, Item item, int amount);
        public delegate void PlayerDamageEventHandler(NPC victim, DamageTypes damageMode, Vec3f hitLoc, Vec3f flyDir, Vob attacker, int weaponMode, Spell spellID, Item weapon, float fallDownDistanceY);

        public delegate void UseItemEventHandler(NPC player, Item item, short state, short targetState);

        public delegate void PlayerKeyEventHandler(Player sender, Dictionary<byte, byte> keys);

        public delegate void MobInterEventHandler(MobInter sender, NPC npc);
        public delegate void MobContainerPickEventHandler(MobInter sender, NPC npc, char pick);

        public delegate void NPCEquipEventHandler(NPC npc, Item item);
        public delegate void NPCAnimationUpdate(NPC npc, short animID, short oldAnimID);

        public delegate void NPCCanSeeEventHandler(int callbackID, NPC proto, Vob vob, bool canSee);
        
        public delegate void ReadInitEventHandler(int callbackID, Player player, String section, String entry, String value);
        public delegate void CheckMd5EventHandler(int callbackID, Player player, String md5, String value);

        public delegate void TimerEvent();
    }
}
