using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicClassGenerator
{
    public class zProperty
    {
        string name = "";
        public string Name { get { return this.name; } }
        public void SetName(string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                this.name = newName.Trim();
            }
            else
            {
                this.name = "";
            }
        }

        public string Identifier
        {
            get
            {
                StringBuilder builder = Utils.GetStringBuilder();
                builder.Append(Type.Name); builder.Append(' ');
                builder.Append(this.Name); builder.Append(" { ");
                if (Get) builder.Append("get; ");
                if (Set) builder.Append("set; ");
                builder.Append('}');
                return builder.ToString();
            }
        }

        public zType Type;
        string typeName = null;

        public bool Get = true;
        public bool Set = true;

        public int VarOffset = 0;

        public void Write(Writer w)
        {
            w.Write("public {0} {1}", Type.Name, Name);
            w.Open();

            string address = string.Format("this.address + (0x{0})", this.VarOffset.ToString("X"));
            if (Get)
                w.Write("get {{ return {0}; }}", Type.PropertyGet(address));
            if (Set)
                w.Write("set {{ {0}; }}", Type.PropertySet(address));

            w.Close();
            w.LineBreak();
        }

        public static zProperty Read(Reader r, string firstLine)
        {
            string line;
            if (firstLine != null && firstLine.StartsWith("public"))
                line = firstLine.Substring("public".Length).Trim();
            else line = r.SkipUntil("public");

            if (line == null)
                return null;
            
            string[] strs = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length != 2)
                return null;

            string typeName = strs[0];
            string name = strs[1];
            bool get = false, set = false;
            string addressStr = null;

            while ((line = r.ReadLine()) != null && !line.StartsWith("}"))
            {
                if (line.StartsWith("get"))
                {
                    get = true;
                    addressStr = line;
                }
                else if (line.StartsWith("set"))
                {
                    set = true;
                    addressStr = line;
                }
            }
            
            if (addressStr == null)
                return null;
            
            int startIndex = addressStr.IndexOf("this.address + (");
            if (startIndex < 0) return null;
            startIndex += "this.address + (".Length;
            
            int endIndex = addressStr.IndexOf(')', startIndex);
            if (endIndex < 0) return null;
            
            if (!Utils.TryParseHex(addressStr.Substring(startIndex, endIndex - startIndex), out int offset))
                return null;
            
            zProperty prop = new zProperty();
            prop.name = name;
            prop.typeName = typeName;
            prop.Get = get;
            prop.Set = set;
            prop.VarOffset = offset;
            return prop;
        }

        public void ResolveRefs()
        {
            this.Type = UIStuff.TypesNoVoid.FirstOrDefault(t => t.Name == typeName);
        }
    }
}
