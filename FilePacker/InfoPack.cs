using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Compression;

namespace FilePacker
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

        public void Build(Action<string> SetState = null)
        {
            if (SetState != null) SetState("Writing header...");

            using (MemoryStream ms = new MemoryStream(64000))
            using (PacketStream stream = new PacketStream(ms))
            {
                stream.WriteStringShort(Website);
                stream.WriteStringLong(InfoText);

                stream.Write(ImageData.Length);
                stream.WriteBytes(ImageData);

                stream.WriteByte(Packs.Count);
                for (int i = 0; i < Packs.Count; i++)
                {
                    if (SetState != null)
                        SetState(string.Format("Writing '{0}' Pack ({1}/{2}) ...", Packs[i].Name, i + 1, Packs.Count));

                    Packs[i].Write(stream);
                }

                if (SetState != null)
                    SetState("Finishing header...");

                ms.Position = 0;
                using (FileStream fs = new FileStream("info.bin", FileMode.Create, FileAccess.Write))
                using (DeflateStream ds = new DeflateStream(fs, CompressionLevel.Optimal))
                {
                    fs.Write(BitConverter.GetBytes((int)ms.Length), 0, 4);
                    ms.CopyTo(ds);
                }
            }

            GC.Collect();
            if (SetState != null)
                SetState("Finished.");
        }

        public enum UpdateUIEnum
        {
            Website,
            InfoText,
            Image
        }

        public void Read(Stream stream, Action<UpdateUIEnum> UpdateUI, Action<float> SetPercent, Action<string> SetStatus)
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
                        SetStatus(string.Format("Reading data packs ({0}/{1})...", i+1, count));
                        DataPack pack = new DataPack();
                        checkBytes += pack.Read(header);
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
                if (Packs[i].NeededList.Count > 0)
                    return true;
            return false;
        }
    }

}
