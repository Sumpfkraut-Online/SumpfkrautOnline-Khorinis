using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Log;
using System.Diagnostics;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    static class AniParser
    {
        public static void ReadMDSFiles()
        {
            DirectoryInfo info = new DirectoryInfo("anims");
            if (info.Exists)
            {
                var files = info.GetFiles("*.MDS");
                for (int i = 0; i < files.Length; i++)
                {
                    ParseMDS(files[i], false);
                }
            }
            else
            {
                Logger.Log("No anim files found to parse.");
            }

            info = new DirectoryInfo("anims\\overlays");
            if (info.Exists)
            {
                var files = info.GetFiles("*.MDS");
                for (int i = 0; i < files.Length; i++)
                {
                    ParseMDS(files[i], false);
                }
            }
            else
            {
                Logger.Log("No overlay files found to parse.");
            }
        }

        /* NOTE: Anis with EndFrame == -1 will be ignored!!!
        *
        *
        */

        static readonly char[] seperators = new char[] { ' ', ' ', '	', '(', ')' };

        public static void ParseMDS(FileInfo fileInfo, bool isOverlay = false)
        {
            if (isOverlay)
                return;

            try
            {
                if (fileInfo == null)
                {
                    Logger.LogError("AniParser: FileInfo is null!");
                    return;
                }

                if (!fileInfo.Exists)
                {
                    Logger.LogError("AniParser: {0} does not exist!", fileInfo.FullName);
                    return;
                }

                if (string.Compare(fileInfo.Extension, ".mds", true) != 0)
                {
                    Logger.LogError("AniParser: {0} is no MDS file!", fileInfo.FullName);
                    return;
                }

                string name = Path.GetFileNameWithoutExtension(fileInfo.Name);

                int newAnis = 0;
                int newJobs = 0;

                ModelDef modelDef = new ModelDef(name, fileInfo.Name);
                modelDef.SetAniCatalog(new Visuals.AniCatalogs.NPCCatalog());
                ScriptOverlay overlay = null;

                using (StreamReader sr = new StreamReader(fileInfo.OpenRead()))
                {
                    string line = null;
                    try
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] strs = line.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                            if (strs.Length < 9)
                                continue;

                            string type = strs[0].Trim(seperators);
                            if (string.Compare(type, "ani", true) == 0)
                            {
                                ReadAni(modelDef, overlay, strs, ref newAnis, ref newJobs);
                            }
                            else if (string.Compare(type, "aniAlias", true) == 0)
                            {
                                ReadAniAlias(modelDef, overlay, strs, ref newAnis, ref newJobs);
                            }
                        }

                        modelDef.Create();
                    }
                    catch (Exception e)
                    {
                        File.WriteAllText("lastline.txt", line == null ? "" : line);
                        Logger.Log(e);
                    }
                }

                if (isOverlay)
                {
                    Logger.Log("AniParser: Overlay '{0}': {1} jobs and {2} anis parsed.", overlay.CodeName, newJobs, newAnis);
                }
                else
                {
                    Logger.Log("AniParser: ModelDef '{0}': {1} jobs and {2} anis parsed.", modelDef.CodeName, newJobs, newAnis);
                }
            }
            catch (Exception e)
            {
                Logger.Log("AniParser: {0} excepted!", fileInfo.FullName);
                Logger.Log(e);
            }
        }

        static void ReadAni(ModelDef modelDef, ScriptOverlay overlay, string[] strs, ref int newAnis, ref int newJobs)
        {
            string aniName = ExtractString(strs[1]);

            int layer;
            if (!int.TryParse(strs[2], out layer))
                return;

            string nextAni = ExtractString(strs[3]);

            int startFrame;
            if (!int.TryParse(strs[9], out startFrame))
                return;

            int endFrame;
            if (!int.TryParse(strs[10], out endFrame) || endFrame < 0)
                return;

            float fps = 25;
            if (strs.Length > 11)
            {
                float.TryParse(strs[11].Substring("FPS:".Length), out fps);
            }

            ScriptAniJob aniJob;
            if (!modelDef.TryGetAniJob(aniName, out aniJob))
            {
                aniJob = new ScriptAniJob(aniName, aniName);
                if (!modelDef.Catalog.ContainsPropertyForJob(aniJob))
                    return;

                modelDef.AddAniJob(aniJob);
                newJobs++;
            }

            ScriptAni ani = new ScriptAni(startFrame, endFrame);
            ani.FPS = fps;
            ani.Layer = layer;
            if (overlay == null)
            {
                aniJob.SetDefaultAni(ani);
            }
            else
            {
                aniJob.AddOverlayAni(ani, overlay);
            }
            newAnis++;
        }

        static void ReadAniAlias(ModelDef modelDef, ScriptOverlay overlay, string[] strs, ref int newAnis, ref int newJobs)
        {
            //newJobs++;
            //newAnis++;
        }

        static string ExtractString(string str)
        {
            int firstIndex = str.IndexOf('"') + 1;
            if (firstIndex == 0) return null;

            int lastIndex = str.IndexOf('"', firstIndex);
            if (lastIndex == -1) return null;

            return str.Substring(firstIndex, lastIndex - firstIndex);
        }
    }
}
