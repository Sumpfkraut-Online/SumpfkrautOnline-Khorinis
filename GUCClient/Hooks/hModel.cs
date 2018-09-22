using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApiNew;
using Gothic.Objects.Meshes;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    static class hModel
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited)
                return;
            inited = true;

            Process.AddFastHook(AdvanceAni, 0x57CE90, 0x7);
        }

        static void AdvanceAni(RegisterMemory mem)
        {
            var gModel = new zCModel(mem.EBX);
            var gActiveAni = new zCModelAniActive(mem.ESI);
            string aniName = gActiveAni.ModelAni.Name.ToString();

            if (World.Current.TryGetVobByAddress(gModel.Owner.Address, out Vob vob))
            {
                vob.Model.ForEachActiveAniPredicate(aa =>
                {
                    if (string.Equals(aa.AniJob.Name, aniName, StringComparison.OrdinalIgnoreCase))
                    {
                        // guc ani
                        if (!aa.IsIdleAni)
                        {
                            float startFrame = aa.Ani.StartFrame;
                            float endFrame = aa.Ani.EndFrame;

                            float percent = gActiveAni.ModelAni.IsReversed ? (1 - aa.GetProgress()) : aa.GetProgress();

                            float actFrame = startFrame + (endFrame - startFrame) * percent;
                            gActiveAni.SetActFrame(actFrame);
                        }
                        return false;
                    }
                    return true;
                });
            }
        }
    }
}
