using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;
using GUC.Server.Network.Messages;

namespace GUC.Server.WorldObjects
{
    public class NPC : AbstractCtrlVob, ItemContainer
    {
        #region Instance
        protected NPCInstance instance;
        public NPCInstance Instance { get { return instance; } }
        #endregion

        //Things everyone sees
        #region Appearance

        public NPCAppearance Appearance { get; private set; }
        public class NPCAppearance
        {
            NPC npc;
            public NPCAppearance(NPC npc)
            {
                this.npc = npc;

                name = npc.instance.Name;
                visual = npc.instance.Visual;
                bodyMesh = npc.instance.BodyMesh;
                bodyTex = npc.instance.BodyTex;
                headMesh = npc.instance.HeadMesh;
                headTex = npc.instance.HeadTex;
                bodyHeight = npc.instance.BodyHeight;
                bodyWidth = npc.instance.BodyWidth;
                fatness = npc.instance.Fatness;

                changeFlags = 0;
            }

            protected short changeFlags;

            #region Fields

            protected string name;
            public string Name
            {
                get { return name; }
                set
                {
                    name = value;
                    if (name == npc.instance.Name)
                    {
                        changeFlags &= ~NPCAppearanceFlags.Name;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.Name;
                    }
                }
            }

            protected string visual;
            public string Visual
            {
                get { return visual; }
                set
                {
                    visual = value.ToUpper();
                    if (visual == npc.instance.Visual)
                    {
                        changeFlags &= ~NPCAppearanceFlags.Visual;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.Visual;
                    }
                }
            }

            protected string bodyMesh;
            public string BodyMesh
            {
                get { return bodyMesh; }
                set
                {
                    bodyMesh = value.ToUpper();
                    if (bodyMesh == npc.instance.BodyMesh)
                    {
                        changeFlags &= ~NPCAppearanceFlags.BodyMesh;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.BodyMesh;
                    }
                }
            }

            protected byte bodyTex;
            public int BodyTex
            {
                get { return bodyTex; }
                set
                {
                    bodyTex = value > byte.MaxValue ? byte.MaxValue : (byte)value;
                    if (bodyTex == npc.instance.BodyTex)
                    {
                        changeFlags &= ~NPCAppearanceFlags.BodyTex;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.BodyTex;
                    }
                }
            }

            protected string headMesh;
            public string HeadMesh
            {
                get { return headMesh; }
                set
                {
                    headMesh = value.ToUpper();
                    if (headMesh == npc.instance.HeadMesh)
                    {
                        changeFlags &= ~NPCAppearanceFlags.HeadMesh;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.HeadMesh;
                    }
                    changeFlags &= ~NPCAppearanceFlags.HumanHead;
                }
            }
            protected HumanHeadMesh hheadMesh; //special case for humans
            public HumanHeadMesh HumanHead
            {
                get { return hheadMesh; }
                set
                {
                    hheadMesh = value;
                    if (hheadMesh == null)
                    {
                        changeFlags &= ~NPCAppearanceFlags.HumanHead;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.HumanHead;
                    }
                }
            }

            protected byte headTex;
            public int HeadTex
            {
                get { return headTex; }
                set
                {
                    headTex = value > byte.MaxValue ? byte.MaxValue : (byte)value;
                    if (headTex == npc.instance.HeadTex)
                    {
                        changeFlags &= ~NPCAppearanceFlags.HeadTex;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.HeadTex;
                    }
                }
            }

            protected float bodyHeight;
            public float BodyHeight
            {
                get { return bodyHeight; }
                set
                {
                    bodyHeight = value;
                    if (bodyHeight == npc.instance.BodyHeight)
                    {
                        changeFlags &= ~NPCAppearanceFlags.BodyHeight;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.BodyHeight;
                    }
                }
            }

            protected float bodyWidth;
            public float BodyWidth
            {
                get { return bodyWidth; }
                set
                {
                    bodyWidth = value;
                    if (bodyWidth == npc.instance.BodyWidth)
                    {
                        changeFlags &= ~NPCAppearanceFlags.BodyWidth;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.BodyWidth;
                    }
                }
            }

            protected float fatness;
            public float Fatness
            {
                get { return fatness; }
                set
                {
                    fatness = value;
                    if (fatness == npc.instance.Fatness)
                    {
                        changeFlags &= ~NPCAppearanceFlags.Fatness;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.Fatness;
                    }
                }
            }

            protected byte voice;
            public int Voice
            {
                get { return voice; }
                set
                {
                    voice = value > byte.MaxValue ? byte.MaxValue : (byte)value;
                    if (voice == npc.instance.Voice)
                    {
                        changeFlags &= ~NPCAppearanceFlags.Voice;
                    }
                    else
                    {
                        changeFlags |= NPCAppearanceFlags.Voice;
                    }
                }
            }

            #endregion

            public void NetUpdate()
            {

            }

            internal void Write(BitStream stream)
            {
                stream.mWrite(changeFlags);
                if ((changeFlags & NPCAppearanceFlags.Name) != 0)
                {
                    stream.mWrite(name);
                }
                if ((changeFlags & NPCAppearanceFlags.Visual) != 0)
                {
                    stream.mWrite(visual);
                }
                if ((changeFlags & NPCAppearanceFlags.BodyMesh) != 0)
                {
                    stream.mWrite(bodyMesh);
                }
                if ((changeFlags & NPCAppearanceFlags.BodyTex) != 0)
                {
                    stream.mWrite(bodyTex);
                }
                if ((changeFlags & NPCAppearanceFlags.HumanHead) != 0)
                {
                    stream.mWrite((byte)hheadMesh);
                }
                else if ((changeFlags & NPCAppearanceFlags.HeadMesh) != 0)
                {
                    stream.mWrite(headMesh);
                }
                if ((changeFlags & NPCAppearanceFlags.HeadTex) != 0)
                {
                    stream.mWrite(headTex);
                }
                if ((changeFlags & NPCAppearanceFlags.BodyHeight) != 0)
                {
                    stream.mWrite(bodyHeight);
                }
                if ((changeFlags & NPCAppearanceFlags.BodyWidth) != 0)
                {
                    stream.mWrite(bodyWidth);
                }
                if ((changeFlags & NPCAppearanceFlags.Fatness) != 0)
                {
                    stream.mWrite(fatness);
                }
                if ((changeFlags & NPCAppearanceFlags.Voice) != 0)
                {
                    stream.mWrite(voice);
                }
            }
        }

        #endregion

        //Things only the playing Client should know
        #region Player stats

        public AttributeArray Attributes;
        public class AttributeArray
        {
            NPC npc;
            public AttributeArray(NPC npc)
            {
                this.npc = npc;
            }

            internal bool[] changed = new bool[NPCAttributes.MAX_ATTRIBUTES]; //for networking

            internal byte healthPercent = 0;
            ushort[] arr = new ushort[NPCAttributes.MAX_ATTRIBUTES];
            public int this[int i]
            {
                get { return arr[i]; }
                set
                {
                    arr[i] = value > ushort.MaxValue ? ushort.MaxValue : (ushort)value;
                    if (i <= NPCAttributes.Health_Max)
                    {
                        healthPercent = (byte)(100.0f * (float)arr[NPCAttributes.Health] / (float)arr[NPCAttributes.Health_Max]);
                    }
                    changed[i] = true;
                }
            }

            public void UpdateHealth(int value) //send immediatly
            {
                this[NPCAttributes.Health] = value;
                Write();
            }

            internal void Write()
            {
                BitStream stream = Program.server.SetupStream(NetworkID.NPCHitMessage);
                stream.mWrite(npc.ID);
                stream.mWrite(healthPercent);
                foreach (Client client in npc.cell.SurroundingClients(npc.client))
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE, 'W', client.guid, false);
                }

                if (npc.isPlayer) //Player gets all the info
                {
                    stream = Program.server.SetupStream(NetworkID.PlayerAttributeMessage);
                    for (int i = 0; i < NPCAttributes.MAX_ATTRIBUTES; i++)
                    {
                        stream.mWrite(changed[i]);
                        if (changed[i])
                        {
                            stream.mWrite(arr[i]);
                            changed[i] = false;
                        }
                    }
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE, 'M', npc.client.guid, false);
                }
            }
        }

        public ushort[] Talents = new ushort[NPCTalents.MAX_TALENTS];

        #endregion

        #region States

        protected NPCState state = NPCState.Stand;
        public NPCState State { get { return state; } internal set { state = value; } }

        protected NPCWeaponState wpState = NPCWeaponState.None;
        public NPCWeaponState WeaponState { get { return wpState; } internal set { wpState = value; } }

        #endregion

        #region Constructors

        public NPC()
        {
            //Attributes = new AttributeArray(this);
            //Appearance = new NPCAppearance(this);
        }

        #endregion

        #region Networking

        internal Client client;
        public bool isPlayer { get { return client != null; } }

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldNPCSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(pos);
            stream.mWrite(dir);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #endregion

        #region Equipment

        protected ItemInstance equippedMeleeWeapon;
        public ItemInstance EquippedMeleeWeapon { get { return equippedMeleeWeapon; } }

        protected ItemInstance equippedRangedWeapon;
        public ItemInstance EquippedRangedWeapon { get { return equippedRangedWeapon; } }

        protected ItemInstance equippedArmor;
        public ItemInstance EquippedArmor { get { return equippedArmor; } }

        public void Equip(ItemInstance inst)
        {
            /*if (inst.MainFlags == MainFlags.ITEM_KAT_NF)
            {
                equippedMeleeWeapon = inst;
            }
            else if (inst.MainFlags == MainFlags.ITEM_KAT_FF)
            {
                equippedRangedWeapon = inst;
            }
            else if (inst.MainFlags == MainFlags.ITEM_KAT_ARMOR)
            {
                equippedArmor = inst;
            }
            else return;*/
            NPCMessage.WriteEquipMessage(this.cell.SurroundingClients(), this, inst);
        }

        public void Unequip(ItemInstance inst)
        {
            if (inst == equippedMeleeWeapon)
                equippedMeleeWeapon = null;
            else if (inst == equippedRangedWeapon)
                equippedRangedWeapon = null;
            else if (inst == equippedArmor)
                equippedArmor = null;
            else return;

            NPCMessage.WriteEquipMessage(this.cell.SurroundingClients(), this, inst);
        }

        #endregion

        #region Itemcontainer

        protected Dictionary<ItemInstance, int> inventory = new Dictionary<ItemInstance, int>();
        public Dictionary<ItemInstance, int> Inventory { get { return inventory; } }

        protected int weight = 0;
        public int Weight { get { return weight; } }

        public bool HasItem(ItemInstance instance)
        {
            return HasItem(instance, 1);
        }

        public bool HasItem(ItemInstance instance, int amount)
        {
            int has;
            inventory.TryGetValue(instance, out has);
            return has >= amount;
        }

        public bool AddItem(ItemInstance instance)
        {
            return AddItem(instance, 1);
        }

        public bool AddItem(ItemInstance instance, int amount)
        {
            int newWeight = weight + instance.Weight * amount;
            if (Attributes[NPCAttributes.Capacity] >= newWeight)
            {
                weight = newWeight;
                if (inventory.ContainsKey(instance))
                {
                    inventory[instance] += amount;
                }
                else
                {
                    inventory.Add(instance, amount);
                }

                if (this.isPlayer)
                {
                    Network.Messages.InventoryMessage.WriteAddItem(client, instance, amount);
                }
                return true;
            }
            return false;
        }

        public void RemoveItem(ItemInstance instance)
        {
            RemoveItem(instance, 1);
        }

        public void RemoveItem(ItemInstance instance, int amount)
        {
            int current;
            if (inventory.TryGetValue(instance, out current))
            {
                if (current - amount <= 0)
                    inventory.Remove(instance);
                else
                    inventory[instance] -= amount;

                weight -= instance.Weight * amount;

                if (this.isPlayer)
                {
                    Network.Messages.InventoryMessage.WriteRemoveItem(client, instance, amount);
                }
            }
        }
        #endregion

        #region Events
        public delegate void OnHitHandler(NPC attacker, NPC target);
        public OnHitHandler OnHit;
        public static OnHitHandler sOnHit;

        internal void DoHit(NPC attacker)
        {
            if (sOnHit != null)
            {
                sOnHit(attacker, this);
            }
            if (OnHit != null)
            {
                OnHit(attacker, this);
            }
        }
        #endregion
    }
}
