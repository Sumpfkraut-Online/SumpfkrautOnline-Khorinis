using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using RakNet;
using GUC.Network;
using GUC.Types;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GUC.Client.Network.Messages
{
    static class WorldMessage
    {
        public static void ReadVobSpawn(BitStream stream)
        {
            Vob vob = new Vob(stream.mReadUInt());
            vob.visual = stream.mReadString();
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            vob.cdDyn = stream.ReadBit();
            vob.cdStatic = stream.ReadBit();

            vob.Spawn(pos, dir, stream.ReadBit());
        }

        public static void ReadVobDelete(BitStream stream)
        {
            uint id = stream.mReadUInt();
            Vob vob = World.GetVobByID(id);
            if (vob != null)
            {
                vob.Despawn();
            }
        }

        public static void ReadNPCSpawn(BitStream stream)
        {
            uint ID = stream.mReadUInt();
            ushort instID = stream.mReadUShort();

            NPC npc = new NPC(ID, instID);
            npc.position = stream.mReadVec();
            npc.direction = stream.mReadVec();

            if (instID <= 2)
            {
                byte BodyTex = stream.mReadByte();
                byte HeadMesh = stream.mReadByte();
                byte HeadTex = stream.mReadByte();
                npc.voice = stream.mReadByte();

                npc.SetBodyVisuals(npc.instance.bodyMesh, BodyTex, ((Enumeration.HumHeadMesh)HeadMesh).ToString(), HeadTex);
            }

            npc.bodyHeight = (float)stream.mReadByte() / 100.0f;
            npc.bodyWidth = (float)stream.mReadByte() / 100.0f;
            npc.fatness = (float)stream.mReadShort() / 100.0f;

            string customName = stream.mReadString();
            if (customName.Length > 0)
            {
                npc.name = customName;
            }

            npc.gNpc.HPMax = stream.mReadUShort();
            npc.gNpc.HP = stream.mReadUShort();

            npc.Spawn();
        }

        public static void ReadItemSpawn(BitStream stream)
        {
            Item item = new Item(stream.mReadUInt(), stream.mReadUShort());
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            bool drop = stream.ReadBit();
            item.Spawn(pos, dir, drop);
        }
    }
}
