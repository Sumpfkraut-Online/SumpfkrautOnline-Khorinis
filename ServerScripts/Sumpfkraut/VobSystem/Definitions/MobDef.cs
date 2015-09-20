using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions
{

    public enum MobInterType
    {
        None            = 0,
        MobInter        = None + 1,
        MobBed          = MobInter + 1,
        MobSwitch       = MobBed + 1,
        MobDoor         = MobSwitch + 1,
        MobContainer    = MobDoor + 1,
    } 

    /**
     *   Class from which all mobs are instatiated (which are handled by the serverscript).
     */
    class MobDef : VobDef
    {

        Dictionary<int, MobDef> defById = new Dictionary<int, MobDef>();
        Dictionary<string, MobDef> defByName = new Dictionary<string, MobDef>();

        // general identi- and classifiers

        protected int MobDefID;
        public int GetID () { return this.MobDefID; }
        public void SetID (int MobDefID) { this.MobDefID = MobDefID; }

        protected MobInterType MobInterType;
        public MobInterType GetMobInterType () { return this.MobInterType; }
        public void SetMobInterType (MobInterType MobInterType) { this.MobInterType = MobInterType; }


        // necessary for both interactice and non-interactive vobs 

        protected string Visual;
        public string GetVisual () { return this.Visual; }
        public void SetVisual (string Visual) { this.Visual = Visual; }

        protected bool CDDyn;
        public bool GetCDDyn () { return this.CDDyn; }
        public void SetCDDyn (bool CDDyn) { this.CDDyn = CDDyn; }

        protected bool CDStatic;
        public bool GetCDStatic () { return this.CDStatic; }
        public void SetCDStatic (bool CDStatic) { this.CDStatic = CDStatic; }


        // necessary for interactice vobs (MobInter, MobSwitch)
        // -- all previous attributes are still required

        protected string FocusName;
        public string GetFocusName () { return this.FocusName; }
        public void SetFocusName (string FocusName) { this.FocusName = FocusName; }

        protected ItemInstance UseWithItem;
        public ItemInstance GetUseWithItem () { return this.UseWithItem; }
        public void SetUseWithItem (ItemInstance UseWithItem) { this.UseWithItem = UseWithItem; }

        protected string TriggerTarget;
        public string GetTriggerTarget () { return this.TriggerTarget; }
        public void SetTriggerTarget (string TriggerTarget) { this.TriggerTarget = TriggerTarget; }

        
        // necessary for lockable mobs (MobDoor)

        protected bool IsLocked;
        public bool GetIsLocked () { return this.IsLocked; }
        public void SetIsLocked (bool IsLocked) { this.IsLocked = IsLocked; }

        protected ItemInstance KeyInstance;
        public ItemInstance GetKeyInstance () { return this.KeyInstance; }
        public void SetKeyInstance (ItemInstance KeyInstance) { this.KeyInstance = KeyInstance; }

        protected string PicklockString;
        public string GetPicklockString () { return this.PicklockString; }
        public void SetPicklockString (string PicklockString) { this.PicklockString = PicklockString; }


        // necessary for container-mobs (MobContainer)
        // will be handled as multiple EffectChangesDef, no direct loading from the Mob_def-table of the database

        protected ItemInstance[] Items;
        public ItemInstance[] GetItems () { return this.Items; }
        public void SetItems (ItemInstance[] Items) { this.Items = Items; }

        protected int[] Amounts;
        public int[] GetAmounts () { return this.Amounts; }
        public void SetAmounts (int[] Amounts) { this.Amounts = Amounts; }



        public MobDef (MobInterType mobInterType, String visual, String focusName, ItemInstance[] items, int[] amounts, 
            bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, 
            String triggerTarget, bool cdDyn, bool cdStatic)
        {
            this.objName = "MobDef (default)";
            this.MobInterType = mobInterType;
            this.Visual = visual;
            this.FocusName = focusName;
            this.Items = items;
            this.Amounts = amounts;
            this.IsLocked = isLocked;
            this.KeyInstance = keyInstance;
            this.PicklockString = pickLockString;
            this.UseWithItem = useWithItem;
            this.TriggerTarget = triggerTarget;
            this.CDDyn = cdDyn;
            this.CDStatic = cdStatic;
        }

    }
}
