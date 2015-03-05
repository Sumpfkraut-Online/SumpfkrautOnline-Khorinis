using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;

namespace GUC.Server.Scripting.Objects.Mob
{
    public abstract class MobLockable : MobInter
    {
        internal MobLockable(GUC.WorldObjects.Mobs.MobLockable vob)
            : base(vob)
        {
            
        }

        internal MobLockable(GUC.WorldObjects.Mobs.MobInter mobInter, String visual, String focusName, bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic)
            : base(mobInter, visual, focusName, useWithItem, triggerTarget, cdDyn, cdStatic, false)
        {
            Proto.IsLocked = isLocked;
            if(keyInstance != null)
                Proto.KeyInstance = keyInstance.itemInstances;
            Proto.PickLockStr = pickLockString;
        }


        internal GUC.WorldObjects.Mobs.MobLockable Proto { get { return (GUC.WorldObjects.Mobs.MobLockable)this.vob; } }


        public bool IsLocked { get { return Proto.IsLocked; } set { setIsLocked(value); } }
        public String PicklockStr { get { return Proto.PickLockStr; } set { setPicklockStr(value); } }
        public ItemInstance KeyInstance { get { return (Proto.KeyInstance == null) ? null : Proto.KeyInstance.ScriptingProto; } set { setItemInstance(value); } }

        public void setIsLocked(bool x)
        {
            Proto.IsLocked = x;
            setPropertie(VobChangeID.IsLocked, x);
        }

        public void setPicklockStr(String x)
        {
            Proto.PickLockStr = x;
            setPropertie(VobChangeID.PickLockStr, x);
        }

        public void setItemInstance(ItemInstance x)
        {
            Proto.KeyInstance = (x == null) ? null : x.itemInstances;
            setPropertie(VobChangeID.KeyInstance, x);
        }





        #region Events
        public event GUC.Server.Scripting.Events.MobContainerPickEventHandler OnPickLocked;
        internal void OnContainerPickLocks(MobLockable mobInter, NPCProto npc, char c)
        {
            if (OnPickLocked != null)
                OnPickLocked(mobInter, npc, c);
        }

        #endregion
        #region Static Events:

        public static event Events.MobContainerPickEventHandler OnPickLock;
        internal static void OnContainerPickLock(MobLockable mobInter, NPCProto npc, char c)
        {
            mobInter.OnContainerPickLocks(mobInter, npc, c);
            if (OnPickLock != null)
                OnPickLock(mobInter, npc, c);
        }


        #endregion


    }
}
