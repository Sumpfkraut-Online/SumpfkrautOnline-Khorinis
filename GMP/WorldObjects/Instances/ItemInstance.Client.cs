using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances
{
    public partial class ItemInstance : VobInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCItem ret = (vob == null || !(vob is oCItem)) ? oCItem.Create() : (oCItem)vob;
            base.CreateVob(ret); // vob.CreateVob

            ret.Amount = 1;
            ret.Instance = this.ID;
            //ret.ItemVisual.Set(Visual);
            ret.VisualChange.Set(VisualChange);
            ret.Name.Set(Name);
            ret.Material = (int)Material;
            //ret.MainFlag = MainFlags;
            //ret.Flags = Flags;
            //ret.Wear = Wear;

            return ret;
        }

        /*
               void SetFlags()
               {
                   switch (Type)
                   {
                       case ItemType.Sword_1H:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                           Flags = oCItem.ItemFlags.ITEM_SWD;
                           break;
                       case ItemType.Sword_2H:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                           Flags = oCItem.ItemFlags.ITEM_2HD_SWD;
                           break;
                       case ItemType.Blunt_1H:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                           Flags = oCItem.ItemFlags.ITEM_AXE;
                           break;
                       case ItemType.Blunt_2H:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_NF;
                           Flags = oCItem.ItemFlags.ITEM_2HD_AXE;
                           break;
                       case ItemType.Bow:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_FF;
                           Flags = oCItem.ItemFlags.ITEM_BOW;
                           break;
                       case ItemType.XBow:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_FF;
                           Flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                           break;
                       case ItemType.Armor:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_ARMOR;
                           Flags = 0;
                           Wear = 1;
                           break;
                       case ItemType.Arrow:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                           Flags = oCItem.ItemFlags.ITEM_BOW;
                           break;
                       case ItemType.XBolt:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_MUN;
                           Flags = oCItem.ItemFlags.ITEM_CROSSBOW;
                           break;
                       case ItemType.Ring:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                           Flags = oCItem.ItemFlags.ITEM_RING;
                           break;
                       case ItemType.Amulet:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                           Flags = oCItem.ItemFlags.ITEM_AMULET;
                           break;
                       case ItemType.Belt:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_MAGIC;
                           Flags = oCItem.ItemFlags.ITEM_BELT;
                           break;
                       case ItemType.Food_Small:
                       case ItemType.Food_Huge:
                       case ItemType.Drink:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_FOOD;
                           Flags = 0;
                           break;
                       case ItemType.Potions:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_POTIONS;
                           Flags = 0;
                           break;
                       case ItemType.Document:
                       case ItemType.Book:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_DOCS;
                           Flags = 0;
                           break;
                       case ItemType.Rune:
                       case ItemType.Scroll:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_RUNE;
                           Flags = 0;
                           break;
                       case ItemType.Misc:
                       case ItemType.Misc_Usable:
                       default:
                           MainFlags = oCItem.MainFlags.ITEM_KAT_NONE;
                           Flags = 0;
                           break;
                   }

                   Flags |= MainFlags;
               }*/
    }
}