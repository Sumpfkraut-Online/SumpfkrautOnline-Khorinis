using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Server.WorldObjects;
using GUC.Network;
using GUC.Types;

namespace GUC.Server.Network.Messages
{
    static class NPCMessage
    {
        #region Animation

        public static void ReadAniStart(BitStream stream, Client client)
        {
            Animations ani = (Animations)stream.mReadUShort();
            WriteAniStart(client.character.cell.SurroundingClients(client), client.character, ani);
        }

        public static void ReadAniStop(BitStream stream, Client client)
        {
            Animations ani = (Animations)stream.mReadUShort();
            bool fadeout = stream.ReadBit();
            WriteAniStop(client.character.cell.SurroundingClients(client), client.character, ani, fadeout);
        }

        public static void WriteAniStart(IEnumerable<Client> list, NPC npc, Animations ani)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCAniStartMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((ushort)ani);

            foreach(Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W', client.guid, false);
        }

        public static void WriteAniStop(IEnumerable<Client> list, NPC npc, Animations ani, bool fadeout)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCAniStartMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((ushort)ani);
            stream.mWrite(fadeout);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE, 'W', client.guid, false);
        }

        #endregion

        public static void WriteFoodMessage(IEnumerable<Client> list, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCFoodMessage);
            //stream.mWrite()
        }

        public static void WriteEquipMessage(IEnumerable<Client> list, NPC npc, ItemInstance inst)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCEquipMessage);
            stream.mWrite(npc.ID);
            stream.mWrite(inst.ID);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #region States

        public static void ReadState(BitStream stream, Client client)
        {
            uint id = stream.mReadUInt();

            AbstractVob vob;

            sWorld.AllVobs.TryGetValue(id, out vob);
            if (vob == null || !(vob is NPC))
                return;

            if (vob != client.character)
                return;

            NPCState newState = (NPCState)stream.mReadByte();
            Vec3f pos = stream.mReadVec();
            Vec3f dir = stream.mReadVec();

            ((NPC)vob).State = newState;
            ((NPC)vob).pos = pos;
            ((NPC)vob).dir = dir;
            WriteState(vob.cell.SurroundingClients(client), (NPC)vob);
        }
        
        public static void WriteState(IEnumerable<Client> list, NPC npc)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCStateMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((byte)npc.State);
            stream.mWrite(npc.pos);
            stream.mWrite(npc.dir);
            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W',client.guid,false);
        }

        public static void ReadWeaponState(BitStream stream, Client client)
        {
            client.character.WeaponState = (NPCWeaponState)stream.mReadByte();
            bool removeType1 = stream.ReadBit();
            client.character.pos = stream.mReadVec();
            client.character.dir = stream.mReadVec();
            WriteWeaponState(client.character.cell.SurroundingClients(client), client.character, removeType1);
        }

        public static void WriteWeaponState(IEnumerable<Client> list, NPC npc, bool removeType1)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCWeaponStateMessage);
            stream.mWrite(npc.ID);
            stream.mWrite((byte)npc.WeaponState);
            stream.mWrite(removeType1);
            stream.mWrite(npc.pos);
            stream.mWrite(npc.dir);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        #endregion

        #region Combat

        public static void ReadAttack(BitStream stream, Client client)
        {
            client.character.State = (NPCState)stream.mReadByte();
            client.character.pos = stream.mReadVec();
            client.character.dir = stream.mReadVec();

            NPC target = client.character.World.GetNpcOrPlayer(stream.mReadUInt());

            WriteAttack(client.character.cell.SurroundingClients(client), client.character, target);
        }

        public static void WriteAttack(IEnumerable<Client> list, NPC attacker, NPC target)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.NPCAttackMessage);
            stream.mWrite(attacker.ID);
            stream.mWrite((byte)attacker.State);
            stream.mWrite(attacker.pos);
            stream.mWrite(attacker.dir);
            if (target == null)
            {
                stream.mWrite(0);
            }
            else
            {
                stream.mWrite(target.ID);
            }

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'W', client.guid, false);
        }

        public static void ReadHitMessage(BitStream stream, Client client)
        {
            NPC attacker = client.character.World.GetNpcOrPlayer(stream.mReadUInt());
            byte num = stream.mReadByte();

            if (num == 0) //this client got hit
            {
                client.character.DoHit(attacker);
            }
            else //list of npcs this client has hit
            {
                uint id;
                NPC npc;
                for (int i = 0; i < num; i++)
                {
                    id = stream.mReadUInt(); 

                    npc = null; //only check bots, because players send hit events themselves
                    client.character.World.NPCDict.TryGetValue(id, out npc);
                    if (npc != null)
                    {
                        npc.DoHit(attacker);
                    }
                }
            }
        }

        #endregion
    }
}
