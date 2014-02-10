using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Modules;
using GMP_Server.Net.Message;
using Network;
using GMP_Server;

namespace ListModuleServer
{
    public class ListModuleServer : Message, LoadModule
    {
        public static Options sOptions;

        /// <summary>
        /// sOptions laden und MessageListener einfügen
        /// </summary>
        /// <param name="module"></param>
        public void Load(Module module)
        {
            sOptions = Options.Load();
            Program.server.messageListener.Add(0xFE, this);
            Program.server.messageListener.Add(0xFD, new AdminMessage());
            Program.server.messageListener.Add(0xFC, new SoundSynchMessage());
        }

        /// <summary>
        /// Wird aufgerufen wenn eine Anfrage vom Client eintritt.
        /// Der typ ist 0xFE
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="packet"></param>
        /// <param name="server"></param>
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xFE);

            //Optionen schreiben
            stream.Write(sOptions.PlayerListAvailable);
            stream.Write(sOptions.AdminAvailable);
            stream.Write(sOptions.animationListAvailable);
            stream.Write(sOptions.speakListAvailable);
            stream.Write(sOptions.animationListWhiteList);

            byte blockOptions = 0;
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsAnimation.blockInWater)) << 0);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsAnimation.blockWhenDead)) << 1);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsAnimation.blockWhenSleep)) << 2);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsAnimation.blockWhenUnconscious)) << 3);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsSpeak.blockInWater)) << 4);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsSpeak.blockWhenDead)) << 5);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsSpeak.blockWhenSleep)) << 6);
            blockOptions |= (byte)((Convert.ToByte(sOptions.BlockOptionsSpeak.blockWhenUnconscious)) << 7);
            stream.Write(blockOptions);
            //    sOptions.BlockOptionsAnimation.blockInWater = ((flag & 1) == 1);
            //sOptions.BlockOptionsAnimation.blockWhenDead = ((flag & 2) == 2);
            //sOptions.BlockOptionsAnimation.blockWhenSleep = ((flag & 4) == 4);
            //sOptions.BlockOptionsAnimation.blockWhenUnconscious = ((flag & 8) == 8);

            //sOptions.BlockOptionsSpeak.blockInWater = ((flag & 16) == 16);
            //sOptions.BlockOptionsSpeak.blockWhenDead = ((flag & 32) == 32);
            //sOptions.BlockOptionsSpeak.blockWhenSleep = ((flag & 64) == 64);
            //sOptions.BlockOptionsSpeak.blockWhenUnconscious = ((flag & 128) == 128);

            stream.Write(sOptions.AvailableAnimations.Count);
            for(int i = 0; i< sOptions.AvailableAnimations.Count; i++)
                stream.Write(sOptions.AvailableAnimations[i]);

            stream.Write(Program.spawnConfig.SpawnFunctions.Count);
            foreach (SpawnFunction sf in Program.spawnConfig.SpawnFunctions)
            {
                stream.Write(sf.name);

                stream.Write(sf.Spawns.Count);
                foreach (SpawnContent sc in sf.Spawns)
                {
                    stream.Write(sc.instance);
                    stream.Write(sc.world);

                    stream.Write(sc.Spawns.Count);
                    foreach (object obj in sc.Spawns)
                    {

                        if (obj.GetType() == typeof(Vec3))
                        {
                            Console.WriteLine("0");
                            Vec3 pos = (Vec3)obj;
                            stream.Write((byte)0);
                            stream.Write(pos.x);
                            stream.Write(pos.y);
                            stream.Write(pos.z);
                        }
                        else
                        {

                            Console.WriteLine("1");
                            stream.Write((byte)1);
                            stream.Write((String)obj);
                        }
                    }
                }
            }
            server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, false);
        }
        
    }
}
