using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GothicClassGenerator
{
    public abstract class zType
    {
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

        public abstract string PropertyGet(string variable);
        public abstract string PropertySet(string variable);
    }

    public class zVoid : zType
    {
        public static readonly zVoid Void = new zVoid();

        public zVoid()
        {
            SetName("void");
        }

        public override string PropertyGet(string address)
        {
            throw new NotSupportedException();
        }

        public override string PropertySet(string address)
        {
            throw new NotSupportedException();
        }
    }

    public class zFloat : zType
    {
        public static readonly zFloat Float = new zFloat();

        public zFloat()
        {
            SetName("float");
        }

        public override string PropertyGet(string address)
        {
            return string.Format("Process.ReadFloat({0})", address);
        }

        public override string PropertySet(string address)
        {
            return string.Format("Process.Write({0}, value)", address);
        }
    }

    public class zInt : zType
    {
        public static readonly zInt Int = new zInt();

        public zInt()
        {
            SetName("int");
        }

        public override string PropertyGet(string address)
        {
            return string.Format("Process.ReadInt({0})", address);
        }

        public override string PropertySet(string address)
        {
            return string.Format("Process.Write({0}, value)", address);
        }
    }

    public class zBool : zType
    {
        public static readonly zBool Bool = new zBool();

        public zBool()
        {
            SetName("bool");
        }

        public override string PropertyGet(string address)
        {
            return string.Format("Process.ReadBool({0})", address);
        }

        public override string PropertySet(string address)
        {
            return string.Format("Process.Write({0}, value)", address);
        }
    }
}
