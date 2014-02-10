using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using ClassChooserServerModule;
using RakNet;
using Network;

namespace ClassChooserModule
{
    public class ClassMessage : Message
    {
        public override void Write(RakNet.BitStream stream, Client client)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xff);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int count = 0;
            
            GothicClasses classes = new GothicClasses();

            stream.Read(out classes.respawnTime);
            stream.Read(out count);

            classes.classes = new List<GothicClass>();
            for (int i = 0; i < count; i++)
            {
                GothicClass cls = new GothicClass();
                stream.Read(out cls.name);
                stream.Read(out cls.description);
                stream.Read(out cls.instance);
                stream.Read(out cls.hp);
                stream.Read(out cls.mp);
                stream.Read(out cls.str);
                stream.Read(out cls.dex);
                stream.Read(out cls.weapon);
                stream.Read(out cls.rangeweapon);
                stream.Read(out cls.armor);

                int itemcount = 0;
                stream.Read(out itemcount);
                for (int iItem = 0; iItem < itemcount; iItem++)
                {
                    item it = new item();
                    stream.Read(out it.code);
                    stream.Read(out it.Amount);
                    cls.items.Add(it);
                }

                int talentcount = 0;
                stream.Read(out talentcount);

                for (int iTalent = 0; iTalent < talentcount; iTalent++)
                {
                    talent tal = new talent();
                    stream.Read(out tal.id);
                    stream.Read(out tal.value);
                    cls.talents.Add(tal);
                }

                int iSpawnCount = 0;
                stream.Read(out iSpawnCount);
                for (int iSpawn = 0; iSpawn < iSpawnCount; iSpawn++)
                {
                    byte type = 0;

                    stream.Read(out type);
                    if (type == 0)
                    {
                        Vec3 pos = new Vec3();
                        stream.Read(out pos.x); stream.Read(out pos.y); stream.Read(out pos.z);
                        cls.Spawn.Add(pos);
                    }
                    else if (type == 1)
                    {
                        String wp = "";
                        stream.Read(out wp);
                        cls.Spawn.Add(wp);
                    }
                }

                stream.Read(out iSpawnCount);
                for (int iSpawn = 0; iSpawn < iSpawnCount; iSpawn++)
                {
                    byte type = 0;

                    stream.Read(out type);
                    if (type == 0)
                    {
                        Vec3 pos = new Vec3();
                        stream.Read(out pos.x); stream.Read(out pos.y); stream.Read(out pos.z);
                        cls.Respawn.Add(pos);
                    }
                    else if (type == 1)
                    {
                        String wp = "";
                        stream.Read(out wp);
                        cls.Respawn.Add(wp);
                    }
                }

                classes.classes.Add(cls);
            }

            ClassChooser.classes = classes;
        }
    }
}
