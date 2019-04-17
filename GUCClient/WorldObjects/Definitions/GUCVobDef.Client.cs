using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using Gothic.Objects.Meshes;

namespace GUC.WorldObjects.Definitions
{
    public partial class GUCVobDef
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            zCVob ret = vob == null ? zCVob.Create() : vob;
            
            var visualName = this.ModelInstance.Visual;
            if (visualName.EndsWith(".ZEN"))
            {
                ret.SetVisual(new zCVisual(0));
                ret.SetCollDetDyn(false);
                ret.SetCollDetStat(false);
                Gothic.zCOption.ChangeDir(0xE); // world directory
                var world = GothicGlobals.Game.GetWorld();
                world.AddVob(ret);
                ret.SetPositionWorld(0,0,0);

                zCVob zenVob = world.MergeVobSubTree(visualName);
                if (zenVob.Address != 0)
                {
                    zenVob.SetCollDetDyn(false);
                    zenVob.SetCollDetStat(false);
                    zenVob.SetPositionWorld(0,0,0);
                    ret.TrafoObjToWorld.Set(zenVob.TrafoObjToWorld);
                    zenVob.AddRefVobSubtree();
                    world.RemoveVobSubTree(zenVob);
                    world.AddVobAsChild(zenVob, ret);
                    world.RemoveVobSubTree(ret);
                }
                else
                {
                    Log.Logger.LogWarning("Could not load " + visualName);
                }
            }
            else
            {
                zCVisual vis = zCVisual.LoadVisual(this.ModelInstance.Visual);
                ret.SetVisual(vis);
            }

            ret.SetCollDetDyn(CDDyn);
            ret.SetCollDetStat(CDStatic);
            
            return ret;
        }
    }
}
