using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.WorldObjects.Character;
using WinApi;
using Gothic.zClasses;

namespace GUC.WorldObjects
{
    internal partial class Item
    {
        public override void Spawn(string map, Types.Vec3f position, Types.Vec3f direction)
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
            oCItem item = oCObjectFactory.GetFactory(process).CreateItem("ITGUC_"+this.ItemInstance.ID);
            item.Amount = this.Amount;

            
            oCGame.Game(process).World.AddVob(item);
            this.Address = item.Address;
            sWorld.SpawnedVobDict.Add(item.Address, this);

            this.setDirection(Direction);
            this.setPosition(Position);
            

        }



        public override Vob.VobSendFlags Read(BitStream stream)
        {
            Vob.VobSendFlags sendInfo = base.Read(stream);

            int instanceID, amount = 1;
            stream.Read(out instanceID);
            if(sendInfo.HasFlag(Vob.VobSendFlags.Amount))
                stream.Read(out amount);

            if (!ItemInstance.ItemInstanceDict.ContainsKey(instanceID))
                throw new Exception("ItemInstance with id \"" + instanceID + "\" was not found! " + ItemInstance.ItemInstanceDict.Count);

            this.itemInstance = ItemInstance.ItemInstanceDict[instanceID];
            this.amount = amount;

            return sendInfo;
        }
    }
}
