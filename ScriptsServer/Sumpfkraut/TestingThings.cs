using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Scripts.Sumpfkraut
{
    public class TestingThings : ExtendedObject
    {

        new public static readonly string _staticName = "TestingThings (static)";



        public static void Init ()
        {
            Logger.Log(Directory.GetCurrentDirectory());

            WorldSystem.WorldLoader worldLoader = new WorldSystem.WorldLoader(">>GOTCHA<<", 
                Directory.GetCurrentDirectory() + @"\DB");
            worldLoader.Load();

            string bla = "Miami Vice";
            Logger.Log(bla);

            //Logger.Log(WorldSystem.WorldLoader.DBStructure["WorldEffect"]);

        }

    }
}
