using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace GUC.Client
{
    public static class ClientPaths
    {
        public static string G2Base { get { return GetFullPath("\\"); } }
        public static string G2System { get { return GetFullPath(g2System); } }
        public static string GUCBase { get { return GetFullPath(gucBase); } }
        public static string GUCDlls { get { return GetFullPath(gucDlls); } }
        public static string GUCConfig { get { return GetFullPath(gucConfig); } }

        static string g2Base = null;
        static string g2System = "\\System\\";
        static string gucBase = "\\System\\UntoldChapter\\";
        static string gucDlls = "\\System\\UntoldChapter\\DLL\\";
        static string gucConfig = "\\System\\UntoldChapter\\conf\\";
        
        static string GetFullPath(string path)
        {
            if (g2Base == null || !File.Exists(g2Base + "\\System\\Gothic2.exe"))
            {
                g2Base = null;

                string current = Path.GetFullPath(Environment.CurrentDirectory);
                for (int i = 0; i < 2; i++)
                {
                    if (File.Exists(current + "\\System\\Gothic2.exe"))
                    {
                        g2Base = current;
                        break;
                    }
                    else if (File.Exists(current + "\\Gothic2.exe"))
                    {
                        g2Base = Path.GetDirectoryName(current);
                        break;
                    }
                    current = Path.GetDirectoryName(current); // jump one folder up
                }

                if (g2Base == null)
                {
                    throw new Exception("Gothic 2 not found!");
                }
            }

            return g2Base + path;
        }

        internal static void CreateFolders()
        {
            StringBuilder sb = new StringBuilder();

            // Create folders if required
            foreach (PropertyInfo prop in typeof(ClientPaths).GetProperties(BindingFlags.Static | BindingFlags.Public))
            {
                if (prop.PropertyType != typeof(string))
                    continue;

                string value = (string)prop.GetValue(null, null);
                sb.AppendLine(value);

                if (!Directory.Exists(value))
                {
                    Directory.CreateDirectory(value);
                }
            }
        }
    }
}
