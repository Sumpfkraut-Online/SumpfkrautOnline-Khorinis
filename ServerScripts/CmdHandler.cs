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

        static void OnMovement(NPC npc, NPCStates state)
        {
            switch (state)
            {
                case NPCStates.Stand:
                    break;
                case NPCStates.MoveForward:
                case NPCStates.MoveBackward:
                    break;
            }

            npc.SetMoveState(state);
        }

        static void OnTargetMovement(NPC npc, NPC target, NPCStates state)
        {
            switch (state)
            {
                case NPCStates.MoveLeft:
                case NPCStates.MoveRight:
                    //Strafing
                    break;

                case NPCStates.AttackForward:
                    break;
                case NPCStates.AttackLeft:
                    break;
                case NPCStates.AttackRight:
                    break;
                case NPCStates.AttackRun:
                    break;

                case NPCStates.Parry:
                    break;
                case NPCStates.DodgeBack:
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
            if (npc.State == NPCStates.Stand && npc.DrawnItem == null)
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
            if (npc.State == NPCStates.Stand && npc.DrawnItem == null)
            {
                switch (item.Type)
                {
                    case ItemTypes.Sword_1H:
                    case ItemTypes.Sword_2H:
                    case ItemTypes.Blunt_1H:
                    case ItemTypes.Blunt_2H:
                        npc.EquipSlot(1, item);
                        break;
                    case ItemTypes.Bow:
                    case ItemTypes.XBow:
                        npc.EquipSlot(2, item);
                        break;
                    case ItemTypes.Armor:
                        npc.EquipSlot(10, item);
                        break;
                    case ItemTypes.Ring:
                        break;
                    case ItemTypes.Amulet:
                        break;
                    case ItemTypes.Belt:
                        break;
                    case ItemTypes.Food_Huge:
                    case ItemTypes.Food_Small:
                    case ItemTypes.Drink:
                    case ItemTypes.Potions:
                        break;
                    case ItemTypes.Document:
                    case ItemTypes.Book:
                        break;
                    case ItemTypes.Rune:
                    case ItemTypes.Scroll:
                        break;
                    case ItemTypes.Misc_Usable:
                        break;
                    case ItemTypes.Misc:
                        break;
                }
            }
        }
    }
}
