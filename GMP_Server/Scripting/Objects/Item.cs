using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.Objects
{
    public class Item : Vob
    {
        internal Item(WorldObjects.Item item)
            : base(item)
        {
            
        }

        public Item(ItemInstance instance, int amount)
            : this(instance, amount, true)
        {
            
        }

        protected Item(ItemInstance instance, int amount, bool useCreate)
            : base(new GUC.WorldObjects.Item(instance.itemInstances, amount))
        {


            if (useCreate)
                CreateVob();
        }

        internal WorldObjects.Item ProtoItem { get { return (WorldObjects.Item)this.vob; } }

        public override void Spawn(string world, Types.Vec3f position, Types.Vec3f direction)
        {
            if (ProtoItem.Container != null)
                throw new Exception("Item can be only spawned if not in a container!");
            base.Spawn(world, position, direction);
        }
        

        public override void setVisual(string visual)
        {
            throw new Exception("Visual of Item can't be set!");
        }

        public void Delete()
        {
            if (!this.created || !this.ProtoItem.Created)
                throw new Exception("This Item was not created!");
            setAmount(0);
        }

        public void setAmount(int amount)
        {
            ProtoItem.Amount = amount;
            if (ProtoItem.Amount < 0)
            {
                ProtoItem.Amount = 0;
            }

            if (ProtoItem.Amount == 0)
            {
                this.created = false;
                ProtoItem.Created = false;

                GUC.WorldObjects.sWorld.removeVob(this.ProtoItem);
            }

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ItemChangeAmount);
            stream.Write(this.ProtoItem.ID);
            stream.Write(amount);
            
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }


        public void toContainer(NPCProto proto)
        {
            proto.proto.addItem(this.ProtoItem);
            _toContainer(proto);
        }

        public void toContainer(Mob.MobContainer container)
        {
            container.Proto.addItem(this.ProtoItem);
            _toContainer(container);
        }

        private void _toContainer(Vob vob)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ItemChangeContainer);
            stream.Write(this.ProtoItem.ID);
            stream.Write(vob.ID);

            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public ItemInstance ItemInstance { get { return ProtoItem.ItemInstance.ScriptingProto; } }
        public int Amount { get { return ProtoItem.Amount; } set { setAmount(value); } }


        #region ItemInstanceSetter

        public int getProtection(DamageTypeIndex index)
        {
            return ItemInstance.getProtection(index);
        }

        public int getProtection(DamageType index)
        {
            return getProtection(index.getDamageTypeIndex());
        }

        public DamageType DamageType { get { return ItemInstance.DamageType; } }
        public int TotalDamage { get { return ItemInstance.TotalDamage; } }

        #endregion
    }
}
