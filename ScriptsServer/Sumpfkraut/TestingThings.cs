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

namespace GUC.Scripts.Sumpfkraut
{
    public class TestingThings : ExtendedObject
    {

        new public static readonly string _staticName = "TestingThings (static)";



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
            //Logger.Log("-------> " + eh.LinkedObjectType);

            //EffectSystem.EffectDelegateReference delRef = 
            //    new EffectSystem.EffectDelegateReference(null, null);



            //Change change1 = new Change(EffectSystem.Enumeration.ChangeType.Effect_Name_Set, new object[] { 1, 2, 3, 4, 5, 6 });
            //Change change2 = new Change(EffectSystem.Enumeration.ChangeType.Effect_Name_Set, new object[] { 1, 2, 3, 4, 5, 6 });
            //Logger.Log("### " + (change1 == change2));

            //bool check;
            //int[] lapsesSteps = new int[] { 1, 10, 100, 1000, 1000, 10000, 100000, 1000000 };
            //int tempLapses = 0;
            //List<long> elapsedTicks = new List<long>();

            //Stopwatch sw = Stopwatch.StartNew();
            //for (int s = 0; s < lapsesSteps.Length; s++)
            //{
            //    tempLapses = lapsesSteps[s];
            //    for (int l = 0; l < tempLapses; l++)
            //    {
            //        check = change1 == change2;
            //    }
            //    sw.Stop();
            //    elapsedTicks.Add(sw.ElapsedTicks);
            //    sw.Restart();
            //}

            //sw.Stop();
            //for (int i = 0; i < elapsedTicks.Count; i++)
            //{
            //    Logger.Log("### " + elapsedTicks[i] + " " + ((double) elapsedTicks[i] / TimeSpan.TicksPerMillisecond));
            //}



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



            //List<int> l1 = new List<int>() { 1, 3, 6, 7, 8 };
            //List<int> l2 = new List<int>() { 2, 3, 4, 5, 6, 7, 9, 10};
            //List<int> l3 = l1.Union(l2).ToList();
            //l3.Sort();
            //foreach (int i in l3) { Logger.Log(i); }



            ////SomeClass o = new SomeClass(new object[0]);
            //SomeClass o = SomeClass.Create(new object[0]);
            //Logger.Log("===> " + (o == null));
            //Logger.Log("===> " + o);
            ////Logger.Log("===> " + o.parameters);


            //object o = "Banannanana";
            //Logger.Log("===> " + o.GetType());

            //SomeClass.ParameterTypeCheck(new object[] { "s", 'c', 1, false });


            Change_Effect_Name o = Change_Effect_Name.Create(null, EffectSystem.Enumeration.ChangeType.Undefined, new object[0]);
            Logger.Log("===> " + (o));
            


            Logger.Log("===> !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        }

    }

    //public class MAndM
    //{
    //    protected static Dictionary<int, Change> bla = new Dictionary<int, Change>();
    //    public static Dictionary<int, Change> Bla { get { return bla; } }
    //}

    public class SomeClass
    {
        public object[] parameters;

        public SomeClass (object[] parameters)
        {
            this.parameters = parameters;
            if (parameters.Length < 1)
            {
                throw new Exception("parameters of Length < 1 are invalid!");
            }
        }

        public static SomeClass Create (object[] parameters)
        {
            if (parameters.Length < 1) { return null; }
            return new SomeClass(parameters);
        }

        public static void ParameterTypeCheck (object[] parameters)
        {
            foreach (object o in parameters) { Logger.Log("===> " + o.GetType()); }
        }
    }

    public class AnotherClass : SomeClass
    {
        public AnotherClass (object[] parameters) : base(parameters)
        { }
    }

}