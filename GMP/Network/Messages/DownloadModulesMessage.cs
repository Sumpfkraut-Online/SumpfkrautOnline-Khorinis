using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;
using System.IO;
using GMP.Modules;
using WinApi;
using Gothic.mClasses;
using System.Windows.Forms;
using Injection;
using Gothic.zClasses;

namespace GMP.Net.Messages
{
    public class DownloadModulesMessage : Message
    {
        public override void Write(RakNet.BitStream stream, Client client)
        {
            Module module = GetActiveModule();
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.DownloadModulesMessage);
            stream.Write(module.name);
            stream.Write(module.size);
            stream.Write(module.loadingSize);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.DownloadModulesMessage))
                StaticVars.sStats[(int)NetWorkIDS.DownloadModulesMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.DownloadModulesMessage] += 1;
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            String name = "";
            long size = 0;
            long loadingSize = 0;

            stream.Read(out name);
            stream.Read(out size);
            stream.Read(out loadingSize);

            Module module = GetActiveModule();
            if (module.name != name)
                return;
            module.size = size;

            byte[] arr = new byte[loadingSize];
            stream.Read(arr, (uint)loadingSize);

            module.loadingSize += loadingSize;
            FileStream fs = new FileStream(module.fileName, FileMode.Append, FileAccess.Write);
            fs.Write(arr, 0, (int)loadingSize);
            fs.Close();

            

            if (module.loadingSize == size && size != 0)
            {
                if (Datei2MD5(module.fileName) == module.Hash)
                {
                    module.downloaded = true;
                    ModuleLoader.load(module);
                }
                else
                {
                    module.size = 0;
                    module.loadingSize = 0;
                    File.Delete(module.fileName);
                }
                Next(client);
                
            }
            else
            {
                Write(stream, client);
            }

        }

        public static Module GetActiveModule()
        {
            foreach (Module m in Modules.StaticVars.serverConfig.Modules)
            {
                if (!m.downloaded)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Module laden: " + m.fileName, 0, "Program.cs", 0);
                    return m;
                }
            }
            return null;
        }

        public static void Next(Client client)
        {
            Module module = null;
            String path = @".\gothic_multiplayer\Modules\";

            module = GetActiveModule();
            if (module == null)
            {

                Program.StartupModule();
                return;
            }
            
            string[] files = System.IO.Directory.GetFiles(path);
            
            foreach (string file in files)
            {
                
                if (Datei2MD5(file) == module.Hash)
                {
                    module.fileName = file;
                    module.downloaded = true;
                    ModuleLoader.load(module);
                    Next(client);
                    return;
                }
            }


            //Datei downloaden!

            if (!File.Exists(path + module.name))
                module.fileName = path + module.name;
            else
            {
                String file = path +"0_"+module.name;
                int i= 0;
                while(File.Exists(file))
                {
                    i++;
                    file = path +i+"_"+module.name;
                }
                module.fileName = path+i + "_" + module.name;
            }
            module.fileName = Path.GetFullPath(module.fileName);

            new DownloadModulesMessage().Write(client.sentBitStream, client);


        }

        

        public static String Datei2MD5(string Dateipfad)
        {
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(Dateipfad);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();

            return BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
        }
    }
}
