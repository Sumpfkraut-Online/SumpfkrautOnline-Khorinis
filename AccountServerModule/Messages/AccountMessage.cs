using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using AccountServerModule.SqlLite;
using RakNet;

namespace AccountServerModule.Messages
{
    class AccountMessage : Message
    {
        public static List<Character> characterList = new List<Character>();

        public static void Disconnected(object sender, GMP_Server.Net.Message.DisconnectedMessage.DisconnectedEventArgs arg)
        {
            foreach (Character chara in characterList)
            {
                if (chara.mGUID == arg.guid)
                {
                    characterList.Remove(chara);
                    Console.WriteLine("Account-Server-Module: Remove character...");
                    break;
                }
            }
        }


        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server)
        {
            
            byte type = 0;
            String username; String password;
            stream.Read(out type);
            stream.Read(out username);
            stream.Read(out password);


            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xdf);
            stream.Write(type);

            String world = null;

            if (type == 0)
            {
                Character chara = Character.getCharacter(username, password);
                if (chara == null)
                {
                    Console.WriteLine("Falsche Daten: " + username);
                    stream.Write((byte)1);//Falsche Daten
                }
                else
                {
                    bool contains = false;
                    foreach (Character _chara in characterList)
                    {
                        if (_chara.mID == chara.mID)
                        {
                            contains = true;
                            break;
                        }
                    }

                    if (contains)
                    {
                        Console.WriteLine("Schon eingeloggt: " + chara.mID + " " + username + " " + chara.world);
                        stream.Write((byte)2);//Charakter schon eingelogged!
                    }
                    else
                    {
                        Console.WriteLine("Eingeloggt: " + chara.mID + " " + username + " " + chara.world);
                        chara.mGUID = packet.guid.g;
                        characterList.Add(chara);
                        stream.Write((byte)0);//Einloggen erfolgreich!
                        stream.Write(chara.mID);
                        
                        world = chara.world;
                    }
                }
            }
            else if (type == 1)//Registrieren...
            {
                bool available = Character.isCharacterAvailable(username);
                if (!available)
                {
                    stream.Write((byte)1);//Schon vorhanden...
                }
                else
                {
                    Console.WriteLine("Registriert: " + username);
                    Character chara = Character.Create(username, password);
                    chara.mGUID = packet.guid.g;
                    characterList.Add(chara);

                    stream.Write((byte)0);
                    stream.Write(chara.mID);
                }
            }
            server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, false);

            if (world != null && world.Trim() != "")
            {
                new StartLevelChangeMessage().Write(stream, server, world, packet.guid);
            }
        }
    }
}
