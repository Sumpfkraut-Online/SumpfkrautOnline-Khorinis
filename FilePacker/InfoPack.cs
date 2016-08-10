using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace FilePacker
{
    class InfoPack
    {
        public byte[] ImageData = new byte[0];
        public string InfoText = string.Empty;

        public readonly List<DataPack> Packs = new List<DataPack>();

        public void Build(Action<int> SetPercent)
        {
            using (MemoryStream ms = new MemoryStream(2048))
            using (BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8, true))
            {
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
    }

}
