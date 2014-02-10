using System;
using System.Collections.Generic;
using System.Text;

namespace GMP.RenderTasks
{
    public class RenderTask
    {
        protected static List<RenderTaskModel> mTasks = new List<RenderTaskModel>();

        public static void add(RenderTaskModel task)
        {
            if (task.combineAble())
            {
                foreach (RenderTaskModel rtm in mTasks)
                {
                    if (rtm.GetType() == task.GetType())
                    {
                        rtm.combine(task);
                        return;
                    }
                }
                mTasks.Add(task);
            }
            else
            {
                mTasks.Add(task);
            }

        }

        public static void run()
        {
            foreach (RenderTaskModel rtm in mTasks)
            {
                rtm.task();
            }

            mTasks.Clear();
        }
    }
}
