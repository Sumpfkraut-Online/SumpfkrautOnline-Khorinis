using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.WorldObjects;

namespace GUC.Client.Network.Messages
{
    static class ScriptMessage
    {
        public static void ReadVobMsg(PacketReader stream)
        {
            BaseVob vob;
            if (World.current.TryGetVob(stream.ReadUShort(), out vob))
            {
                vob.ScriptObject.OnReadScriptVobMsg(stream);
            }
        }
    }
}
