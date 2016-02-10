using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class NPCDef : VobDef, NPCInstance.IScriptNPCInstance
    {
        new protected NPCInstance baseDef { get { return (NPCInstance)base.baseDef; } }

        public string Name { get { return baseDef.Name; }  set { baseDef.Name = value; } }
        public string BodyMesh { get { return baseDef.BodyMesh; } set { baseDef.BodyMesh = value; } }
        public int BodyTex { get { return baseDef.BodyTex; } set { baseDef.BodyTex = value; } }
        public string HeadMesh { get { return baseDef.HeadMesh; } set { baseDef.HeadMesh = value; } }
        public int HeadTex { get { return baseDef.HeadTex; } set { baseDef.HeadTex = value; } }
    }
}