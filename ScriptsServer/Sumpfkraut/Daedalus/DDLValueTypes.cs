using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    abstract class DDLValueType
    {
        public abstract void Clear();
        public abstract void Set(string valueStr, string indexStr);
        public abstract DDLValueType Clone();
    }

    class DDLInt : DDLValueType
    {
        int value;
        public int Value { get { return this.value; } }

        public override void Clear()
        {
            value = 0;
        }

        public override void Set(string valueStr, string indexStr)
        {
            ConstParser.TryParse(valueStr, out value);
        }

        public override DDLValueType Clone()
        {
            var ret = new DDLInt();
            ret.value = this.value;
            return ret;
        }
    }

    class DDLString : DDLValueType
    {
        string value;
        public string Value { get { return this.value; } }

        public override void Clear()
        {
            value = "";
        }

        public override void Set(string valueStr, string indexStr)
        {
            ConstParser.TryParse(valueStr, out value);
        }

        public override DDLValueType Clone()
        {
            var ret = new DDLString();
            ret.value = this.value;
            return ret;
        }
    }

    class DDLFloat : DDLValueType
    {
        float value;
        public float Value { get { return this.value; } }

        public override void Clear()
        {
            value = 0;
        }

        public override void Set(string valueStr, string indexStr)
        {
            ConstParser.TryParse(valueStr, out value);
        }

        public override DDLValueType Clone()
        {
            var ret = new DDLFloat();
            ret.value = this.value;
            return ret;
        }
    }

    class DDLArray<T> : DDLValueType where T : DDLValueType, new()
    {
        T[] arr;
        public T GetValue(int index)
        {
            return arr[index];
        }

        public DDLArray(int length)
        {
            arr = new T[length];
            for (int i = 0; i < length; i++)
                arr[i] = new T();
        }

        public override void Clear()
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i].Clear();
        }

        public override void Set(string valueStr, string indexStr)
        {
            int index;
            if (ConstParser.TryParse(indexStr, out index))
                if (index >= 0 && index < arr.Length)
                    arr[index].Set(valueStr, null);
        }


        public override DDLValueType Clone()
        {
            var ret = new DDLArray<T>(arr.Length);
            for (int i = 0; i < arr.Length; i++)
                ret.arr[i] = (T)arr[i].Clone();
            return ret;
        }
    }
}
