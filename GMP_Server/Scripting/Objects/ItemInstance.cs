using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;
using GUC.Server.Scripting.Objects.Character;
using System.Collections;

namespace GUC.Server.Scripting.Objects
{
    /**
    * Base class for all patterns of different items which are used by the Item-class to actually create and spawn items into the world.
    * The name IntemInstance might be misleading because individual ingame items are actually instances of the Item-class.
    * Instances/objects of ItemInstance mere represent building plans/patterns which are passed to the Item-constructor(s).
    * It further holds a dictionary of these plans/patterns for later use.
    * @see Item
    */
    public class ItemInstance
    {
        /**
        * Distionary of building plans/patterns that are used in Item-instatiation (creating actual ingame items).
        */
        public static Dictionary<String, ItemInstance> ItemInstances = new Dictionary<string, ItemInstance>();


        internal GUC.WorldObjects.ItemInstance itemInstances;
        protected bool created;

        protected String mItemInstance = "";

        public static IEnumerable ToEnumerable()
        {
            foreach (ItemInstance item in ItemInstances.Values)
            {
                yield return item;
            }
        }

        public static ItemInstance getItemInstance(String name)
        {
            
            ItemInstance ii = null;
            ItemInstances.TryGetValue(name.ToUpper().Trim(), out ii);
            
            return ii;
        }

        public static ItemInstance getItemInstance(int id)
        {
            return WorldObjects.ItemInstance.ItemInstanceDict[id].ScriptingProto;
        }

        internal ItemInstance(WorldObjects.ItemInstance ii){
            itemInstances = ii;
        }

        protected ItemInstance(String instanceName)
        {
            instanceName = instanceName.Trim().ToUpper();
            if (instanceName == null)
                throw new ArgumentNullException("instanceName can not be null");
            if (instanceName.Length == 0)
                throw new ArgumentException("Instancename can not has a length of 0");
            if (ItemInstances.ContainsKey(instanceName))
                throw new ArgumentException("InstanceName is already in use! "+instanceName);


            itemInstances = new GUC.WorldObjects.ItemInstance();
            itemInstances.ScriptingProto = this;

            mItemInstance = instanceName;
            ItemInstances.Add(instanceName, this);
        }

        public ItemInstance(String instanceName, String name, String scemeName, int value, String visual, String effect)///Potions
            : this(instanceName, name, scemeName, value, MainFlags.ITEM_KAT_POTIONS, Flags.ITEM_MULTI, visual, null, effect)
        { }
        public ItemInstance(String instanceName, String name, DamageTypes dmgType, MainFlags mainFlags, Flags flags, int totalDamage, int range, int value, String visual)///Weapons
            : this(instanceName, name, null, null, null, value, mainFlags, flags, 0, dmgType, totalDamage, range, visual, null, null, 0)
        { }
        public ItemInstance(String instanceName, String name, int[] protection, int value, String visual, String visual_Change)//Armors!
            : this(instanceName, name, null, protection, null, value, MainFlags.ITEM_KAT_ARMOR, 0, ArmorFlags.WEAR_TORSO, 0, 0, 0, visual, visual_Change, null, 0)
        { }



        public ItemInstance(String instanceName, String name, String scemeName, int value, MainFlags mainFlags, Flags flags, String visual)
            : this(instanceName, name, scemeName, null, null, value, mainFlags, flags, 0, 0, 0, 0, visual, null)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int value, MainFlags mainFlags, Flags flags, String visual, String visual_Change, String effect)
            : this(instanceName, name, scemeName, null, null, value, mainFlags, flags, 0, 0, 0, 0, visual, visual_Change, effect)
        { }

        public ItemInstance(String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual)
            : this(instanceName, name, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemInstance(String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change)
            : this(instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, null, 0)
        {}

        public ItemInstance(String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : this(instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0)
        {}

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, "", 0)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, 0, null)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, null)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialType types, ItemInstance munition)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, munition)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialType types, ItemInstance munition)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, visualSkin, types, munition, false, false, false, false, false)
        { }
        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageTypes dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialType types, ItemInstance munition, bool keyInstance, bool torch, bool torchBurning, bool torchBurned, bool gold)
            : this(instanceName)
        {
            itemInstances.Name = name;
            itemInstances.Protection = protection;
            itemInstances.Damages = damages;
            itemInstances.Value = value;
            itemInstances.MainFlags = mainFlags;
            itemInstances.Flags = flags;
            itemInstances.Wear = armorFlags;
            itemInstances.Visual = visual;
            itemInstances.Visual_Change = visual_Change;
            itemInstances.Visual_skin = visualSkin;
            itemInstances.ScemeName = scemeName;
            itemInstances.Effect = effect;
            if(munition != null)
                itemInstances.Munition = munition.itemInstances;

            itemInstances.DamageType = dmgType;
            itemInstances.Range = range;
            itemInstances.TotalDamage = totalDamage;
            itemInstances.isKeyInstance = keyInstance;
            IsTorch = torch;
            IsTorchBurned = torchBurned;
            IsTorchBurning = torchBurning;
            IsGold = gold;

            CreateItemInstance();
            
        }

        

        /// <summary>
        /// For Potions:
        /// </summary>
        public ItemInstance(String instanceName, String name, int value, String visual, String effect)
            : this(instanceName)
        {
            itemInstances.Name = name;
            itemInstances.Value = value;
            itemInstances.Visual = visual;
            itemInstances.MainFlags = MainFlags.ITEM_KAT_POTIONS;
            itemInstances.Flags = Flags.ITEM_MULTI;

            itemInstances.Materials = MaterialType.MAT_GLAS;
            itemInstances.Description = itemInstances.Name;

            CreateItemInstance();
        }

        protected void CreateItemInstance()
        {
            if (created)
                return;

            GUC.WorldObjects.ItemInstance.addItemInstance(itemInstances);


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.CreateItemInstanceMessage);
            itemInstances.Write(stream);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            created = true;
        }


        public String Name { get { return itemInstances.Name; } protected set { itemInstances.Name = value; } }
        public int ID { get { return itemInstances.ID; } }


        public String InstanceName
        {
            get { return mItemInstance;  }
        }

        public int getDamage(DamageTypeIndex index)
        {
            if (index == DamageTypeIndex.DAM_INDEX_BARRIER)
                throw new Exception("Don't use DamageType Barrier!");
            return itemInstances.Damages[(int)index-1];
        }

        public int getProtection(DamageTypeIndex index)
        {
            if (index == DamageTypeIndex.DAM_INDEX_BARRIER)
                throw new Exception("Don't use Protectiontype Barrier!");
            return itemInstances.Protection[(int)index - 1];

            
        }

        public void setProtection(DamageTypeIndex index, int value)
        {
            if (index == DamageTypeIndex.DAM_INDEX_BARRIER)
                throw new Exception("Don't use Protectiontype Barrier!");
            itemInstances.Protection[(int)index - 1] = value;

        }

        public int Range { get { return itemInstances.Range; } protected set { itemInstances.Range = value; } }

        public DamageTypes DamageType { get { return itemInstances.DamageType; } protected set { itemInstances.DamageType = value; } }
        public int TotalDamage { get { return itemInstances.TotalDamage; } protected set { itemInstances.TotalDamage = value; } }

        public Flags Flags { get { return itemInstances.Flags; } protected set { itemInstances.Flags = value; } }
        public MainFlags MainFlags { get { return itemInstances.MainFlags; } protected set { itemInstances.MainFlags = value; } }

        public int Value { get { return itemInstances.Value; } protected set { itemInstances.Value = value; } }
        public ArmorFlags Wear { get { return itemInstances.Wear; } protected set { itemInstances.Wear = value; } }
        public MaterialType Materials { get { return itemInstances.Materials; } protected set { itemInstances.Materials = value; } }

        public String Visual { get { return itemInstances.Visual; } protected set { itemInstances.Visual = value; } }
        public String Visual_Change { get { return itemInstances.Visual_Change; } protected set { itemInstances.Visual_Change = value; } }
        public String Effect { get { return itemInstances.Effect; } protected set { itemInstances.Effect = value; } }

        public String ScemeName { get { return itemInstances.ScemeName; } protected set { itemInstances.ScemeName = value; } }


        public int Visual_skin { get { return itemInstances.Visual_skin; } protected set { itemInstances.Visual_skin = value; } }

        public String Description { get { return itemInstances.Description; } protected set { itemInstances.Description = value; } }


        public Spell Spell { get { return itemInstances.Spell.ScriptingProto; } protected set { itemInstances.Spell = value.spell; } }

        public String Text0 { get { return itemInstances.Text[0]; } protected set { itemInstances.Text[0] = value; } }
        public String Text1 { get { return itemInstances.Text[1]; } protected set { itemInstances.Text[1] = value; } }
        public String Text2 { get { return itemInstances.Text[2]; } protected set { itemInstances.Text[2] = value; } }
        public String Text3 { get { return itemInstances.Text[3]; } protected set { itemInstances.Text[3] = value; } }
        public String Text4 { get { return itemInstances.Text[4]; } protected set { itemInstances.Text[4] = value; } }
        public String Text5 { get { return itemInstances.Text[5]; } protected set { itemInstances.Text[5] = value; } }

        public int Count0 { get { return itemInstances.Count[0]; } protected set { itemInstances.Count[0] = value; } }
        public int Count1 { get { return itemInstances.Count[1]; } protected set { itemInstances.Count[1] = value; } }
        public int Count2 { get { return itemInstances.Count[2]; } protected set { itemInstances.Count[2] = value; } }
        public int Count3 { get { return itemInstances.Count[3]; } protected set { itemInstances.Count[3] = value; } }
        public int Count4 { get { return itemInstances.Count[4]; } protected set { itemInstances.Count[4] = value; } }
        public int Count5 { get { return itemInstances.Count[5]; } protected set { itemInstances.Count[5] = value; } }



        public bool IsLockPick { get { return itemInstances.isKeyInstance; } protected set { itemInstances.isKeyInstance = value; } }
        public bool IsTorch { get { return itemInstances.isTorch; } protected set { itemInstances.isTorch = value; } }
        public bool IsTorchBurning { get { return itemInstances.isTorchBurning; } protected set { itemInstances.isTorchBurning = value; } }
        public bool IsTorchBurned { get { return itemInstances.isTorchBurned; } protected set { itemInstances.isTorchBurned = value; } }
        public bool IsGold { get { return itemInstances.isGold; } protected set { itemInstances.isGold = value; } }

        #region Protection
        public int ProtectionFire
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_FIRE); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_FIRE, value); }
        }

        public int ProtectionEdge
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_EDGE); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_EDGE, value); }
        }

        public int ProtectionBarrier
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_BARRIER); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_BARRIER, value); }
        }

        public int ProtectionBlunt
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_BLUNT); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_BLUNT, value); }
        }

        public int ProtectionFall
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_FALL); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_FALL, value); }
        }

        public int ProtectionFly
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_FLY); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_FLY, value); }
        }

        public int ProtectionMagic
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_MAGIC); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_MAGIC, value); }
        }

        public int ProtectionPoint
        {
            get { return getProtection(DamageTypeIndex.DAM_INDEX_POINT); }
            protected set { setProtection(DamageTypeIndex.DAM_INDEX_POINT, value); }
        }
        #endregion


        public override string ToString()
        {
            return InstanceName;
        }

        #region Events

        #region OnEquip
        public event GUC.Server.Scripting.Events.NPCEquipEventHandler OnEquip;
        internal void iOnEquip(NPCProto proto, Item item)
        {
            if (OnEquip != null)
                OnEquip(proto, item);
        }

        public event GUC.Server.Scripting.Events.NPCEquipEventHandler OnUnEquip;
        internal void iOnUnEquip(NPCProto proto, Item item)
        {
            if (OnUnEquip != null)
                OnUnEquip(proto, item);
        }

        #endregion


        #region OnUse
        public event GUC.Server.Scripting.Events.UseItemEventHandler OnUse;
        internal void iOnUse(NPCProto proto, Item item, short state, short targetState)
        {
            if (OnUse != null)
                OnUse(proto, item, state, targetState);
        }

        #endregion

        #endregion

    }
}
