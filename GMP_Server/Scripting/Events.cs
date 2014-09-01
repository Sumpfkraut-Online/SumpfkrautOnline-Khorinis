using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.GUI;
using GUC.Types;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI.GuiList;

namespace GUC.Server.Scripting
{
    public static class Events
    {
        public delegate void AttributeChangedEventHandler(NPCProto proto, NPCAttribute attrib, int oldValue, int newValue);
        public delegate void TalentChangedEventHandler(NPCProto proto, NPCTalent talent, int oldValue, int newValue);
        public delegate void CastSpell(NPCProto caster, Spell spell, Vob target );

        public delegate void ContainerItemMessage(MobContainer container, Player pl, Item item, int amount);

        public delegate void PlayerEventHandler(Player sender);
        public delegate void PlayerItemEventHandler(NPCProto player, Item item, int amount);
        public delegate void PlayerDamageEventHandler(NPCProto victim, DamageTypes damageMode, Vec3f hitLoc, Vec3f flyDir, Vob attacker, int weaponMode, Spell spellID, Item weapon, float fallDownDistanceY);
        public delegate void TextBoxMessageEventHandler(TextBox sender, Player player, String message);
        public delegate void ButtonEventHandler(Button sender, Player player);

        public delegate void TextureEventHandler(Texture sender, Player player);
        public delegate void TextureHoverEventHandler(Texture sender, Player player, bool hover);
        public delegate void ListButtonEventHandler(ListButton sender, Player player);
        public delegate void ListTextboxEventHandler(ListTextBox sender, Player player, String text);


        public delegate void UseItemEventHandler(NPCProto player, Item item, short state, short targetState);

        public delegate void PlayerKeyEventHandler(Player sender, Dictionary<byte, byte> keys);

        public delegate void MobInterEventHandler(MobInter sender, NPCProto npc);
        public delegate void MobContainerPickEventHandler(MobInter sender, NPCProto npc, char pick);

        public delegate void NPCEquipEventHandler(NPCProto npc, Item item);

        public delegate void NPCCanSeeEventHandler(int callbackID, NPCProto proto, Vob vob, bool canSee);
        
        public delegate void ReadInitEventHandler(int callbackID, Player player, String section, String entry, String value);
        public delegate void CheckMd5EventHandler(int callbackID, Player player, String md5, String value);

        public delegate void TimerEvent();
    }
}
