using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Network;
using Injection;

namespace GMP.Modules
{
    public class ModuleLoader
    {
        public static void load(Module module)
        {
            if (module.ModuleObj == null)
                module.ModuleObj = new List<object>();
            module.fileName = Path.GetFullPath(module.fileName);

            module.assembly = System.Reflection.Assembly.LoadFile(module.fileName);
            Type[] t = module.assembly.GetTypes();
            foreach (Type types in t)
            {
                if (types.GetConstructor(new Type[] { }) == null)
                    continue;


                Object obj = types.GetConstructor(new Type[] { }).Invoke(null);

                if (IsExtendedFromClass<LoadModule>(obj) || IsExtendedFromClass<StartState>(obj) || IsExtendedFromClass<UpdateModule>(obj))
                    module.ModuleObj.Add(obj);
                else
                    continue;

                

                if (IsExtendedFromClass<LoadModule>(obj))
                {

                    LoadModule s = (LoadModule)obj;
                    s.Load(module);
                }
            }
        }

        public static void updateStartState()
        {
            int i = 0;
            foreach (Module module in StaticVars.serverConfig.Modules)
            {
                if (module.started && !module.ended)
                {
                    Object[] objList = module.ModuleObj.ToArray();
                    foreach (Object obj in objList)
                    {
                        if (IsExtendedFromClass<StartState>(obj))
                        {
                            StartState s = (StartState)obj;
                            s.Update(module);
                            i++;
                        }
                    }
                }
            }

            if (i == 0)
            {
                Program.StopModule();
            }
        }



        public static void updateAllModules()
        {
            foreach (Module module in StaticVars.serverConfig.Modules)
            {
                Object[] objList = module.ModuleObj.ToArray();
                foreach (Object obj in objList)
                {
                    if (IsExtendedFromClass<UpdateModule>(obj))
                    {
                        UpdateModule s = (UpdateModule)obj;
                        s.update(module);
                    }
                }
                
            }
        }


        public static void startFirstStartState()
        {
            foreach (Module module in StaticVars.serverConfig.Modules)
            {
                if (!module.started && !module.ended)
                {
                    Object[] objList = module.ModuleObj.ToArray();
                    foreach (Object obj in objList)
                    {

                        if (IsExtendedFromClass<StartState>(obj))
                        {
                            module.started = true;
                            return;
                        }

                    }
                }
            }
        }


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

        //public static bool IsExtendedFromClass<I>(Type obj) where I : class
        //{
        //    try
        //    {
        //        I te = (I)obj.GetConstructor(new Type[] { }).Invoke(null);
        //        return true;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return false;
        //    }
        //}


    }
}
