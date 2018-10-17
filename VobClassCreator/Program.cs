using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace VobClassCreator
{
    class Program
    {
        static string basePath, name, parentName, subfolder;
        static bool abstr;
        static void Main(string[] args)
        {
            basePath = AppDomain.CurrentDomain.BaseDirectory;
            basePath = basePath.Remove(basePath.Length - @"\VobClassCreator\bin\Debug\".Length);
            basePath = "D:\\test";
            Console.WriteLine("Base path: '" + basePath + "' please check if this correct.\n");

            Console.WriteLine("Name of the new vob type (without inst/def):");

            name = Console.ReadLine();
            if (name.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new Exception("Invalid path chars!");

            Console.WriteLine("Name of the parent class:");
            parentName = Console.ReadLine();
            if (parentName.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new Exception("Invalid path chars!");

            Console.WriteLine("Shall the new vob type be abstract? (y/n)");
            string answer = Console.ReadLine();
            abstr = (answer.Length > 0 && answer[0] == 'y');

            Console.WriteLine("Subfolder (leave empty for none):");
            subfolder = Console.ReadLine();

            CreateVobFiles(VobKind.Def, Side.Server);
            CreateVobFiles(VobKind.Def, Side.Shared);
            CreateVobFiles(VobKind.Def, Side.Client);

            CreateVobFiles(VobKind.Inst, Side.Server);
            CreateVobFiles(VobKind.Inst, Side.Shared);
            CreateVobFiles(VobKind.Inst, Side.Client);

            //UpdateVobTypes();
        }

        static void UpdateProject(Side side)
        {
            string filePath = string.Format("{0}/Scripts{1}/Scripts{1}.csproj", basePath, side);

            List<string> lines = new List<string>(File.ReadAllLines(filePath));

            int index = lines.FindLastIndex(line => line.TrimStart().StartsWith("<Compile Include="));

            string text = ParseText("Include." + side + ".txt", '#');

            lines.Insert(index + 1, text);

            File.WriteAllLines(filePath + ".edit", lines);
        }

        static void UpdateVobTypes()
        {
            string filePath = Path.Combine(basePath, "ScriptsShared/Sumpfkraut/VobSystem/VobTypes.cs");

            List<string> lines = new List<string>(File.ReadAllLines(filePath));

            int startIndex = lines.FindIndex(line => line.TrimStart().StartsWith("public enum VobType"));
            int index = lines.FindIndex(startIndex + 1, line => line.TrimStart().StartsWith("}"));

            lines.Insert(index, string.Format("        {0},", name));

            File.WriteAllLines(filePath, lines);
        }

        enum VobKind
        {
            Inst,
            Def
        }

        enum Side
        {
            Shared,
            Server,
            Client
        }

        static void CreateVobFiles(VobKind kind, Side side)
        {
            string nameEnding = side == Side.Shared ? kind.ToString() : (kind + "." + side);
            string folder = kind == VobKind.Def ? "Definitions" : "Instances";

            string text = ParseText(nameEnding + ".txt", '/');

            string path = string.Format("{0}/Scripts{1}/Sumpfkraut/VobSystem/{2}/", basePath, side, folder);
            if (!string.IsNullOrWhiteSpace(subfolder))
                path += subfolder + '/';

            Directory.CreateDirectory(path);

            File.WriteAllText(path + name + nameEnding + ".cs", text);
        }

        static string ParseText(string resourceFile, char wordChar)
        {
            StringBuilder output;
            StringBuilder ident = new StringBuilder();
            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream("VobClassCreator.Resources." + resourceFile))
            using (StreamReader sr = new StreamReader(s))
            {
                output = new StringBuilder((int)s.Length);
                int c;
                while ((c = sr.Read()) != -1)
                {
                    if (c == '/')
                    {
                        while ((c = sr.Read()) != wordChar)
                        {
                            ident.Append((char)c);
                        }
                        output.Append(GetWord(ident.ToString()));
                        ident.Clear();
                    }
                    else
                    {
                        output.Append((char)c);
                    }
                }
            }

            return output.ToString();
        }

        static string GetWord(string ident)
        {
            switch(ident)
            {
                case "name":
                    return name;
                case "vobtypeoverride":
                    if (!abstr)
                    {
                        return string.Format("public override VobType VobType {{ get {{ return VobType.{0}; }} }}", name);
                    }
                    else
                    {
                        return string.Empty;
                    }
                case "subfolder":
                    if (!string.IsNullOrWhiteSpace(subfolder))
                    {
                        return "." + subfolder;
                    }
                    else
                    {
                        return string.Empty;
                    }
                case "parent":
                    return parentName;
                case "usinginsts":
                    if (!string.IsNullOrWhiteSpace(subfolder))
                    {
                        return "\nusing GUC.Scripts.Sumpfkraut.VobSystem.Instances;";
                    }
                    else
                    {
                        return "";
                    }
                case "usingdefs":
                    if (!string.IsNullOrWhiteSpace(subfolder))
                    {
                        return "\nusing GUC.Scripts.Sumpfkraut.VobSystem.Definitions;";
                    }
                    else
                    {
                        return "";
                    }
                case "abstract":
                    return abstr ? "abstract" : "";
                default:
                    throw new Exception(ident);
            }
        }
    }
}
