using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Client;
using Gothic.Types;
using Gothic;
using Gothic.Objects;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ProjInst
    {
        static readonly SoundInstance sfx_shoot = new SoundInstance("BOWSHOOT");
        static readonly SoundInstance sfx_wo_wo = new SoundInstance("CS_IHL_WO_WO");
        static readonly SoundInstance sfx_wo_me = new SoundInstance("CS_IHL_WO_ME");
        static readonly SoundInstance sfx_wo_st = new SoundInstance("CS_IHL_WO_ST");
        static readonly SoundInstance sfx_wo_wa = new SoundInstance("CS_IHL_WO_WA");
        static readonly SoundInstance sfx_wo_ea = new SoundInstance("CS_IHL_WO_EA");
        static readonly SoundInstance sfx_wo_sa = new SoundInstance("CS_IHL_WO_SA");

        partial void pSpawn(WorldInst world, Vec3f pos, Vec3f dir)
        {
            // create arrow trail
            var ai = oCAIArrow.Create();
            var gVob = this.BaseInst.gVob;
            gVob.SetSleeping(true);
            gVob.SetAI(ai);
            ai.CreateTrail(gVob);
            gVob.SetSleeping(false);

            // play shooting sound
            SoundHandler.PlaySound3D(sfx_shoot, this.BaseInst);
        }

        partial void pDespawn()
        {
            // check the level for hits
            Vec3f currentPos = this.GetPosition();

            Vec3f startDir = this.BaseInst.GetStartDir().Normalise();

            Vec3f startPos = currentPos + startDir * -50;
            Vec3f endPos = currentPos + startDir * 5000; // need to be really generous or gothic reports nonsense

            using (zVec3 start = zVec3.Create(startPos.X, startPos.Y, startPos.Z))
            using (zVec3 end = zVec3.Create(endPos.X, endPos.Y, endPos.Z))
            {
                var gWorld = oCGame.GetWorld();

                zCWorld.zTraceRay parm = zCWorld.zTraceRay.Ignore_Alpha | zCWorld.zTraceRay.Test_Water | zCWorld.zTraceRay.Return_POLY | zCWorld.zTraceRay.Ignore_NPC | zCWorld.zTraceRay.Ignore_Projectiles | zCWorld.zTraceRay.Ignore_Vob_No_Collision;
                if (gWorld.TraceRayNearestHit(start, end, parm) != 0 && gWorld.Raytrace_FoundHit != 0 && currentPos.GetDistance((Vec3f)gWorld.Raytrace_FoundIntersection) < 80)
                {
                    var poly = gWorld.Raytrace_FoundPoly;

                    SoundInstance sfx;
                    if (poly.Address == 0)
                    {
                        sfx = sfx_wo_wo; // wood
                    }
                    else
                    {
                        switch (poly.Material.MatGroup)
                        {
                            case 0: // wood
                                sfx = sfx_wo_wo;
                                break;
                            case 1: // metal
                                sfx = sfx_wo_me;
                                break;
                            case 2: // stone
                                sfx = sfx_wo_st;
                                break;
                            case 3: // water
                                sfx = sfx_wo_wa;
                                break;
                            case 4: // earth
                                sfx = sfx_wo_ea;
                                break;
                            default: // sand
                                sfx = sfx_wo_sa;
                                break;
                        }
                    }
                    SoundHandler.PlaySound3D(sfx, startPos);
                }
            }
        }
    }
}
