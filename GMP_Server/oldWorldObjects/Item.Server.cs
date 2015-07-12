using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;

namespace GUC.WorldObjects
{
    internal partial class Item
    {
        public Item(ItemInstance instance, int amount)
            : this()
        {
            if (instance == null)
                new ArgumentNullException("Instance can't be null!");
            this.itemInstance = instance;
            this.amount = amount;
        }

        

        protected override Vob.VobSendFlags getSendInfo()
        {
            Vob.VobSendFlags b = 0;
            if (this.amount != 1)
                b |= VobSendFlags.Amount;
            
            return b;
        }

        public override Vob.VobSendFlags Write(BitStream stream)
        {
            Vob.VobSendFlags b = base.Write(stream);
            stream.Write(this.itemInstance.ID);
            if(b.HasFlag(Vob.VobSendFlags.Amount))
                stream.Write(this.amount);

            return b;
        }

        protected Server.Scripting.Objects.Item scriptingProto;

        public Server.Scripting.Objects.Item ScriptingProto
        {
            get
            {
                if (this.scriptingProto == null)
                {
                    this.scriptingProto = new Server.Scripting.Objects.Item(this);
                }
                return this.scriptingProto;
            }
        }


    }
}
