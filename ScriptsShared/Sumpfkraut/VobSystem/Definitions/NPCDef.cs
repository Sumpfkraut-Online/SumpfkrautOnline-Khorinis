using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class NPCDef : VobDef, NPCInstance.IScriptNPCInstance
    {
        new public NPCInstance BaseDef { get { return (NPCInstance)base.BaseDef; } }

        public string Name { get { return BaseDef.Name; }  set { BaseDef.Name = value; } }
        public string BodyMesh { get { return BaseDef.BodyMesh; } set { BaseDef.BodyMesh = value; } }
        public int BodyTex { get { return BaseDef.BodyTex; } set { BaseDef.BodyTex = value; } }
        public string HeadMesh { get { return BaseDef.HeadMesh; } set { BaseDef.HeadMesh = value; } }
        public int HeadTex { get { return BaseDef.HeadTex; } set { BaseDef.HeadTex = value; } }

        public NPCDef() : base(new NPCInstance())
        {
        }
    }
}