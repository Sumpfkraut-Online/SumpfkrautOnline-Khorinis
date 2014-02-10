using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class AllPlayerSynchMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            int size = 0;

            stream.Read(out id);
            stream.Read(out size);

            if (size == 0)
                return;

            int[] idnpc = new int[size];
            byte[] type = new byte[size];
            int[] hp = new int[size];
            for (int i = 0; i < size; i++)
            {
                stream.Read(out idnpc[i]);
                stream.Read(out type[i]);
                stream.Read(out hp[i]);

                Player pl = Player.getPlayerSort(idnpc[i], Program.playerList);
                if(pl == null)
                    continue;



                if (type[i] == (byte)AllPlayerSynchMessageTypes.HP)
                {
                    pl.lastHP = hp[i];
                }
                else if (type[i] == (byte)AllPlayerSynchMessageTypes.HP_Max)
                {
                    pl.lastHP_Max = hp[i];
                }
                else if (type[i] == (byte)AllPlayerSynchMessageTypes.MP)
                {
                    pl.lastMP = hp[i];
                }
                else if (type[i] == (byte)AllPlayerSynchMessageTypes.MP_Max)
                {
                    pl.lastMP_Max = hp[i];
                }
                else if (type[i] == (byte)AllPlayerSynchMessageTypes.Dex)
                {
                    pl.lastDex = hp[i];
                }
                else if (type[i] == (byte)AllPlayerSynchMessageTypes.Str)
                {
                    pl.lastStr = hp[i];
                }
                else if (type[i] >= (byte)AllPlayerSynchMessageTypes.last)
                {
                    if (type[i] >= (byte)AllPlayerSynchMessageTypes.last + 4)
                    {
                        int type2 = type[i] - ((int)AllPlayerSynchMessageTypes.last + 4);
                        pl.lastTalentSkills[type2] = hp[i];
                    }
                    else
                    {
                        int type2 = type[i] - (byte)AllPlayerSynchMessageTypes.last;
                        pl.lastHitChances[type2] = hp[i];
                    }
                }
            }

            
                

            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AllPlayerSynchMessage);
            stream.Write(id);
            stream.Write(size);

            for (int i = 0; i < size; i++)
            {
                stream.Write(idnpc[i]);
                stream.Write(type[i]);
                stream.Write(hp[i]);
            }
            server.server.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);

        }


        public void Write(BitStream stream, Server server, int id, int npcid, byte type, int hp)
        {
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AllPlayerSynchMessage);
            stream.Write(id);
            stream.Write(DateTime.Now.Ticks);
            stream.Write(1);

            stream.Write(npcid);
            stream.Write(type);
            stream.Write(hp);
            
            server.server.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }
    }
}
