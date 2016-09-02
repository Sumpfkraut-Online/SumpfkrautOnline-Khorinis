using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using Gothic.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class NPCInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCNpc ret = (vob == null || !(vob is oCNpc)) ? oCNpc.Create() : (oCNpc)vob;
            base.CreateVob(ret);

            ret.Instance = this.ID;
            ret.Name.Set(Name);
            ret.SetAdditionalVisuals(BodyMesh, BodyTex, 0, HeadMesh, HeadTex, 0, -1);

            ret.Guild = 1;
            ret.InitHumanAI();

            return ret;
        }
    }
}