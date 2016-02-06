using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.Network;
using GUC.WorldObjects.Collections;

namespace GUC.WorldObjects
{
    public partial class NPC : Vob//, IContainer
    {
        public override VobTypes VobType { get { return VobTypes.NPC; } }

        #region ScriptObject

        public partial interface IScriptNPC : IScriptVob
        {
        }

        new public IScriptNPC ScriptObject { get { return (IScriptNPC)base.ScriptObject; } }

        #endregion

        #region Properties

        new public NPCInstance Instance { get { return (NPCInstance)base.Instance; } }

        protected ushort hpmax = 100;
        protected ushort hp = 100;

        protected NPCStates state = NPCStates.Stand;

        protected MobInter usedMob = null;

        protected Item drawnItem = null;
        protected bool inAttackMode = false;
        public bool InAttackMode
        {
            get { return inAttackMode; }
        }

        //public ItemContainer Inventory { get; protected set; }
        //protected Dictionary<byte, Item> equippedSlots = new Dictionary<byte, Item>();
        //public IEnumerable<Item> GetEquippedItems() { return equippedSlots.Values; }

        public string Name { get { return Instance.Name; } }
        public string BodyMesh { get { return Instance.BodyMesh; } }
        public int BodyTex { get { return Instance.BodyTex; } }
        public string HeadMesh { get { return Instance.HeadMesh; } }
        public int HeadTex { get { return Instance.HeadTex; } }

        #endregion
        
        #region Constructors

        /// <summary>
        /// Creates a new Vob with the given Instance and ID or [-1] a free ID.
        /// </summary>
        public NPC(IScriptNPC scriptObject, NPCInstance instance, int id = -1) : base(scriptObject, instance, id)
        {
        }

        /// <summary>
        /// Creates a new Vob by reading a networking stream.
        /// </summary>
        public NPC(IScriptNPC scriptObject, PacketReader stream) : base(scriptObject, stream)
        {
        }

        #endregion

        #region Read & Write

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(hpmax);
            stream.Write(hp);

            /*stream.Write((byte)equippedSlots.Count);
            foreach (KeyValuePair<byte, Item> slot in equippedSlots)
            {
                stream.Write(slot.Key);
                slot.Value.WriteEquipProperties(stream);
            }

            if (drawnItem == null)
            {
                stream.Write(false);
            }
            else
            {
                stream.Write(true);
                drawnItem.WriteEquipProperties(stream);
            }

            //Overlays*/
        }

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
        }

        #endregion
    }
}
