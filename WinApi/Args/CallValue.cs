using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public abstract class CallValue
    {
        public abstract byte[] GetData();

        public virtual uint ValueLength()
        {
            return 0;
        }

        public virtual List<byte[]> getCallParam()
        {
            List<byte[]> b = new List<byte[]>();
            b.Add(BitConverter.GetBytes(Address));
            return b;
        }

        public int Address
        {
            get { return mAddress; }
            protected set { mAddress = value; }
        }
        
    }

    public class NullReturnCall : CallValue
    {
        public NullReturnCall()
        {

        }

        public override List<byte[]> getCallParam()
        {
            return new List<byte[]>();
        }

        public override uint ValueLength()
        {
            return 0;
        }
    }
}
