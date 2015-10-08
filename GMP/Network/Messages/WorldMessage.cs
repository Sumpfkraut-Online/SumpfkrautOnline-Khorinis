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
            NPC npc = new NPC(stream.mReadUInt());
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();

            ushort instID = stream.mReadUShort();
            NPCInstance inst;
            NPCInstance.InstanceList.TryGetValue(instID, out inst);
            if (inst != null)
            {
                inst.CreateNPC(npc.gNpc);
                if (instID <= 1)
                {
                    byte BodyTex = stream.mReadByte();
                    byte HeadMesh = stream.mReadByte();
                    byte HeadTex = stream.mReadByte();
                    npc.gNpc.Voice = stream.mReadByte();

                    using (zString z = zString.Create(Program.Process, ((Enumeration.HumHeadMesh)HeadMesh).ToString()))
                        npc.gNpc.SetAdditionalVisuals(inst.BodyMesh, BodyTex, 0, z, HeadTex, 0, -1);
                }

                float BodyHeight = (float)stream.mReadByte() / 100.0f;
                float BodyWidth = (float)stream.mReadByte() / 100.0f;
                float Fatness = (float)stream.mReadShort() / 100.0f;

                using (zVec3 z = zVec3.Create(Program.Process))
                {
                    z.X = BodyWidth;
                    z.Y = BodyHeight;
                    z.Z = BodyWidth;
                    npc.gNpc.SetModelScale(z);
                }
                npc.gNpc.SetFatness(Fatness);

                string customName = stream.mReadString();
                if (customName != "")
                {
                    npc.Name = customName;
                }

                npc.gNpc.HPMax = stream.mReadUShort();
                npc.gNpc.HP = stream.mReadUShort();

                bool isEquipped = stream.ReadBit();
                if (isEquipped)
                {
                    ItemInstance ii = null;
                    ItemInstance.InstanceList.TryGetValue(stream.mReadUShort(), out ii);
                    if (ii != null)
                    {
                        npc.gNpc.EquipWeapon(ii.CreateItem());
                        npc.EquippedMeleeWeapon = ii;   
                    }
                }

                isEquipped = stream.ReadBit();
                if (isEquipped)
                {
                    ItemInstance ii = null;
                    ItemInstance.InstanceList.TryGetValue(stream.mReadUShort(), out ii);
                    if (ii != null)
                    {
                        npc.gNpc.EquipWeapon(ii.CreateItem());
                        npc.EquippedRangedWeapon = ii; 
                    }
                }

                isEquipped = stream.ReadBit();
                if (isEquipped)
                {
                    ItemInstance ii = null;
                    ItemInstance.InstanceList.TryGetValue(stream.mReadUShort(), out ii);
                    if (ii != null)
                    {
                        npc.gNpc.EquipWeapon(ii.CreateItem());
                        npc.EquippedRangedWeapon = ii; 
                    }
                }

                npc.State = (Enumeration.NPCState)stream.mReadByte();
                npc.WeaponState = (Enumeration.NPCWeaponState)stream.mReadByte();
                switch (npc.WeaponState)
                {
                    case Enumeration.NPCWeaponState.Fists:
                    case Enumeration.NPCWeaponState.Melee:
                        npc.gNpc.SetWeaponMode(1);
                        break;
                    case Enumeration.NPCWeaponState.Ranged:
                    case Enumeration.NPCWeaponState.Magic: //FIXME
                        npc.gNpc.SetWeaponMode(2);
                        break;
                    default:
                        break;
                }

                npc.Spawn(pos, dir);
            }
            else
            {
                zERROR.GetZErr(Program.Process).Report(3, 'G', "NPC Spawn failed: NPCInstance unknown! ID: " + instID, 0, "WorldMessage.cs", 0);
            }
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
