using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Server.Scripts.Sumpfkraut.Database;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{

    /**
     *   Class from which all mobs are instatiated (which are handled by the serverscript).
     */
    class MobDef : VobDef
    {

        #region dictionaries

        protected static Dictionary<int, MobDef> defById = new Dictionary<int, MobDef>();
        protected static Dictionary<string, MobDef> defByCodeName = new Dictionary<string, MobDef>();

        public static readonly String dbIdColName = "MobDefId";
        new public static readonly Dictionary<String, SQLiteGetTypeEnum> defTab_GetTypeByColumn =
            new Dictionary<String, SQLiteGetTypeEnum>
            {
                {dbIdColName,                SQLiteGetTypeEnum.GetInt32},
                {"ChangeDate",              SQLiteGetTypeEnum.GetString},
                {"CreationDate",            SQLiteGetTypeEnum.GetString},
            };

        #endregion



        #region standard attributes

        new public static readonly String _staticName = "MobDef (static)";
        new protected String _objName = "MobDef (default)";

        new protected static Type _type = typeof(MobDef);
        new public static readonly String dbTable = "MobDef";

        // general identi- and classifiers

        protected MobInterType mobInterType;
        public MobInterType GetMobInterType () { return this.mobInterType; }
        public void SetMobInterType (MobInterType mobInterType) { this.mobInterType = mobInterType; }


        // necessary for both interactice and non-interactive vobs 

        protected string visual;
        public string GetVisual () { return this.visual; }
        public void SetVisual (string visual) { this.visual = visual; }

        protected bool cdDyn;
        public bool GetCDDyn () { return this.cdDyn; }
        public void SetCDDyn (bool cdDyn) { this.cdDyn = cdDyn; }

        protected bool cdStatic;
        public bool GetCDStatic () { return this.cdStatic; }
        public void SetCDStatic (bool cdStatic) { this.cdStatic = cdStatic; }


        // necessary for interactice vobs (MobInter, MobSwitch)
        // -- all previous attributes are still required

        protected string focusName;
        public string GetFocusName () { return this.focusName; }
        public void SetFocusName (string focusName) { this.focusName = focusName; }

        protected ItemInstance useWithItem;
        public ItemInstance GetUseWithItem () { return this.useWithItem; }
        public void SetUseWithItem (ItemInstance useWithItem) { this.useWithItem = useWithItem; }

        protected string triggerTarget;
        public string GetTriggerTarget () { return this.triggerTarget; }
        public void SetTriggerTarget (string triggerTarget) { this.triggerTarget = triggerTarget; }

        
        // necessary for lockable mobs (MobDoor)

        protected bool isLocked;
        public bool GetIsLocked () { return this.isLocked; }
        public void SetIsLocked (bool isLocked) { this.isLocked = isLocked; }

        protected ItemInstance keyInstance;
        public ItemInstance GetKeyInstance () { return this.keyInstance; }
        public void SetKeyInstance (ItemInstance keyInstance) { this.keyInstance = keyInstance; }

        protected string picklockString;
        public string GetPicklockString () { return this.picklockString; }
        public void SetPicklockString (string picklockString) { this.picklockString = picklockString; }


        // necessary for container-mobs (MobContainer)
        // will be handled as multiple EffectChangesDef, no direct loading from the Mob_def-table of the database

        protected ItemInstance[] items;
        public ItemInstance[] GetItems () { return this.items; }
        public void SetItems (ItemInstance[] items) { this.items = items; }

        protected int[] amounts;
        public int[] GetAmounts () { return this.amounts; }
        public void SetAmounts (int[] amounts) { this.amounts = amounts; }

        #endregion



        #region constructors

        //public MobDef (MobInterType mobInterType, String visual, String focusName, ItemInstance[] items, int[] amounts, 
        //    bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, 
        //    String triggerTarget, bool cdDyn, bool cdStatic)
        //{
        //    this._objName = "MobDef (default)";
        //    this.MobInterType = mobInterType;
        //    this.Visual = visual;
        //    this.FocusName = focusName;
        //    this.Items = items;
        //    this.Amounts = amounts;
        //    this.IsLocked = isLocked;
        //    this.KeyInstance = keyInstance;
        //    this.PicklockString = pickLockString;
        //    this.UseWithItem = useWithItem;
        //    this.TriggerTarget = triggerTarget;
        //    this.CDDyn = cdDyn;
        //    this.CDStatic = cdStatic;
        //}

        #endregion



        #region dictionary-methods

        public static bool Add(MobDef def)
        {
            return Add(_type, def);
        }

        public static bool ContainsCodeName(String codeName)
        {
            return ContainsCodeName(_type, codeName);
        }

        public static bool ContainsId(int id)
        {
            return ContainsId(_type, id);
        }

        public static bool ContainsDefinition(VobDef def)
        {
            return ContainsDefinition(_type, def);
        }

        public static bool RemoveCodeName(String codeName)
        {
            return RemoveCodeName(_type, codeName);
        }

        public static bool RemoveId(int id)
        {
            return RemoveId(_type, id);
        }

        public static bool TryGetValueByCodeName(String codeName, out MobDef def)
        {
            VobDef tempDef;
            bool result = TryGetValueByCodeName(_type, codeName, out tempDef);
            def = (MobDef)tempDef;
            return result;
        }

        public static bool TryGetValueById(int id, out MobDef def)
        {
            VobDef tempDef;
            bool result = TryGetValueById(_type, id, out tempDef);
            def = (MobDef)tempDef;
            return result;
        }

        #endregion

    }
}
