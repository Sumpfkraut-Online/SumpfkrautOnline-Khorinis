using System;
using System.Collections.Generic;
using System.Text;
using GUC.WorldObjects.Character;

namespace GUC.WorldObjects
{
    internal partial class World : IContainer
    {
        #region lists
        protected List<NPCProto> npcList = new List<NPCProto>();
        protected List<Item> itemList = new List<Item>();
        protected List<Vob> vobList = new List<Vob>();//Only for real Vobs/Mobs! Don't add Items or NPCs to it!

        public List<NPCProto> NPCList { get { return npcList; } }
        public List<Item> ItemList { get { return itemList; } }
        public List<Vob> VobList { get { return vobList; } }
        #endregion


        #region attributes
        protected String mapName;
        
        #endregion


        #region Property

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


        #endregion




        public void addVob(Vob vob)
        {
            if (vob.Map != null && vob.Map.Length != 0)
                sWorld.getWorld(vob.Map).removeVob(vob);

            

            vob.Map = this.Map;
            vob.IsSpawned = true;
#if D_SERVER
            this.setVobPosition(null, vob.Pos, vob);
#endif
            

            if (!(vob is NPCProto) && !(vob is Item))
                VobList.Add(vob);


            if (vob is NPCProto)
                pAddPlayer((NPCProto)vob);
            else if (vob is Item)
                pAddItem((Item)vob);
        }

        protected void pAddPlayer(NPCProto proto)
        {
            this.NPCList.Add(proto);
        }

        protected void pAddItem(Item item)
        {
            if (item.Container != null)
                item.Container.removeItem(item);
            item.Container = this;
            item.Map = this.Map;
            this.itemList.Add(item);
        }

        public void removeVob(Vob vob)
        {
#if D_SERVER
            this.setVobPosition(vob.Pos, null, vob);
#endif
            if (vob is NPCProto)
                pRemovePlayer((NPCProto)vob);
            else if (vob is Item)
                pRemoveItem((Item)vob);
            vob.Map = null;

        }

        protected void pRemovePlayer(NPCProto proto)
        {
            NPCList.Remove(proto);
        }

        protected void pRemoveItem(Item item)
        {
            this.itemList.Remove(item);
        }




        public void addItem(Item item)
        {
            addVob(item);
        }

        public void removeItem(Item item)
        {
            removeVob(item);
        }
    }
}
