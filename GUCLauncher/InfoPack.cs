using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Build(Action<double> SetPercent)
        {
            using (MemoryStream ms = new MemoryStream(2048))
            using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8))
            {
                bw.Write(Website);
                bw.Write(InfoText);

                bw.Write(ImageData.Length);
                bw.Write(ImageData, 0, ImageData.Length);

                bw.Write(Packs.Count);
                for (int i = 0; i < Packs.Count; i++)
                {
                    Packs[i].Write(bw, SetPercent);
                }

                ms.Position = 0;
                using (FileStream fs = new FileStream("info.bin", FileMode.Create, FileAccess.Write))
                using (DeflateStream ds = new DeflateStream(fs, CompressionLevel.Optimal))
                {
                    ms.CopyTo(ds);
                }
            }
        }

        public void Read(Stream stream, Action<double> SetPercent)
        {
            using (DeflateStream ds = new DeflateStream(stream, CompressionMode.Decompress))
            using (BinaryReader br = new BinaryReader(ds, Encoding.UTF8))
            {
                this.Website = br.ReadString();
                SetPercent(5);
                this.InfoText = br.ReadString();
                SetPercent(15);

                int byteLen = br.ReadInt32();
                this.ImageData = br.ReadBytes(byteLen);
                SetPercent(50);

                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    DataPack pack = new DataPack();
                    pack.Read(br, SetPercent);
                    SetPercent((i + 1d) / count * 50 + 50);
                }
            }
        }
    }

}
