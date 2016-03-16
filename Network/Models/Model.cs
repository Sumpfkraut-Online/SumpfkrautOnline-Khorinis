using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Models
{
    public class Model : GameObject
    {
        public string Visual;

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);

            this.Visual = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(this.Visual);
        }
    }
}
