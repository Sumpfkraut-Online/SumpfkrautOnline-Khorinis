using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Modules;
using Network;
using GMP_Server;
using GMP_Server.Net.Message;
using RakNet;

namespace ClassChooserServerModule
{
    public class ClassChooser : LoadModule
    {
        public static GothicClasses classes;
        public void Load(Module module)
        {
            try
            {
                classes = GothicClasses.Load();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Program.server.messageListener.Add(0xff, new ClassMessage());
        }
    }

    public class ClassMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xff);

            stream.Write(ClassChooser.classes.respawnTime);
            stream.Write(ClassChooser.classes.classes.Count);

            

            foreach (GothicClass cls in ClassChooser.classes.classes)
            {
                stream.Write(cls.name);
                stream.Write(cls.description);
                stream.Write(cls.instance);
                stream.Write(cls.hp);
                stream.Write(cls.mp);
                stream.Write(cls.str);
                stream.Write(cls.dex);
                stream.Write(cls.weapon);
                stream.Write(cls.rangeweapon);
                stream.Write(cls.armor);

                if (cls.items == null)
                    stream.Write(0);
                else
                {
                    stream.Write(cls.items.Count);
                    for (int i = 0; i < cls.items.Count; i++)
                    {
                        stream.Write(cls.items[i].code);
                        stream.Write(cls.items[i].amount);
                    }
                }

                stream.Write(cls.talents.Count);
                for (int i = 0; i < cls.talents.Count; i++)
                {
                    stream.Write(cls.talents[i].id);
                    stream.Write(cls.talents[i].value);
                }


                stream.Write(cls.Spawn.Count);
                for (int i = 0; i < cls.Spawn.Count; i++)
                {
                    if (cls.Spawn[i].GetType() == typeof(Vec3))
                    {
                        stream.Write((byte)0);
                        stream.Write(((Vec3)cls.Spawn[i]).x);
                        stream.Write(((Vec3)cls.Spawn[i]).y);
                        stream.Write(((Vec3)cls.Spawn[i]).z);
                    }
                    else if (cls.Spawn[i].GetType() == typeof(String))
                    {
                        stream.Write((byte)1);
                        stream.Write((String)cls.Spawn[i]);
                    }
                    else
                    {
                        stream.Write((byte)0);
                        stream.Write(0); stream.Write(0); stream.Write(0);
                    }
                }

                stream.Write(cls.Respawn.Count);
                for (int i = 0; i < cls.Respawn.Count; i++)
                {
                    if (cls.Respawn[i].GetType() == typeof(Vec3))
                    {
                        stream.Write((byte)0);
                        stream.Write(((Vec3)cls.Respawn[i]).x);
                        stream.Write(((Vec3)cls.Respawn[i]).y);
                        stream.Write(((Vec3)cls.Respawn[i]).z);
                    }
                    else if (cls.Respawn[i].GetType() == typeof(String))
                    {
                        stream.Write((byte)1);
                        stream.Write((String)cls.Respawn[i]);
                    }
                    else
                    {
                        stream.Write((byte)0);
                        stream.Write(0); stream.Write(0); stream.Write(0);
                    }
                }

            }
            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, false);
        }
    }
}
