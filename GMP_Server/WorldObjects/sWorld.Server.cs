using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;

namespace GUC.WorldObjects
{
    internal partial class sWorld
    {
        protected static Dictionary<ulong, Player> guidToPlayer = new Dictionary<ulong, Player>();


        public static Dictionary<ulong, Player> GUIDToPlayer { get { return guidToPlayer; } }

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

            if (vob is NPCProto)
                removePlayer((NPCProto)vob);
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

            if( !(vob is NPCProto ) && !( vob is Item) )
                VobList.Add(vob);

            if (vob is NPCProto)
                addPlayer((NPCProto)vob);
            else if (vob is Item)
                addItem((Item)vob);
                
        }

        protected static void addPlayer(NPCProto player)
        {
            npcProtoList.Add(player);

            if (player is Player)
            {
                guidToPlayer.Add(player.Guid, (Player)player);
                playerList.Add((Player)player);
            }
            else
            {
                npcList.Add((NPC)player);
            }
        }

        protected static void addItem(Item item)
        {
            itemList.Add(item);
        }

        protected static void removePlayer(NPCProto player)
        {
            npcProtoList.Remove(player);

            if (player is Player)
            {
                guidToPlayer.Remove(player.Guid);
                playerList.Remove((Player)player);
            }
            else
            {
                npcList.Remove((NPC)player);
            }
        }

        protected static void removeItem(Item item)
        {
            itemList.Remove(item);
            if(item.Container != null)
                item.Container.removeItem(item);
        }
    }
}
