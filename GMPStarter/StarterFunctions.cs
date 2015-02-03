using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Options;
using GMPStarter.Options;
using WinApi.Kernel;

namespace GMPStarter
{
    public class StarterFunctions
    {
        private static ClientOptions co = null;
        public static ClientOptions getClientOptions()
        {
            if (co == null)
            {
                try
                {
                    co = ClientOptions.Load("./conf/gmp.xml");
                }
                catch (Exception ex)
                {
                    co = new ClientOptions();
                    co.Save("./conf/gmp.xml");
                }
            }
            return co;
        }

        private static FavoritesOptions fo = null;
        public static FavoritesOptions getFavoritesOptions()
        {
            if (fo == null)
            {
                try
                {
                    fo = FavoritesOptions.Load("./conf/favorites.xml");
                }
                catch (Exception ex)
                {
                    fo = new FavoritesOptions();
                    fo.Save("./conf/favorites.xml");
                }
            }
            return fo;
        }




        public static void InitDefaultFolders()
        {
            if (!Directory.Exists("./conf"))
                Directory.CreateDirectory("./conf");
            if (!Directory.Exists("./DLL"))
                Directory.CreateDirectory("./DLL");
            if (!Directory.Exists("./Log"))
                Directory.CreateDirectory("./Log");
            if (!Directory.Exists("./tempScripts"))
                Directory.CreateDirectory("./tempScripts");
            if (!Directory.Exists("./temp_guc"))
                Directory.CreateDirectory("./temp_guc");
            if (!Directory.Exists("./Data"))
                Directory.CreateDirectory("./Data");
            if (!Directory.Exists("./Downloads"))
                Directory.CreateDirectory("./Downloads");
        }



        public static void StartGothic(ClientOptions co, String nickname, String ip, ushort port, String serverpassword, int logLevel, int maxFPS, bool startWindowed)
        {
            if (!Datei2MD5("../Gothic2.exe", "3c436bd199caaaa64e9736e3cc1c9c32"))// &&  !Datei2MD5("../Gothic2.exe", "b75d03422af54286f1f4ed846b8fd4b8")
            {
                throw new Exception("Wrong Gothic Version. Gothic2.exe needs a MD5 hash of 3c436bd199caaaa64e9736e3cc1c9c32");
            }

            String vdfsFile = "[VDFS]\r\n";
            vdfsFile += "Data\\*.VDF\r\n";
            vdfsFile += "Data\\*.MOD\r\n";
            vdfsFile += "System\\UntoldChapter\\Data\\*.VDF\r\n";
            vdfsFile += "System\\UntoldChapter\\Downloads\\"+ip+"_"+port+"\\*.VDF\r\n";
            vdfsFile += "[END]\r\n";


            if (System.IO.File.Exists("../../vdfs.cfg"))
            {
                System.IO.File.Delete("../../vdfs.cfg");
            }
            System.IO.File.WriteAllText("../../vdfs.cfg", vdfsFile);




            if (co == null)
                co = getClientOptions();

            if (serverpassword == null)
                serverpassword = "";


            co.name = nickname;
            co.ip = ip;
            co.port = port;
            co.password = serverpassword;

            co.loglevel = logLevel;
            co.fps = maxFPS;
            co.startWindowed = startWindowed;
            co.Save("./conf/gmp.xml");


            String dll = "UntoldChapter/DLL/NetInject.dll";
            String RakNetDLL = "UntoldChapter/DLL/RakNet.dll";

            
            if (!System.IO.File.Exists("../" + dll))
                throw new FileNotFoundException(dll + " nicht gefunden");
            if (!System.IO.File.Exists("../" + RakNetDLL))
                throw new FileNotFoundException(RakNetDLL + " nicht gefunden");

            System.Diagnostics.ProcessStartInfo psi = null;
            //zSpy starten
            if (logLevel != -1 && System.IO.File.Exists("..\\..\\_work\\tools\\zSpy\\zSpy.exe"))
            {
                psi = new System.Diagnostics.ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.WorkingDirectory = Environment.CurrentDirectory + "\\Log";

                psi.FileName = "..\\..\\..\\_work\\tools\\zSpy\\zSpy.exe";
                WinApi.Process.Start(psi);
            }


            //Starten...
            psi = new System.Diagnostics.ProcessStartInfo();
            psi.WorkingDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
            psi.Arguments = "-nomenu";

            if (startWindowed)
                psi.Arguments += " -zwindow";

            if (logLevel != -1)
                psi.Arguments += " -zlog:" + logLevel + ",s";

            if (maxFPS > 9 && maxFPS <= 999)
                psi.Arguments += " -zMaxFrameRate:" + maxFPS;
            

            //Weitere:  -ztexconvert  -zautoconvertdata  -zconvertall  -vdfs:physicalfirst

            psi.FileName = "Gothic2.exe";
            WinApi.Process process = WinApi.Process.Start(psi);

            if (process.LoadLibary(dll) == IntPtr.Zero)
                Error.GetLastError();

        }

        public static bool Datei2MD5(string Dateipfad, string Checksumme)
        {
            //Datei einlesen
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(Dateipfad);
            // MD5-Hash aus dem Byte-Array berechnen
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();

            //in string wandeln
            string Berechnet = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            // Vergleichen
            if (Berechnet == Checksumme.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ContainsFavorite(FavoritesOptions fo, String ip, int port)
        {
            foreach (FavoritesOptions.cFavorite fav in fo.Favorites)
            {
                if (fav.ip.Equals(ip) && fav.port == port)
                {
                    return true;
                }
            }
            return false;
        }

        public static FavoritesOptions.cFavorite addFavorite(FavoritesOptions fo, String ip, int port)
        {
            if (fo == null)
                fo = getFavoritesOptions();
            FavoritesOptions.cFavorite rv = new FavoritesOptions.cFavorite() { ip = ip.Trim().ToLower(), port = port };
            fo.Favorites.Add(rv);
            fo.Save("./conf/favorites.xml");

            return rv;
        }

        public static void removeFavorite(FavoritesOptions fo, String ip, int port)
        {
            if (fo == null)
                fo = getFavoritesOptions();

            ip = ip.Trim().ToLower();
            foreach (FavoritesOptions.cFavorite fav in fo.Favorites)
            {
                if (fav.ip.Equals(ip) && fav.port == port)
                {
                    fo.Favorites.Remove(fav);
                    fo.Save("./conf/favorites.xml");
                    break;
                }
            }
        }
    }
}
