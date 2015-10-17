using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using RakNet;
using GUC.Network;
using GUC.Types;
using Gothic.zClasses;
using GUC.Enumeration;

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

            

            NPC npc;
            if (ID == Player.ID)
            {
                
                npc = new NPC(ID, instID, oCNpc.Player(Program.Process));
                Player.Hero = npc;
            }
            else
            {
                npc = new NPC(ID, instID);
            }

            
            npc.Position = stream.mReadVec();
            npc.Direction = stream.mReadVec();

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
            uint ID = stream.mReadUInt();
            ushort instID = stream.mReadUShort();
            Item item = new Item(ID, instID);

            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();
            item.Amount = stream.mReadUShort();
            if (item.instance.type <= ItemType.Armor)
                item.Condition = stream.mReadUShort();
            bool drop = stream.ReadBit();
            item.Spawn(pos, dir, drop);
        }
    }
}
