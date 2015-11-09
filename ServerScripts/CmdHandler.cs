using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Types;
using GUC.Enumeration;

namespace GUC.Server.Scripts
{
    static class CmdHandler
    {
        public static void Init()
        {
            NPC.sOnMovement += OnMovement;
            NPC.sOnTargetMovement += OnTargetMovement;
            NPC.CmdOnUseMob += OnUseMob;
            NPC.CmdOnUnUseMob += OnUnUseMob;
            NPC.CmdOnUseItem += OnUseItem;
        }

        static void OnMovement(NPC npc, NPCState state, Vec3f position, Vec3f direction)
        {
            npc.DoMovement(state, position, direction);
        }

        static void OnTargetMovement(NPC npc, NPC target, NPCState state, Vec3f position, Vec3f direction)
        {
            npc.DoTargetMovement(state, position, direction, target);
        }

        static void OnUseMob(MobInter mob, NPC npc)
        {
            if (npc.State == NPCState.Stand && npc.WeaponState == NPCWeaponState.None)
            {
                npc.DoUseMob(mob);
            }
        }

        static void OnUnUseMob(MobInter mob, NPC npc)
        {
            npc.DoUnUseMob();
        }

        static void OnUseItem(Item item, NPC npc)
        {
            Log.Logger.log(npc.State + " " + npc.WeaponState);
            //if (npc.State == NPCState.Stand && npc.WeaponState == NPCWeaponState.None)
            {
                switch (item.Type)
                {
                    case ItemType.Sword_1H:
                    case ItemType.Sword_2H:
                    case ItemType.Blunt_1H:
                    case ItemType.Blunt_2H:
                        npc.EquipSlot(1, item);
                        break;
                    case ItemType.Bow:
                    case ItemType.XBow:
                        npc.EquipSlot(2, item);
                        break;
                    case ItemType.Armor:
                        npc.EquipSlot(10, item);
                        break;
                    case ItemType.Ring:
                        break;
                    case ItemType.Amulet:
                        break;
                    case ItemType.Belt:
                        break;
                    case ItemType.Food_Huge:
                    case ItemType.Food_Small:
                    case ItemType.Drink:
                    case ItemType.Potions:
                        break;
                    case ItemType.Document:
                    case ItemType.Book:
                        break;
                    case ItemType.Rune:
                    case ItemType.Scroll:
                        break;
                    case ItemType.Misc_Usable:
                        break;
                    case ItemType.Misc:
                        break;
                }
            }
        }
    }
}
