using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Web.Actions
{
    public abstract class Action
    {
        protected volatile bool isFinished = false;

        public bool IsFinished
        {
            get { return isFinished; }
        }
        public abstract void update(ActionTimer timer);
    }
}
