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




            new EffectSystem.EffectHandlers.SomeEffectHandler("SomeEffectHandler", null);
            new EffectSystem.EffectHandlers.AnotherEffectHandler("AnotherEffectHandler", null);
            new EffectSystem.EffectHandlers.AnotherEffectHandler("AnotherEffectHandler", null);
            new EffectSystem.EffectHandlers.AnotherEffectHandler("AnotherEffectHandler", null);

            //Logger.Log(EffectSystem.EffectHandlers.BaseEffectHandler.isInitialized 
            //    == EffectSystem.EffectHandlers.AnotherEffectHandler.isInitialized);
            //Logger.Log(EffectSystem.EffectHandlers.BaseEffectHandler.isInitialized);
            //Logger.Log(EffectSystem.EffectHandlers.SomeEffectHandler.isInitialized);
            //Logger.Log(EffectSystem.EffectHandlers.AnotherEffectHandler.isInitialized);

        }

    }
}
