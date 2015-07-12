using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Server.Network.Messages.Callbacks
{
    class CallbackNPCCanSee : IMessage
    {
        public void Read(RakNet.BitStream stream, Client client)
        {
            int callBackID = 0, npcID = 0, vobID = 0;
            bool canSee = false;
            stream.Read(out callBackID);
            stream.Read(out npcID);
            stream.Read(out vobID);
            stream.Read(out canSee);

            if (!sWorld.VobDict.ContainsKey(npcID) || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob or NPC weren't in the List!");

            NPC proto = (NPC)sWorld.VobDict[npcID];
            Vob vob = (Vob)sWorld.VobDict[vobID];

            Scripting.Objects.Character.NPC.iOnCanSeeCallback(callBackID, proto.ScriptingNPC, vob.ScriptingVob, canSee);
        }
    }
}
