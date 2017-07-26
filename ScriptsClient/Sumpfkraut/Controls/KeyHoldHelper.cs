using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using System.Collections;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    class KeyHoldHelper : IEnumerable
    {
        class ActionKeysPair
        {
            public Action Action;
            public List<VirtualKeys> Keys;
            public ActionKeysPair(Action action, List<VirtualKeys> keys)
            {
                this.Action = action;
                this.Keys = keys;
            }
        }
        
        public int HoldTime;
        public int Rate;

        List<ActionKeysPair> list = new List<ActionKeysPair>();
        
        ActionKeysPair current;
        long nextTime;

        public KeyHoldHelper(int holdTime = 600, int rate = 150)
        {
            this.HoldTime = holdTime;
            this.Rate = rate;
        }

        public void Add(Action action, params VirtualKeys[] keyCombination)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            List<VirtualKeys> keys = new List<VirtualKeys>(keyCombination.Where(key => Enum.IsDefined(typeof(VirtualKeys), key) && key != VirtualKeys.None));
            if (keys.Count == 0)
                throw new ArgumentNullException("No or only invalid keys!");

            list.Add(new ActionKeysPair(action, keys));
        }
        
        public void Update(long now)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ActionKeysPair pair = list[i];
                if (pair.Keys.TrueForAll(key => InputHandler.IsPressed(key)))
                {
                    if (pair != current)
                    {
                        current = pair;
                        nextTime = now + HoldTime * TimeSpan.TicksPerMillisecond;
                        pair.Action();
                    }
                    else if (now > nextTime)
                    {
                        nextTime = now + Rate * TimeSpan.TicksPerMillisecond;
                        pair.Action();
                    }
                    return;
                }                    
            }
            current = null;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
