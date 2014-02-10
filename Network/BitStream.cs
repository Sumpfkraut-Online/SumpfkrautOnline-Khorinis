using System;
using System.Collections.Generic;
using System.Text;

namespace Network
{
    public class BitStreamFunc
    {
        public static void WriteNames(RakNet.BitStream stream, String name)
        {
            byte[] str = new System.Text.UTF8Encoding().GetBytes(name);
            stream.Write(str.Length);
            stream.WriteAlignedBytes(str, (uint)str.Length);
        }

        public static void ReadNames(RakNet.BitStream stream, out String name)
        {
            int length = 0;
            name = "";

            stream.Read(out length);
            byte[] str = new byte[length];
            stream.ReadAlignedBytes(str, (uint)length);
            name = new System.Text.UTF8Encoding().GetString(str);

        }
    }
}
