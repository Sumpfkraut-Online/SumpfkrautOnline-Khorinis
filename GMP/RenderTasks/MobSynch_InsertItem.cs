using System;
using System.Collections.Generic;
using System.Text;
using Injection;
using Gothic.zClasses;
using WinApi;

namespace GMP.RenderTasks
{
    public class MobSynch_InsertItem : RenderTaskModel
    {
        class container
        {
            public int addr = 0;
            public String val = "";
            public int amount = 0;
        }

        private List<container> containerList = new List<container>();

        public MobSynch_InsertItem(int Address, String value, int amount)
        {
            containerList.Add(new container() { addr = Address, val = value, amount = amount });
        }

        private void addToContainer(container con)
        {
            container aftCont = null;
            foreach (container cont in containerList)
            {
                if (cont.addr == con.addr && cont.val.Trim().ToLower() == con.val.Trim().ToLower())
                {
                    aftCont = cont;
                    break;
                }
            }
            if (aftCont == null)
                containerList.Add(con);
            else
                aftCont.amount += con.amount;
        }

        public override void combine(RenderTaskModel task)
        {
            MobSynch_InsertItem mITask = (MobSynch_InsertItem)task;
            foreach (container con in mITask.containerList)
            {
                this.addToContainer(con);
            }
        }

        public override void task()
        {
            foreach (container con in containerList)
            {
                new GMP.Net.Messages.MobSynch().Write(Program.client.sentBitStream, Program.client, 14, new oCMobInter(Process.ThisProcess(), con.addr), con.val, con.amount + "");
            }
        }
    }
}
