using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class NewPlayerMessage : Message
    {
        int id;
        public NewPlayerMessage(int id)
        {
            this.id = id;
        }

        public override void Write(RakNet.BitStream stream, Server server)
        {
            stream.Reset();
            Player pl = Player.getPlayerSort(id, Program.playerList);

            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.NewPlayerMessage);
            stream.Write(pl.id);
            stream.Write(pl.guidStr);
            stream.Write(pl.name);
            stream.Write(pl.actualMap);
            stream.Write(pl.instance);
            stream.Write(pl.lastHP);
            stream.Write(pl.lastHP_Max);
            stream.Write(pl.lastStr);
            stream.Write(pl.lastDex);

            for (int iTal = 0; iTal < pl.lastTalentSkills.Length; iTal++)
                stream.Write(pl.lastTalentSkills[iTal]);
            for (int iHit = 1; iHit < 5; iHit++)
                stream.Write(pl.lastHitChances[iHit - 1]);

            stream.Write(pl.pos[0]);
            stream.Write(pl.pos[1]);
            stream.Write(pl.pos[2]);

            //Inventar
            stream.Write(pl.itemList.Count);
            foreach (item itm in pl.itemList)
            {
                stream.Write(itm.code); stream.Write(itm.Amount);
            }

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }
    }
}
