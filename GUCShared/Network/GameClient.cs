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
            void SetControl(NPC npc);
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

        partial void pSetControl(NPC npc);
        public void SetControl(NPC npc)
        {
            if (npc == null)
                throw new ArgumentNullException("NPC is null!");

            pSetControl(npc);
        }

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
