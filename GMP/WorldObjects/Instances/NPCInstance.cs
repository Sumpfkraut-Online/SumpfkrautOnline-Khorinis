using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using GUC.Server.WorldObjects.Collections;

namespace GUC.Client.WorldObjects.Instances
{
    public class NPCInstance : VobInstance
    {
        new public readonly static VobType sVobType = VobType.NPC;
        new public readonly static InstanceDictionary Instances = Network.Server.sInstances.GetDict(sVobType);

        #region Properties

        public ushort HealthMax = 100;    

        /// <summary>The default name of the NPC.</summary>
        public string Name = "";

        protected string bodyMesh = "";
        /// <summary>The body mesh of the NPC.</summary>
        public string BodyMesh
        {
            get { return bodyMesh; }
            set { bodyMesh = value.Trim().ToUpper(); }
        }

        /// <summary>The body texture of the NPC.</summary>
        public byte BodyTex = 0;

        protected string headMesh = "";
        /// <summary>The head mesh of the NPC.</summary>
        public string HeadMesh
        {
            get { return headMesh; }
            set { headMesh = value.Trim().ToUpper(); }
        }

        /// <summary>The default head texture of the NPC.</summary>
        public byte HeadTex = 0;

        /// <summary>The default body height of the NPC in percent. Default: 100</summary>
        public byte BodyHeight = 100;
        /// <summary>The default body width (x & z) of the NPC in percent. Default: 100</summary>
        public byte BodyWidth = 100;
        /// <summary>The default fatness of the NPC in percent. Default: 0</summary>
        public short Fatness = 0;

        public override bool CDDyn { get { return true; } }
        public override bool CDStatic { get { return true; } }

        #endregion

        public NPCInstance(string instanceName, object scriptObject)
            : this(0, instanceName, scriptObject)
        {
        }

        public NPCInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
            this.VobType = sVobType;
        }

        /// <summary> Use this to send additional information to the clients. The base properties are already written! </summary>
        new public static Action<NPCInstance, PacketWriter> OnWriteProperties;
        internal override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);

            stream.Write(Name);
            stream.Write(BodyMesh);
            stream.Write(BodyTex);
            stream.Write(HeadMesh);
            stream.Write(HeadTex);
            stream.Write(BodyHeight);
            stream.Write(BodyWidth);
            stream.Write(Fatness);

            if (OnWriteProperties != null)
            {
                OnWriteProperties(this, stream);
            }
        }
    }
}