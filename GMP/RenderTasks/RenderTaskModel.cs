using System;
using System.Collections.Generic;
using System.Text;

namespace GMP.RenderTasks
{
    public class RenderTaskModel
    {
        public virtual bool combineAble()
        {
            return true;
        }

        public virtual void combine(RenderTaskModel task)
        {

        }

        public virtual void task()
        {

        }
    }
}
