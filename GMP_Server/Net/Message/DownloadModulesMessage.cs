using System;
using System.Collections.Generic;
using System.Text;
using Network;
using System.IO;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class DownloadModulesMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int defaultLoadingSize = 1024;


            String name;
            long size;
            long loadingSize;

            stream.Read(out name); stream.Read(out size); stream.Read(out loadingSize);
            
            Module module = GetModuleByName(name);
            if (module == null)
                return;

            FileStream fs = new FileStream(@"Modules/" + module.name, FileMode.Open, FileAccess.Read);
            size = fs.Length;
            fs.Position = loadingSize;
            if (fs.Position + defaultLoadingSize > fs.Length)
                loadingSize = fs.Length - fs.Position;
            else
                loadingSize = defaultLoadingSize;

            byte[] arr = new byte[loadingSize];
            fs.Read(arr, 0, (int)loadingSize);
            fs.Close();

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.DownloadModulesMessage);
            stream.Write(name);
            stream.Write(size);
            stream.Write(loadingSize);
            stream.Write(arr, (uint)loadingSize);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, false);
        }

        public static Module GetModuleByName(String name)
        {
            foreach (Module module in Program.config.Modules)
            {
                if (module.name == name)
                    return module;
            }
            return null;
        }
    }
}
