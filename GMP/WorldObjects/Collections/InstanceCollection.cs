using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Client.WorldObjects.Instances;
using GUC.Network;
using System.Security.Cryptography;
using System.IO;

namespace GUC.Client.WorldObjects.Collections
{
    public class InstanceCollection : VobObjCollection<ushort, VobInstance>
    {
        string FileName = States.StartupState.getDaedalusPath() + "Instances.pak";

        public byte[] ReadFile()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    byte[] data = File.ReadAllBytes(FileName);

                    PacketReader stream = new PacketReader();
                    stream.Load(data, data.Length);
                    stream.ReadShort(); //ignore first 2 bytes
                    ReadData(stream);

                    byte[] hash;
                    using (MD5 md5 = new MD5CryptoServiceProvider())
                    {
                        md5.TransformFinalBlock(data, 0, data.Length);
                        hash = md5.Hash;
                    }

                    return hash;
                }
            }
            catch { }
            return new byte[16];
        }

        public void ReadData(PacketReader stream)
        {
            stream.Decompress();
            for (int i = 0; i < (int)VobType.Maximum; i++)
            {
                InstanceDictionary dict = new InstanceDictionary();
                ushort count = stream.ReadUShort();
                for (int j = 0; j < count; j++)
                {
                    VobInstance inst = VobInstance.CreateByType((VobType)i);
                    inst.ReadProperties(stream);
                    dict.Add(inst);
                }
                vobDicts[i] = dict;
            }
        }

        public void WriteFile()
        {
            PacketWriter stream = Program.client.SetupStream(NetworkID.ConnectionMessage);

            stream.StartCompressing();
            for (int i = 0; i < (int)VobType.Maximum; i++)
            {
                InstanceDictionary dict = (InstanceDictionary)vobDicts[i];
                stream.Write((ushort)dict.GetCount());
                foreach (VobInstance inst in dict.GetAll().OrderBy(v => v.ID))
                {
                    inst.WriteProperties(stream);
                }
            }
            stream.StopCompressing();

            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                fs.Write(stream.GetData(), 0, stream.GetLength());
            }
        }

        internal InstanceCollection()
        {
            for (int i = 0; i < (int)VobType.Maximum; i++)
            {
                vobDicts[i] = new InstanceDictionary();
            }
        }

        new public InstanceDictionary GetDict(VobType type)
        {
            return (InstanceDictionary)base.GetDict(type);
        }
    }
}
