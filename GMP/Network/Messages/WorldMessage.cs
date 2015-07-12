using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using RakNet;
using GUC.Network;
using GUC.Types;

namespace GUC.Client.Network.Messages
{
    static class WorldMessage
    {
        public static void ReadVobSpawn(BitStream stream)
        {
            Vob vob = new Vob(stream.mReadUInt());
            vob.Visual = stream.mReadString();
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            vob.CDDyn = stream.ReadBit();
            vob.CDStatic = stream.ReadBit();

            vob.Spawn(pos, dir, stream.ReadBit());
        }

        public static void ReadVobDelete(BitStream stream)
        {
            Vob vob;
            World.VobDict.TryGetValue(stream.mReadUInt(), out vob);
            if (vob != null)
            {
                vob.Despawn();
            }
        }

        public static void ReadNPCSpawn(BitStream stream)
        {
            NPC vob = new NPC(stream.mReadUInt());
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            vob.Name = stream.mReadString();
            vob.gNpc.SetVisual("HUMANS.MDS");
            vob.gNpc.SetAdditionalVisuals("HUM_BODY_NAKED0", 1, 0, "HUM_HEAD_THIEF", 2, 0, -1);
            vob.Spawn(pos, dir);
        }

        public static void ReadItemSpawn(BitStream stream)
        {
            Item item = new Item(stream.mReadUInt(), stream.mReadUInt());
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            bool drop = stream.ReadBit();
            item.Spawn(pos, dir, drop);
        }
    }
}
