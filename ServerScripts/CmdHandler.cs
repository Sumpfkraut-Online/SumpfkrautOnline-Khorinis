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
            NPC.CmdOnMove += OnMovement;
            NPC.CmdOnTargetMove += OnTargetMovement;
            NPC.CmdOnUseMob += OnUseMob;
            NPC.CmdOnUnUseMob += OnUnUseMob;
            NPC.CmdOnUseItem += OnUseItem;
            NPC.CmdOnJump += OnJump;
            NPC.CmdOnDrawItem += OnDrawEquipment;
            NPC.CmdOnUndrawItem += OnUndrawItem;
        }

        static void OnDrawEquipment(NPC npc, Item item)
        {
            if (npc.UsedMob == null)
            {
                npc.Drawitem(item);
            }
        }

        static void OnUndrawItem(NPC npc)
        {
            npc.UndrawItem();
        }

        static void OnMovement(NPC npc, NPCState state)
        {
            switch (state)
            {
                case NPCState.Stand:
                    break;
                case NPCState.MoveForward:
                case NPCState.MoveBackward:
                    break;
            }

            npc.SetMoveState(state);
        }

        static void OnTargetMovement(NPC npc, NPC target, NPCState state)
        {
            switch (state)
            {
                case NPCState.MoveLeft:
                case NPCState.MoveRight:
                    //Strafing
                    break;

                case NPCState.AttackForward:
                    break;
                case NPCState.AttackLeft:
                    break;
                case NPCState.AttackRight:
                    break;
                case NPCState.AttackRun:
                    break;

                case NPCState.Parry:
                    break;
                case NPCState.DodgeBack:
                    break;
            }

            npc.SetMoveState(state, target);
        }

        static void OnJump(NPC npc)
        {
            npc.Jump();
        }

        static void OnUseMob(NPC npc, MobInter mob)
        {
            if (npc.State == NPCState.Stand && npc.DrawnItem == null)
            {
                npc.UseMob(mob);
            }
        }

        static void OnUnUseMob(NPC npc)
        {
            npc.UnuseMob();
        }

        static void OnUseItem(NPC npc, Item item)
        {
            if (npc.State == NPCState.Stand && npc.DrawnItem == null)
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
