using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Server.WorldObjects
{
    public class NPCInstance : AbstractInstance
    {
        #region Client fields
        public string Name = "";
        public string Visual = "";
        public string BodyMesh = "";
        public byte BodyTex = 0;
        public string HeadMesh = "";
        public byte HeadTex = 0;

        public byte BodyHeight = 100;
        public byte BodyWidth = 100;
        public short Fatness = 0;

        public byte Voice = 0;
        #endregion

        protected override void Write(BinaryWriter bw)
        {
            bw.Write(ID);

            bw.Write(Name);
            bw.Write(Visual);
            bw.Write(BodyMesh);
            bw.Write(BodyTex);
            bw.Write(HeadMesh);
            bw.Write(HeadTex);
            bw.Write(BodyHeight);
            bw.Write(BodyWidth);
            bw.Write(Fatness);
            bw.Write(Voice);
        }

        // actually unnecessary but now we don't have to cast every time
        public static new NPCInstance Get(string instanceName)
        {
            if (instanceName == null)
                return null;

            AbstractInstance inst = null;
            instanceDict.TryGetValue(instanceName, out inst);
            return (NPCInstance)inst;
        }

        public static new NPCInstance Get(ushort id)
        {
            if (id == 0)
                return null;

            AbstractInstance inst = null;
            instanceList.TryGetValue(id, out inst);
            return (NPCInstance)inst;
        }
    }
}
