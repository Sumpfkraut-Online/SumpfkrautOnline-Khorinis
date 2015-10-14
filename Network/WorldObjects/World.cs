using System;
using System.Collections.Generic;
using System.Text;
using GUC.WorldObjects.Character;

namespace GUC.WorldObjects
{
    internal partial class World : IContainer
    {
        #region lists
        protected List<NPC> npcList = new List<NPC>();
        protected List<Item> itemList = new List<Item>();
        protected List<Vob> vobList = new List<Vob>();//Only for real Vobs/Mobs! Don't add Items or NPCs to it!

        public List<NPC> NPCList { get { return npcList; } }
        public List<Item> ItemList { get { return itemList; } }
        public List<Vob> VobList { get { return vobList; } }
        #endregion

        protected String mapName;
        public String Map
        {
            get{
                return mapName;
            }
            set{
                if (value == null || value.Length == 0)
                    throw new ArgumentNullException("Map name can't be null!");
                mapName = sWorld.getMapName(value);
            }
        }

        public void addVob(Vob vob)
        {
            if (vob.Map != null)
                vob.Map.removeVob(vob);

            vob.Map = this;
            vob.IsSpawned = true;            

            if (vob is NPC)
            {
/*#if D_SERVER
                if (((NPC)vob).client != null)
                {
                    playerList.Add
                }
#endif
                pAddPlayer((NPC)vob);*/
            }
            else if (vob is Item)
            {
                pAddItem((Item)vob);
            }
            else
            {
                VobList.Add(vob);
            }
        }

        protected void pAddItem(Item item)
        {
            if (item.Container != null)
                item.Container.removeItem(item);
            item.Container = this;
            item.Map = this;
            this.itemList.Add(item);
        }

        public void removeVob(Vob vob)
        {
            if (vob is NPC)
                pRemovePlayer((NPC)vob);
            else if (vob is Item)
                pRemoveItem((Item)vob);
            vob.Map = null;

        }

        protected void pRemoveItem(Item item)
        {
            this.itemList.Remove(item);
        }
    }
}
