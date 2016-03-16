using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Animation
{
    public class Animation : GameObject
    {
        #region Properties

        /// <summary>
        /// The Gothic-Name of the animation.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// The duration of the animation in ms.
        /// </summary>
        public int Duration = 0;

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            this.ID = stream.ReadUShort();
            this.Duration = stream.ReadInt();
            this.Name = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            stream.Write((ushort)this.ID);
            stream.Write(this.Duration);
            stream.Write(this.Name);
        }

        #endregion
    }
}
