using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using GMP.Net;
using Network;
using Injection;
using RakNet;
using ListModule.Messages;

namespace AdminAndListModule
{
    public class Loader : Message, LoadModule
    {
        public static Options sOptions = null;
        public static ClientOptions sClientOptions = null;
        public void Load(Module module)
        {
            sClientOptions = ClientOptions.Load(module);
            Program.client.messageListener.Add(0xFE, this);
            Program.client.messageListener.Add(0xFD, new AdminMessage());
            Program.client.messageListener.Add(0xFC, new SoundSynchMessage());

            Write(Program.client.sentBitStream, Program.client);
        }

        public override void Write(RakNet.BitStream stream, Client client)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xFE);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public static SpawnFunction[] spawnFunctions = null;
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            sOptions = new Options();
            stream.Read(out sOptions.PlayerListAvailable);
            stream.Read(out sOptions.AdminAvailable);
            stream.Read(out sOptions.animationListAvailable);
            stream.Read(out sOptions.speakListAvailable);
            stream.Read(out sOptions.animationListWhiteList);

            byte flag = 0;
            stream.Read(out flag);
            
            sOptions.BlockOptionsAnimation.blockInWater = ((flag & 1) == 1);
            sOptions.BlockOptionsAnimation.blockWhenDead = ((flag & 2) == 2);
            sOptions.BlockOptionsAnimation.blockWhenSleep = ((flag & 4) == 4);
            sOptions.BlockOptionsAnimation.blockWhenUnconscious = ((flag & 8) == 8);

            sOptions.BlockOptionsSpeak.blockInWater = ((flag & 16) == 16);
            sOptions.BlockOptionsSpeak.blockWhenDead = ((flag & 32) == 32);
            sOptions.BlockOptionsSpeak.blockWhenSleep = ((flag & 64) == 64);
            sOptions.BlockOptionsSpeak.blockWhenUnconscious = ((flag & 128) == 128);

            int count = 0;
            stream.Read(out count);
            for (int i = 0; i < count; i++)
            {
                String ani = "";
                stream.Read(out ani);
                sOptions.AvailableAnimations.Add(ani);
            }


            stream.Read(out count);
            spawnFunctions = new SpawnFunction[count];
            for (int i = 0; i < count; i++)
            {
                SpawnFunction sf = new SpawnFunction();
                spawnFunctions[i] = sf;
                stream.Read(out sf.name);

                int countsf = 0;
                stream.Read(out countsf);
                for (int i2 = 0; i2 < countsf; i2++)
                {
                    SpawnContent sc = new SpawnContent();
                    sf.Spawns.Add(sc);
                    stream.Read(out sc.instance);
                    stream.Read(out sc.world);

                    int countsc = 0;
                    stream.Read(out countsc);
                    for (int i3 = 0; i3 < countsc; i3++)
                    {
                        byte type = 0;
                        stream.Read(out type);//0 => Positionen 1 => WP
                        if (type == 0)
                        {
                            Vec3 pos = new Vec3();
                            stream.Read(out pos.x);
                            stream.Read(out pos.y);
                            stream.Read(out pos.z);
                            sc.Spawns.Add(pos);
                        }
                        else
                        {
                            String wp = "";
                            stream.Read(out wp);
                            sc.Spawns.Add(wp);
                        }
                    }
                }
            }
        }
    }
}
