using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;

namespace GUC.Server.Network.Messages.NpcCommands
{
    class UseItemMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int npcID = 0, itemID = 0;
            short itemState = 0;
            short itemTargetState = 0;

            stream.Read(out npcID);
            stream.Read(out itemID);
            stream.Read(out itemState);
            stream.Read(out itemTargetState);

            Vob npcVob = null;
            Vob itemVob = null;

            sWorld.VobDict.TryGetValue(npcID, out npcVob);
            sWorld.VobDict.TryGetValue(itemID, out itemVob);

            if (npcVob == null)
                throw new Exception("NPC was not found!");
            if (!(npcVob is NPCProto))
                throw new Exception("NPC was not a NPCProto " + npcVob);
            if (itemVob == null)
                throw new Exception("Item was not found!");
            if (!(itemVob is Item))
                throw new Exception("Item was not an item " + npcVob);
            NPCProto npc = (NPCProto)npcVob;
            Item item = (Item)itemVob;

            Scripting.Objects.Character.NPCProto.isOnUseItem(npc.ScriptingNPC, item.ScriptingProto, itemState, itemTargetState);

        }
    }
}
