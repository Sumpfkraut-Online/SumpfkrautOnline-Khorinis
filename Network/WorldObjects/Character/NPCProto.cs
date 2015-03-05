using System;
using System.Collections.Generic;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Mobs;
using GUC.Types;

namespace GUC.WorldObjects.Character
{
    internal abstract partial class NPCProto : Vob, IContainer
    {
        

        #region General
        protected String name;

        public String Name { get { return this.name; } set { this.name = value; } }
        #endregion

        public bool IsInvisible = false;
        public bool hideName = false;

        public String PortalRoom = "";

        public int TrueGuild = 0;
        public Item ActiveSpell = null;



        #region Animation
        public List<String> Overlays = new List<string>();
        protected short animation = short.MaxValue;

        public short Animation { get { return animation; } set { animation = value; } }
        public long AnimationStartTime = 0;
        #endregion


        public float Fatness = 1.0f;
        #region Scale
        protected float[] scale = new float[] { 1f, 1f, 1f };

        public Vec3f Scale
        {
            get { return (Vec3f)this.scale; }
            set
            {
                this._Scale = value.Data;
            }
        }
        public float[] _Scale
        {
            get { return this.scale; }
            set
            {
                if (value == null || value.Length < 3)
                {
                    this.scale[0] = 1;
                    this.scale[1] = 1;
                    this.scale[2] = 1;

                    return;
                }
                this.scale[0] = value[0];
                this.scale[1] = value[1];
                this.scale[2] = value[2];
            }
        }

        #endregion





        #region Appearance
        protected String bodyMesh = "hum_body_Naked0";
        protected String headMesh = "Hum_Head_Pony";

        protected int bodyTex = 9, skinColor = 0, headTex = 18, teethTex = 0;

        public int BodyTex { get { return bodyTex; } set { bodyTex = value; } }
        public int SkinColor { get { return skinColor; } set { skinColor = value; } }
        public int HeadTex { get { return headTex; } set { headTex = value; } }
        public int TeethTex { get { return teethTex; } set { teethTex = value; } }

        public String BodyMesh { get { return this.bodyMesh; } set { this.bodyMesh = value.ToUpper(); } }
        public String HeadMesh { get { return this.headMesh; } set { this.headMesh = value.ToUpper(); } }
        #endregion

        #region Fíght
        protected int weaponMode = 0;
        public int WeaponMode { get { return weaponMode; } set { weaponMode = value; } }
        #endregion

        #region Equipment
        protected Item armor = null;
        protected Item weapon = null;
        protected Item rangeWeapon = null;

        protected Item[] slots = new Item[9];

        public List<Item> EquippedList = new List<Item>();

        public Item Armor { get { return armor; } set { armor = value; } }
        public Item Weapon { get { return weapon; } set { weapon = value; } }
        public Item RangeWeapon { get { return rangeWeapon; } set { rangeWeapon = value; } }

        public Item[] Slots { get { return slots; } }
        #endregion

        #region Vobs
        protected Vob focusVob = null;
        protected NPCProto enemy = null;
        protected MobInter mobInter = null;
        public Vob FocusVob { get { return focusVob; } set { focusVob = value; } }
        public NPCProto Enemy { get { return enemy; } set { enemy = value; } }
        public MobInter MobInter { get { return mobInter; } set { mobInter = value; } }
        #endregion

        #region States
        public bool IsDead = false;
        public bool IsUnconcious = false;
        public bool IsSwimming = false;
        #endregion

        #region Talents
        protected int[] attributes = new int[(byte)NPCAttribute.ATR_MAX]{1, 1, 0, 0, 0, 0, 0, 0};
        protected int[] hitchances = new int[5];
        protected int[] talentSkills = new int[(byte)NPCTalent.MaxTalents];
        protected int[] talentValues = new int[(byte)NPCTalent.MaxTalents];

        public int[] Attributes { get { return attributes; } set {
            if (value == null)
                throw new ArgumentNullException("Attribute Value can't be null!");
            if (value.Length != (byte)NPCAttribute.ATR_MAX)
                throw new ArgumentException("Value needs a length of " + (byte)NPCAttribute.ATR_MAX);
            for (int i = 0; i < value.Length; i++)
                attributes[i] = value[i];
        } }

        public int[] TalentSkills
        {
            get { return talentSkills; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("TalentSkills Value can't be null!");
                if (value.Length != (byte)NPCTalent.MaxTalents)
                    throw new ArgumentException("Value needs a length of " + ((byte)NPCTalent.MaxTalents));
                for (int i = 0; i < value.Length; i++)
                    talentSkills[i] = value[i];
            }
        }

        public int[] TalentValues
        {
            get { return talentValues; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("TalentValues Value can't be null!");
                if (value.Length != (byte)NPCTalent.MaxTalents)
                    throw new ArgumentException("Value needs a length of " + ((byte)NPCTalent.MaxTalents));
                for (int i = 0; i < value.Length; i++)
                    talentValues[i] = value[i];
            }
        }

        public int[] Hitchances
        {
            get { return hitchances; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Hitchances Value can't be null!");
                if (value.Length != 5)
                    throw new ArgumentException("Value needs a length of " + 5);
                for (int i = 0; i < value.Length; i++)
                    hitchances[i] = value[i];
            }
        }
        #endregion

        

        #region Items:
        protected List<Item> itemList = new List<Item>();

        public Item getItemByInstance(ItemInstance instance)
        {
            foreach (Item i in itemList)
            {
                if (i.ItemInstance == instance)
                {
                    return i;
                }
            }
            return null;
        }

        public Item HasItem(int id)
        {
            Item it = null;
            ItemInstance ii = ItemInstance.ItemInstanceDict[id];

            foreach (Item i in itemList)
            {
                if (i.ItemInstance == ii)
                {
                    return i;
                }
            }

            return it;
        }

        public List<Item> ItemList { get { return itemList; } }
        public Item TakeItem(Item item)
        {
            if (item.Container != null)
                item.Container.removeItem(item);

            Item oldItem = getItemByInstance(item.ItemInstance);

            if (oldItem != null && item.ItemInstance.Flags.HasFlag(Flags.ITEM_MULTI))//Delete Old Item
            {
                item.Map = null;
                sWorld.getWorld(this.Map).removeVob(item);

                oldItem.Amount += item.Amount;
            }
            else
            {
                item.Container = this;
                itemList.Add(item);
            }

            if (oldItem == null)
                return item;
            else
                return oldItem;
            
        }
        public void DropItem(Item item)
        {
            item.Position = this.Position;
            item.Direction = this.Direction;
            sWorld.getWorld(this.Map).addItem(item);

        }
        public abstract void StealItem(Vob other, String item, int amount);
        public abstract void StealItem(Vob other, Item item, int amount);


        public void addItem(Item item)
        {
            if (item.Container != null)
                item.Container.removeItem(item);

            item.Container = this;
            item.Map = null;
            itemList.Add(item);

            addItemToContainer( item );
        }

        public void addItem(Item item, int amount)
        {
            throw new NotImplementedException();
        }

        public void addItem(string item, int amount)
        {
            throw new NotImplementedException();
        }

        public void removeItem(Item item)
        {
            item.Container = null;
            itemList.Remove(item);

#if D_CLIENT
            //if (this.Address != 0 && item.Address != 0)
            //{
            //    new Gothic.zClasses.oCNpc(WinApi.Process.ThisProcess(), this.Address).RemoveFromInv(new Gothic.zClasses.oCItem(WinApi.Process.ThisProcess(), item.Address), item.Amount);
            //}
#endif
        }

        public void removeItem(Item item, int amount)
        {
            item.Amount -= amount;
            if (item.Amount <= 0)
                removeItem(item);
        }

        public void removeItem(string item, int amount)
        {
            throw new NotImplementedException();
        }

        partial void addItemToContainer(Item item);

        #endregion
    }
}
