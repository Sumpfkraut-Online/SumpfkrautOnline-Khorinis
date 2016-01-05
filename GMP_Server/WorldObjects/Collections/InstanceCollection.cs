using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.WorldObjects.Instances;
using GUC.Network;
using System.Security.Cryptography;

namespace GUC.Server.WorldObjects.Collections
{
    public class InstanceCollection : VobObjCollection<ushort, VobInstance>
    {
        internal byte[] Data;
        internal byte[] Hash;

        public void NetUpdate()
        {
            PacketWriter stream = Program.server.SetupStream(NetworkIDs.ConnectionMessage);

            stream.StartCompressing();
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                InstanceDictionary dict = (InstanceDictionary)vobDicts[i];
                stream.Write((ushort)dict.GetCount());
                foreach (VobInstance inst in dict.GetAll().OrderBy(v => v.ID))
                {
                    inst.WriteProperties(stream);
                }
            }
            stream.StopCompressing();

            Data = new byte[stream.GetLength()];
            Buffer.BlockCopy(stream.GetData(), 0, Data, 0, stream.GetLength());

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                md5.TransformFinalBlock(Data, 0, Data.Length);
                Hash = md5.Hash;
            }

            //FIXME: Send to all clients

            StringBuilder sb = new StringBuilder();
            foreach (byte b in Hash)
                sb.Append(b.ToString("X2"));
            System.IO.File.WriteAllBytes(sb.ToString(), Data);
        }

        internal InstanceCollection()
        {
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                vobDicts[i] = new InstanceDictionary();
            }
        }

        new public InstanceDictionary GetDict(VobTypes type)
        {
            return (InstanceDictionary)base.GetDict(type);
        }
    }
}
