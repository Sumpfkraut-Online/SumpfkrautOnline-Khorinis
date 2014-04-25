using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

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



        #region Events
        public event GUC.Server.Scripting.Events.MobInterEventHandler OnTriggered;
        internal void OnTrigger(MobInter mobInter, NPCProto npc)
        {
            if (OnTriggered != null)
                OnTriggered(mobInter, npc);
        }

        public event GUC.Server.Scripting.Events.MobInterEventHandler OnUnTriggered;
        internal void OnUnTrigger(MobInter mobInter, NPCProto npc)
        {
            if (OnUnTriggered != null)
                OnUnTriggered(mobInter, npc);
        }

        public event GUC.Server.Scripting.Events.MobInterEventHandler OnStartInteraction;
        internal void OnMobStartInteraction(MobInter mobInter, NPCProto npc)
        {
            if (OnStartInteraction != null)
                OnStartInteraction(mobInter, npc);
        }

        public event GUC.Server.Scripting.Events.MobInterEventHandler OnStopInteraction;
        internal void OnMobStopInteraction(MobInter mobInter, NPCProto npc)
        {
            if (OnStopInteraction != null)
                OnStopInteraction(mobInter, npc);
        }

        #endregion
        #region Static Events:

        public static event Events.MobInterEventHandler OnTriggers;
        internal static void OnMobInterTriggers(MobInter mobInter, NPCProto npc)
        {
            mobInter.OnTrigger(mobInter, npc);
            if (OnTriggers != null)
                OnTriggers(mobInter, npc);
        }

        public static event Events.MobInterEventHandler OnUnTriggers;
        internal static void OnMobInterUnTriggers(MobInter mobInter, NPCProto npc)
        {
            mobInter.OnUnTrigger(mobInter, npc);
            if (OnTriggers != null)
                OnUnTriggers(mobInter, npc);
        }


        public static event Events.MobInterEventHandler OnStartInteractions;
        internal static void OnMobStartInteractions(MobInter mobInter, NPCProto npc)
        {
            mobInter.OnMobStartInteraction(mobInter, npc);
            if (OnStartInteractions != null)
                OnStartInteractions(mobInter, npc);
        }

        public static event Events.MobInterEventHandler OnStopInteractions;
        internal static void OnMobStopInteractions(MobInter mobInter, NPCProto npc)
        {
            mobInter.OnMobStopInteraction(mobInter, npc);
            if (OnStopInteractions != null)
                OnStopInteractions(mobInter, npc);
        }


        #endregion

    }
}
