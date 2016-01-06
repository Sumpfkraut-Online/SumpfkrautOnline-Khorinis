using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    abstract class InputReport : Report
    {
        public InputReport(HIDDevice device)
            : base(device)
        {

        }

        public void SetData(byte[] arrData)
        {
            SetBuffer(arrData);
            ProcessData();
        }

        public abstract void ProcessData();
    }
}
