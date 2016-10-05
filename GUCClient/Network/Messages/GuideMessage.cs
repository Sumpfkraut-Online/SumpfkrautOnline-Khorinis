using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.WorldObjects;
using GUC.WorldObjects.VobGuiding;

namespace GUC.Network.Messages
{
    static class GuideMessage
    {
        public static void ReadGuideAddMessage(PacketReader stream)
        {
            GameClient.Client.guidedIDs.Add(stream.ReadUShort(), null);
        }

        public static void ReadGuideAddCmdMessage(PacketReader stream)
        {
            var cmd = ScriptManager.Interface.CreateGuideCmd(stream.ReadByte());
            cmd.ReadStream(stream);
            int id = stream.ReadUShort();
            GameClient.Client.guidedIDs.Add(id, cmd);

            GuidedVob vob;
            if (World.current.TryGetVob(id, out vob))
            {
                vob.SetGuideCommand(cmd);
            }
        }

        public static void ReadGuideRemoveMessage(PacketReader stream)
        {
            int id = stream.ReadUShort();
            GameClient.Client.guidedIDs.Remove(id);

            GuidedVob vob;
            if (World.current.TryGetVob(id, out vob))
            {
                vob.RemoveGuideCommand();
            }
        }

        public static void ReadGuideSetCmdMessage(PacketReader stream)
        {
            var cmd = ScriptManager.Interface.CreateGuideCmd(stream.ReadByte());
            cmd.ReadStream(stream);

            int id = stream.ReadUShort();
            if (GameClient.Client.guidedIDs.ContainsKey(id))
            {
                GameClient.Client.guidedIDs[id] = cmd;

                GuidedVob vob;
                if (World.current.TryGetVob(id, out vob))
                {
                    vob.SetGuideCommand(cmd);
                }
            }
        }

        public static void ReadGuideRemoveCmdMessage(PacketReader stream)
        {
            int id = stream.ReadUShort();
            if (GameClient.Client.guidedIDs.ContainsKey(id))
            {
                GameClient.Client.guidedIDs[id] = null;

                GuidedVob vob;
                if (World.current.TryGetVob(id, out vob))
                {
                    vob.RemoveGuideCommand();
                }
            }
        }
    }
}
