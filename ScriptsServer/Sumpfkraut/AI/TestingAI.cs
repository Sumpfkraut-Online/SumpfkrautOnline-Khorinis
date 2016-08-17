using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

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





            // single threaded AIManager-handling
            AIManager aiManager01 = new AIManager(false, false, new TimeSpan(0, 0, 0, 0, 10));
            aiManager01.SetObjName("aiManager01");
            aiManager01.Start();

            NPCDef npcDef01 = new NPCDef();
            npcDef01.SetObjName("npcDef01");
            npcDef01.Name = "npcDef01";
            npcDef01.Create();
            NPCInst npcInst01 = new NPCInst(npcDef01);
            npcInst01.SetObjName("npcInst01");
            npcInst01.Fatness = 2.0f;
            npcInst01.Spawn(WorldSystem.WorldInst.Current);

            AIAgent aiAgent01 = new AIAgent(new List<VobInst> { npcInst01 });
            aiAgent01.SetObjName("aiAgent01");
            aiManager01.SubscribeAIAgent(aiAgent01);
        }

    }

}
