using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public abstract class Report
    {
        protected HIDDevice device;
        private byte[] m_arrBuffer;
        private int m_nLength;

        public Report(HIDDevice device)
        {
            this.device = device;
        }

        protected void SetBuffer(byte[] arrBytes)
        {
            m_arrBuffer = arrBytes;
            m_nLength = m_arrBuffer.Length;
        }

        public byte[] Buffer
        {
            get { return m_arrBuffer; }
            set { this.m_arrBuffer = value; }
        }

        public int BufferLength
        {
            get
            {
                return m_nLength;
            }
        }
    }
}
