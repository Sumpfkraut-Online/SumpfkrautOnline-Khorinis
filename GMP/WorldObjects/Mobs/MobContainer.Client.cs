using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects.Character;
using GUC.Types;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobContainer
    {
        public override void Spawn(String map, Vec3f position, Vec3f direction)
        {
            this.Map = map;
            this.Position = position;
            this.Direction = direction;

            spawned = true;

            if (this.Address != 0)
                return;

            if (this.Map != Player.Hero.Map)
                return;

            Process process = Process.ThisProcess();

            oCMobContainer gVob = oCMobContainer.Create(process);
            gVob.VobType = zCVob.VobTypes.MobContainer;


            this.Address = gVob.Address;
            sWorld.SpawnedVobDict.Add(this.Address, this);

            setVobData(process, gVob);
            setMobInterData(process, gVob);
            setMobLockableData(process, gVob);
            setMobContainerData(process, gVob);
            

            oCGame.Game(process).World.AddVob(gVob);

            triggerMobInter(process, gVob);


        }

        protected virtual void setMobContainerData(Process process, oCMobContainer vob)
        {
            if (this.itemList == null || this.itemList.Count == 0)
                return;
            foreach (Item item in this.itemList)
            {
                addItemToContainer(item);
            }
        }

        partial void addItemToContainer(Item item)
        {
            if (this.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCItem it = null;
            if (item.Address == 0)
                it = oCObjectFactory.GetFactory(process).CreateItem("ITGUC_" + item.ItemInstance.ID);
            else
                it = new oCItem(process, item.Address);
            it.Amount = item.Amount;

            oCMobContainer mobContainer = new oCMobContainer(Process.ThisProcess(), Address);
            mobContainer.Insert(it);

            if (item.Address == 0)
            {
                item.Address = it.Address;
                sWorld.SpawnedVobDict.Add(item.Address, item);
            }
        }

        public override VobSendFlags Read(RakNet.BitStream stream)
        {
            VobSendFlags b = base.Read(stream);

            if(b.HasFlag(VobSendFlags.MCItemList))
            {
                int itemCount = 0;
                stream.Read(out itemCount);
                for(int i = 0; i < itemCount; i++){
                    int itemID = 0;
                    stream.Read(out itemID);

                    if (!sWorld.VobDict.ContainsKey(itemID))
                        throw new Exception("Item was not found! :"+itemID);

                    Item itm = (Item)sWorld.VobDict[itemID];
                    addItem(itm);
                }
            }

            return b;
        }
    }
}
