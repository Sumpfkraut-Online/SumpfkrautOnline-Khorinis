using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.Objects
{
    public class ItemInstance
    {
        public static Dictionary<String, ItemInstance> ItemInstances = new Dictionary<string, ItemInstance>();


        internal GUC.WorldObjects.ItemInstance itemInstances;
        protected bool created;

        protected String mItemInstance = "";


        public static ItemInstance getItemInstance(String name)
        {
            
            ItemInstance ii = null;
            ItemInstances.TryGetValue(name.ToUpper().Trim(), out ii);
            if(ii == null)
                Console.WriteLine(name);
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
                throw new ArgumentException("InstanceName is already in use!");


            itemInstances = new GUC.WorldObjects.ItemInstance();
            itemInstances.ScriptingProto = this;

            mItemInstance = instanceName;
            ItemInstances.Add(instanceName, this);
        }

        public ItemInstance(String instanceName, String name, String scemeName, int value, String visual, String effect)///Potions
            : this(instanceName, name, scemeName, value, MainFlags.ITEM_KAT_POTIONS, Flags.ITEM_MULTI, visual, null, effect)
        { }
        public ItemInstance(String instanceName, String name, DamageType dmgType, MainFlags mainFlags, Flags flags, int totalDamage, int range, int value, String visual)///Weapons
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

        public ItemInstance(String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual)
            : this(instanceName, name, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, null)
        { }
        public ItemInstance(String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change)
            : this(instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, null, 0)
        {}

        public ItemInstance(String instanceName, String name, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : this(instanceName, name, null, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0)
        {}

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, "", 0)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, 0, null)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialTypes types)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, null)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, MaterialTypes types, ItemInstance munition)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, 0, types, munition)
        { }

        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialTypes types, ItemInstance munition)
            : this(instanceName, name, scemeName, protection, damages, value, mainFlags, flags, armorFlags, dmgType, totalDamage, range, visual, visual_Change, effect, visualSkin, types, munition, false)
        { }
        public ItemInstance(String instanceName, String name, String scemeName, int[] protection, int[] damages, int value, MainFlags mainFlags, Flags flags, ArmorFlags armorFlags, DamageType dmgType, int totalDamage, int range, String visual, String visual_Change, String effect, int visualSkin, MaterialTypes types, ItemInstance munition, bool keyInstance)
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

            itemInstances.Materials = MaterialTypes.MAT_GLAS;
            itemInstances.Description = itemInstances.Name;

            CreateItemInstance();
        }

        protected void CreateItemInstance()
        {
            if (created)
                return;

            GUC.WorldObjects.ItemInstance.addItemInstance(itemInstances);


            BitStream stream = Program.server.sendBitStream;
            stream.Reset();

            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.CreateItemInstanceMessage);
            itemInstances.Write(stream);
            Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            created = true;
        }


        public String Name { get { return itemInstances.Name; } protected set { itemInstances.Name = value; } }
        public int ID { get { return itemInstances.ID; } }

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

        public DamageType DamageType { get { return itemInstances.DamageType; } protected set { itemInstances.DamageType = value; } }
        public int TotalDamage { get { return itemInstances.TotalDamage; } protected set { itemInstances.TotalDamage = value; } }

        public Flags Flags { get { return itemInstances.Flags; } protected set { itemInstances.Flags = value; } }
        public MainFlags MainFlags { get { return itemInstances.MainFlags; } protected set { itemInstances.MainFlags = value; } }

        public int Value { get { return itemInstances.Value; } protected set { itemInstances.Value = value; } }
        public ArmorFlags Wear { get { return itemInstances.Wear; } protected set { itemInstances.Wear = value; } }
        public MaterialTypes Materials { get { return itemInstances.Materials; } protected set { itemInstances.Materials = value; } }

        public String Visual { get { return itemInstances.Visual; } protected set { itemInstances.Visual = value; } }
        public String Visual_Change { get { return itemInstances.Visual_Change; } protected set { itemInstances.Visual_Change = value; } }
        public String Effect { get { return itemInstances.Effect; } protected set { itemInstances.Effect = value; } }

        public int Visual_skin { get { return itemInstances.Visual_skin; } protected set { itemInstances.Visual_skin = value; } }

        public String Description { get { return itemInstances.Description; } protected set { itemInstances.Description = value; } }


        public Spell Spell { get { return itemInstances.Spell.ScriptingProto; } protected set { itemInstances.Spell = value.spell; } }

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


    }
}
