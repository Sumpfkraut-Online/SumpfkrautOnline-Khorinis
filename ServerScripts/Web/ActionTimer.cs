#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting;

namespace GUC.Server.Scripts.Web
{
    public class ActionTimer : Timer
    {
        private static ActionTimer at = null;
        public static ActionTimer get()
        {
            
            if (at == null)
                at = new ActionTimer();
            return at;
            
        }
        List<Actions.Action> actionList = new List<Actions.Action>();
        protected ActionTimer()
            : base(0)
        {
            Start();
        }

        public void addAction(Actions.Action action)
        {
            lock (actionList)
            {
                actionList.Add(action);
            }
        }

        /// <summary>
        /// Only call this by Action.update()
        /// </summary>
        /// <param name="action"></param>
        public void removeAction(Actions.Action action)
        {
            actionList.Remove(action);
        }

        protected override void update(long now)
        {
            lock (actionList)
            {
                foreach (Actions.Action action in actionList.ToArray())
                {
                    action.update(this);
                }
            }
        }
    }
}


#endif