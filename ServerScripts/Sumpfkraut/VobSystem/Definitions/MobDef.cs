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
    class MobDef
    {

        // general identi- and classifiers

        protected int ID;
        public int getID () { return this.ID; }
        public void setID (int ID) { this.ID = ID; }

        protected MobInterType MobInterType;
        public MobInterType getMobInterType () { return this.MobInterType; }
        public void setMobInterType (MobInterType MobInterType) { this.MobInterType = MobInterType; }


        // necessary for both interactice and non-interactive vobs 

        protected string Visual;
        public string getVisual () { return this.Visual; }
        public void setVisual (string Visual) { this.Visual = Visual; }

        protected bool CDDyn;
        public bool getCDDyn () { return this.CDDyn; }
        public void setCDDyn (bool CDDyn) { this.CDDyn = CDDyn; }

        protected bool CDStatic;
        public bool getCDStatic () { return this.CDStatic; }
        public void setCDStatic (bool CDStatic) { this.CDStatic = CDStatic; }


        // necessary for interactice vobs (MobInter, MobSwitch)
        // -- all previous attributes are still required

        protected string FocusName;
        public string getFocusName () { return this.FocusName; }
        public void setFocusName (string FocusName) { this.FocusName = FocusName; }

        protected ItemInstance UseWithItem;
        public ItemInstance getUseWithItem () { return this.UseWithItem; }
        public void setUseWithItem (ItemInstance UseWithItem) { this.UseWithItem = UseWithItem; }

        protected string TriggerTarget;
        public string getTriggerTarget () { return this.TriggerTarget; }
        public void setTriggerTarget (string TriggerTarget) { this.TriggerTarget = TriggerTarget; }

        
        // necessary for lockable mobs (MobDoor)

        protected bool IsLocked;
        public bool getIsLocked () { return this.IsLocked; }
        public void setIsLocked (bool IsLocked) { this.IsLocked = IsLocked; }

        protected ItemInstance KeyInstance;
        public ItemInstance getKeyInstance () { return this.KeyInstance; }
        public void setKeyInstance (ItemInstance KeyInstance) { this.KeyInstance = KeyInstance; }

        protected string PicklockString;
        public string getPicklockString () { return this.PicklockString; }
        public void setPicklockString (string PicklockString) { this.PicklockString = PicklockString; }


        // necessary for container-mobs (MobContainer)
        // will be handled as multiple EffectChangesDef, no direct loading from the Mob_def-table of the database

        protected ItemInstance[] Items;
        public ItemInstance[] getItems () { return this.Items; }
        public void setItems (ItemInstance[] Items) { this.Items = Items; }

        protected int[] Amounts;
        public int[] getAmounts () { return this.Amounts; }
        public void setAmounts (int[] Amounts) { this.Amounts = Amounts; }



        public MobDef (MobInterType mobInterType, String visual, String focusName, ItemInstance[] items, int[] amounts, 
            bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, 
            String triggerTarget, bool cdDyn, bool cdStatic)
        {
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
