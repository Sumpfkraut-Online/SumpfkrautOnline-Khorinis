using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Server.WorldObjects
{
    internal class AnimationControl
    {
        public static Dictionary<string, AnimationControl> dict;
        public static void Init()
        {
            dict = new Dictionary<string, AnimationControl>();

            AnimationControl ani = new AnimationControl();
            ani.Fist.Forward[0].Attack = 8100000;
            ani.Fist.Forward[0].Combo = 4050000;
            ani.Fist.Forward[0].Hit = 3000000;

            ani.Fist.Forward[1].Attack = 8300000;
            ani.Fist.Forward[1].Combo = 4150000;
            ani.Fist.Forward[1].Hit = 3000000;

            ani.Fist.Parry.Attack = 5400000; // inaccurate

            ani.Fist.Dodge.Attack = 4100000;

            ani.Fist.Run.Attack = 13800000;
            ani.Fist.Run.Hit = 9000000;

            ani._1H[0].Forward[0].Attack = 8600000;
            ani._1H[0].Forward[0].Combo = 5000000;
            ani._1H[0].Forward[0].Hit = 3000000;

            ani._1H[0].Forward[1].Attack = 6000000;
            ani._1H[0].Forward[1].Combo = 4000000;
            ani._1H[0].Forward[1].Hit = 3000000;

            ani._1H[0].Right.Attack = 12800000;
            ani._1H[0].Right.Combo = 8000000;
            ani._1H[0].Right.Hit = 4000000;

            ani._1H[0].Left.Attack = 12800000;
            ani._1H[0].Left.Combo = 8000000;
            ani._1H[0].Left.Hit = 4000000;

            ani._1H[0].Parry.Attack = 5250000;
            ani._1H[0].Dodge.Attack = 4900000;
            ani._1H[0].Run.Attack = 13700000;
            ani._1H[0].Run.Hit = 9000000;

            dict.Add("HUMANS", ani);

            ani = new AnimationControl();
            dict.Add("SCAVENGER", ani);

            //...
        }

        public struct Attacks
        {
            public struct Info
            {
                public int Attack;
                public int Combo;
                public int Hit;
            }
            public Info[] Forward;
            public Info Left;
            public Info Right;
            public Info Run;
            public Info Parry;
            public Info Dodge;

            int comboNum;
            public int ComboNum { get { return comboNum; } }

            public Attacks(int comboNum)
            {
                this.comboNum = comboNum;
                Forward = new Info[comboNum];
                Left = new Info();
                Right = new Info();
                Run = new Info();
                Parry = new Info();
                Dodge = new Info();
            }
        }

        public Attacks Fist;
        public Attacks[] _1H;
        public Attacks[] _2H;

        public AnimationControl()
        {
            Fist = new Attacks(2);
            _1H = new Attacks[3] { new Attacks(2), new Attacks(4), new Attacks(4) };
            _2H = new Attacks[3] { new Attacks(2), new Attacks(4), new Attacks(4) };
        }
    }
}
