using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GUC.Log;
using GUC.Utilities.FileSystem;

namespace GUC.Scripts.Sumpfkraut
{
    public class TestingThings : ExtendedObject
    {

        new public static readonly string _staticName = "TestingThings (static)";



        public static void Init ()
        {
            //Logger.Log(Directory.GetCurrentDirectory());

            //WorldSystem.WorldLoader worldLoader = new WorldSystem.WorldLoader(">>GOTCHA<<", 
            //    Directory.GetCurrentDirectory() + @"\DB");
            //worldLoader.Load();

            //string bla = "Miami Vice";
            //Logger.Log(bla);

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



        }

    }
}
