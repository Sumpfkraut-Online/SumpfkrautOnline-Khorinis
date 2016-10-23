using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Scripts.Sumpfkraut.Daedalus
{
    abstract class DDLInstance
    {
        protected Dictionary<string, DDLValueType> properties;

        public DDLInstance(DDLInstance prototype)
        {
            properties = new Dictionary<string, DDLValueType>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<string, DDLValueType> pair in prototype.properties)
            {
                properties.Add(pair.Key, pair.Value.Clone());
            }
        }

        public DDLInstance(Dictionary<string, DDLValueType> properties)
        {
            if (properties == null)
                throw new ArgumentNullException("Properties is null!");

            this.properties = properties;
        }

        public bool TryGetValue(string s, out DDLValueType value)
        {
            return properties.TryGetValue(s, out value);
        }

        public bool TryGetValue<T>(string s, out T value) where T : DDLValueType
        {
            DDLValueType res;
            if (properties.TryGetValue(s, out res) && res is T)
            {
                value = (T)res;
                return true;
            }
            value = default(T);
            return false;
        }

        public virtual void HandleEquation(string propertyName, string valueStr, string indexStr)
        {
            DDLValueType value;
            if (properties.TryGetValue(propertyName, out value))
            {
                value.Set(valueStr, indexStr);
            }
        }

        public void HandleTextBody(string text)
        {
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(ms, Encoding.Default))
            {
                sw.Write(text);
                sw.Flush();
                ms.Position = 0;

                using (Parser parser = new Parser(ms))
                {
                    Tuple<string, string> func;
                    Tuple<string, string, string> equation;
                    while (parser.ReadNextCommand(out func, out equation))
                    {
                        if (func != null)
                        {
                            HandleFunc(func.Item1, func.Item2);
                        }
                        else if (equation != null)
                        {
                            HandleEquation(equation.Item1, equation.Item2, equation.Item3);
                        }
                    }
                }
            }
        }

        protected virtual void HandleFunc(string funcName, string args)
        {
            string funcBody;
            if (FuncParser.TryGetFunc(funcName, out funcBody))
            {
                HandleTextBody(funcBody);
            }
        }
    }

    class DDLItem : DDLInstance
    {
        public DDLItem(DDLItem prototype) : base(prototype)
        {
        }

        public DDLItem() : base(new Dictionary<string, DDLValueType>(StringComparer.OrdinalIgnoreCase)
            {
                { "name", new DDLString() },
                { "mainflag", new DDLInt() },
                { "flags", new DDLInt() },
                { "damage", new DDLArray<DDLInt>(8) },
                { "protection", new DDLArray<DDLInt>(8) },
                { "visual", new DDLString() },
                { "visual_Change", new DDLString() },
                { "effect", new DDLString() },
                { "material", new DDLInt() },
            })
        {
        }
    }

    class DDLNpc : DDLInstance
    {
        public DDLNpc(DDLNpc prototype) : base(prototype)
        {
            this.equipment = new List<string>(prototype.equipment);
        }

        public DDLNpc() : base(new Dictionary<string, DDLValueType>(StringComparer.OrdinalIgnoreCase)
            {
                { "name", new DDLString() },
                { "attribute", new DDLArray<DDLInt>(8) },
                { "protection", new DDLArray<DDLInt>(8) },
                { "visual", new DDLString() },
                { "bodymesh", new DDLString() },
                { "bodytex", new DDLInt() },
                { "headmesh", new DDLString() },
                { "headtex", new DDLInt() },
                { "fatness", new DDLInt() },
                { "overlay", new DDLString() },
                { "armor", new DDLString() },
            })
        {
            equipment = new List<string>();
        }

        List<string> equipment;
        public string[] Equipment { get { return equipment.ToArray(); } }

        protected override void HandleFunc(string funcName, string args)
        {
            string[] strs = args.Split(new char[] { ' ', ',', (char)0x9 }, StringSplitOptions.RemoveEmptyEntries);
            if (string.Compare(funcName, "Mdl_SetVisual", true) == 0)
            {
                if (strs.Length == 2)
                {
                    properties["visual"].Set(strs[1], null);
                }
            }
            else if (string.Compare(funcName, "Mdl_SetVisualBody", true) == 0)
            {
                if (strs.Length == 8)
                {
                    properties["bodymesh"].Set(strs[1], null);
                    properties["bodytex"].Set(strs[2], null);
                    properties["headmesh"].Set(strs[4], null);
                    properties["headtex"].Set(strs[5], null);
                }
            }
            else if (string.Compare(funcName, "Mdl_SetModelFatness", true) == 0)
            {
                if (strs.Length == 2)
                {
                    properties["fatness"].Set(strs[1], null);
                }
            }
            else if (string.Compare(funcName, "B_SetNpcVisual", true) == 0)
            {
                if (strs.Length == 6)
                {
                    properties["visual"].Set("\"humans.mds\"", null);
                    int gender, bodyTex;
                    ConstParser.TryParse(strs[1], out gender);
                    ConstParser.TryParse(strs[4], out bodyTex);

                    if (gender == 0) // MALE
                    {
                        properties["bodymesh"].Set("\"hum_body_Naked0\"", null);
                    }
                    else
                    {
                        if (bodyTex >= 0 && bodyTex <= 3)
                            bodyTex += 4;

                        properties["bodymesh"].Set("\"Hum_Body_Babe0\"", null);
                    }

                    properties["bodytex"].Set(bodyTex.ToString(), null);
                    properties["headmesh"].Set(strs[2], null);
                    properties["headtex"].Set(strs[3], null);
                    properties["armor"].Set(string.Format("\"{0}\"", strs[5]), null); // item instance
                }
            }
            else if (string.Compare(funcName, "Mdl_ApplyOverlayMds", true) == 0)
            {
                if (strs.Length == 2)
                {
                    properties["overlay"].Set(strs[1], null);
                }
            }
            else if (string.Compare(funcName, "EquipItem", true) == 0)
            {
                if (strs.Length == 2 && !string.IsNullOrWhiteSpace(strs[1]))
                {
                    equipment.Add(strs[1]);
                }
            }
            else
            {
                base.HandleFunc(funcName, args);
            }
        }
    }
}
