using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using Gothic.Types;

namespace GUC.WorldObjects.Instances
{
    public partial class NPCInstance : VobInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCNpc ret = (vob == null || !(vob is oCNpc)) ? oCNpc.Create() : (oCNpc)vob;
            base.CreateVob(ret);

            ret.Instance = this.ID;
            ret.Name.Set(Name);
            ret.SetAdditionalVisuals(BodyMesh, BodyTex, 0, HeadMesh, HeadTex, 0, -1);
            using (zVec3 z = zVec3.Create())
            {
                z.X = (float)BodyWidth / 100.0f;
                z.Y = (float)BodyHeight / 100.0f;
                z.Z = (float)BodyWidth / 100.0f;
                ret.SetModelScale(z);
            }
            ret.SetFatness((float)Fatness / 100.0f);

            //gNpc.Voice = Voice;

            return ret;
        }
    }
}