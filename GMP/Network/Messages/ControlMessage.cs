using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.Client.Hooks;
using Gothic.zClasses;
using Gothic.zStruct;

namespace GUC.Client.Network.Messages
{
    static class ControlMessage
    {
        public static void ReadAddVob(BitStream stream)
        {
            AbstractVob vob = World.GetVobByID(stream.mReadUInt());
            if (vob != null)
            {
                Player.VobControlledList.Add(vob);
            }
        }

        public static void ReadRemoveVob(BitStream stream)
        {
            AbstractVob vob = World.GetVobByID(stream.mReadUInt());
            if (vob != null)
            {
                Player.VobControlledList.Remove(vob);
                if (vob is NPC)
                    ((NPC)vob).cmd = ControlCmd.Stop;
            }
        }

        public static void ReadVobControlCmd(BitStream stream)
        {
            uint ID = stream.mReadUInt();
            AbstractVob vob = Player.VobControlledList.Find(v => v.ID == ID);
            if (vob == null || !(vob is NPC))
                return;
            
            NPC npc = (NPC)vob;

            npc.cmd = (ControlCmd)stream.mReadByte();

            switch (npc.cmd)
            {
                case ControlCmd.GoToPos:
                    break;

                case ControlCmd.GoToVob:
                    npc.cmdTargetVob = stream.mReadUInt();
                    npc.cmdTargetRange = stream.mReadFloat();
                    break;

                case ControlCmd.Stop:
                    npc.cmd = ControlCmd.Stop;
                    break;
            }
        }
    }
}
