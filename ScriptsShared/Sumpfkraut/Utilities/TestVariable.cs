using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities
{
    abstract class TestVarAbstract
    {
        string ident;
        public string Identifier { get { return this.ident; } }
        public TestVarAbstract(string identifier)
        {
            this.ident = identifier;
            variables.Add(this.ident, this);
        }

        protected abstract bool ParseValue(string str);

        static Dictionary<string, TestVarAbstract> variables = new Dictionary<string, TestVarAbstract>(StringComparer.OrdinalIgnoreCase);
        public static void Parse(string line)
        {
            string[] strs = line.Split(new char[] { ' ', '=', ':', }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                if (variables.TryGetValue(strs[i], out TestVarAbstract variable))
                {
                    for (i = i + 1; i < strs.Length; i++)
                    {
                        if (variable.ParseValue(strs[i]))
                            break;
                    }
                    break;
                }
            }
        }
    }

    class TestVar<T> : TestVarAbstract
    {
        public T Value;
        public TestVar(string identifier, T initial = default(T)) : base(identifier)
        {
            this.Value = initial;
        }

        protected override bool ParseValue(string str)
        {
            bool result;
            if (typeof(int) == typeof(T))
            {
                result = int.TryParse(str, out int val);
                this.Value = (T)(object)val;
            }
            else if (typeof(float) == typeof(T))
            {
                result = float.TryParse(str, out float val);
                this.Value = (T)(object)val;
            }
            else if (typeof(string) == typeof(T))
            {
                result = true;
                this.Value = (T)(object)str;
            }
            else
            {
                throw new NotImplementedException();
            }
            return result;
        }
    }
}
