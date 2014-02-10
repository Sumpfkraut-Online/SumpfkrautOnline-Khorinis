using System;
using System.Collections.Generic;
using System.Text;
using Network.Types;

namespace Network.Savings
{
    public class WorldItems
    {
        public item itm;
        public float[] pos;
        public String world;
    }

    public class WorldContainer : WorldMobInter
    {
        public List<item> itemList = new List<item>();
        public bool open;
    }

    public class WorldMobInter
    {
        public int vobType = 0;
        public String name;
        public float[] pos;
        public String world;

        public bool triggered = false;
    }


    public class World
    {
        public List<WorldItems> Items = new List<WorldItems>();
        public List<WorldContainer> container = new List<WorldContainer>();
        public List<WorldMobInter> mobInter = new List<WorldMobInter>();


        #region EventHandler

        public delegate void MobInterEventHandler(object sender, EventArgs e, int vobtype, string name, float[] pos, String world, bool triggered);
        public event MobInterEventHandler MobInterInsert;
        public event MobInterEventHandler MobInterUpdate;
        public event MobInterEventHandler MobInterRemove;

        public delegate void ContainerEventHandler(object sender, EventArgs e, string name, float[] pos, String world);
        public event ContainerEventHandler ContainerInsert;
        public event ContainerEventHandler ContainerRemove;

        public delegate void ContainerItemEventHandler(object sender, EventArgs e, String item, int amount, string name, float[] pos, String world);
        public event ContainerItemEventHandler ContainerItemInsert;
        public event ContainerItemEventHandler ContainerItemUpdate;
        public event ContainerItemEventHandler ContainerItemRemove;

        public delegate void ItemsEventHandler(object sender, EventArgs e, string name, int amount, float[] pos, String world);
        public event ItemsEventHandler ItemsInsert;
        public event ItemsEventHandler ItemsRemove;
        #endregion

        #region MobInter

        public void TriggerMobInter(int vobType, String name, float[] pos, String world, bool triggered)
        {
            WorldMobInter wmi = getMobInter(vobType, name, world, pos);
            if (wmi == null && triggered == false)
                return;
            if (wmi == null && triggered)
            {
                wmi = new WorldMobInter() { vobType = vobType, name = name, pos = pos, world = world, triggered = true };
                mobInter.Add(wmi);

                //TODO: SQLite Insert
                if (MobInterInsert != null)
                {
                    MobInterInsert(this, new EventArgs(), vobType, name, pos, world, triggered);
                }
            }
            else if (wmi != null && triggered)
            {
                wmi.triggered = true;

                //TODO: SQLite Update
                if (MobInterUpdate != null)
                {
                    MobInterUpdate(this, new EventArgs(), vobType, name, pos, world, triggered);
                }
            }
            else if (!triggered)
            {
                wmi.triggered = false;
                mobInter.Remove(wmi);

                //TODO: SQLite delete
                if (MobInterRemove != null)
                {
                    MobInterRemove(this, new EventArgs(), vobType, name, pos, world, triggered);
                }
            }
        }

        public void getMobInterDB(int vobType, String name, String world, float[] pos)
        {

        }

        public int getMobCount( String world )
        {
            int count = 0;
            foreach (WorldMobInter wbi in mobInter)
            {
                if (Player.isSameMap(world,wbi.world))
                    count++;
            }
            return count;
        }

        public WorldMobInter getMobInter(int vobType, String name, String world, float[] pos)
        {
            foreach (WorldMobInter wc in mobInter)
            {
                if (Player.isSameMap(world, wc.world) && new Vec3f(pos).getDistance((Vec3f)wc.pos) <= MobInterMinDistance
                    && name == wc.name && wc.vobType == vobType)
                {
                    return wc;
                }
            }
            return null;
        }
        #endregion

        #region Items
        public int getItemCount(String world)
        {
            int count = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                if (Player.isSameMap(Items[i].world, world))
                {
                    count++;
                }
            }
            return count;
        }

        public void InsertItem(WorldItems item)
        {
            Items.Add(item);

            //TODO: SQLite Insert
            if (ItemsInsert != null)
            {
                ItemsInsert(this, new EventArgs(), item.itm.code, item.itm.Amount, item.pos, item.world);
            }
        }

        public void RemoveItem(WorldItems item)
        {
            WorldItems itm = getItem(item);
            if (itm == null)
                return;

            Items.Remove(itm);

            //TODO: SQLite Delete
            if (ItemsRemove != null)
            {
                ItemsRemove(this, new EventArgs(), item.itm.code, item.itm.Amount, item.pos, item.world);
            }
        }

        /// <summary>
        /// Sucht nach einen ähnlichen angegebenen Item.
        /// Dieser muss sich in der Nähe des WorldItems befinden und sowohl der Instance entsprechen, als auch der Anzahl.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public WorldItems getItem(WorldItems item)
        {
            WorldItems rItem = null;
            float distance = 0;
            foreach (WorldItems itm in Items)
            {
                if (item.itm.code.Trim().ToLower() == itm.itm.code.Trim().ToLower() && item.itm.Amount == itm.itm.Amount && isNearItem(itm, item))
                {
                    float d = new Vec3f(item.pos).getDistance((Vec3f)itm.pos);
                    if(rItem == null || d < distance)
                    {
                        distance = d;
                        rItem = itm;
                    }
                }
            }
            return rItem;
        }

        public bool isNearItem(WorldItems a, WorldItems b)
        {
            if (!Player.isSameMap(a.world, b.world))
                return false;
            if (new Vec3f(a.pos).getDistance((Vec3f)b.pos) < ItemsMinDistance)
                return true;
            return false;
        }

        #endregion

        

        #region Container

        public int getContainerCount(String world)
        {
            int count = 0;
            for (int i = 0; i < container.Count; i++)
            {
                if (Player.isSameMap(container[i].world, world))
                {
                    count++;
                }
            }
            return count;
        }

        public void InsertContainerItem(String item, int amount, String conName, float[] pos, String world)
        {
            WorldContainer wc = getContainer(conName, world, pos);
            bool newWC = false;
            if (wc == null)
            {
                wc = new WorldContainer();
                wc.name = conName;
                wc.world = world;
                wc.pos = pos;
                wc.itemList = new List<item>();

                container.Add(wc);
                //TODO: SQLite Insert
                if (ContainerInsert != null)
                {
                    ContainerInsert(this, new EventArgs(), conName, pos, world);
                }
            }

            
            item itm = getContainerItem(wc, item);
            if (itm == null)
            {
                itm = new item();
                itm.Amount = amount;
                itm.code = item;
                itm.inInv = false;

                wc.itemList.Add(itm);
                //TODO: SQLite Insert
                if (ContainerItemInsert != null)
                {
                    ContainerItemInsert(this, new EventArgs(), item, amount, conName, pos, world);
                }
            }
            else
            {
                itm.Amount += amount;
                //TODO: SQLite Update
                if (ContainerItemUpdate != null)
                {
                    ContainerItemUpdate(this, new EventArgs(), item, itm.Amount, conName, pos, world);
                }
            }
        }

        public void RemoveContainerItem(String item, int amount, String conName, float[] pos, String world)
        {
            WorldContainer wc = getContainer(conName, world, pos);
            if (wc == null)
                return;

            item itm = getContainerItem(wc, item);
            if (itm == null)
                return;
            if (itm.Amount - amount <= 0)
            {
                wc.itemList.Remove(itm);
                //TODO: SQLite Delete
                if (ContainerItemRemove != null)
                {
                    ContainerItemRemove(this, new EventArgs(), item, amount, conName, pos, world);
                }
            }
            else
            {
                itm.Amount -= amount;
                //TODO: SQLite Update
                if (ContainerItemUpdate != null)
                {
                    ContainerItemUpdate(this, new EventArgs(), item, itm.Amount, conName, pos, world);
                }
            }

            if (wc.itemList.Count == 0)
            {
                container.Remove(wc);

                if (ContainerInsert != null)
                {
                    ContainerRemove(this, new EventArgs(), conName, pos, world);
                }
            }
        }

        public item getContainerItem( WorldContainer  container, String item)
        {
            foreach (item itm in container.itemList)
            {
                if (itm.code.Trim().ToLower() == item.Trim().ToLower())
                {
                    return itm;
                }
            }
            return null;
        }

        public WorldContainer getContainer(String name, String world,  float[] pos)
        {
            foreach (WorldContainer wc in container)
            {
                if (Player.isSameMap(world, wc.world) && new Vec3f(pos).getDistance((Vec3f)wc.pos) <= ContainerMinDistance
                    && name == wc.name)
                {
                    return wc;
                }
            }
            return null;
        }
        #endregion

        
        public static int ContainerMinDistance = 10;
        public static int ItemsMinDistance = 1000;
        public static int MobInterMinDistance = 10;

        public bool SaveInDB = true;
    }
}
