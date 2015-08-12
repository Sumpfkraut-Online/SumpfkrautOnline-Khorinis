using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Network;
using RakNet;
using GUC.Network;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Server.WorldObjects
{
    public class NPCInstance
    {
        static ushort idCount = 0;

        static Dictionary<string, NPCInstance> instanceDict = new Dictionary<string, NPCInstance>();
        public static Dictionary<string, NPCInstance> InstanceDict { get { return instanceDict; } }

        static Dictionary<ushort, NPCInstance> instanceList = new Dictionary<ushort, NPCInstance>();
        public static Dictionary<ushort, NPCInstance> InstanceList { get { return instanceList; } }

        public ushort ID { get; protected set; }
        public string _CodeName { get; protected set; }

        #region Client fields
        public string Name = "";
        public string Visual = "";
        public string BodyMesh = "";
        public byte BodyTex = 0;
        public string HeadMesh = "";
        public byte HeadTex = 0;

        public byte BodyHeight = 100;
        public byte BodyWidth = 100;
        public sbyte Fatness = 0;

        public byte Voice = 0;
        #endregion

        #region Constructors

        public NPCInstance(string codeName)
        {
            this._CodeName = codeName.ToUpper();

            if (instanceDict.ContainsKey(this._CodeName))
            {
                Log.Logger.log("ERR: NPCInstance creation failed: CodeName is already existing " + this._CodeName);
                return;
            }

            for (int i = 0; i < ushort.MaxValue; i++)
            {
                if (!instanceList.ContainsKey(idCount))
                {
                    this.ID = idCount;
                    instanceList.Add(idCount, this);
                    instanceDict.Add(codeName, this);
                    idCount++;
                    return;
                }
                idCount++;
            }
            Log.Logger.log("ERR: NPCInstance creation failed: Maximum reached " + ushort.MaxValue);
        }

        public NPCInstance(ushort ID, string codeName)
        {
            this.ID = ID;
            this._CodeName = codeName.ToUpper();

            if (instanceDict.ContainsKey(this._CodeName))
            {
                Log.Logger.log("ERR: NPCInstance creation failed: CodeName is already existing " + this._CodeName);
                return;
            }

            if (instanceList.ContainsKey(ID))
            {
                Log.Logger.log("ERR: NPCInstance creation failed: ID is already existing: " + this.ID);
                return;
            }

            instanceList.Add(this.ID, this);
            instanceDict.Add(this._CodeName, this);
            idCount = (ushort)(this.ID + 1);
        }

        #endregion

        #region Networking

        internal static byte[] data;
        internal static byte[] hash;

        public static void NetUpdate()
        {
            using (MemoryStream ms = new MemoryStream())
            {   //SO MANY STREAMS
                using (GZipStream gz = new GZipStream(ms, CompressionMode.Compress))
                using (BufferedStream bs = new BufferedStream(gz))
                using (BinaryWriter bw = new BinaryWriter(bs, Encoding.UTF8))
                {
                    bw.Write((ushort)instanceList.Count);
                    //ordered by IDs
                    foreach (NPCInstance inst in instanceList.Values.OrderBy(n => n.ID))
                    {
                        bw.Write(inst.ID);
                        bw.Write(inst.Name);
                        bw.Write(inst.Visual);
                        bw.Write(inst.BodyMesh);
                        bw.Write(inst.BodyTex);
                        bw.Write(inst.HeadMesh);
                        bw.Write(inst.HeadTex);
                        bw.Write(inst.BodyHeight);
                        bw.Write(inst.BodyWidth);
                        bw.Write(inst.Fatness);
                        bw.Write(inst.Voice);
                    }
                }
                data = ms.ToArray();
            }

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                md5.TransformFinalBlock(data, 0, data.Length);
                hash = md5.Hash;
            }

            System.IO.File.WriteAllBytes("data1", data);
        }

        #endregion
    }
}
