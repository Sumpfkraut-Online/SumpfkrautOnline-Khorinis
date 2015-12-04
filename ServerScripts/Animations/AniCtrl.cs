using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Server.Scripts.Animations
{
    /// <summary> Basic animation info. </summary>
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

    /// <summary> Animation info with an additional value for timing an event. </summary>
    public class AniEventInfo : AniInfo
    {
        public int EventTime { get; protected set; }

        public AniEventInfo(int eventTime, int duration)
            : base(duration)
        {
            this.EventTime = eventTime;
        }
    }

    /// <summary> Attack animation info with Event/Hittime, ComboTime, Duration. </summary>
    public class AniAttackInfo : AniEventInfo
    {
        public int ComboTime { get; protected set; }

        public AniAttackInfo(int eventTime, int comboTime, int duration)
            : base(eventTime, duration)
        {
            this.ComboTime = comboTime;
        }
    }

    /// <summary> Class to represent an animation and its possible overlays. </summary>
    public class AniJob
    {
        // all possible overlays for this job
        Dictionary<byte, AniInfo> overlays;
        AniInfo baseInfo;

        /// <summary> Gets the AniInfo selected by given and available overlays. </summary>
        /// <param name="overlays"> What overlays are active / should be considered? </param>
        public AniInfo GetInfo(List<byte> overlays)
        {
            if (this.overlays != null && overlays != null && overlays.Count > 0)
            {
                for (int i = overlays.Count - 1; i >= 0; i--)
                {
                    AniInfo info;
                    if (this.overlays.TryGetValue(overlays[i], out info))
                    {
                        return info;
                    }
                }
            }
            return baseInfo;
        }

        public AniJob(AniInfo baseInfo, Dictionary<byte, AniInfo> overlays)
        {
            // save some performance
            if (overlays != null && overlays.Count > 0)
            {
                this.overlays = overlays;
            }
            else
            {
                this.overlays = null;
            }
            this.baseInfo = baseInfo;
        }
    }

    /// <summary> Collection of all possible animations and their durations of a MDS-Model. </summary>
    public class AniCtrl
    {
        /// <summary> Gets the AniEventInfo for the item pickup animation. EventTime = Time to pickup the item</summary>
        public AniEventInfo TakeItem { get; protected set; }
        /// <summary> Gets the AniEventInfo for the item drop animation. EventTime = Time to drop the item.</summary>
        public AniEventInfo DropItem { get; protected set; }
        //fuck overlays

        /// <summary> An AniJob collection for melee attacks. Use this[int i] to get the forward combos. </summary>
        public class Attacks
        {
            AniJob[] forward;

            /// <summary> Gets the forward combo Anijob of the given zero-based index or null. </summary>
            public AniJob this[int fwdCombo]
            {
                get { return (fwdCombo >= 0 && fwdCombo < forward.Length) ? forward[fwdCombo] : null; }
            }

            /// <summary> Gets the left attack Anijob. </summary>
            public AniJob Left { get; protected set; }

            /// <summary> Gets the right attack Anijob. </summary>
            public AniJob Right { get; protected set; }

            /// <summary> Gets the run attack Anijob. </summary>
            public AniJob Run { get; protected set; }

            /// <summary> Gets a parade Anijob. Note: This is an AniInfo, no AniAttackInfo. </summary>
            public AniJob Parry { get; protected set; }

            /// <summary> Gets the dodge / jump back Anijob. Note: This is an AniInfo, no AniAttackInfo. </summary>
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

        /// <summary> Collection for fist attacks. (Unarmed) monsters always use fists. </summary>
        public Attacks Fist { get; protected set; }
        /// <summary> Collection of attacks for one handed weapons. </summary>
        public Attacks OneHanded { get; protected set; }
        /// <summary> Collection of attacks for two handed weapons. </summary>
        public Attacks TwoHanded { get; protected set; }

        static Dictionary<string, string> AttackReferences = new Dictionary<string, string>() 
        { 
            // These will be used to search the right animations for the "Attacks"-Classes above
            // { VariableName, Gothic-Animationname }
            // f.e. "s_1hAttack" -> { "OneHanded", "1h" }
            { "Fist", "Fist" }, 
            { "OneHanded", "1H" },
            { "TwoHanded", "2H" } 
        };

        Dictionary<string, AniJob> animations = new Dictionary<string, AniJob>();
        /// <summary>Gets an AniJob by the gothic animation's name. (case insensitive)</summary>
        public AniJob GetJob(string aniName)
        {
            AniJob job;
            animations.TryGetValue(aniName.ToUpper(), out job);
            return job;
        }

        static Dictionary<string, AniCtrl> ctrls = null;
        /// <summary>Gets an AniCtrl by the gothic MDS-Model's name. (case insensitive)</summary>
        public static AniCtrl GetCtrl(string mdsName)
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

        const float aniTimeMultiplier = 1.10f; // Since the animations are ingame FPS-depending and somewhat slower ._.

        public static void InitAnimations()
        {
            if (ctrls != null)
                return;

            Log.Logger.log("############### Initialise Animation-System ##############");

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
                                    duration = 0;
                                    fwdBaseInfos.ToList().ForEach(i => duration += i.Duration);
                                    baseInfo = new AniInfo(duration);

                                    overlays = new Dictionary<byte, AniInfo>();
                                    foreach (KeyValuePair<byte, AniInfo[]> pair in fwdOverlays)
                                    {
                                        duration = 0;
                                        pair.Value.ToList().ForEach(i => duration += i.Duration);
                                        overlays.Add(pair.Key, new AniInfo(duration));
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

                    ResolveAniReferences(ctrl);
                    // FIXME: Search overlays for new animations?
                }
                AniCtrl.ctrls.Add(mdsName, ctrl);

                if (ctrl.Fist != null && ctrl.Fist[0] != null && ctrl.Fist[0].GetInfo(null) != null)
                    Console.WriteLine(mdsName + " ComboDuration: " + ctrl.Fist[0].GetInfo(null).Duration);
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

        static void ResolveAniReferences(AniCtrl ctrl)
        {
            for (int x = AttackReferences.Count-1; x >= 0; x--)
            {
                KeyValuePair<string, string> pair = AttackReferences.ElementAt(x);

                var attackField = typeof(AniCtrl).GetProperty(pair.Key);
                if (attackField == null)
                {
                    Log.Logger.logError("AniCtrl: AttackReference could not be found! " + pair.Key);
                    AttackReferences.Remove(pair.Key);
                    continue;
                }

                List<AniJob> FwdCombos = new List<AniJob>();
                AniJob Left = null;
                AniJob Right = null;
                AniJob Run = null;
                AniJob Parade = null;
                AniJob Dodge = null;

                // search forward combos
                for (int i = 0; i < 9999; i++)
                {
                    AniJob fwd = ctrl.GetJob(String.Format("s_{0}Attack{1}", pair.Value, i));
                    if (fwd == null)
                    {
                        break; // break as soon as there are no more combos
                    }
                    FwdCombos.Add(fwd);
                }

                Left = ctrl.GetJob(String.Format("t_{0}AttackL", pair.Value));
                Right = ctrl.GetJob(String.Format("t_{0}AttackR", pair.Value));
                Run = ctrl.GetJob(String.Format("t_{0}AttackMove", pair.Value));
                Parade = ctrl.GetJob(String.Format("t_{0}Parade_0", pair.Value)); // there are sometimes more several parade animations, but the times should all be the same
                Dodge = ctrl.GetJob(String.Format("t_{0}ParadeJumpB", pair.Value));

                attackField.SetValue(ctrl, new Attacks(FwdCombos.ToArray(), Left, Right, Run, Parade, Dodge), null);
            }

            ctrl.TakeItem = CombineAnis(ctrl, "c_Stand_2_IGet_1", "c_IGet_2_Stand_1"); // there are several pickup animations, but the times should all be the same
            ctrl.DropItem = CombineAnis(ctrl, "t_Stand_2_IDrop", "t_IDrop_2_Stand");
        }

        static AniEventInfo CombineAnis(AniCtrl ctrl, string ani1, string ani2)
        {
            AniEventInfo result = null;

            AniJob job = ctrl.GetJob(ani1); 
            if (job != null)
            {
                // base info
                int eventTime = 0;
                int duration = 0;

                AniInfo info = job.GetInfo(null);
                if (info != null)
                {
                    eventTime = info.Duration;
                    duration = info.Duration;
                }

                job = ctrl.GetJob(ani2);
                if (job != null)
                {
                    // base info
                    info = job.GetInfo(null);
                    if (info != null)
                    {
                        duration += info.Duration;
                    }
                }

                if (eventTime > 0 && duration > 0)
                {
                    result = new AniEventInfo(eventTime, duration);
                }
            }
            return result;
        }


    }
}
