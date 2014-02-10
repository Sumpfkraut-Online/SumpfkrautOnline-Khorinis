using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Network;

namespace GMP_Server.Modules
{
    public class ModuleLoader
    {
        public static List<Module> modules = new List<Module>();
        //public static void load(Network.Module module)
        //{
        //    if (module.ModuleObj == null)
        //        module.ModuleObj = new List<object>();
        //    module.fileName = Path.GetFullPath(module.fileName);

        //    module.assembly = System.Reflection.Assembly.LoadFile(module.fileName);
            
        //    Type[] t = module.assembly.GetTypes();
        //    foreach (Type types in t)
        //    {
        //        if (types.GetConstructor(new Type[] { }) == null)
        //            continue;


        //        Object obj = types.GetConstructor(new Type[] { }).Invoke(null);

        //        if (IsExtendedFromClass<LoadModule>(obj))
        //            module.ModuleObj.Add(obj);
        //        else
        //            continue;


        //        if (IsExtendedFromClass<LoadModule>(obj))
        //        {

        //            LoadModule s = (LoadModule)obj;
        //            s.Load(module);
        //        }
        //    }

        //    modules.Add(module);
        //}

        public static void load(Network.Module module)
        {
            if (module.ModuleObj == null)
                module.ModuleObj = new List<object>();
            module.fileName = Path.GetFullPath(module.fileName);

            module.assembly = System.Reflection.Assembly.LoadFile(module.fileName);

            //Alle nötigen Module-Obj heraussuchen - Nur 1 mal ausführen!!!!
            Type[] t = module.assembly.GetTypes();
            foreach (Type types in t)
            {
                if (types.GetConstructor(new Type[] { }) == null)
                    continue;


                Object obj = types.GetConstructor(new Type[] { }).Invoke(null);

                if (IsExtendedFromClass<LoadModule>(obj) || IsExtendedFromClass<UpdateModule>(obj))
                    module.ModuleObj.Add(obj);
                else
                    continue;
            }

            //Load-Funktion ausführen
            foreach (Object obj in module.ModuleObj)
            {
                if (IsExtendedFromClass<LoadModule>(obj))
                {
                    LoadModule s = (LoadModule)obj;
                    s.Load(module);
                }
            }

            modules.Add(module);
        }

        public static void loadAllModules()
        {
            string[] modulesfiles = Directory.GetFiles("ServerModules/");
            int i = 0;
            foreach (string modulefile in modulesfiles)
            {
                if (!modulefile.EndsWith(".dll"))
                    continue;

                Module module = new Module();
                module.name = modulefile;
                module.fileName = modulefile;
                module.size = i;
                i++;
                load(module);
            }

        }

        public static void updateAllModules()
        {
            foreach (Module module in modules)
            {
                foreach (Object obj in module.ModuleObj)
                {
                    if (IsExtendedFromClass<UpdateModule>(obj))
                    {
                        UpdateModule s = (UpdateModule)obj;
                        s.update(module);
                    }
                }
            }
        }
        //public static void updateAllModules()
        //{
        //    foreach (Module module in modules)
        //    {
        //        Type[] t = module.assembly.GetTypes();
        //        foreach (Type types in t)
        //        {
        //            if (types.GetConstructor(new Type[] { }) == null)
        //                continue;
                    

        //            Object obj = types.GetConstructor(new Type[] { }).Invoke(null);
                    
        //            if (!IsExtendedFromClass<LoadModule>(obj) && IsExtendedFromClass<UpdateModule>(obj))
        //                module.ModuleObj.Add(obj);
        //            else if (IsExtendedFromClass<LoadModule>(obj))
        //            {
        //                foreach (Object objL in module.ModuleObj)
        //                {
        //                    if (objL.GetType() == obj.GetType())
        //                    {
        //                        obj = objL;
        //                        break;
        //                    }
        //                }
        //            }


        //            if (IsExtendedFromClass<UpdateModule>(obj))
        //            {
                        
        //                UpdateModule s = (UpdateModule)obj;
        //                s.update(module);
        //            }
        //        }
        //    }
        //}

        public static bool IsExtendedFromClass<I>(Object obj) where I : class
        {
            try
            {
                I te = (I)obj;
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }
}
