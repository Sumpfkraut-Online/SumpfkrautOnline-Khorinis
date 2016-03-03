using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Network
{
    public partial class GameClient : GameObject
    {
        #region ScriptObject

        /// <summary>
        /// The ScriptObject interface
        /// </summary>
        public partial interface IScriptClient : IScriptGameObject
        {
            void OnReadMenuMsg(PacketReader stream);
            void OnReadIngameMsg(PacketReader stream);
        }

        /// <summary>
        /// The ScriptObject of this Vob.
        /// </summary>
        public new IScriptClient ScriptObject
        {
            get { return (IScriptClient)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        internal NPC character = null;
        public NPC Character { get { return this.character; } }

        new public int ID { get { return base.ID; } }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
        }

        protected override void WriteProperties(PacketWriter stream)
        {
        }

        #endregion
    }
}
