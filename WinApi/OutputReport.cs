using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public abstract class OutputReport : Report
    {
        public OutputReport(HIDDevice device)
            : base(device)
        {
            SetBuffer(new byte[device.HidCap.OutputReportByteLength]);
        }
    }
}
