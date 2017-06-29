using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{

    public partial class NPCDef : NamedVobDef, NPCInstance.IScriptNPCInstance
    {
        new public NPCInstance BaseDef { get { return (NPCInstance)base.BaseDef; } }

        new public string Name
        {
            get { return this.name; }
            set { BaseDef.Name = value; this.name = value; }
        }
        public string BodyMesh { get { return BaseDef.BodyMesh; } set { BaseDef.BodyMesh = value; } }
        public int BodyTex { get { return BaseDef.BodyTex; } set { BaseDef.BodyTex = value; } }
        public string HeadMesh { get { return BaseDef.HeadMesh; } set { BaseDef.HeadMesh = value; } }
        public int HeadTex { get { return BaseDef.HeadTex; } set { BaseDef.HeadTex = value; } }

        #region Constructors

        partial void pConstruct();
        public NPCDef()
        {
            effectHandler = effectHandler ?? new EffectSystem.EffectHandlers.NPCInstEffectHandler(null, this);
            pConstruct();
        }

        protected override BaseVobInstance CreateVobInstance()
        {
            return new NPCInstance(this);
        }

        #endregion
    }
}