using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Server.Scripts.Animations
{
    // class to represent animations
    public class AniInfo
    {
        /// <summary>
        /// How long the animation endures in ticks = 1/10000ms.
        /// </summary>
        public int Duration { get; protected set; }

        public AniInfo(int duration)
        {
            this.Duration = duration;
        }
    }

    // animation to store a time to fire an event
    public class AniEventInfo : AniInfo
    {
        public int EventTime { get; protected set; }

        public AniEventInfo(int eventTime, int duration)
            : base(duration)
        {
            this.EventTime = eventTime;
        }
    }

    // attack animation with duration, hitTime, comboTime
    public class AniAttackInfo : AniEventInfo
    {
        public int ComboTime { get; protected set; }

        public AniAttackInfo(int eventTime, int comboTime, int duration)
            : base(eventTime, duration)
        {
            this.ComboTime = comboTime;
        }
    }

    // class to represent animations grouped together by their use/job
    public class AniJob
    {
        // all possible overlays for this job
        Dictionary<byte, AniInfo> overlays;
        AniInfo baseInfo;

        public AniInfo GetInfo(List<byte> overlays)
        {
            for (int i = overlays.Count - 1; i >= 0; i--)
            {
                AniInfo info;
                if (this.overlays.TryGetValue(overlays[i], out info))
                {
                    return info;
                }
            }
            return baseInfo;
        }

        public AniJob(AniInfo baseInfo, Dictionary<byte, AniInfo> overlays)
        {
            this.overlays = overlays;
            this.baseInfo = baseInfo;
        }
    }

    public class AniCtrl
    {
        public class Attacks
        {
            AniJob[] forward;
            public AniJob this[int fwdCombo]
            {
                get { return (fwdCombo >= 0 && fwdCombo < forward.Length) ? forward[fwdCombo] : null; }
            }

            public AniJob Left { get; protected set; }
            public AniJob Right { get; protected set; }
            public AniJob Run { get; protected set; }
            public AniJob Parry { get; protected set; }
            public AniJob Dodge { get; protected set; }

            public Attacks(AniJob[] fwd, AniJob left, AniJob right, AniJob run, AniJob parry, AniJob dodge)
            {
                this.forward = fwd;
                this.Left = left;
                this.Right = right;
                this.Run = run;
                this.Parry = parry;
                this.Dodge = dodge;
            }
        }

        public Attacks Fist { get; protected set; }
        public Attacks OneHanded { get; protected set; }
        public Attacks TwoHanded { get; protected set; }

        Dictionary<string, AniJob> animations = new Dictionary<string, AniJob>();

        public AniJob GetJob(string aniName)
        {
            AniJob job;
            animations.TryGetValue(aniName.ToUpper(), out job);
            return job;
        }




        static Dictionary<string, AniCtrl> ctrls = null;
        public static AniCtrl Get(string mdsName)
        {
            AniCtrl result;
            ctrls.TryGetValue(mdsName.ToUpper(), out result);
            return result;
        }




        /*
         * 
         * 
         * Initialisation: Reading of animation time files
         * 
         * 
         */

        const string aniTimesPath = "Animations\\";
        const string aniOverlayTimesPath = "Animations\\Overlays\\";

        const float aniTimeMultiplier = 1.0f;

        public static void InitAnimations()
        {
            if (ctrls != null)
                return;

            Log.Logger.log("################ Initalise Animation-System ##############");

            ctrls = new Dictionary<string, AniCtrl>();

            string[] mdsfiles = Directory.GetFiles(aniTimesPath, "*.times");

            foreach (string filePath in mdsfiles)
            {

                AniCtrl ctrl = new AniCtrl();

                string mdsName = Path.GetFileNameWithoutExtension(filePath).ToUpper();

                using (StreamReader sr = new StreamReader(filePath)) //read main mds
                {
                    string[] overlayContents = GetOverlayContents(mdsName);

                    List<Tuple<string, string>> aniAliases = new List<Tuple<string, string>>();

                    // read the main MDS

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine().ToUpper();
                        string[] strs = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (strs.Length < 3)
                            continue;

                        string aniName = strs[1]; // Animation Name

                        try
                        {
                            AniInfo baseInfo;
                            Dictionary<byte, AniInfo> overlays;

                            switch (strs[0])
                            {
                                case "ANI": // Generic animation
                                    int duration = (int)(Convert.ToInt32(strs[2]) * aniTimeMultiplier);
                                    baseInfo = new AniInfo(duration);
                                    overlays = GetAniOverlays(aniName, overlayContents);
                                    break;
                                case "ATK": // Attack (Left, Right, RunAttack)
                                    baseInfo = GetAniAttack(line);
                                    overlays = GetAniAttackOverlays(aniName, overlayContents);
                                    break;
                                case "FWDATK": // Forward combo attack
                                    AniInfo[] fwdBaseInfos = GetFwdAttacks(line);
                                    Dictionary<byte, AniInfo[]> fwdOverlays = GetFwdAttackOverlays(aniName, overlayContents);

                                    // find the highest combo number
                                    int maximum = fwdBaseInfos.Length;
                                    foreach (AniInfo[] arr in fwdOverlays.Values)
                                        if (arr.Length > maximum)
                                            maximum = arr.Length;

                                    // add the combos as single animations
                                    for (byte i = 0; i < maximum; i++)
                                    {
                                        baseInfo = i < fwdBaseInfos.Length ? fwdBaseInfos[i] : null;
                                        overlays = new Dictionary<byte, AniInfo>();
                                        foreach (KeyValuePair<byte, AniInfo[]> pair in fwdOverlays)
                                        {
                                            if (i < pair.Value.Length)
                                                overlays.Add(pair.Key, pair.Value[i]);
                                        }

                                        ctrl.animations.Add(aniName + i, new AniJob(baseInfo, overlays));
                                    }

                                    // add the whole animation
                                    baseInfo = fwdBaseInfos[fwdBaseInfos.Length - 1];
                                    overlays = new Dictionary<byte, AniInfo>();
                                    foreach (KeyValuePair<byte, AniInfo[]> pair in fwdOverlays)
                                    {
                                        overlays.Add(pair.Key, pair.Value[pair.Value.Length - 1]);
                                    }
                                    continue;
                                case "ANIALIAS":
                                    aniAliases.Add(new Tuple<string, string>(strs[1], strs[2]));
                                    continue;
                                default:
                                    continue;
                            }

                            ctrl.animations.Add(aniName, new AniJob(baseInfo, overlays));
                        }
                        catch (Exception e)
                        {
                            Log.Logger.logWarning(mdsName + " " + aniName);
                            Log.Logger.logWarning(e.ToString());
                        }
                    }

                    // resolve aliases
                    foreach (Tuple<string, string> aliasTuple in aniAliases)
                    {
                        AniJob job = null;
                        ctrl.animations.TryGetValue(aliasTuple.Item2, out job);
                        if (job != null)
                        {
                            ctrl.animations.Add(aliasTuple.Item1, job);
                        }
                    }

                    // FIXME: Resolve attack references
                    // FIXME: Search overlays for new animations?

                }

                AniCtrl.ctrls.Add(mdsName, ctrl);

            }
        }

        static string[] GetOverlayContents(string mdsName)
        {
            List<string> contents = new List<string>();

            string enumName = "Overlays_" + mdsName;
            foreach (Type type in System.Reflection.Assembly.GetExecutingAssembly().GetTypes())
            {
                if (String.Equals(type.Namespace, "GUC.Server.Scripts.Animations", StringComparison.OrdinalIgnoreCase)
                 && String.Equals(type.Name, enumName, StringComparison.OrdinalIgnoreCase)
                 && type.IsEnum && Enum.GetUnderlyingType(type) == typeof(byte))
                {
                    foreach (string overlayName in Enum.GetNames(type))
                    {
                        string overlayFileName = aniOverlayTimesPath + overlayName + ".times";
                        if (File.Exists(overlayFileName))
                        {
                            contents.Add(File.ReadAllText(overlayFileName).ToUpper());
                        }
                        else
                        {
                            contents.Add("");
                        }
                    }
                    break;
                }
            }

            return contents.ToArray();
        }

        static Dictionary<byte, AniInfo> GetAniOverlays(string aniName, string[] overlayContents)
        {
            Dictionary<byte, AniInfo> overlays = new Dictionary<byte, AniInfo>();
            for (byte i = 0; i < overlayContents.Length; i++)
            {
                string text = overlayContents[i];
                if (text.Length == 0)
                    continue;

                int startIndex = text.IndexOf("ANI " + aniName);
                if (startIndex > 0)
                {
                    int endIndex = text.IndexOf('\n', startIndex);
                    string[] strs = text.Substring(startIndex, endIndex - startIndex).Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length == 3)
                    {
                        int duration = (int)(Convert.ToInt32(strs[2]) * aniTimeMultiplier);
                        //if (duration > 0)
                        {
                            overlays.Add(i, new AniInfo(duration));
                        }
                    }
                }
            }
            return overlays;
        }

        static AniInfo GetAniAttack(string line)
        {
            string[] strs = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            int hitTime = 0;
            int comboTime = 0;
            int duration = 0;
            for (int i = 2; i < strs.Length; i++)
            {
                string str = strs[i];
                string time = str.Substring(2, str.Length - 2);
                switch (str[0])
                {
                    case 'H':
                        hitTime = (int)(Convert.ToInt32(time)*aniTimeMultiplier);
                        break;
                    case 'C':
                        comboTime = (int)(Convert.ToInt32(time) * aniTimeMultiplier);
                        break;
                    case 'D':
                        duration = (int)(Convert.ToInt32(time) * aniTimeMultiplier);
                        break;
                }
            }
            return new AniAttackInfo(hitTime <= 0 ? duration : hitTime, comboTime <= 0 ? duration : comboTime, duration);
        }

        static Dictionary<byte, AniInfo> GetAniAttackOverlays(string aniName, string[] overlayContents)
        {
            Dictionary<byte, AniInfo> overlays = new Dictionary<byte, AniInfo>();
            for (byte i = 0; i < overlayContents.Length; i++)
            {
                string text = overlayContents[i];
                if (text.Length == 0)
                    continue;

                int startIndex = text.IndexOf(aniName);
                if (startIndex > 0)
                {
                    int endIndex = text.IndexOf('\n', startIndex);
                    overlays.Add(i, GetAniAttack(text.Substring(startIndex, endIndex - startIndex)));
                }
            }
            return overlays;
        }

        static AniInfo[] GetFwdAttacks(string line)
        {
            string[] strs = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            AniInfo[] infos = new AniInfo[(strs.Length - 2) / 3];
            for (int i = 0; i < infos.Length; i++)
            {
                infos[i] = GetAniAttack(String.Format("{0} {1} {2} {3} {4}\n", strs[0], strs[1], strs[i + 2], strs[i + 3], strs[i + 4]));
            }

            return infos;
        }

        static Dictionary<byte, AniInfo[]> GetFwdAttackOverlays(string aniName, string[] overlayContents)
        {
            Dictionary<byte, AniInfo[]> overlays = new Dictionary<byte, AniInfo[]>();
            for (byte i = 0; i < overlayContents.Length; i++)
            {
                string text = overlayContents[i];
                if (text.Length == 0)
                    continue;

                int startIndex = text.IndexOf("FWDATK " + aniName);
                if (startIndex > 0)
                {
                    int endIndex = text.IndexOf('\n', startIndex);
                    string line = text.Substring(startIndex, endIndex - startIndex);
                    string[] strs = line.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length >= 5)
                    {
                        overlays.Add(i, GetFwdAttacks(line));
                    }
                }
            }

            return overlays;
        }
    }
}
