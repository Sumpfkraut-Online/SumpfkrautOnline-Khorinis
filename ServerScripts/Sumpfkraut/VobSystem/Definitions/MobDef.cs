using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{

    /**
     *   Class from which all mobs are instatiated (which are handled by the serverscript).
     */
    class MobDef : VobDef
    {

        #region dictionaries

        private static Dictionary<int, MobDef> defById = new Dictionary<int, MobDef>();
        private static Dictionary<string, MobDef> defByName = new Dictionary<string, MobDef>();

        #endregion



        #region standard attributes

        protected String _objName = "MobDef (default)";
        protected static string _staticName = "MobDef (static)";

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

    }
}
