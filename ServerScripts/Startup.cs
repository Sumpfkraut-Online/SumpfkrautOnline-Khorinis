using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Server.WorldObjects;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripts.Sumpfkraut.VobSystem;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Server.Scripts.Sumpfkraut;
using GUC.Server.Scripts.Sumpfkraut.Database;
using GUC.Server.Scripts.Sumpfkraut.CommandConsole;
using GUC.Server.Scripts.Sumpfkraut.Database.DBQuerying;
using System.Text.RegularExpressions;

namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{

        //public delegate void MyEventHandler (Sumpfkraut.Utilities.Threading.Runnable runnable);
        //public MyEventHandler MyEvent;

        //public delegate void OnSomething (params object[] args);

        public void OnServerInit()
		{
            Log.Logger.log("######################## Initalise ########################");

            Animations.AniCtrl.InitAnimations();

            //CommandConsole cmdConsole = new CommandConsole();


            //Type bla = typeof(ScriptObject);
            ////Console.WriteLine(bla.GetProperty("_staticName"));
            ////Console.WriteLine(bla.GetProperty("_staticName").GetGetMethod());
            //Console.WriteLine(bla.GetMembers().Length);
            //Console.WriteLine(bla.GetMember("_staticName").Length);
            //Console.WriteLine(bla.GetMember("_staticName").GetValue(0));
            //System.Reflection.MemberInfo[] mInfo = bla.GetMember("_staticName");

            //for (int i = 0; i < mInfo.Length; i++)
            //{

            //    Console.WriteLine(mInfo[i]);
            //}

            //object temp;
            //int i = 0;
            //while (true)
            //{
            //    try
            //    {
            //        temp = mInfo.GetValue(i);
            //        Console.WriteLine(temp.GetType());
            //        Console.WriteLine(temp);
            //        Console.WriteLine("--------------------------");
            //    } catch
            //    {
            //        break;
            //    }
            //    i++;
            //}


            //Type type = typeof(ScriptObject); // MyClass is static class with static properties
            //foreach (var p in type.GetProperties())
            //{
            //    var v = p.GetValue(null, null); // static classes cannot be instanced, so use null...
            //    Console.WriteLine(v.GetType());
            //    Console.WriteLine(v);
            //    Console.WriteLine("--------------------------");
            //}

            //Type t = typeof(ScriptObject);
            //System.Reflection.PropertyInfo prop = t.GetProperty("_staticName");
            //object value = prop.GetValue(null, null);
            //Console.WriteLine(value);

            //Console.WriteLine(t.GetField("_staticName"));
            //Console.WriteLine(t.GetField("_staticName").GetValue(""));
            //Console.WriteLine(typeof(ScriptObject).GetField("_staticName").GetValue(""));
            //Console.WriteLine(typeof(VobHandler).GetField("_staticName").GetValue(""));

            //VobDef def = new VobDef();
            //def.Print("Hello World!");

            //VobDef.PrintStatic(typeof(VobHandler), "GOTCHA!");


            //ItemDef.MakeLogErrorStatic(typeof(ItemDef), 
            //    String.Format("Prevented attempt of adding a definition to the dictionaries:"
            //        + " The {0} {1} is already taken.", "id", 9999));

            //Func<bool, bool, bool> check = (x, y) => x && y;
            //Console.WriteLine(check.Invoke(true, false));

            //ItemDef def0 = new ItemDef(); def0.setId(0); ItemDef.Add(def0);
            //ItemDef def1 = new ItemDef(); def1.setId(1); ItemDef.Add(def1);
            //ItemDef def2 = new ItemDef(); def2.setId(2); ItemDef.Add(def2);
            //ItemDef def2_ = new ItemDef(); def2_.setId(2); ItemDef.Add(def2_);

            //ItemInstance ii = new ItemInstance("Schmarn");
            //ii.Name = "Schmarn (Ya mei!)";
            //ii.Material = Enumeration.ItemMaterial.Metal;
            //ii.Type = Enumeration.ItemType.Food_Small;
            //ii.Weight = 99;
            //ii.Description = String.Format("Lorem ipsum dolor sit amet, consectetur adipiscing elit. "
            //    +"Sed faucibus magna sem, at lobortis magna dignissim ac. " 
            //    + "Suspendisse vitae augue ultrices velit suscipit pellentesque et quis ligula. "
            //    + "Duis vitae pharetra nisl. Aenean id sollicitudin ligula, ut mattis arcu. "
            //    + "Maecenas varius euismod ornare. Etiam id leo sodales, facilisis leo ac, "
            //    + "tincidunt enim. Nulla eget efficitur sapien, eu egestas dolor. Nullam fermentum "
            //    + "tincidunt massa, non tempus magna lacinia a. In feugiat vel risus ac vestibulum.");
            //ii.Visual = "ItFo_FishSoup.3ds";
            //Item item = new Item(ii);
            //item.Amount = 9001;
            //item.Position = new GUC.Types.Vec3f(0.0f, 0.0f, 0.0f);
            ////item.CDDyn = false;
            ////item.CDStatic = true;
            ////World.NewWorld.ItemDict.Add(1, item);
            ////item.Spawn(World.NewWorld);
            //item.Drop(World.NewWorld, item.Position);

            //IGTime newTime = new IGTime();
            //newTime.day = 4; newTime.hour = 22; newTime.minute = 30;
            //Console.WriteLine(">>>> " + newTime.day + " " + newTime.hour + " " + newTime.minute);
            //World.NewWorld.ChangeTime(newTime.day, newTime.hour, newTime.minute);
            //Console.WriteLine(String.Format(">>> CHANGED TIME to day {0} {1}:{2}<<<",
            //    World.NewWorld.GetIGTime().day, 
            //    World.NewWorld.GetIGTime().hour, 
            //    World.NewWorld.GetIGTime().minute));


            //Sumpfkraut.Utilities.Threading.TestRunnable timeRunner =
            //    new Sumpfkraut.Utilities.Threading.TestRunnable(false, new TimeSpan(0, 0, 2), false);
            //timeRunner.SetObjName("timeRunner");
            //timeRunner.printStateControls = true;
            //timeRunner.Start();


            //Sumpfkraut.WeatherSystem.Weather weather_1 =
            //    new Sumpfkraut.WeatherSystem.Weather(false, "MyWeather");
            //weather_1.Start();


            //Dictionary<int, VobDef> myDict = new Dictionary<int, VobDef>();
            //ItemDef myDef = new ItemDef();
            //myDef.setEffect("myEffect");
            //myDict.Add(1, myDef);
            //ItemDef myDef_ = (ItemDef)myDict[1];
            //Console.WriteLine(">>> " + myDef_.getEffect());


            //ItemDef def1 = new ItemDef();
            //def1.SetId(1);
            //def1.SetCodeName("myCodeName");
            //def1.SetEffect("myEffect");
            //ItemDef.Add(def1);
            //ItemDef def1_;
            //if (ItemDef.TryGetValueById(1, out def1_))
            //{
            //    ItemDef.MakeLogStatic(typeof(ItemDef), "GOTCHA!");
            //    ItemDef.MakeLogStatic(typeof(ItemDef), def1 == def1_);
            //    ItemDef.MakeLogStatic(typeof(ItemDef), def1_.GetEffect());
            //}

            //List<String> commandQueue = new List<string> { "SELECT 1; SELECT 11; ", "SELECT 2;", "SELECT 3;" };
            //Sumpfkraut.Database.DBAgent agentBlack = new Sumpfkraut.Database.DBAgent(commandQueue, false);
            //agentBlack.ReceivedResults += delegate(object sender, 
            //    Sumpfkraut.Database.DBAgent.ReceivedResultsEventArgs e) 
            //{
            //    List<List<List<object>>> results = e.GetResults();
            //    Console.WriteLine("+++ " + results[0][0][0]);
            //};
            //agentBlack.FinishedQueue += delegate (object sender)
            //{
            //    Console.WriteLine("+++ FINISHED");
            //};

            //Sumpfkraut.Utilities.Threading.Runnable homeRun =
            //    new Sumpfkraut.Utilities.Threading.Runnable(false, new TimeSpan(0, 0, 0, 1), false);
            //homeRun.printStateControls = true;
            //homeRun.OnInit += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable) { Console.WriteLine("Inuit 1"); };
            ////homeRun.OnInit += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable) { Console.WriteLine("Inuit 2"); };
            //homeRun.OnRun += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable) { Console.WriteLine("Task 1"); };
            ////homeRun.OnRun += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable) { Console.WriteLine("Task 2"); };
            //homeRun.OnRun += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable)
            //{
            //    runnable.Print("-----> " + (DateTime.Now.Second % 10));
            //    if ((DateTime.Now.Second % 10) == 0)
            //    {
            //        runnable.Suspend();
            //    }
            //};
            //homeRun.Start();
            ////homeRun.Suspend();



            //Sumpfkraut.Utilities.Threading.Runnable homeRun =
            //    new Sumpfkraut.Utilities.Threading.Runnable(false, new TimeSpan(0, 0, 0, 0, 200), false);
            //homeRun.printStateControls = true;
            //homeRun.OnRun += delegate (Sumpfkraut.Utilities.Threading.Runnable sender)
            //{
            //    Console.WriteLine(DateTime.Now);
            //    sender.Suspend();
            //};
            //MyEvent += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable) { runnable.Resume(); };
            //for (int i = 0; i < 20; i++)
            //{
            //    Console.WriteLine("--> " + i);
            //    MyEvent.Invoke(homeRun);
            //}
            //homeRun.Start();
            //homeRun.Suspend();


            //Sumpfkraut.Utilities.Threading.TestRunnable myRun =
            //    new Sumpfkraut.Utilities.Threading.TestRunnable(false, new TimeSpan(0, 0, 1), false);
            ////myRun.OnRun += delegate (Sumpfkraut.Utilities.Threading.Runnable runnable) 
            ////{
            ////    Sumpfkraut.Utilities.Threading.TestRunnable _runnable = 
            ////        (Sumpfkraut.Utilities.Threading.TestRunnable) runnable;
            ////    //_runnable.TestEvent.Invoke(DateTime.Now);
            ////};
            //myRun.TestEvent += delegate (DateTime dt) { Console.WriteLine("~~~> " + dt); };
            //myRun.Start();



            //DateTime myResults_1 = DateTime.Now.AddHours(2);
            //int myResults_2 = -1;
            //Sumpfkraut.Utilities.Threading.TestRunnable homeRun =
            //    new Sumpfkraut.Utilities.Threading.TestRunnable(false, new TimeSpan(0, 0, 1), true);
            //homeRun.printStateControls = true;
            //System.Threading.AutoResetEvent waitHandle = new System.Threading.AutoResetEvent(false);
            //homeRun.waitHandle = waitHandle;
            //homeRun.OnRun += delegate (Sumpfkraut.Utilities.Threading.Runnable sender)
            //{
            //    int bla;
            //    for (int i = 0; i < 999999; i++)
            //    {
            //        bla = i - 999999;
            //    }
            //    myResults_1 = DateTime.Now;
            //    myResults_2 = 999;
            //};
            //homeRun.Start();
            //waitHandle.WaitOne();
            //Console.WriteLine("~~~> " + myResults_1 + " " + myResults_2);




            //DBQueryHandler queryHandler = new DBQueryHandler("Data Source=save.db");

            //List<List<List<object>>> sqlResults = new List<List<List<object>>>();
            //Action<List<List<List<object>>>> receiveResults =
            //    new Action<List<List<List<object>>>>(delegate (List<List<List<object>>> results)
            //    {
            //        Console.WriteLine(">> Received results! Oh my, oh my-!");

            //        if ((results == null) || (results.Count < 1))
            //        {
            //            return;
            //        }
            //        for (int res = 0; res < results.Count; res++)
            //        {
            //            Console.WriteLine(">> " + res);
            //            for (int row = 0; row < results[res].Count; row++)
            //            {
            //                Console.WriteLine(">>>> " + row);
            //                for (int col = 0; col < results[res][row].Count; col++)
            //                {
            //                    Console.WriteLine(">>>>>> " + results[res][row][col]);
            //                }
            //            }
            //        }
            //    });

            //Sumpfkraut.Database.DBQuerying.DBQuery query_1 =
            //    new Sumpfkraut.Database.DBQuerying.DBQuery("SELECT 111; SELECT 222; SELECT 333;",
            //        DBReaderMode.loadData, receiveResults);

            //queryHandler.Add(query_1);

            //Logger.log(Math.Sign(1));
            //Logger.log(Math.Sign(-1));
            //Logger.log(Math.Sign(889));
            //Logger.log(Math.Sign(-889));
            //Logger.log(Math.Sign(5.553));
            //Logger.log(Math.Sign(-5.553));
            //Logger.log((555555555555555L / 5));

            //Logger.log(DateTime.Now - DateTime.UtcNow);
            //Logger.log((DateTime.Now - DateTime.UtcNow).TotalMinutes);
            //Logger.log((DateTime.Now - DateTime.UtcNow).TotalSeconds);
            //Logger.log((long) (5.5d + 10.1d));
            //Logger.log(((DateTime.Now - DateTime.UtcNow) * 2));


            //IGTime igTime_1 = new IGTime(60L * 24L);
            //Logger.log(60L * 24L);
            //Logger.log(igTime_1);
            //Logger.log(igTime_1 + 5);
            //Logger.log(igTime_1 + 61);
            //Logger.log(igTime_1 / igTime_1);

            //Sumpfkraut.TimeSystem.WorldClock clock = new Sumpfkraut.TimeSystem.WorldClock(
            //    new List<World>() { World.NewWorld }, new IGTime(0, 0, 0), 60 * 30, false,
            //    new TimeSpan(0, 0, 1));
            //clock.OnTimeChange += delegate (IGTime igTime) { Logger.log(igTime); };



            //OnSomething del = delegate (object[] args)
            //{
            //    foreach (object a in args)
            //    {
            //        Logger.print("--> " + a);
            //    }
            //};

            //del(1, 2, 3, del, new object());

            //object[] myObjects = new object[] { 5, "Fünf", false, null };
            //del(myObjects.);


            //DBQueryHandler queryHandler = new DBQueryHandler("Data Source=save.db");
            //Action<List<List<List<object>>>, String> callback = 
            //    new Action<List<List<List<object>>>, string>(
            //        delegate (List<List<List<object>>> results, string arg1) 
            //        {
            //            Logger.print(results[0][0][0] + " " + arg1);
            //        });
            //DBQuery<String> query_1 = new DBQuery<String>("SELECT 9001;", callback, "Popoklatsche");
            //queryHandler.Add(query_1);

            //TestClass t1 = new TestClass();
            //t1.Print("BLA");
            //SpellDef mySpellDef = new SpellDef();
            //mySpellDef.Print("BLA");

            //ItemDef itemDef  = new ItemDef();
            //ItemDef.PrintStatic(typeof(ItemDef), "#1 ", false);
            //ItemDef.PrintStatic(typeof(ItemDef), "#1 ", false);
            //ItemDef.PrintStatic(typeof(ItemDef), "#1 ", false);
            //ItemDef.PrintStatic(typeof(ItemDef), "#2 ", true);
            //itemDef.Print("#2 ", true);

            //Regex rgx = new Regex("(?<!(^\\/\\w+)).+");
            //String totalCmd = "/help me please!!! 5345 >34234_";
            //String cmd = "/help";
            ////Log.Logger.print(rgx.Match(totalCmd));
            ////for (int i = 0; i < totalCmd.Length; i++)
            ////{
            ////    if (totalCmd[i] == ' ')
            ////    {
            ////        break;
            ////    }
            ////    Logger.print(i + ": " + totalCmd[i]);
            ////}
            //Logger.print(totalCmd.Substring(cmd.Length));

            //Logger.print("3:22:1".Split(':').Length);
            //foreach (String t in "3:22:1".Split(':'))
            //{
            //    Logger.print(t);
            //}


            //String bla = "{ }";
            //Logger.print("1)");
            //Logger.print(bla);
            //Logger.print("2)");
            //String[] blaArr = Regex.Split(bla, " ", RegexOptions.IgnoreCase);
            ////Logger.print(blaArr.Length);
            //foreach (String bi in blaArr)
            //{
            //    Logger.print(bi);
            //}

            //IGTime t;
            //IGTime.TryParse("0:10:3", out t);
            //Logger.log(t);

            //String s1 = "12.1213.3213.12121:111";
            //Logger.print(s1.GetHashCode());
            //Logger.print(s1.GetHashCode());
            //Logger.print("12.1213.3213.12121:111".GetHashCode());
            //Logger.print(Guid.NewGuid().ToString("N"));
            //Logger.print(Guid.NewGuid().ToString("N"));
            //Logger.print(Guid.NewGuid().ToString("N"));
            //Logger.print(Guid.NewGuid().ToString("N"));

            //Utilities.TestObject myObj = new Utilities.TestObject();
            //myObj.Print("Print");
            //myObj.MakeLog("MakeLog");
            //myObj.MakeLogWarning("MakeLogWarning");
            //myObj.MakeLogError("MakeLogError");


            //GUC.Utilities.Threading.TestRun testRun = new GUC.Utilities.Threading.TestRun(true,
            //    new TimeSpan(0, 0, 1), false);
            //testRun.Suspend();
            //testRun.Resume();
            //testRun.Reset();

            //int h = 25;
            //int d = 1 + ((Math.Abs(h) / 24) * Math.Sign(h));
            //Logger.print(d);

            //int r = (int)Math.Round(((double)(-30) / 60), MidpointRounding.AwayFromZero);
            //Logger.print(r);

            //Logger.print(Math.Round(-0.4, MidpointRounding.AwayFromZero));
            //Logger.print(Math.Floor(-0.9));
            //Logger.print((int) ((double) -70 / 60));
            //Logger.print(4 / 3);



            //Types.IgTime igTime1;
            ////igTime1 = new Types.IgTime(1, -22, -30);
            //igTime1 = new Types.IgTime(1, 0, 30);
            ////Logger.print(igTime1);
            ////Logger.print(-igTime1);
            ////Logger.print(new Types.IgTime(0, 0, 0) - igTime1);
            //Logger.print(igTime1 - 91);
            //Logger.print(new Types.IgTime(1, 0, -31));
            ////if (Types.IgTime.TryParse("1:22:-30", out igTime1))
            ////{
            ////    Logger.print(igTime1);
            ////}

            //Logger.print(new Types.IgTime(1, 25, 0));



            Sumpfkraut.Web.WS.WSServer websocketTest = new Sumpfkraut.Web.WS.WSServer();
            websocketTest.Init();
            websocketTest.Start();


            CommandConsole cmdConsole = new CommandConsole();




            Instances.VobInstances.Init();

            Accounts.AccountSystem.Get(); //init

            DamageScript.Init();

            CmdHandler.Init();
            
            Logger.log(Logger.LogLevel.INFO, "###################### End Initalise ######################");
		}

        private void AgentBlack_ReceivedResults(object sender, Sumpfkraut.Database.DBAgent.ReceivedResultsEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
