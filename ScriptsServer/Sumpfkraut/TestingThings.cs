using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GUC.Log;
using GUC.Utilities.FileSystem;
using GUC.Scripts.Sumpfkraut.Database;
using Mono.Data.Sqlite;
using GUC.Scripts.Sumpfkraut.EffectSystem;
using System.Diagnostics;
using System.Reflection;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.EffectSystem.Destinations;
using System.Drawing;
using GUC.Scripts.Sumpfkraut.Utilities;
using System.Threading;
using GUC.Scripts.Sumpfkraut.Utilities.Functions;

namespace GUC.Scripts.Sumpfkraut
{
    public class TestingThings : ExtendedObject
    {

        public class O
        {
            
        }

        public class A : O
        {


            public A () { }
        }

        public class B : O
        {
            public B () { }
        }



        public static void Init ()
        {
            Logger.Log("****** TestingThings *************************************");

            //Logger.Log(Directory.GetCurrentDirectory());

            //string dbFilePath = Directory.GetCurrentDirectory() + @"\DB\someDB.db";
            //string dataSource = "data source=" + Directory.GetCurrentDirectory() + @"\DB\someDB.db";
            //Logger.Log("-------> " + dbFilePath);
            //SqliteConnection.CreateFile(Directory.GetCurrentDirectory() + @"\DB\someDB.db");

            //List<List<List<object>>> results = new List<List<List<object>>>();
            //DBReader.LoadFromDB(ref results, "SELECT 1;", dataSource);
            //Logger.Log(results[0][0][0]);

            //List<List<List<object>>> results = new List<List<List<object>>>();
            //DBReader.LoadFromDB(ref results,  @"DROP TABLE IF EXISTS WorldEffect;
            //    CREATE TABLE IF NOT EXISTS WorldEffect
            //    (
            //        WorldEffectID INTEGER NOT NULL,
            //        ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
            //        CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
            //        CONSTRAINT WorldEffect_PK PRIMARY KEY (WorldEffectID)
            //    );", dataSource);
            //Logger.Log(results[0][0][0]);

            //DBReader.SaveToDB("data source=" + Directory.GetCurrentDirectory() + @"\DB\someDB.db",
            //    @"DROP TABLE IF EXISTS WorldEffect;
            //    CREATE TABLE IF NOT EXISTS WorldEffect
            //    (
            //        WorldEffectID INTEGER NOT NULL,
            //        ChangeDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
            //        CreationDate DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
            //        CONSTRAINT WorldEffect_PK PRIMARY KEY (WorldEffectID)
            //    );");



            //string dataSource = string.Format("data source=\"{0}\"", 
            //    "\\DB\\someDB.db");

            //Log.Logger.Log(">>>>> " + dataSource);

            //List<List<List<object>>> results = new List<List<List<object>>>();
            //DBReader.LoadFromDB(ref results, "SELECT 1;", dataSource);

            //Mono.Data.Sqlite.SqliteConnectionStringBuilder csb = new Mono.Data.Sqlite.SqliteConnectionStringBuilder();
            //csb.DataSource = Directory.GetCurrentDirectory() + @"\DB\someDB.db";
            //Log.Logger.Log(">>>>> " + csb.ConnectionString);



            //WorldSystem.WorldLoader worldLoader = new WorldSystem.WorldLoader(">>GOTCHA<<",
            //    Directory.GetCurrentDirectory() + @"\DB\someDB.db");
            //worldLoader.Load();

            //Logger.Log(WorldSystem.WorldLoader.DBStructure["WorldEffect"]);

            //List<string> cmdList = new List<string> { "SELECT 1;" };
            //Database.DBAgent agentOrange = new Database.DBAgent("BLA.db", cmdList, false);
            //agentOrange.SetObjName("Agent Orange");
            //agentOrange.Start();

            //Logger.Log(Directory.Exists(@"Scripts"));
            //Logger.Log(File.Exists(@"Scripts\Newtonsoft.Json.xml"));


            //FileStream fs = File.Create(@"FileManagerTest.txt");
            //Database.DBFileManager.DeleteFile(@"FileManagerTest.txt",
            //    delegate (bool success)
            //    {
            //        if (success) { Logger.Log("<<<< HOOOOORRRRAAAYYYYYY! >>>"); }
            //        else { Logger.Log("<<<< MEEEEEEEEEEEHHHHHH! >>>"); }
            //    });

            //List<int> myList = new List<int> { 0, 1, 3 };
            //myList.Insert(3, 999);
            //foreach (int i in myList) { Logger.Log(i); };


            //List<int> myList = new List<int> { };
            ////List<int> myList = new List<int> { 0, 1, 2, 3, 4, 5 };
            //for (int i = myList.Count; i --> 0; )
            //{
            //    Logger.Log(i);
            //}


            //FileSystemManager fsManager = new FileSystemManager("", true, TimeSpan.MinValue, false);
            //fsManager.SetObjName("MrBusiness");
            //fsManager.printStateControls = true;

            //fsManager.MoveFile(@"FileManagerTest.txt",
            //    options: new List<object> { "FileManagerTest_GOTCHA.txt" },
            //    handler: delegate (ref FileSystemProtocol protocol,
            //   GUC.Utilities.FileSystem.Enumeration.ProtocolStatus status)
            //   {
            //       Logger.Log(">>>> Gonna try this... <<<<");
            //       if (status == GUC.Utilities.FileSystem.Enumeration.ProtocolStatus.FinalSuccess)
            //       {
            //           Logger.Log(">>>> Yaayyyy! <<<<");
            //       }
            //       if (status == GUC.Utilities.FileSystem.Enumeration.ProtocolStatus.FinalFail)
            //       {
            //           Logger.Log(">>>> Owwwwww! <<<<");
            //       }
            //       return;
            //   });




            //new EffectSystem.EffectHandlers.SomeEffectHandler("SomeEffectHandler", null);
            //new EffectSystem.EffectHandlers.AnotherEffectHandler("AnotherEffectHandler", null);
            //new EffectSystem.EffectHandlers.AnotherEffectHandler("AnotherEffectHandler", null);
            //new EffectSystem.EffectHandlers.AnotherEffectHandler("AnotherEffectHandler", null);

            //Logger.Log(EffectSystem.EffectHandlers.BaseEffectHandler.isInitialized 
            //    == EffectSystem.EffectHandlers.AnotherEffectHandler.isInitialized);
            //Logger.Log(EffectSystem.EffectHandlers.BaseEffectHandler.isInitialized);
            //Logger.Log(EffectSystem.EffectHandlers.SomeEffectHandler.isInitialized);
            //Logger.Log(EffectSystem.EffectHandlers.AnotherEffectHandler.isInitialized);


            //EffectSystem.EffectHandlers.VobEffectHandler eh =
            //    new EffectSystem.EffectHandlers.VobEffectHandler(null, new VobSystem.Instances.VobInst());
            //Logger.Log("-------> " + eh.hostType);

            //EffectSystem.EffectDelegateReference delRef = 
            //    new EffectSystem.EffectDelegateReference(null, null);



            //Change change1 = new Change(EffectSystem.Enumeration.ChangeType.Effect_Name_Set, new object[] { 1, 2, 3, 4, 5, 6 });
            //Change change2 = new Change(EffectSystem.Enumeration.ChangeType.Effect_Name_Set, new object[] { 1, 2, 3, 4, 5, 6 });
            //Logger.Log("### " + (change1 == change2));





            //Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            //long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            //Logger.Log("==> " + totalBytesOfMemoryUsed);

            //List<Change> changeList = new List<Change>();
            //for (int i = 0; i < 10000000; i++)
            //{
            //    changeList.Add(new Change(EffectSystem.Enumeration.ChangeType.Undefined, new object[0]));
            //    if ((i % 1000000) == 0)
            //    {
            //        Logger.Log("==> " + totalBytesOfMemoryUsed);
            //    }
            //}
            //changeList = null;
            //System.Threading.Thread.Sleep(1000);
            //Logger.Log("==> " + totalBytesOfMemoryUsed);

            //Effect effect = new Effect(null, new List<Change>() { new Change(), new Change() });
            //PropertyInfo info = effect.GetType().GetProperty("Changes");
            //List<Change> changes = (List<Change>) info.GetValue(effect, null);
            //Logger.Log("===> " + changes.Count);


            //Change change1 = new Change(null, EffectSystem.Enumeration.ChangeType.Undefined, 
            //    new object[] { 1, false, "mh?" }, new Type[] { typeof(int), typeof(bool), typeof(string) });
            //Change change2 = new Change(null, EffectSystem.Enumeration.ChangeType.Undefined, 
            //    new object[] { 2, true, "masdasdh?" }, new Type[] { typeof(int), typeof(bool), typeof(string) });
            //Change change3 = new Change(null, EffectSystem.Enumeration.ChangeType.Undefined, 
            //    new object[] { 1, false, "mh?" }, new Type[] { typeof(int), typeof(bool), typeof(string) });

            //List<Change> myList = new List<Change> { change1, change2, change3 };

            //myList.Remove(new Change(null, EffectSystem.Enumeration.ChangeType.Undefined,
            //    new object[] { 1, false, "mh?" }, new Type[] { typeof(int), typeof(bool), typeof(string) }));

            //foreach (Change c in myList)
            //{
            //    Logger.Log(c);
            //}


            //MAndM.Bla[1] = new Change(null, EffectSystem.Enumeration.ChangeType.Undefined,
            //    new object[1], new Type[] { typeof(object) });
            //Logger.Log(MAndM.Bla[1]);







            //Change_Effect_Name o = Change_Effect_Name.Create(null, EffectSystem.Enumeration.ChangeType.Undefined, new object[0]);
            //Logger.Log("===> " + (o == null));
            //Logger.Log("===> " + (o));





            //var bla = DestInit_Effect.representative;

            //Logger.Log("~~> changeTypeToDestinations <<<<<<<<<<<<<<<<<<<");
            //foreach (KeyValuePair<ChangeType, List<ChangeDestination>> keyVal
            //    in BaseEffectHandler.changeTypeToDestinations)
            //{
            //    Logger.Log("~~> " + keyVal.Key + ": " + keyVal.Value);
            //}

            //Logger.Log("~~> GetDestToCalcTotal <<<<<<<<<<<<<<<<<<<");
            //foreach (KeyValuePair<ChangeDestination, BaseEffectHandler.CalculateTotalChange> keyVal
            //    in BaseEffectHandler.GetDestToCalcTotal())
            //{
            //    Logger.Log("~~> " + keyVal.Key + ": " + keyVal.Value);
            //}

            //Logger.Log("~~> GetDestToApplyTotal <<<<<<<<<<<<<<<<<<<");
            //foreach (KeyValuePair<ChangeDestination, BaseEffectHandler.ApplyTotalChange> keyVal
            //    in BaseEffectHandler.GetDestToApplyTotal())
            //{
            //    Logger.Log("~~> " + keyVal.Key + ": " + keyVal.Value);
            //}






            //var timeSpans1 = new TimeSpan[]
            //{
            //    new TimeSpan(0, 0, 0, 0, 500),
            //    new TimeSpan(0, 0, 0, 1, 0),
            //    new TimeSpan(0, 0, 0, 2, 0)
            //};

            //var tf1 = new TimedFunction(timeSpans1, new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now.AddSeconds(10)));
            //tf1.SetFunc((object[] param) =>
            //{
            //    return param;
            //});
            //var tf2 = new TimedFunction(timeSpans1, new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now.AddSeconds(10)));
            //tf1.SetFunc((object[] param) =>
            //{
            //    return param;
            //});
            //var tf3 = tf2;

            //Logger.Print(tf1 == tf2);
            //Logger.Print(tf2 == tf3);
            //Logger.Print(tf1.HasEqualAttributes(tf3));

            //var dict = new Dictionary<TimedFunction, int>();


            //var arr1 = new int[] { 1, 2, 3, 4, 5 };
            //var arr2 = new int[] { 1, 2, 3, 4, 5 };

            //Logger.Print(arr1 == arr2);
            //Logger.Print(arr1.Equals(arr2));






            var fm = new FunctionManager();
            fm.Start();


            var startTime = (DateTime.Now);
            Logger.Print("STTTTAAAAAAARRRTT: " + startTime);
            var sw = new Stopwatch();
            sw.Start();

            var specTimes = new DateTime[]
            {
                startTime.AddMilliseconds(1000),    // 0
                startTime.AddMilliseconds(5000),    // 1
                startTime.AddMilliseconds(10000),   // 2
            };

            var intervals = new TimeSpan[]
            {
                new TimeSpan(0, 0, 0, 1, 0),
                new TimeSpan(0, 0, 0, 3, 0),
                new TimeSpan(0, 0, 0, 5, 0)
            };

            Func<object[], object[]> func1 = (object[] param) =>
            {
                Logger.Print("Interval... " + sw.ElapsedMilliseconds);
                return param;
            };

            Func<object[], object[]> func2 = (object[] param) =>
            {
                Logger.Print("SpecTime... " + sw.ElapsedMilliseconds);
                return param;
            };

            var tf1 = new TimedFunction(intervals, new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now.AddSeconds(10)));
            tf1.SetFunc(func1);
            tf1.SetMaxInvocations(5);
            tf1.SetPreserveDueInvocations(true);
            fm.Add(tf1, 1, true);

            //var tf2 = new TimedFunction(specTimes, new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now.AddSeconds(20)));
            //tf2.SetFunc(func2);
            //tf2.SetMaxInvocations(5);
            //tf2.SetPreserveDueInvocations(true);
            //fm.Add(tf2, 1, true);

            Program.OnTick += fm.Run;








            //ChangeInitInfo info;
            //BaseChangeInit.TryGetChangeInitInfo(ChangeType.Effect_Name_Set, out info);
            //Effect e = new Effect(null);
            //Change c = Change.Create(info, e, new List<object>() { "MyEffect" });
            //VobEffectHandler eh = new VobEffectHandler("MyEffectHandler", null, new VobSystem.Definitions.VobDef());
            //Logger.Log(e.EffectName); 

            //var effectLoader = new EffectLoader(@"Data Source=DB\TEST_Main_01.db", "DefEffect", "DefChange");
            //effectLoader.Load(true, (EffectLoader.FinishedLoadingEffectsArgs e) => 
            //{
            //    if (e.effectsByID != null)
            //    {
            //        foreach (var keyVal in e.effectsByID)
            //        {
            //            Log.Logger.Log("~~~~~~~~> " + keyVal.Key + ": " + keyVal.Value.GetGlobalID());
            //        }
            //    }
            //});






            Logger.Log("===> !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

    }

    

}