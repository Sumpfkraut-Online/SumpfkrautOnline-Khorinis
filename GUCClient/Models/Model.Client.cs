using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;
using GUC.Animations;
using GUC.WorldObjects;
using GUC.Network;
using GUC.Types;

namespace GUC.Models
{
    public partial class Model : GameObject
    {
        #region Network Messages

        internal static class Messages
        {
            #region Overlays

            public static void ReadOverlay(PacketReader stream, bool add)
            {
                int id = stream.ReadUShort();

                Vob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    Model model = vob.Model;

                    Overlay ov;
                    if (model.Instance.TryGetOverlay(stream.ReadByte(), out ov))
                    {
                        if (add)
                        {
                            model.ScriptObject.ApplyOverlay(ov);
                        }
                        else
                        {
                            model.ScriptObject.RemoveOverlay(ov);
                        }
                    }
                }
            }

            #endregion

            #region Animations

            public static void ReadAniStartFPS(PacketReader stream)
            {
                ReadAniStart(stream, stream.ReadFloat());
            }

            public static void ReadAniStart(PacketReader stream, float fpsMult = 1.0f)
            {
                float progress = stream.ReadFloat();
                if (World.Current.TryGetVob(stream.ReadUShort(), out Vob vob))
                {
                    Model model = vob.Model;

                    if (model.Instance.TryGetAniJob(stream.ReadUShort(), out AniJob job))
                    {
                        model.ScriptObject.StartAniJob(job, fpsMult, progress);
                    }
                }
            }

            public static void ReadAniStop(PacketReader stream, bool fadeOut)
            {
                if (World.Current.TryGetVob(stream.ReadUShort(), out Vob vob))
                {
                    Model model = vob.Model;
                    if (model.Instance.TryGetAniJob(stream.ReadUShort(), out AniJob job))
                    {
                        ActiveAni aa = model.GetActiveAniFromAniJob(job);
                        if (aa != null)
                        {
                            model.ScriptObject.StopAnimation(aa, fadeOut);
                        }
                    }
                }
            }

            public static void ReadAniStartUncontrolled(PacketReader stream)
            {
                if (World.Current.TryGetVob(stream.ReadUShort(), out Vob vob))
                {
                    Model model = vob.Model;

                    if (model.Instance.TryGetAniJob(stream.ReadUShort(), out AniJob job))
                    {
                        model.ScriptObject.StartAniJobUncontrolled(job);
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Constructors

        partial void pConstruct()
        {
            this.vob.OnSpawn += Vob_OnSpawn;
        }

        #endregion

        #region Overlays

        partial void pAddOverlay(Overlay overlay)
        {
            if (this.vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null)
                    gVob.ApplyOverlay(overlay.Name);
            }
        }

        partial void pRemoveOverlay(Overlay overlay)
        {
            if (this.vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null)
                    gVob.RemoveOverlay(overlay.Name);
            }
        }

        #endregion

        #region Animations

        partial void pStartUncontrolledAni(AniJob aniJob)
        {
            if (this.vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null)
                {
                    gVob.AniCtrl.StopTurnAnis();
                    gVob.GetModel().StartAni(aniJob.Name, 0); 
                }
            }
        }

        partial void pStartAnimation(ActiveAni aa, float fpsMult, float progress)
        {
            if (this.vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null)
                {
                    gVob.AniCtrl.StopTurnAnis();

                    var gModel = gVob.GetModel();
                    int aniID = gModel.GetAniIDFromAniName(aa.AniJob.Name);
                    if (aniID > 0)
                    {
                        var gAni = gModel.GetAniFromAniID(aniID);
                        if (gAni.Address != 0)
                        {
                            gModel.StartAni(gAni, 0);

                            var activeAni = gModel.GetActiveAni(gAni);
                            if (activeAni.Address != 0)
                            {
                                if (!gAni.IsReversed)
                                {
                                    activeAni.SetActFrame(aa.Ani.StartFrame + aa.Ani.GetFrameNum() * progress);
                                }
                                else
                                {
                                    activeAni.SetActFrame(gAni.NumFrames - (aa.Ani.StartFrame + aa.Ani.GetFrameNum() * progress));
                                }
                            }
                        }
                    }
                }
            }

        }

        partial void pStopAnimation(ActiveAni aa, bool fadeOut)
        {
            if (this.vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null)
                {
                    var gModel = gVob.GetModel();
                    int id = gModel.GetAniIDFromAniName(aa.AniJob.Name);
                    var activeAni = gModel.GetActiveAni(id);

                    if (fadeOut)
                    {
                        gModel.StopAni(activeAni);
                    }
                    else
                    {
                        gModel.FadeOutAni(activeAni);
                    }
                }
            }
        }

        partial void pEndAni(Animation ani)
        {
            if (this.vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null)
                {
                    var gModel = gVob.GetModel();
                    int id = gModel.GetAniIDFromAniName(ani.AniJob.Name);
                    var activeAni = gModel.GetActiveAni(id);
                    if (activeAni.Address == 0)
                        return;

                    if (activeAni.ModelAni.Layer == 1)
                    {
                        //if (((NPC)vob).EnvState != EnvironmentState.InAir) // gothic prob has already handled this anyway
                        {
                            if (((NPC)vob).Movement == NPCMovement.Forward)
                                gModel.StartAni(gVob.AniCtrl._s_walkl, 0);
                            else
                            {
                                gModel.StartAni(gVob.AniCtrl._s_walk, 0);
                            }
                        }
                    }
                    else
                    {
                        gModel.FadeOutAni(activeAni);
                    }
                }
            }
        }

        #endregion

        #region OnSpawn

        void Vob_OnSpawn(BaseVob vob, World world, Vec3f pos, Angles ang)
        {
            if (vob is NPC)
            {
                var gVob = ((NPC)vob).gVob;
                if (gVob != null && overlays != null)
                {
                    for (int i = 0; i < overlays.Count; i++)
                        gVob.ApplyOverlay(overlays[i].Name);
                }
            }
        }

        #endregion

        partial void pOnTick(long now)
        {
            if (!(this.vob is NPC))
                return;

            var gModel = ((NPC)vob).gModel;

            for (int i = 0; i < activeAnis.Count; i++)
            {
                var aa = activeAnis[i];
                if (aa.Ani == null)
                    continue;

                if (aa.AniJob.Layer == 1 && vob.Environment.InAir)
                    continue;

                int gAniID = gModel.GetAniIDFromAniName(aa.AniJob.Name);
                if (gModel.GetActiveAni(gAniID).Address == 0)
                {
                    gModel.StartAni(gAniID, 0);
                }

                /* moved to hModel.cs
                
                float startFrame = aa.Ani.StartFrame;
                float endFrame = aa.Ani.EndFrame;

                float percent = gAni.IsReversed ? (1 - aa.GetProgress()) : aa.GetProgress();

                float actFrame = startFrame + (endFrame - startFrame) * percent;
                gActiveAni.SetActFrame(actFrame);*/
            }
        }
    }
}
