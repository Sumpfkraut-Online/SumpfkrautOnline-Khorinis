using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripting;
using GUC.WorldObjects.Instances;
using GUC.Enumeration;

namespace GUC.Client.Network.Messages
{
    static class InstanceMessage
    {
        public static void ReadCreateMessage(PacketReader stream)
        {
            BaseVobInstance inst = ScriptManager.Interface.CreateInstance((VobTypes)stream.ReadByte());
            inst.ReadStream(stream);
            inst.ScriptObject.Create();
        }

        public static void ReadDeleteMessage(PacketReader stream)
        {
            BaseVobInstance inst;
            if (BaseVobInstance.TryGet(stream.ReadUShort(), out inst))
            {
                inst.ScriptObject.Delete();
            }
        }
    }
}
