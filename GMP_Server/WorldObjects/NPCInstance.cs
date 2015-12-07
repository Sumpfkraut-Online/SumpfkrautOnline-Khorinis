using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects
{
    public class NPCInstance : VobInstance
    {
        internal AnimationControl AniCtrl = null;

        #region Server fields

        public ushort AttrHealthMax = 100;
        public ushort AttrManaMax = 10;
        public ushort AttrStaminaMax = 100;
        public ushort AttrStrength = 10;
        public ushort AttrDexterity = 10;

        #endregion

        //Things which the client knows too
        #region Client fields

        /// <summary>The standard name of the NPC.</summary>
        public string Name = "";

        /// <summary>The .MDS-Visual of the NPC.</summary>
        public string Visual
        {
            get { return visual; }
            set 
            { 
                visual = value.Trim().ToUpper();

                AnimationControl.dict.TryGetValue(visual, out AniCtrl);
                if (AniCtrl == null)
                {
                    Log.Logger.logWarning("Warning: Could not find AnimationControl for " + visual);
                }
            }
        }
        string visual = "";

        /// <summary>The body mesh of the NPC.</summary>
        public string BodyMesh
        {
            get { return bodyMesh; }
            set { bodyMesh = value.Trim().ToUpper(); }
        }
        string bodyMesh = "";

        /// <summary>The body texture of the NPC.</summary>
        public byte BodyTex = 0;

        /// <summary>The head mesh of the NPC.</summary>
        public string HeadMesh
        {
            get { return headMesh; }
            set { headMesh = value.Trim().ToUpper(); }
        }
        string headMesh = "";

        /// <summary>The head texture of the NPC.</summary>
        public byte HeadTex = 0;

        /// <summary>The standard body height of the NPC in percent. Default: 100</summary>
        public byte BodyHeight = 100;
        /// <summary>The standard body width (x & z) of the NPC in percent. Default: 100</summary>
        public byte BodyWidth = 100;
        /// <summary>The standard fatness of the NPC in percent. Default: 0</summary>
        public short Fatness = 0;

        /// <summary>The voice index of the NPC. Only used for humans. Default: None</summary>
        public HumVoice Voice = HumVoice.None;

        internal override void Write(BinaryWriter bw)
        {
            bw.Write(ID);

            bw.Write(Name);
            bw.Write(visual);
            bw.Write(bodyMesh);
            bw.Write(BodyTex);
            bw.Write(headMesh);
            bw.Write(HeadTex);
            bw.Write(BodyHeight);
            bw.Write(BodyWidth);
            bw.Write(Fatness);
            bw.Write((byte)Voice);
        }

        #endregion

        #region Constructors

        public NPCInstance(string instanceName, object scriptObject)
            : base(instanceName, scriptObject)
        {
        }

        public NPCInstance(ushort ID, string instanceName, object scriptObject)
            : base(ID, instanceName, scriptObject)
        {
        }

        #endregion

        public static InstanceManager<NPCInstance> Table = new InstanceManager<NPCInstance>();
    }
}