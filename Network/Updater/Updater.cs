using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace GUC.Updater
{
    public class Updater
    {
        String updateList = "http://gothic.mainclain.de/mc/updates/update.xml";
        String updatePath = "http://gothic.mainclain.de/mc/updates/files/";
        //String updateList = "http://gothic.mainclain.de/mc/updates/tesfsadadasd.xml";
        bool linux = false;

        String batfile = "update.bat";
        String tempFolder = "temp_guc/";
        String basePath = "../";

        WebClient wc = null;
        Options op;

        List<updateFile> hasToUpdated = new List<updateFile>();
        public StreamWriter myWriter;

        public Updater()
        {
            wc = new WebClient();
            //wc.Headers.Add(HttpRequestHeader.Authorization, "Basic " + EncodeTo64("mainclain:WHzhqZsUwNMuuqzy"));
            if (linux)
            {
                batfile = "update.sh";
            }

            //Alte-Temporäre Daten löschen
            if (Directory.Exists(tempFolder))
                Directory.Delete(tempFolder, true);


            if (File.Exists(batfile))
                File.Delete(batfile);

            //Neue Ordnerstruktur aufbauen.
            Directory.CreateDirectory(tempFolder);
            Directory.CreateDirectory(tempFolder + "files/");


            myWriter = File.CreateText("update.bat");
        }

        public WebException dlUpdateList()
        {
            try
            {
                wc.DownloadFile(updateList, tempFolder + "\\update.xml");

                op = Options.Load(tempFolder + "\\update.xml");
            }
            catch (WebException ex)
            {
                return ex;
            }

            return null;
        }

        public WebException dlFile(int id)
        {
            String tempFile = tempFolder + "files/" + hasToUpdated[id].filename;
            try
            {
                wc.DownloadFile(updatePath + hasToUpdated[id].filename, tempFile);
            }
            catch (WebException ex)
            {
                return ex;
            }

            if (!File.Exists(tempFile) || !Datei2MD5(tempFile, hasToUpdated[id].md5))
            {
                throw new Exception("Fehler: Datei \"" + tempFile + "\" hatte nicht den gleichen MD5-Hash! Download fehlgeschlagen, oder falsche Update-Konfiguration!");
            }

            for (int i = 0; i < hasToUpdated[id].filePath.Length; i++)
            {
                myWriter.WriteLine(":schleife_" + id + "_" + i);
                myWriter.WriteLine("del \"" + Path.GetFullPath(basePath + hasToUpdated[id].filePath[i]) + "\"");
                myWriter.WriteLine("if exist \"" + Path.GetFullPath(basePath + hasToUpdated[id].filePath[i]) + "\" goto schleife_" + id + "_" + i);
                myWriter.WriteLine("copy \"" + Path.GetFullPath(tempFile) + "\" \"" + Path.GetFullPath(basePath + hasToUpdated[id].filePath[i]) + "\"");
            }
            myWriter.WriteLine("del \"" + Path.GetFullPath(tempFile) + "\"");
            return null;
        }

        public bool IsUpdateNeeded()
        {
            bool update = false;
            foreach (updateFile file in op.Files)
            {
                foreach (string filepath in file.filePath)
                {
                    if (!File.Exists(basePath + filepath) || !Datei2MD5(basePath + filepath, file.md5))
                    {
                        hasToUpdated.Add(file);
                        update = true;
                        break;
                    }
                }
            }


            return update;
        }

        public void finish(bool updated)
        {
            myWriter.WriteLine("GMPStarter.exe");
            myWriter.WriteLine("exit");
            myWriter.Close();

            if (updated)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("update.bat");
                psi.CreateNoWindow = true;
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                Process.Start(psi);
            }
            else
            {
                try
                {
                    if (File.Exists("update.bat"))
                        File.Delete("update.bat");
                }
                catch (Exception ex) { }
            }
        }

        public int filesToUpdate()
        {
            return hasToUpdated.Count;
        }

        public bool isFileListEnd(int id)
        {
            if (hasToUpdated.Count <= id)
                return true;
            else
                return false;
        }

        public void setAuthData(String username, String password)
        {
            wc.Credentials = new NetworkCredential(username, password);
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
    }
}
