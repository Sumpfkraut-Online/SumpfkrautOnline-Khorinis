using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class NPCDef : NamedVobDef, GUCNPCDef.IScriptNPCInstance
    {
        #region Properties

        public override VobType VobType { get { return VobType.NPC; } }

        new public NPCDefEffectHandler EffectHandler { get { return (NPCDefEffectHandler)base.EffectHandler; } }

        new public GUCNPCDef BaseDef { get { return (GUCNPCDef)base.BaseDef; } }

        new public string Name
        {
            get { return this.name; }
            set { BaseDef.Name = value; this.name = value; }
        }
        public string BodyMesh { get { return BaseDef.BodyMesh; } set { BaseDef.BodyMesh = value; } }
        public int BodyTex { get { return BaseDef.BodyTex; } set { BaseDef.BodyTex = value; } }
        public string HeadMesh { get { return BaseDef.HeadMesh; } set { BaseDef.HeadMesh = value; } }
        public int HeadTex { get { return BaseDef.HeadTex; } set { BaseDef.HeadTex = value; } }
        public byte Guild { get { return BaseDef.Guild; } set { BaseDef.Guild = value; } }

        #endregion

        #region Constructors

        partial void pConstruct();
        public NPCDef()
        {
            pConstruct();
        }

        protected override GUCBaseVobDef CreateVobInstance()
        {
            return new GUCNPCDef(this);
        }

        protected override BaseEffectHandler CreateHandler()
        {
            return new NPCDefEffectHandler(null, null, this);
        }

        #endregion
    }
}