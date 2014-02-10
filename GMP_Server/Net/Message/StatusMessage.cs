using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class StatusMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            byte blockOptions = 0;
            blockOptions |= (byte)((Convert.ToByte(Program.config.chatOptions.BlockOptions.blockInWater)) << 0);
            blockOptions |= (byte)((Convert.ToByte(Program.config.chatOptions.BlockOptions.blockWhenDead)) << 1);
            blockOptions |= (byte)((Convert.ToByte(Program.config.chatOptions.BlockOptions.blockWhenSleep)) << 2);
            blockOptions |= (byte)((Convert.ToByte(Program.config.chatOptions.BlockOptions.blockWhenUnconscious)) << 3);

            short otherOptions = 0;
            otherOptions |= (short)((Convert.ToByte(Program.config.HideNames)) << 0);
            otherOptions |= (short)((Convert.ToByte(Program.config.OnlyHumanNames)) << 1);
            otherOptions |= (short)((Convert.ToByte(Program.config.ShowConnectionMessages)) << 2);
            otherOptions |= (short)((Convert.ToByte(Program.config.AvailableFriends)) << 3);
            otherOptions |= (short)((Convert.ToByte(Program.config.ImmortalFriends)) << 4);
            otherOptions |= (short)((Convert.ToByte(Program.config.UnLockAll)) << 5);
            otherOptions |= (short)((Convert.ToByte(Program.config.LoadItemFromFile)) << 6);
            otherOptions |= (short)((Convert.ToByte(Program.config.enableStamina)) << 7);
            otherOptions |= (short)((Convert.ToByte(Program.config.removeAllContainers)) << 8);
            otherOptions |= (short)((Convert.ToByte(Program.config.DamageOnServer)) << 9);
            otherOptions |= (short)((Convert.ToByte(Program.config.NPCAIOnServer)) << 10);
            


            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.Status);


            stream.Write(Program.config.ServerName);
            stream.Write(Program.config.World);
            stream.Write(Program.config.Slots);
            stream.Write(Program.config.Port);

            //Chatoptionen
            stream.Write(Program.config.chatOptions.GetOptions());
            stream.Write(Program.config.chatOptions.DistanceRange);
            stream.Write(blockOptions);
            stream.Write(otherOptions);

            
            stream.Write(Program.config.staminaValue);
            stream.Write(Program.config.InventoryLimit);

            stream.Write(Program.config.Spawn.Count);
            for (int i = 0; i < Program.config.Spawn.Count; i++)
            {
                if (Program.config.Spawn[i].GetType() == typeof(Vec3))
                {
                    stream.Write((byte)0);
                    stream.Write(((Vec3)Program.config.Spawn[i]).x);
                    stream.Write(((Vec3)Program.config.Spawn[i]).y);
                    stream.Write(((Vec3)Program.config.Spawn[i]).z);
                }
                else if (Program.config.Spawn[i].GetType() == typeof(String))
                {
                    stream.Write((byte)1);
                    stream.Write((String)Program.config.Spawn[i]);
                }
                else
                {
                    stream.Write((byte)0);
                    stream.Write(0); stream.Write(0); stream.Write(0);
                }
            }

            Console.WriteLine("Sende Config... Module-Count: " + Program.config.Modules.Count);
            stream.Write(Program.config.Modules.Count);
            foreach(Module module in Program.config.Modules)
            {
                stream.Write(module.name);
                stream.Write(module.Hash);
            }

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, false);
        }
    }
}
