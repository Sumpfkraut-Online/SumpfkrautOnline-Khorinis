using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GothicClassGenerator
{
    public class zClass : zType
    {
        const string GeneratedContentHeader = "// This file was generated with the GothicClassGenerator (do not change this line!)";
        const string RegionGeneratedCode = "Generated Code (do not change this region!)";
        const string RegionCustomCode = "Custom Code (put your code here!)";

        public static List<zClass> Classes = new List<zClass>(100);

        public zClass(List<string> usings = null)
        {
            usingDirectives = usings ?? new List<string>()
            {
                "System",
                "System.Collections.Generic",
                "System.Text",
                "WinApi",
                "WinApi.Calls",
            };
        }

        public bool IsObject
        {
            get
            {
                zClass parent = this;
                do
                {
                    if (parent.Name == "zCObject")
                        return true;
                    parent = parent.BaseClass;
                } while (parent != null);
                return false;
            }
        }

        string nameSpace = "";
        public string NameSpace { get { return this.nameSpace; } }
        public void SetNameSpace(string newNameSpace)
        {
            if (!string.IsNullOrWhiteSpace(newNameSpace))
            {
                string[] strs = newNameSpace.Split(new char[] { ' ', '.', ',', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

                StringBuilder sb = new StringBuilder(newNameSpace.Length);
                int startIndex = (strs.Length > 0 && strs[0] == "Gothic") ? 1 : 0;
                for (int i = startIndex; i < strs.Length; i++)
                {
                    sb.Append(strs[i]);
                    if (i != strs.Length - 1)
                        sb.Append('.');
                }

                nameSpace = sb.ToString();
            }
            else
            {
                nameSpace = "";
            }
        }

        public int VTable = 0;

        public int ByteSize = 0;

        public int Destructor = 0;

        string baseClassName = null;
        public zClass BaseClass = null;

        public string CustomCode = "";

        List<string> usingDirectives;

        public List<zProperty> Properties = new List<zProperty>();
        public List<zCall> Calls = new List<zCall>();

        public void AddUsingDirective(string useme)
        {
            useme = useme.Trim();
            if (string.IsNullOrWhiteSpace(useme) || useme == "Gothic" || useme == this.NameSpace)
                return;

            if (!usingDirectives.Contains(useme))
                usingDirectives.Add(useme);
        }

        public void AddProperty(zProperty prop)
        {
            Properties.Add(prop);
            if (prop.Type is zClass)
                AddUsingDirective(((zClass)prop.Type).NameSpace);
        }

        public void AddCall(zCall call)
        {
            Calls.Add(call);
            if (call.ReturnType is zClass)
                AddUsingDirective(((zClass)call.ReturnType).NameSpace);

            for (int i = 0; i < call.Args.Count; i++)
            {
                var type = call.Args[i].Type;
                if (type is zClass)
                    AddUsingDirective(((zClass)type).NameSpace);
            }
        }

        public void Save()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                return;

            string[] strs = NameSpace.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string path = Path.Combine(Path.Combine(strs), Name + ".cs");

            using (Writer w = new Writer(path))
            {
                w.Write(GeneratedContentHeader);

                // USING DIRECTIVES
                foreach (string useme in usingDirectives)
                    w.Write("using {0};", (useme.StartsWith("System") || useme.StartsWith("WinApi")) ? useme : ("Gothic." + useme));

                w.LineBreak();

                // NAMESPACE
                StringBuilder sb = Utils.GetStringBuilder();
                sb.Append("namespace Gothic");
                if (NameSpace.Length > 0)
                {
                    sb.Append('.');
                    sb.Append(NameSpace);
                }
                w.Write(sb.ToString());
                w.Open();
                {
                    // CLASS
                    sb.Clear();
                    sb.AppendFormat("public struct {0} : IDisposable", Name);
                    if (this.BaseClass != null)
                    {
                        sb.Append(" // : ");
                        sb.Append(this.BaseClass.Name);
                    }

                    w.Write(sb.ToString());
                    w.Open();
                    {
                        w.Write("#region " + RegionCustomCode);
                        w.LineBreak();
                        w.Write(CustomCode);
                        w.LineBreak();
                        w.Write("#endregion");
                        w.LineBreak();

                        w.Write("#region " + RegionGeneratedCode);
                        w.LineBreak();

                        w.Write("const int ByteSize = 0x{0};", this.ByteSize.ToString("X"));
                        w.LineBreak();

                        w.Write("const int VirtualTable = 0x{0};", this.VTable.ToString("X6"));
                        w.LineBreak();

                        w.Write("const int Destructor = 0x{0};", this.Destructor.ToString("X6"));
                        w.LineBreak();

                        if (this.IsObject)
                        {
                            w.Write("static readonly zCClassDef ClassDef = new zCClassDef(Process.ReadInt(Process.ReadInt(VirtualTable) + 1));");
                            w.LineBreak();
                        }

                        this.WriteCalls(w);
                        this.WriteProperties(w);
                        this.WritePointerInheritance(w);

                        // DISPOSE
                        if (this.IsObject)
                            w.Write("static readonly ThisCall<uint> destructor = new ThisCall<uint>(Process.ReadInt(VirtualTable + 0xC));");
                        else if (Destructor != 0)
                            w.Write("static readonly ThisCall destructor = new ThisCall(0x{0});", Destructor.ToString("X6"));

                        w.Write("public void Dispose()");
                        w.Open();
                        if (this.IsObject)
                        {
                            w.Write("destructor.Call(this.address, 1);");
                        }
                        else
                        {
                            if (Destructor != 0)
                            {
                                w.Write("destructor.Call(this.address);");
                            }
                            w.Write("Process.Free(this.address, ByteSize);");
                        }
                        w.Close();
                        w.LineBreak();

                        w.Write("#endregion");
                    }
                    w.Close();

                }
                w.Close();
                w.LineBreak();
            }

            if (File.Exists("Gothic.csproj"))
            {
                string file = NameSpace.Length > 0 ? (NameSpace.Replace('.', '\\') + '\\' + Name) : Name;
                string newLine = string.Format("    <Compile Include=\"{0}.cs\" />", file);

                List<string> lines = new List<string>(File.ReadAllLines("Gothic.csproj"));
                if (!lines.Contains(newLine))
                {
                    int index = lines.FindLastIndex(l => l.StartsWith("    <Compile Include"));
                    if (index >= 0)
                    {
                        lines.Insert(index + 1, newLine);
                        File.WriteAllLines("Gothic.csproj", lines);
                    }
                }
            }

            foreach (zClass child in Classes)
                if (child.BaseClass == this)
                    child.Save();
        }

        void WriteCalls(Writer w, bool constructorsToo = true)
        {
            w.Write("#region Calls of " + this.Name);
            w.LineBreak();

            foreach (var call in this.Calls)
            {
                if (call.Type != CallType.Constructor || constructorsToo)
                    call.Write(w);
            }

            if (this.BaseClass != null)
                this.BaseClass.WriteCalls(w, false);

            w.Write("#endregion");
            w.LineBreak();
        }

        void WriteProperties(Writer w)
        {
            w.Write("#region Properties of " + this.Name);
            w.LineBreak();

            foreach (var prop in this.Properties)
                prop.Write(w);

            if (this.BaseClass != null)
                this.BaseClass.WriteProperties(w);

            w.Write("#endregion");
            w.LineBreak();
        }

        void WritePointerInheritance(Writer w)
        {
            w.Write("public static readonly {0} Null = new {0}(0);", Name);
            w.LineBreak();

            // ADRESS FIELD
            w.Write("int address;");
            w.Write("public int Address { get { return this.address; } }");
            w.Write("public bool IsNull { get { return this.address == 0; } }");
            w.LineBreak();

            // CONSTRUCTOR
            w.Write("public {0}(int address)", Name);
            w.Open();
            w.Write("this.address = address;");
            w.Close();
            w.LineBreak();

            // EQUALITY
            w.Write("#region Equality");
            w.LineBreak();

            w.Write("public bool Equals({0} other)", Name);
            w.Open();
            w.Write("return other.address == this.address;");
            w.Close();
            w.LineBreak();

            w.Write("public override bool Equals(object other)");
            w.Open();
            w.Write("return other is {0} ? (({0})other).address == this.address : false;", Name);
            w.Close();
            w.LineBreak();

            w.Write("public static bool operator ==({0} a, {0} b)", Name);
            w.Open();
            w.Write("return a.address == b.address;");
            w.Close();
            w.LineBreak();

            w.Write("public static bool operator !=({0} a, {0} b)", Name);
            w.Open();
            w.Write("return a.address != b.address;");
            w.Close();
            w.LineBreak();

            w.Write("#endregion");
            w.LineBreak();

            // CONVERSION
            w.Write("#region Conversion");
            w.LineBreak();

            w.Write("public static implicit operator int({0} self)", Name);
            w.Open();
            w.Write("return self.address;");
            w.Close();
            w.LineBreak();

            w.Write("public static implicit operator bool({0} self)", Name);
            w.Open();
            w.Write("return self.address != 0;");
            w.Close();
            w.LineBreak();

            zClass parent = this;
            while ((parent = parent.BaseClass) != null)
            {
                w.Write("public static implicit operator {0}({1} self)", parent.Name, Name);
                w.Open();
                w.Write("return new {0}(self.address);", parent.Name);
                w.Close();
                w.LineBreak();
            }

            w.Write("#endregion");
            w.LineBreak();

            // HASHCODE & TOSTRING
            w.Write("#region HashCode & String");
            w.LineBreak();

            w.Write("public override int GetHashCode()");
            w.Open();
            w.Write("return this.address.GetHashCode();");
            w.Close();
            w.LineBreak();

            w.Write("public override string ToString()");
            w.Open();
            w.Write("return this.address.ToString(\"X8\");");
            w.Close();
            w.LineBreak();

            w.Write("#endregion");
            w.LineBreak();
        }

        public override string PropertyGet(string address)
        {
            return string.Format("new {0}(Process.ReadInt({1}))", Name, address);
        }

        public override string PropertySet(string address)
        {
            return string.Format("Process.Write({0}, value.address)", address);
        }

        public static zClass ReadFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return null;

            if (!File.Exists(filePath))
                return null;

            Reader r = new Reader(filePath);

            // HEADER
            string line = r.ReadLine(skipComments: false);
            if (line != GeneratedContentHeader)
                return null;

            // USINGS
            List<string> usings = new List<string>(5);
            while (true)
            {
                line = r.ReadLine();
                if (line == null)
                    return null;

                if (!line.StartsWith("using"))
                    break;

                int startIndex = "using".Length + 1;
                int endIndex = line.LastIndexOf(';');
                if (endIndex > startIndex)
                    usings.Add(line.Substring(startIndex, endIndex - startIndex).Trim());
            }

            // NAMESPACE
            string nameSpace = line.StartsWith("namespace") ? line.Substring("namespace".Length).TrimStart() : r.SkipUntil("namespace");
            if (string.IsNullOrWhiteSpace(nameSpace))
                return null;

            // CLASS
            line = r.SkipUntil("public struct");
            int index = line.IndexOf(':');
            if (index < 0) return null;
            string name = line.Remove(index).Trim();
            if (string.IsNullOrWhiteSpace(name))
                return null;

            // CUSTOM CODE
            string customCode = r.ReadRegion(RegionCustomCode);
            if (customCode == null)
                return null;

            if (r.SkipUntil("#region " + RegionGeneratedCode) == null)
                return null;

            // Byte size
            line = r.SkipUntil("const int ByteSize =");
            if (string.IsNullOrWhiteSpace(line))
                return null;
            index = line.IndexOf(';');
            if (index < 0 || !Utils.TryParseHex(line.Remove(index), out int byteSize))
                return null;

            // VIRTUAL TABLE
            line = r.SkipUntil("const int VirtualTable =");
            if (string.IsNullOrWhiteSpace(line))
                return null;
            index = line.IndexOf(';');
            if (index < 0 || !Utils.TryParseHex(line.Remove(index), out int vtable))
                return null;

            // DESTRUCTOR
            line = r.SkipUntil("const int Destructor =");
            if (string.IsNullOrWhiteSpace(line))
                return null;
            index = line.IndexOf(';');
            if (index < 0 || !Utils.TryParseHex(line.Remove(index), out int destructor))
                return null;

            zClass newClass = new zClass(usings);
            newClass.SetNameSpace(nameSpace);
            newClass.SetName(name);
            newClass.CustomCode = customCode;
            newClass.VTable = vtable;
            newClass.ByteSize = byteSize;
            newClass.Destructor = destructor;

            if (!newClass.ReadCalls(r))
                return null;

            if (!newClass.ReadProperties(r))
                return null;

            return newClass;
        }

        bool ReadCalls(Reader r)
        {
            if (r.SkipUntil("#region Calls of " + this.Name) == null)
                return false;

            string line;
            while ((line = r.ReadLine()) != null)
            {
                if (line.StartsWith("#endregion"))
                    break;
                if (line.StartsWith("#region Calls of"))
                {
                    this.baseClassName = line.Substring("#region Calls of".Length).Trim();
                    break;
                }

                zCall call = zCall.ReadCall(r, line);
                if (call != null) Calls.Add(call);
            }

            return true;
        }

        bool ReadProperties(Reader r)
        {
            if (r.SkipUntil("#region Properties of " + this.Name) == null)
                return false;

            string line;
            while ((line = r.ReadLine()) != null)
            {
                if (line.StartsWith("#endregion") || line.StartsWith("#region"))
                    break;

                zProperty prop = zProperty.Read(r, line);
                if (prop != null) Properties.Add(prop);
            }

            return true;
        }

        public static void ResolveRefs()
        {
            foreach (zClass z in Classes)
            {
                if (!string.IsNullOrWhiteSpace(z.baseClassName))
                    z.BaseClass = Classes.Find(c => c.Name == z.baseClassName);
                z.Calls.ForEach(c => c.ResolveRefs());
                z.Properties.ForEach(p => p.ResolveRefs());
            }
        }
    }
}
