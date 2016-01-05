using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects.Instances;
using GUC.Network;

namespace GUC.WorldObjects
{
    public partial class NPC : Vob
    {
        public static readonly Collections.VobDictionary NPCs = Vob.AllVobs.GetDict(VobTypes.NPC);

        new public NPCInstance Instance { get; protected set; }

        protected ushort hpmax = 100;
        protected ushort hp = 100;

        protected NPCStates state = NPCStates.Stand;
        public MobInter UsedMob;
        protected Item drawnItem = null;

        protected Dictionary<byte, Item> equippedSlots = new Dictionary<byte, Item>();

        public string Name { get { return Instance.Name; } }
        public string BodyMesh { get { return Instance.BodyMesh; } }
        public byte BodyTex { get { return Instance.BodyTex; } }
        public string HeadMesh { get { return Instance.HeadMesh; } }
        public byte HeadTex { get { return Instance.HeadTex; } }

        public byte BodyHeight { get { return Instance.BodyHeight; } }
        public byte BodyWidth { get { return Instance.BodyWidth; } }
        public short Fatness { get { return Instance.Fatness; } }

        internal NPC()
        {
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
        }

        internal override void ReadSpawnProperties(PacketReader stream)
        {
            base.ReadSpawnProperties(stream);

        }
    }
}
