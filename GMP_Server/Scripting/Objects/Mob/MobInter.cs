using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;

namespace GUC.Server.Scripting.Objects.Mob
{
    public class MobInter : Vob
    {
        internal MobInter(GUC.WorldObjects.Mobs.MobInter vob)
            : base(vob)
        {
            
        }

        public MobInter(String visual, String focusName, ItemInstance useWithItem, String triggerTarget)
            : this(visual, focusName, useWithItem, triggerTarget, true, true)
        { }

        public MobInter(String visual)
            : this(visual, null, null, null, true, true)
        { }

        public MobInter(String visual, String focusName)
            : this(visual, focusName, null, null, true, true)
        { }

        public MobInter(String visual, ItemInstance useWithItem, String triggerTarget)
            : this(visual, null, useWithItem, triggerTarget, true, true)
        { }

        public MobInter(String visual, ItemInstance useWithItem)
            : this(visual, null, useWithItem, null, true, true)
        { }

        public MobInter(String visual, String focusName, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic)
            : this(new GUC.WorldObjects.Mobs.MobInter(), visual, focusName, useWithItem, triggerTarget, cdDyn, cdStatic, true)
        { }

        protected MobInter()
            : this(new GUC.WorldObjects.Mobs.MobInter(), null, null, null, null,  false, false, false)
        { }

        internal MobInter(GUC.WorldObjects.Mobs.MobInter mobInter, String visual, String focusName, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic, bool useCreate)
            : base(mobInter, visual, cdDyn, cdStatic, false)
        {
            Proto.FocusName = focusName;
            Proto.TriggerTarget = triggerTarget;
            if(useWithItem != null)
                Proto.UseWithItem = useWithItem.itemInstances;

            if (useCreate)
                CreateVob();
        }

        private GUC.WorldObjects.Mobs.MobInter Proto { get { return (GUC.WorldObjects.Mobs.MobInter)this.vob; } }



        public String FocusName { get { return Proto.FocusName; } set { setFocusName(value); } }
        public String TriggerTarget { get { return Proto.TriggerTarget; } set { setTriggerTarget(value); } }
        public ItemInstance UseWithItem { get { return (Proto.UseWithItem == null) ? null : Proto.UseWithItem.ScriptingProto; } set { setUseWithItem(value); } }


        public void setFocusName(String x)
        {
            Proto.FocusName = x;
            setPropertie(VobChangeID.FocusName, x);
        }

        public void setTriggerTarget(String x)
        {
            Proto.TriggerTarget = x;
            setPropertie(VobChangeID.TriggerTarget, x);
        }

        public void setUseWithItem(ItemInstance x)
        {
            Proto.UseWithItem = (x == null) ? null : x.itemInstances;
            setPropertie(VobChangeID.UseWithItem, x);
        }




        #region Events

        #region OnTrigger
        public event GUC.Server.Scripting.Events.MobInterEventHandler OnTrigger;
        internal void iOnTrigger(MobInter mobInter, NPCProto npc)
        {
            if (OnTrigger != null)
                OnTrigger(mobInter, npc);
        }

        public static event Events.MobInterEventHandler sOnTrigger;
        internal static void isOnTrigger(MobInter mobInter, NPCProto npc)
        {
            mobInter.iOnTrigger(mobInter, npc);
            if (sOnTrigger != null)
                sOnTrigger(mobInter, npc);
        }
        #endregion

        #region OnUnTrigger
        public event GUC.Server.Scripting.Events.MobInterEventHandler OnUnTrigger;
        internal void iOnUnTrigger(MobInter mobInter, NPCProto npc)
        {
            if (OnUnTrigger != null)
                OnUnTrigger(mobInter, npc);
        }

        public static event Events.MobInterEventHandler sOnUnTrigger;
        internal static void isOnUnTrigger(MobInter mobInter, NPCProto npc)
        {
            mobInter.iOnUnTrigger(mobInter, npc);
            if (sOnUnTrigger != null)
                sOnUnTrigger(mobInter, npc);
        }
        #endregion

        #region OnStartInteraction
        public event GUC.Server.Scripting.Events.MobInterEventHandler OnStartInteraction;
        internal void iOnStartInteraction(MobInter mobInter, NPCProto npc)
        {
            if (OnStartInteraction != null)
                OnStartInteraction(mobInter, npc);
        }

        public static event Events.MobInterEventHandler sOnStartInteraction;
        internal static void isOnStartInteraction(MobInter mobInter, NPCProto npc)
        {
            mobInter.iOnStartInteraction(mobInter, npc);
            if (sOnStartInteraction != null)
                sOnStartInteraction(mobInter, npc);
        }
        #endregion

        #region OnStopInteraction
        public event GUC.Server.Scripting.Events.MobInterEventHandler OnStopInteraction;
        internal void iOnStopInteraction(MobInter mobInter, NPCProto npc)
        {
            if (OnStopInteraction != null)
                OnStopInteraction(mobInter, npc);
        }

        public static event Events.MobInterEventHandler sOnStopInteraction;
        internal static void isOnStopInteraction(MobInter mobInter, NPCProto npc)
        {
            mobInter.iOnStopInteraction(mobInter, npc);
            if (sOnStopInteraction != null)
                sOnStopInteraction(mobInter, npc);
        }
        #endregion

        

        #endregion

    }
}
