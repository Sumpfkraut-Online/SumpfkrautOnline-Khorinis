using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Scripts
{
    public class Init
    {
        public Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
        }

        static System.Reflection.Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(System.IO.Path.GetFullPath("System\\UntoldChapter\\Dll\\" + name + ".dll"));
            if (asm == null)
            {
                asm = System.Reflection.Assembly.LoadFrom(System.IO.Path.GetFullPath("UntoldChapter\\Dll\\" + name + ".dll"));
            }
            return asm;
        }
    }
}
