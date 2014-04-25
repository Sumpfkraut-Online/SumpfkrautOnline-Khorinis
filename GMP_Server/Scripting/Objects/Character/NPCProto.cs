using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects;

namespace GUC.Server.Scripting.Objects.Character
{
    public class NPCProto : Vob
    {
        

        internal NPCProto(GUC.WorldObjects.Character.NPCProto proto)
            : base(proto)
        {
            
        }

        internal GUC.WorldObjects.Character.NPCProto proto { get { return (GUC.WorldObjects.Character.NPCProto)vob; } }


        #region Fields
        public String Name { get { return proto.Name; } set { setName(value); } }

        public int Strength {
            get { return proto.Attributes[(byte)NPCAttributeFlags.ATR_STRENGTH]; }
            set { setAttribute(NPCAttributeFlags.ATR_STRENGTH, value); }
        }

        public int Dexterity
        {
            get { return proto.Attributes[(byte)NPCAttributeFlags.ATR_DEXTERITY]; }
            set { setAttribute(NPCAttributeFlags.ATR_DEXTERITY, value); }
        }

        public int HP
        {
            get { return proto.Attributes[(byte)NPCAttributeFlags.ATR_HITPOINTS]; }
            set { setAttribute(NPCAttributeFlags.ATR_HITPOINTS, value); }
        }

        public int HPMax
        {
            get { return proto.Attributes[(byte)NPCAttributeFlags.ATR_HITPOINTS_MAX]; }
            set { setAttribute(NPCAttributeFlags.ATR_HITPOINTS_MAX, value); }
        }

        public int MP
        {
            get { return proto.Attributes[(byte)NPCAttributeFlags.ATR_MANA]; }
            set { setAttribute(NPCAttributeFlags.ATR_MANA, value); }
        }

        public int MPMax
        {
            get { return proto.Attributes[(byte)NPCAttributeFlags.ATR_MANA_MAX]; }
            set { setAttribute(NPCAttributeFlags.ATR_MANA_MAX, value); }
        }

        /// <summary>
        /// Returns an Integer with the callback ID!
        /// </summary>
        /// <returns></returns>
        public int CanSee(Vob vob)
        {
            Player plToCheck = null;
            if (this is NPC)
            {
                if(((WorldObjects.Character.NPC)proto).NpcController == null)
                    return -1;
            }else{
                plToCheck = (Player)this;
            }

            int callBackID = sWorld.getNewCallBackID();

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.CallbackNPCCanSee);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(vob.ID);
            using(RakNet.RakNetGUID guid = new RakNetGUID(plToCheck.proto.Guid))
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);

            return callBackID;
        }

        public Item EquippedArmor { 
            get {
                if (proto.Armor == null)
                    return null;
                return proto.Armor.ScriptingProto;
            } }

        public Item EquippedWeapon
        {
            get
            {
                if (proto.Weapon == null)
                    return null;
                return proto.Weapon.ScriptingProto;
            }
        }

        public Item EquippedRangeWeapon
        {
            get
            {
                if (proto.RangeWeapon == null)
                    return null;
                return proto.RangeWeapon.ScriptingProto;
            }
        }

        public Item getSlotItem( int index ){
            if (index >= 9)
                throw new ArgumentException("Index can't be greater than 8!");
            if (proto.Slots[index] == null)
                return null;
            return proto.Slots[index].ScriptingProto;
        }

        public int WeaponMode { get { return proto.WeaponMode; } }


        
        #endregion

        #region Methods

        public void setName(String name)
        {
            if (name == null)
                throw new ArgumentNullException("Name can't be null!");

            proto.Name = name;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ChangeNameMessage);
            stream.Write(proto.ID);
            stream.Write(name);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void dropUnconscious(float time)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (HP <= 0)
                return;
            HP = 1;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.DropUnconsciousMessage);
            stream.Write(this.ID);
            stream.Write(time);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void revive()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            HP = HPMax;
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ReviveMessage);
            stream.Write(this.proto.ID);
            
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public int getAttribute(NPCAttributeFlags attrib)
        {
            if (attrib >= NPCAttributeFlags.ATR_MAX)
                throw new ArgumentException("attribute is not valid!");
            return proto.Attributes[(byte)attrib];
        }

        public int getHitChances(NPCTalents talents)
        {
            if (talents != NPCTalents.H1 && talents != NPCTalents.H2 && talents != NPCTalents.Bow && talents != NPCTalents.CrossBow)
                throw new ArgumentException("Talents have to be fighting skills Like H1, H2, Bow or CrossBow!");

            return proto.Hitchances[(byte)talents];
        }

        public int getTalentValues(NPCTalents talent)
        {
            if (talent == NPCTalents.Unknown || talent == NPCTalents.MaxTalents)
                throw new ArgumentException("Invalid Talent: " + talent);

            return proto.TalentValues[(byte)talent];
        }

        public int getTalentSkills(NPCTalents talent)
        {
            if (talent == NPCTalents.Unknown || talent == NPCTalents.MaxTalents)
                throw new ArgumentException("Invalid Talent: " + talent);

            return proto.TalentSkills[(byte)talent];

        }

        public void setInvisible(bool invisible)
        {
            proto.IsInvisible = invisible;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCSetInvisibleMessage);
            stream.Write(proto.ID);
            stream.Write(invisible);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setInvisible(Player player, bool invisible)
        {
            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCSetInvisibleMessage);
            stream.Write(proto.ID);
            stream.Write(invisible);
            using(RakNetGUID guid = player.proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void hideName(bool invisible)
        {
            proto.hideName = invisible;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCSetInvisibleName);
            stream.Write(proto.ID);
            stream.Write(invisible);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void hideNameFrom(Player player, bool invisible)
        {
            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCSetInvisibleName);
            stream.Write(proto.ID);
            stream.Write(invisible);
            using (RakNetGUID guid = player.proto.GUID)
                Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        public void setAttribute(NPCAttributeFlags attrib, int value)
        {
            if (attrib >= NPCAttributeFlags.ATR_MAX)
                throw new ArgumentException("attribute is not valid!");
            proto.Attributes[(byte)attrib] = value;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCChangeAttributeMessage);
            stream.Write(proto.ID);
            stream.Write((byte)attrib);
            stream.Write(value);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setHitchances(NPCTalents talents, int value)
        {
            if (talents != NPCTalents.H1 && talents != NPCTalents.H2 && talents != NPCTalents.Bow && talents != NPCTalents.CrossBow)
                throw new ArgumentException("Talents have to be fighting skills Like H1, H2, Bow or CrossBow!");

            proto.Hitchances[(byte)talents] = value;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCChangeSkillMessage);
            stream.Write(proto.ID);
            stream.Write((byte)ChangeSkillType.Hitchances);
            stream.Write((byte)talents);
            stream.Write(value);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setTalentValues(NPCTalents talent, int value)
        {
            if (talent == NPCTalents.Unknown || talent == NPCTalents.MaxTalents)
                throw new ArgumentException("Invalid Talent: "+talent);

            proto.TalentValues[(byte)talent] = value;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCChangeSkillMessage);
            stream.Write(proto.ID);
            stream.Write((byte)ChangeSkillType.Value);
            stream.Write((byte)talent);
            stream.Write(value);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void setTalentSkills(NPCTalents talent, int value)
        {
            if (talent == NPCTalents.Unknown || talent == NPCTalents.MaxTalents)
                throw new ArgumentException("Invalid Talent: " + talent);

            proto.TalentSkills[(byte)talent] = value;

            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.NPCChangeSkillMessage);
            stream.Write(proto.ID);
            stream.Write((byte)ChangeSkillType.Skill);
            stream.Write((byte)talent);
            stream.Write(value);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public Item[] getItemList()
        {
            Item[] itemList = new Item[this.proto.ItemList.Count];
            int i = 0;
            foreach(WorldObjects.Item item in this.proto.ItemList){
                itemList[i] = item.ScriptingProto;
                i += 1;
            }

            return itemList;
        }

        



        public Item addItem(ItemInstance instance, int amount)
        {
            if (instance == null)
                throw new ArgumentNullException("Instance can't be null!");
            if (amount <= 0)
                throw new ArgumentException("amount can't be 0 or lower!");

            if (instance.itemInstances.Flags.HasFlag(Flags.ITEM_MULTI))
            {
                Item oldItem = null;
                foreach(WorldObjects.Item i in proto.ItemList){
                    if( i.ItemInstance == instance.itemInstances){
                        oldItem = i.ScriptingProto;
                        break;
                    }
                }

                if (oldItem != null)
                {
                    oldItem.Amount = oldItem.Amount + amount;
                    return oldItem;
                }
            }

            WorldObjects.Item itm = new WorldObjects.Item(instance.itemInstances, amount);
            sWorld.addVob(itm);
            proto.addItem(itm);

            if (!created)
                return itm.ScriptingProto;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AddItemMessage);
            stream.Write(proto.ID);
            itm.Write(stream);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


            return itm.ScriptingProto;
        }

        public void hit(NPCProto victim)
        {
            hit(victim, DamageType.DAM_BLUNT, 0, this.proto.Weapon.ScriptingProto, 0, null, null);
        }

        public void hit(NPCProto victim, DamageType damageMode)
        {
            hit(victim, damageMode, 0, this.proto.Weapon.ScriptingProto, 0, null, null);
        }

        public void hit(NPCProto victim, DamageType damageMode, int weaponMode)
        {
            hit(victim, damageMode, weaponMode, this.proto.Weapon.ScriptingProto, 0, null, null);
        }

        public void hit(NPCProto victim, DamageType damageMode, int weaponMode, Vec3f hitLoc)
        {
            hit(victim, damageMode, weaponMode, this.proto.Weapon.ScriptingProto, 0, hitLoc, null);
        }

        public void hit(NPCProto victim, DamageType damageMode, int weaponMode, Item weapon, Vec3f hitLoc)
        {
            hit(victim, damageMode, weaponMode, weapon, 0, hitLoc, null);
        }

        public void hit(NPCProto victim, DamageType damageMode, int weaponMode, Item weapon, int spellID, Vec3f hitLoc)
        {
            hit(victim, damageMode, weaponMode, weapon, spellID, hitLoc, null);
        }

        public void hit(NPCProto victim, DamageType damageMode, int weaponMode, Item weapon, int spellID, Vec3f hitLoc, Vec3f flyDir)
        {
            GUC.Server.Network.Messages.PlayerCommands.OnDamageMessage.Write(victim.proto, damageMode, hitLoc, flyDir, this.proto, weaponMode, spellID, weapon.ProtoItem, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS);
        }

        /// <summary>
        /// The npc/player drops the item into the world.
        /// </summary>
        /// <param name="item">the item needs to be in the inventory of the player</param>
        public void dropItem(Item item)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (item.ProtoItem.Container != this.proto)
                throw new Exception("Item does not belong to NPC!");
            proto.DropItem(item.ProtoItem);

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.DropItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ProtoItem.ID);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        /// <summary>
        /// The npc/player takes an item lying in the world.
        /// </summary>
        /// <param name="item">The item needs to be in the world.</param>
        public void takeItem(Item item)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (item.ProtoItem.Container == null || !(item.ProtoItem.Container is WorldObjects.World))
                throw new ArgumentException("Item has to be an Item in the World!");
            if (item.ProtoItem.Container != GUC.WorldObjects.sWorld.getWorld(this.proto.Map))
                throw new ArgumentException("Item is not in the same World!");

            proto.TakeItem(item.ProtoItem);

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.TakeItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ProtoItem.ID);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        /// <summary>
        /// This function can only be created with npcs which are not created!
        /// For created npcs and players use
        /// <code> setVisual(String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex)</code>
        /// </summary>
        /// <param name="visual"></param>
        public override void setVisual(String visual)
        {
            if (!created)
                vob.Visual = visual;
            else
                throw new NotImplementedException("You can't use setVisual with NPCs, use SetVisual(String visual, ....) instead!");
        }


        public String BodyMesh { get { return proto.BodyMesh; } }
        public String HeadMesh { get { return proto.HeadMesh; } }

        public int SkinColor { get { return proto.SkinColor; } }
        public int BodyTex { get { return proto.BodyTex; } }
        public int HeadTex { get { return proto.HeadTex; } }
        public int TeethTex { get { return proto.TeethTex; } }

        public bool isDead { get { return proto.IsDead; } }
        public bool isUnconscious { get { return proto.IsUnconcious; } }
        public bool isSwimming { get { return proto.IsSwimming; } }

        public virtual void setVisual(String visual, String bodyMesh, int bodyTex, int skinColor, String headMesh, int headTex, int TeethTex)
        {
            vob.Visual = visual;
            proto.BodyMesh = bodyMesh;
            proto.BodyTex = bodyTex;
            proto.SkinColor = skinColor;
            proto.HeadMesh = headMesh;
            proto.HeadTex = headTex;
            proto.TeethTex = TeethTex;
            
            if (!created)
                return;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVisualMessage);
            stream.Write(vob.ID);
            stream.Write(visual);
            stream.Write(bodyMesh);
            stream.Write(bodyTex);
            stream.Write(skinColor);
            stream.Write(headMesh);
            stream.Write(headTex);
            stream.Write(TeethTex);

            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void clearInventory()
        {
            foreach (WorldObjects.Item item in proto.ItemList.ToArray())
            {
                sWorld.removeVob(item);
            }

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ClearInventory);
            stream.Write(this.ID);

            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Equip(Item item)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (item == null)
                throw new Exception("Item was null!");
            if (item.ProtoItem.Container != this.proto)
                throw new Exception("Item must be in the inventory of the player!");

            if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF) ||
                item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF) ||
                item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR))
            {
                if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF) && this.EquippedWeapon != null)
                {
                    UnEquip(this.EquippedWeapon);
                }
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF) && this.EquippedRangeWeapon != null)
                {
                    UnEquip(this.EquippedRangeWeapon);
                }
                else if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR) && this.EquippedArmor != null)
                {
                    UnEquip(this.EquippedArmor);
                }
            }

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.EquipItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ID);
            stream.Write(true);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void UnEquip(Item item)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            if (item == null)
                throw new Exception("Item was null!");
            if (item.ProtoItem.Container != this.proto)
                throw new Exception("Item must be in the inventory of the player!");


            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.EquipItemMessage);
            stream.Write(proto.ID);
            stream.Write(item.ID);
            stream.Write(false);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        public void clearAnimation()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write("");
            stream.Write((byte)5);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void playAnimation( String anim )
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)1);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void stopAnimation(String anim)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)0);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void ApplyOverlay(String anim)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");

            if (!this.proto.Overlays.Contains(anim))
            {
                this.proto.Overlays.Add(anim);
            }

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)2);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void RemoveOverlay(String anim)
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");

            if (this.proto.Overlays.Contains(anim))
            {
                this.proto.Overlays.Remove(anim);
            }

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write(anim);
            stream.Write((byte)3);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void ClearOverlays()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");

            this.proto.Overlays.Clear();

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.AnimationMessage);
            stream.Write(proto.ID);
            stream.Write("");
            stream.Write((byte)4);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        public void startDialogAnimation()
        {
            if (!created)
                throw new Exception("The Player was not created! You can't use this function!");
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.StartDialogAnimMessage);
            stream.Write(proto.ID);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
        #endregion



        #region Events
        #region Callbacks

        #region OnCanSee
        public event GUC.Server.Scripting.Events.NPCCanSeeEventHandler OnCanSee;
        internal void iOnCanSee(int callbackID, NPCProto proto, Vob vob, bool canSee)
        {
            if (OnCanSee != null)
                OnCanSee(callbackID, proto, vob, canSee);
        }

        public static event GUC.Server.Scripting.Events.NPCCanSeeEventHandler sOnCanSeeCallback;
        internal static void iOnCanSeeCallback(int callbackID, NPCProto proto, Vob vob, bool canSee)
        {
            proto.iOnCanSee(callbackID, proto, vob, canSee);
            if (sOnCanSeeCallback != null)
                sOnCanSeeCallback(callbackID, proto, vob, canSee);
        }

        #endregion

        #endregion

        #region OnEquip
        public event GUC.Server.Scripting.Events.NPCEquipEventHandler OnEquip;
        internal void iOnEquip(NPCProto proto, Item item)
        {
            if (OnEquip != null)
                OnEquip(proto, item);
        }

        public static event GUC.Server.Scripting.Events.NPCEquipEventHandler sOnEquip;
        internal static void isOnEquip(NPCProto proto, Item item)
        {
            proto.iOnEquip(proto, item);
            item.ItemInstance.iOnEquip(proto, item);
            if (sOnEquip != null)
                sOnEquip(proto, item);
        }

        public event GUC.Server.Scripting.Events.NPCEquipEventHandler OnUnEquip;
        internal void iOnUnEquip(NPCProto proto, Item item)
        {
            if (OnUnEquip != null)
                OnUnEquip(proto, item);
        }

        public static event GUC.Server.Scripting.Events.NPCEquipEventHandler sOnUnEquip;
        internal static void isOnUnEquip(NPCProto proto, Item item)
        {
            proto.iOnUnEquip(proto, item);
            item.ItemInstance.iOnUnEquip(proto, item);
            if (sOnUnEquip != null)
                sOnUnEquip(proto, item);
        }

        #endregion




        public event GUC.Server.Scripting.Events.PlayerDamageEventHandler OnDamaged;
        internal void OnDamage(NPCProto victim, DamageType damageMode, Vec3f hitLoc, Vec3f flyDir, NPCProto attacker, int weaponMode, int spellID, Item weapon)
        {
            if (OnDamaged != null)
                OnDamaged(victim, damageMode, hitLoc, flyDir, attacker, weaponMode, spellID, weapon);
        }

        public event GUC.Server.Scripting.Events.PlayerItemEventHandler OnTakeItem;
        internal void OnItemTaked(NPCProto npc, Item item, int amount)
        {
            if (OnTakeItem != null)
                OnTakeItem(npc, item, amount);
        }

        public event GUC.Server.Scripting.Events.PlayerItemEventHandler OnDropItem;
        internal void OnItemDroped(NPCProto npc, Item item, int amount)
        {
            if (OnDropItem != null)
                OnDropItem(npc, item, amount);
        }

        #endregion
        #region Static Events:

        public static event Events.PlayerDamageEventHandler OnDamages;

        internal static void OnPlayerDamages(NPCProto victim, DamageType damageMode, Vec3f hitLoc, Vec3f flyDir, NPCProto attacker, int weaponMode, int spellID, Item weapon)
        {
            victim.OnDamage(victim, damageMode, hitLoc, flyDir, attacker, weaponMode, spellID, weapon);
            if (OnDamages != null)
                OnDamages(victim, damageMode, hitLoc, flyDir, attacker, weaponMode, spellID, weapon);
        }


        public static event GUC.Server.Scripting.Events.PlayerItemEventHandler OnTakesItem;
        internal static void OnItemTakes(NPCProto npc, Item item, int amount)
        {
            npc.OnItemTaked(npc, item, amount);
            if (OnTakesItem != null)
                OnTakesItem(npc, item, amount);
        }

        public static event GUC.Server.Scripting.Events.PlayerItemEventHandler OnDropsItem;
        internal static void OnItemDrops(NPCProto npc, Item item, int amount)
        {
            npc.OnItemDroped(npc, item, amount);
            if (OnDropsItem != null)
                OnDropsItem(npc, item, amount);
        }


        


        #endregion

    }
}
