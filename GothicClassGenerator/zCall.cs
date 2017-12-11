using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GothicClassGenerator
{
    public enum CallType
    {
        ThisCall,
        VirtualCall,
        Constructor,

        StdCall,
        CdeclCall,

        //FastCall
    }

    public class zCall
    {
        public class Argument
        {
            public zType Type;
            public string Name;
            public string typeName;

            public Argument(zType type, string name)
            {
                this.Type = type;
                this.Name = name.Trim();
                typeName = type?.Name;
            }
        }

        string name = "";
        public string Name { get { return this.name; } }
        public void SetName(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                name = newName.Trim();
            }
            else
            {
                name = "";
            }
        }

        public string Identifier
        {
            get
            {
                StringBuilder builder = Utils.GetStringBuilder();
                builder.Append(ReturnType.Name); builder.Append(' ');
                builder.Append(this.Name); builder.Append('(');
                for (int i = 0; i < Args.Count; i++)
                {
                    builder.Append(Args[i].Type.Name);
                    if (i < Args.Count - 1)
                        builder.Append(',');
                }
                builder.Append(')');
                return builder.ToString();
            }
        }

        public int Address = 0;

        public CallType Type = CallType.ThisCall;

        public zType ReturnType = zVoid.Void;
        string retType = null;

        public List<Argument> Args = new List<Argument>();

        public void Write(Writer w)
        {
            StringBuilder sb = Utils.GetStringBuilder();
            sb.Append(this.Name);
            sb.Append('_');
            sb.Append(this.ReturnType.Name);
            if (Args.Count > 0)
            {
                sb.Append('_');
                for (int i = 0; i < Args.Count; i++)
                    sb.Append(Args[i].Type.Name);
            }

            string callName = sb.ToString();

            sb.Clear();
            switch (this.Type)
            {
                case CallType.ThisCall:
                case CallType.VirtualCall:
                case CallType.Constructor:
                    sb.Append("This");
                    break;
                case CallType.StdCall:
                    sb.Append("Std");
                    break;
                case CallType.CdeclCall:
                    sb.Append("Cdecl");
                    break;
            }

            bool hasReturn = this.ReturnType != zVoid.Void;
            if (hasReturn)
            {
                sb.Append("Return");
            }
            sb.Append("Call");

            bool hasArgs = this.Args.Count > 0;
            if (hasReturn || hasArgs)
            {
                sb.Append('<');
                if (hasReturn)
                {
                    sb.Append(this.ReturnType.Name);
                    if (hasArgs) sb.Append(',');
                }

                for (int i = 0; i < Args.Count; i++)
                {
                    sb.Append(this.Args[i].Type.Name);
                    if (i < Args.Count - 1)
                        sb.Append(',');
                }

                sb.Append('>');
            }

            string callClass = sb.ToString();

            string addrStr;
            if (this.Type == CallType.VirtualCall)
            {
                addrStr = string.Format("Process.ReadInt(VirtualTable + 0x{0})", Address.ToString("X"));
            }
            else
            {
                addrStr = "0x" + this.Address.ToString("X6");
            }
            w.Write("static readonly {0} {1} = new {0}({2});", callClass, callName, addrStr);

            // METHOD
            sb.Clear();
            sb.Append("public ");
            bool staticCall = Type == CallType.StdCall || Type == CallType.CdeclCall || Type == CallType.Constructor;
            if (staticCall) sb.Append("static ");
            sb.Append(ReturnType.Name); sb.Append(' ');
            sb.Append(this.Name); sb.Append('(');

            for (int i = 0; i < Args.Count; i++)
            {
                sb.Append(Args[i].Type.Name);
                sb.Append(' ');
                sb.Append(Args[i].Name);
                if (i < Args.Count - 1)
                    sb.Append(", ");
            }
            sb.Append(')');
            w.Write(sb.ToString());
            w.Open();

            if (Type == CallType.Constructor && ((zClass)ReturnType).IsObject)
            {
                w.Write("zCObject obj = new zCObject(Process.Alloc(ByteSize).ToInt32());");
                w.Write("zCClassDef.ObjectCreated(obj, ClassDef);");
            }

            sb.Clear();
            if (hasReturn)
                sb.Append("return ");
            sb.Append(callName);
            sb.Append(".Call(");
            if (!staticCall || Type == CallType.Constructor)
            {
                if (Type == CallType.Constructor)
                {
                    sb.Append(((zClass)ReturnType).IsObject ? "obj" : "Process.Alloc(ByteSize).ToInt32()");
                }
                else
                {
                    sb.Append("this.address");
                }
                if (hasArgs)
                    sb.Append(", ");
            }
            for (int i = 0; i < Args.Count; i++)
            {
                sb.Append(Args[i].Name);
                if (i < Args.Count - 1)
                    sb.Append(", ");
            }
            sb.Append(");");

            w.Write(sb.ToString());

            w.Close();
            w.LineBreak();
        }

        public static zCall ReadCall(Reader r, string firstLine)
        {
            string line;
            if (firstLine != null && firstLine.StartsWith("static readonly"))
                line = firstLine.Substring("static readonly".Length).Trim();
            else line = r.SkipUntil("static readonly");

            if (line == null)
                return null;
            
            CallType type;
            if (line.StartsWith("This")) type = CallType.ThisCall;
            else if (line.StartsWith("Std")) type = CallType.StdCall;
            else if (line.StartsWith("Cdecl")) type = CallType.CdeclCall;
            else return null;
            
            string addressStr = line.FindBetween("(", ")");
            string vAddressStr = addressStr.FindBetween("Process.ReadInt(VirtualTable + ", ")");
            if (vAddressStr != null)
            {
                addressStr = vAddressStr;
                if (type == CallType.ThisCall)
                    type = CallType.VirtualCall;
                else return null;
            }
            
            if (!Utils.TryParseHex(addressStr, out int address))
                return null;

            line = r.SkipUntil("public");
            if (line == null) return null;
            
            string[] strs = line.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            int index = 0;
            if (strs[index] == "static") index++;

            string returnType = strs[index++];
            string name = strs[index++];

            zCall call = new zCall();
            call.name = name;
            call.Type = name == "Create" ? CallType.Constructor : type;
            call.retType = returnType;
            call.Address = address;

            for (int i = index; i < strs.Length - 1; i++)
            {
                string typeName = strs[i++];
                Argument arg = new Argument(null, strs[i]);
                arg.typeName = typeName;
                call.Args.Add(arg);
            }

            r.SkipUntil("}");

            return call;
        }

        public void ResolveRefs()
        {
            Args.ForEach(a => a.Type = UIStuff.TypesNoVoid.FirstOrDefault(t => t.Name == a.typeName));
            ReturnType = UIStuff.AllTypes.FirstOrDefault(t => t.Name == retType);
        }
    }
}
