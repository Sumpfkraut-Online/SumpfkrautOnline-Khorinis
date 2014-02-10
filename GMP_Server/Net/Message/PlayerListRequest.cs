using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class PlayerListRequest : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            int hp = 0; int str = 0; int dex = 0;
            String instance = "";

            float[] pos = new float[3];


            Player player = null;

            stream.Read(out id);
            player = Player.getPlayer(id, Program.playList);

            if (player == null)
                return;
            
            stream.Read(out hp);
            stream.Read(out str);
            stream.Read(out dex);

            for (int i = 0; i < player.lastTalentSkills.Length; i++)
                stream.Read(out player.lastTalentSkills[i]);
            for (int i = 0; i < player.lastTalentValues.Length; i++)
                stream.Read(out player.lastTalentValues[i]);
            for (int i = 1; i < 5; i++)
                stream.Read(out player.lastHitChances[i - 1]);

            stream.Read(out instance);

            //Inventar
            int count = 0;
            stream.Read(out count);

            for (int i = 0; i < count; i++)
            {
                string code; int amount;
                stream.Read(out code);
                stream.Read(out amount);
                player.itemList.Add(new item() { code=code, Amount=amount });
            }



            stream.Read(out pos[0]); stream.Read(out pos[1]); stream.Read(out pos[2]); 
            

            
            player.pos = pos;
            player.lastHP = hp;
            player.lastHP_Max = hp;
            player.lastDex = dex;
            player.lastStr = dex;
            player.instance = instance;
            player.isSpawned = true;

            


            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PlayerListRequest);

            stream.Write(Program.gTime.GetDay());
            stream.Write(Program.gTime.getHourInDay());
            stream.Write(Program.gTime.GetMinuteInHour());

            stream.Write(Program.playList.Count);

            for (int i = 0; i < Program.playList.Count; i++)
            {
                Player pl = Program.playList[i];
                stream.Write(pl.id);
                stream.Write(pl.guidStr);
                stream.Write(pl.name);
                stream.Write(pl.actualMap);
                stream.Write(pl.instance);
                stream.Write(pl.lastHP);
                stream.Write(pl.lastHP_Max);
                
                stream.Write(pl.lastStr);
                stream.Write(pl.lastDex);

                stream.Write(pl.lastAniID);
                stream.Write(pl.lastAniValue);

                stream.Write(pl.lastWeaponModeType);
                stream.Write(pl.lastWeaponMode);

                for (int iTal = 0; iTal < pl.lastTalentSkills.Length; iTal++)
                    stream.Write(pl.lastTalentSkills[iTal]);
                for (int iTal = 0; iTal < pl.lastTalentValues.Length; iTal++)
                    stream.Write(pl.lastTalentValues[iTal]);
                for (int iHit = 1; iHit < 5; iHit++)
                    stream.Write(pl.lastHitChances[iHit - 1]);
                stream.Write(pl.itemList.Count);
                for (int iItems = 0; iItems < pl.itemList.Count; iItems++)
                {
                    stream.Write(pl.itemList[iItems].code);
                    stream.Write(pl.itemList[iItems].Amount);
                }
                stream.Write(pl.pos[0]);
                stream.Write(pl.pos[1]);
                stream.Write(pl.pos[2]);

                stream.Write(pl.isImmortal);
                stream.Write(pl.isInvisible);
                stream.Write(pl.isFreeze);
            }

            stream.Write(Program.npcList.Count);
            for (int i = 0; i < Program.npcList.Count; i++)
            {
                NPC npc = Program.npcList[i];
                stream.Write(npc.npcPlayer.id);
                stream.Write(npc.npcPlayer.actualMap);
                stream.Write(npc.npcPlayer.instance);
                stream.Write(npc.npcPlayer.lastHP);
                stream.Write(npc.npcPlayer.lastHP_Max);
                stream.Write(npc.npcPlayer.lastStr);
                stream.Write(npc.npcPlayer.lastDex);

                stream.Write(npc.npcPlayer.lastAniID);
                stream.Write(npc.npcPlayer.lastAniValue);

                stream.Write(npc.npcPlayer.lastWeaponModeType);
                stream.Write(npc.npcPlayer.lastWeaponMode);

                if (npc.isStatic || npc.isSummond)
                {
                    if (npc.isStatic)
                        stream.Write((byte)0);
                    else
                        stream.Write((byte)1);
                    //spawn
                    stream.Write(npc.spawn[0]);
                    stream.Write(npc.spawn[1]);
                    stream.Write(npc.spawn[2]);
                }
                else
                {
                    stream.Write((byte)2);
                    stream.Write(npc.wp);
                }
                //position
                stream.Write(npc.npcPlayer.pos[0]);
                stream.Write(npc.npcPlayer.pos[1]);
                stream.Write(npc.npcPlayer.pos[2]);
            }

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, false);

            new NewPlayerMessage(id).Write(stream, server);
        }
    }
}
