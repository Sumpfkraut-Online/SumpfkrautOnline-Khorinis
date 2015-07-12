using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using RakNet;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Client.Network.Messages
{
    static class ControlMessage
    {
        public static void ReadAddVob(BitStream stream)
        {
            Player.VobControlledList.Add(stream.mReadUInt());
        }

        public static void ReadRemoveVob(BitStream stream)
        {
            Player.VobControlledList.Remove(stream.mReadUInt());
        }
    }
}
