using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class CallValue
    {
        protected Process mProcess;
        
        protected int mAddress;
        
        //Leerer Konstruktor muss vorhanden sein!
        public CallValue()
        {

        }

        public void Initialize(Process process, int address)
        {
            Process = process;
            Address = address;
        }

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

        #region Fields
        public WinApi.Process Process
        {
            get { return mProcess; }
            protected set { mProcess = value; }
        }

        public int Address
        {
            get { return mAddress; }
            protected set { mAddress = value; }
        }

        #endregion
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
