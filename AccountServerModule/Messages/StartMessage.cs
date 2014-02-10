using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;
using GMP_Server;
using AccountServerModule.SqlLite;

namespace AccountServerModule.Messages
{
    class StartMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server)
        {
            int accountID; byte type;
            stream.Read(out accountID);
            stream.Read(out type);

            Character chara = null;
            foreach(Character _chara in AccountMessage.characterList)
            {
                if (_chara.mID == accountID && _chara.mGUID == packet.guid.g)
                {
                    chara = _chara;
                    break;
                }
            }

            Player player = null;
            foreach (Player _player in Program.playList)
            {
                if (_player.mGuid == packet.guid.g)
                {
                    player = _player;
                    break;
                }
            }

            if (chara == null || player == null)
            {
                //Todo: Eventuell kick?
                return;
            }
            Console.WriteLine(chara.saved + " | " + "Gesendet!");
            if (type == 1)
            {
                Console.WriteLine(chara.saved + " | " + "Save Started ! Gesendet!");
                chara.saveStarted = true;
                return;
            }

            chara.LoadCharacter();

            
            

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xde);
            if (chara.saved)
            {
                stream.Write(chara.saved);
                stream.Write(chara.hp);
                stream.Write(chara.hp_max);
                stream.Write(chara.mp);
                stream.Write(chara.mp_max);
                stream.Write(chara.str);
                stream.Write(chara.dex);
                stream.Write(chara.posX);
                stream.Write(chara.posY);
                stream.Write(chara.posZ);

                for (int i = 0; i < 22; i++)
                    stream.Write(chara.talents[i]);
                for (int i = 0; i < 4; i++)
                    stream.Write(chara.hitChances[i]);

                stream.Write(chara.itemList.Count);
                Console.WriteLine("Anzahl Items: " + chara.itemList.Count);
                foreach (item itm in chara.itemList)
                {
                    Console.WriteLine(itm.code + " | "+itm.Amount);
                    stream.Write(itm.code);
                    stream.Write(itm.Amount);
                }

                player.lastHP = chara.hp;
                player.lastHP_Max = chara.hp_max;
                player.lastStr = chara.str;
                player.lastDex = chara.dex;
                player.lastTalentSkills = chara.talents;
                player.lastHitChances = chara.hitChances;
                player.itemList = chara.itemList;
            }
            else
            {
                stream.Write(chara.saved);
            }

            server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0,packet.guid, false);
            
        }
    }
}
