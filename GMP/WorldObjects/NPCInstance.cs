using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using Gothic.zTypes;
using Gothic.zClasses;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

namespace GUC.Client.WorldObjects
{
    class NPCInstance
    {
        public static Dictionary<ushort, NPCInstance> InstanceList;

        public ushort ID;

        public string Name;
        public string Visual;
        public string BodyMesh;
        public int BodyTex;
        public string HeadMesh;
        public int HeadTex;

        public float BodyHeight;
        public float BodyWidth;
        public float Fatness;

        public int Voice;

        public oCNpc CreateNPC()
        {
            return CreateNPC(oCNpc.Create(Program.Process));
        }

        public oCNpc CreateNPC(oCNpc npc)
        {
            npc.Instance = (int)ID;
            npc.Name.Set(Name);
            npc.SetVisual(Visual);
            npc.SetAdditionalVisuals(BodyMesh, BodyTex, 0, HeadMesh, HeadTex, 0, -1);
            using (zVec3 z = zVec3.Create(Program.Process))
            {
                z.X = BodyWidth;
                z.Y = BodyHeight;
                z.Z = BodyWidth;
                npc.SetModelScale(z);
            }
            npc.SetFatness(Fatness);

            npc.Voice = Voice;

            return npc;
        }

        static string FileName = "Data1.pak";

        public static byte[] ReadFile()
        {
            try
            {
                if (File.Exists(FileName))
                {
                    byte[] hash;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (FileStream fs = new FileStream(FileName, FileMode.Open))
                        using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
                        {
                            gz.CopyTo(ms);
                        }

                        ms.Position = 0;

                        using (BinaryReader br = new BinaryReader(ms,Encoding.UTF8))
                        {
                            NPCInstance inst;
                            ushort num = br.ReadUInt16();
                            InstanceList = new Dictionary<ushort, NPCInstance>(num);
                            for (int i = 0; i < num; i++)
                            {
                                inst = new NPCInstance();

                                inst.ID = br.ReadUInt16();
                                inst.Name = br.ReadString();
                                inst.Visual = br.ReadString();
                                inst.BodyMesh = br.ReadString();
                                inst.BodyTex = br.ReadByte();
                                inst.HeadMesh = br.ReadString();
                                inst.HeadTex = br.ReadByte();
                                inst.BodyHeight = (float)br.ReadByte() / 100.0f;
                                inst.BodyWidth = (float)br.ReadByte() / 100.0f;
                                inst.Fatness = (float)br.ReadSByte() / 100.0f;
                                inst.Voice = br.ReadByte();

                                InstanceList.Add(inst.ID, inst);
                            }

                            using (MD5 md5 = new MD5CryptoServiceProvider())
                            {
                                md5.TransformFinalBlock(ms.ToArray(), 0, (int)ms.Length);
                                hash = md5.Hash;
                            }
                        }
                    }
                    return hash;
                }
            }
            catch { }
            return new byte[16];
        }

        public static void WriteFile()
        {
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
            using (BinaryWriter bw = new BinaryWriter(gz,Encoding.UTF8))
            {
                bw.Write((ushort)InstanceList.Count);
                //ordered by IDs
                foreach (NPCInstance inst in InstanceList.Values.OrderBy(n => n.ID))
                {
                    bw.Write(inst.ID);
                    bw.Write(inst.Name);
                    bw.Write(inst.Visual);
                    bw.Write(inst.BodyMesh);
                    bw.Write((byte)inst.BodyTex);
                    bw.Write(inst.HeadMesh);
                    bw.Write((byte)inst.HeadTex);
                    bw.Write((byte)Math.Round(100.0f*(float)inst.BodyHeight));
                    bw.Write((byte)Math.Round(100.0f*(float)inst.BodyWidth));
                    bw.Write((sbyte)Math.Round(100.0f*(float)inst.Fatness));
                    bw.Write((byte)inst.Voice);
                }
            }
        }
    }
}
