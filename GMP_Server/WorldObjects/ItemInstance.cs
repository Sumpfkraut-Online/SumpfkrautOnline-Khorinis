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
    public class ItemInstance
    {
        static ushort idCount = 0;

        protected static Dictionary<ushort, ItemInstance> instanceList = new Dictionary<ushort, ItemInstance>();
        public static Dictionary<ushort, ItemInstance> InstanceList { get { return instanceList; } }

        protected static Dictionary<string, ItemInstance> instanceDict = new Dictionary<string, ItemInstance>();
        public static Dictionary<string, ItemInstance> InstanceDict { get { return instanceDict; } }

        public ushort ID { get; protected set; }
        public string _CodeName { get; protected set; }

        #region Client fields

        public String Name = "";

        public ushort Range = 0;
        public ushort Weight = 0;

        public ItemType Type = ItemType.Misc;
        public ItemMaterial Material = ItemMaterial.Wood;

        public String Description = "";
        public String[] Text = new string[6] { "", "", "", "", "", "" };
        public ushort[] Count = new ushort[6];

        //Visuals:
        public String Visual = "";
        public String Visual_Change = "";
        public String Effect = "";

        public ItemInstance Munition = null;

        #endregion 

        #region Constructors

        public ItemInstance(string codeName)
        {
            this._CodeName = codeName.ToUpper();

            if (instanceDict.ContainsKey(this._CodeName))
            {
                Log.Logger.log("ERR: ItemInstance creation failed: CodeName is already existing " + this._CodeName);
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
            Log.Logger.log("ERR: ItemInstance creation failed: Maximum reached " + ushort.MaxValue);
        }

        public ItemInstance(ushort ID, string codeName)
        {
            this.ID = ID;
            this._CodeName = codeName.ToUpper();

            if (instanceDict.ContainsKey(this._CodeName))
            {
                Log.Logger.log("ERR: ItemInstance creation failed: CodeName is already existing " + this._CodeName);
                return;
            }

            if (instanceList.ContainsKey(ID))
            {
                Log.Logger.log("ERR: ItemInstance creation failed: ID is already existing: " + this.ID);
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
                    foreach (ItemInstance inst in instanceList.Values.OrderBy(n => n.ID))
                    {
                        bw.Write(inst.ID);
                        bw.Write(inst.Name);
                        bw.Write(inst.Range);
                        bw.Write(inst.Weight);
                        bw.Write((byte)inst.Type);
                        bw.Write((byte)inst.Material);
                        bw.Write(inst.Description);
                        for (int i = 0; i < 6; i++)
                        {
                            bw.Write(inst.Text[i]);
                            bw.Write(inst.Count[i]);
                        }
                        bw.Write(inst.Visual);
                        bw.Write(inst.Visual_Change);
                        bw.Write(inst.Effect);
                        bw.Write(inst.Munition == null ? ushort.MinValue : inst.Munition.ID);
                    }
                }
                data = ms.ToArray();
            }

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                md5.TransformFinalBlock(data, 0, data.Length);
                hash = md5.Hash;
            }
        }

        #endregion
    }
}
