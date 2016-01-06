using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public abstract class CallValue
    {
        public abstract void Initialize(int registerAddress);
        public abstract List<byte[]> GetCallParams();
        public abstract uint ValueLength();
    }

    public class NullReturnCall : CallValue
    {
        public override void Initialize(int registerAddress)
        {
        }

        public override List<byte[]> GetCallParams()
        {
            return new List<byte[]>();
        }

        public override uint ValueLength()
        {
            return 0;
        }
    }
}
