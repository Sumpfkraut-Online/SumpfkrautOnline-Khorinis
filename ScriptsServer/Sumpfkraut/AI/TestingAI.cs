using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GUC.Scripts.Sumpfkraut.AI
{

    public class TestingAI : ExtendedObject
    {

        new public static readonly string _staticName = "TestingAI (static)";

        public static void Test ()
        {
            //Stopwatch outerSW = new Stopwatch();

            //int i = 0;
            //int lapses = 1000000;

            //int tempInt;
            //object tempObject;
            //SimpleAI.AIAgent tempAIAgent;

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    tempInt = 999;
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds);

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    tempObject = new object();
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds);

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    tempAIAgent = new SimpleAI.AIAgent();
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds);


            //Stopwatch outerSW = new Stopwatch();
            //int lapses = 1000000;
            //int i = 0;

            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds
            //    + "ms / lapse" + ((double) outerSW.ElapsedMilliseconds / lapses));

            //object lockObj = new object();
            //i = 0;
            //outerSW.Restart();
            //while (i < lapses)
            //{
            //    lock (lockObj) { }
            //    i++;
            //}
            //outerSW.Stop();
            //PrintStatic(typeof(TestingAI), "ticks: " + outerSW.ElapsedTicks + ", ms: " + outerSW.ElapsedMilliseconds
            //    + "ms / lapse" + ((double) outerSW.ElapsedMilliseconds / lapses));


            //SimpleAI.AIAgent agentBlack = new SimpleAI.AIAgent();
            //object bla = agentBlack.AIPersonality.Bla;
            //bla = null;
            //Log.Logger.Log(">>>> " + bla);

        }

    }

}
