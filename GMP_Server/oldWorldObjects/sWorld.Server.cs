using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;

namespace GUC.WorldObjects
{
    internal partial class sWorld
    {
        protected static int CallBackIDS = 0;
        public static int getNewCallBackID()
        {
            CallBackIDS += 1;
            return CallBackIDS;
        }

        public static void removeVob(Vob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob can't be null!");
            if (vob.ID == 0)
                throw new ArgumentException("Vob.ID can't be null!");

            
            VobDict.Remove(vob.ID);

            if (vob is NPC)
            {
                if (((NPC)vob).client == null)
                {
                    npcList.Remove((NPC)vob);
                }
                else
                {
                    playerList.Remove((NPC)vob);
                }
            }
            else if (vob is Item)
                removeItem((Item)vob);
        }

        public static void addVob(Vob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("AddVob: Vob can't be null!");
            if (vob.ID == 0)
                throw new ArgumentException("AddVob: Vob.ID can't be null!");

            VobDict.Add(vob.ID, vob);

            if( !(vob is NPC ) && !( vob is Item) )
                VobList.Add(vob);

            if (vob is NPC)
            {
                if (((NPC)vob).client == null)
                {
                    npcList.Add((NPC)vob);
                }
                else
                {
                    playerList.Add((NPC)vob);
                }
            }
            else if (vob is Item)
                addItem((Item)vob);
                
        }

        protected static void addItem(Item item)
        {
            itemList.Add(item);
        }

        protected static void removeItem(Item item)
        {
            itemList.Remove(item);
            if(item.Container != null)
                item.Container.removeItem(item);
        }
    }
}
