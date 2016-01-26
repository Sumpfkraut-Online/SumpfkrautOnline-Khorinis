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
    public partial class NPC : Vob, IContainer
    {
        public partial interface IScriptNPC : IScriptVob
        {
        }

        new public const VobTypes sVobType = NPCInstance.sVobType;
        public static readonly VobDictionary NPCs = Vob.AllVobs.GetDict(sVobType);

        new public NPCInstance Instance { get; protected set; }
        new public IScriptNPC ScriptObj { get; protected set; }

        protected ushort hpmax = 100;
        protected ushort hp = 100;

        protected NPCStates state = NPCStates.Stand;
        protected MobInter usedMob = null;
        protected Item drawnItem = null;
        
        public ItemContainer Inventory { get; protected set; }
        protected Dictionary<byte, Item> equippedSlots = new Dictionary<byte, Item>();

        public string Name { get { return Instance.Name; } }
        public string BodyMesh { get { return Instance.BodyMesh; } }
        public byte BodyTex { get { return Instance.BodyTex; } }
        public string HeadMesh { get { return Instance.HeadMesh; } }
        public byte HeadTex { get { return Instance.HeadTex; } }

        public byte BodyHeight { get { return Instance.BodyHeight; } }
        public byte BodyWidth { get { return Instance.BodyWidth; } }
        public short Fatness { get { return Instance.Fatness; } }

        public NPC(NPCInstance instance, IScriptNPC scriptObject) : base(instance, scriptObject)
        {
            this.Inventory = new ItemContainer(this);
        }

        internal override void WriteSpawnProperties(PacketWriter stream)
        {
            base.WriteSpawnProperties(stream);

            stream.Write(hpmax);
            stream.Write(hp);

            stream.Write((byte)equippedSlots.Count);
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

            //Overlays

            this.ScriptObj.OnWriteSpawnProperties(stream);
        }

        internal override void ReadSpawnProperties(PacketReader stream)
        {
            base.ReadSpawnProperties(stream);

            this.ScriptObj.OnReadSpawnProperties(stream);
        }
    }
}
