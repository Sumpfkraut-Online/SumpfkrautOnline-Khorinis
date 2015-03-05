using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;
using GUC.Enumeration;
using RakNet;

namespace GUC.Network.Messages.Callbacks
{
    class CallbackNPCCanSee : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int callBackID = 0, npcID = 0, vobID = 0;

            stream.Read(out callBackID);
            stream.Read(out npcID);
            stream.Read(out vobID);

            if(!sWorld.VobDict.ContainsKey(npcID) || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob or NPC weren't in the List!");

            NPCProto proto = (NPCProto)sWorld.VobDict[npcID];
            Vob vob = (Vob)sWorld.VobDict[vobID];

            bool canSee = false;
            if (proto.Address != 0 && vob.Address == 0)
            {
                canSee = (new oCNpc(Process.ThisProcess(), proto.Address).CanSee(new zCVob(Process.ThisProcess(), vob.Address), 0) == 1) ? true : false;
            }

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.CallbackNPCCanSee);
            stream.Write(callBackID);
            stream.Write(proto.ID);
            stream.Write(vob.ID);
            stream.Write(canSee);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
