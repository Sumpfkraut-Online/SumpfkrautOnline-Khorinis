using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Compression;

namespace GUCLauncher
{
    /// <summary>
    /// The Information Packet. Consists of a text, a background image and a list of all Data Packs and their File-Infos (Type,Name,Hash,Offset).
    /// </summary>
    class InfoPack
    {
        public string Website = string.Empty;
        public byte[] ImageData = new byte[0];
        public string InfoText = string.Empty;

        public readonly List<DataPack> Packs = new List<DataPack>();

        public enum UpdateUIEnum
        {
            Website,
            InfoText,
            Image
        }

        public void Read(Stream stream, string path, Action<UpdateUIEnum> UpdateUI, Action<float> SetPercent, Action<string> SetStatus)
        {
            Packs.Clear();

            using (DeflateStream ds = new DeflateStream(stream, CompressionMode.Decompress))
            {
                byte[] buf = new byte[4];
                stream.Read(buf, 0, 4);
                int fileSize = BitConverter.ToInt32(buf, 0) - 4;

                using (PacketStream header = new PacketStream(ds, fileSize, value => SetPercent(0.4f * value)))
                {
                    SetStatus("Reading website...");
                    this.Website = header.ReadStringShort();
                    UpdateUI(UpdateUIEnum.Website);

                    SetStatus("Reading information text...");
                    this.InfoText = header.ReadStringLong();
                    UpdateUI(UpdateUIEnum.InfoText);

                    SetStatus("Reading background image...");
                    int len = header.ReadInt();
                    ImageData = header.ReadBytes(len);
                    UpdateUI(UpdateUIEnum.Image);

                    int count = header.ReadByte();
                    int checkBytes = 0;
                    for (int i = 0; i < count; i++)
                    {
                        SetStatus(string.Format("Reading data packs ({0}/{1})...", i + 1, count));
                        DataPack pack = new DataPack();
                        checkBytes += pack.Read(header, path);
                        Packs.Add(pack);
                    }

                    int doneBytes = 0;
                    for (int i = 0; i < count; i++)
                    {
                        SetStatus(string.Format("Checking data packs ({0}/{1})...", i + 1, count));
                        Packs[i].EndCheck(value =>
                        {
                            doneBytes += value;
                            SetPercent(0.4f + 0.6f * doneBytes / checkBytes);
                        });
                    }

                    SetPercent(1);
                }
            }
        }

        public bool NeedsUpdate()
        {
            for (int i = 0; i < Packs.Count; i++)
                if (Packs[i].NeedsUpdate())
                    return true;
            return false;
        }

        public void DoUpdate(Action<float> SetPercent, Action<string> SetStatus)
        {
            int bytesToLoad = 0;
            for (int i = 0; i < Packs.Count; i++)
                bytesToLoad += Packs[i].PreUpdate();
            
            int doneBytes = 0;

            int lastUpdate = int.MinValue;

            for (int i = 0; i < Packs.Count; i++)
                Packs[i].DoUpdate(value =>
                {
                    doneBytes += value;
                    SetPercent((float)doneBytes / bytesToLoad);
                    int currentUpdate = doneBytes / 100000;
                    if (currentUpdate > lastUpdate)
                    {
                        SetStatus(string.Format("Updating... ({0:0.0}MB / {1:0.0}MB)", currentUpdate / 10f, bytesToLoad / 1000000f));
                        lastUpdate = currentUpdate;
                    }
                });

            SetStatus("Finished.");
        }
    }

}
