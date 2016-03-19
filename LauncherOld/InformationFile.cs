using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Launcher2
{
    class InformationFile
    {
        private const string URL = "https://dl.dropboxusercontent.com/u/83946034/info";
        public string serverIP;
        private DataPack SumpfkrautOnline;

        public bool needsUpdate;

        public InformationFile()
        {
            serverIP = "127.0.0.1";
            SumpfkrautOnline = new DataPack("SumpfkrautOnline");
            needsUpdate = false;
        }

        public async Task GetInfoFile()
        {
            Download dl = new Download(URL);
            using (Stream strm = await dl.StartStream())
            {
                if (strm != null)
                {
                    try
                    {
                        using (BinaryReader br = new BinaryReader(strm))
                        {
                            byte[] ipBuf = br.ReadBytes(4);
                            serverIP = string.Copy(ipBuf[0].ToString() + "." + ipBuf[1].ToString() + "." + ipBuf[2].ToString() + "." + ipBuf[3].ToString());

                            Global.Ping.Start(serverIP);

                            await SumpfkrautOnline.ReadInfoFile(br);
                            if (SumpfkrautOnline.needsUpdate)
                            {
                                needsUpdate = true;
                            }
                        }
                    }
                    catch
                    {
                        Global.Progress.Error("Geladene Updateinformationen sind fehlerhaft!");
                    }
                }
            }
        }

        public void Update()
        {
            if (SumpfkrautOnline.needsUpdate)
            {
                SumpfkrautOnline.Update();
            }
        }

        public void FinishUpdates()
        {
            if (SumpfkrautOnline.needsUpdate)
            {
                SumpfkrautOnline.FinishUpdate();
            }
        }
    }
}
