using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    /// <summary>
    /// Serverside wrapper class for BaseVobDef.
    /// </summary>
    public abstract class SBaseVobDef
    {
        
        /// <summary>
        /// Access all existing and registered vob definitions by their name.
        /// </summary>
        private static Dictionary<string, SBaseVobDef> nameToVobDef = 
            new Dictionary<string, SBaseVobDef>(StringComparer.OrdinalIgnoreCase);



        /// <summary>
        /// 
        /// </summary>
        BaseVobDef shared;

        string codeName;
        public string CodeName
        {
            get { return codeName; }
            set
            {
                if (shared.IsCreated)
                {
                    throw new NotSupportedException(
                        "Can't change CodeName when the Definition is already added to the static collection!");
                }
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("CodeName is null or white space!");
                }
                this.codeName = value;
            }
        }



        public SBaseVobDef (string codeName)
        {
            //shared = new BaseVobDef();
            this.CodeName = codeName;
        }



        public static bool TryGetVobDef <T> (string codeName, out T vobDef) 
            where T : SBaseVobDef
        {
            vobDef = GetVobDef<T>(codeName);
            return vobDef != null;
        }

        public static T GetVobDef <T> (string codeName) where T : SBaseVobDef
        {
            SBaseVobDef value;
            if (nameToVobDef.TryGetValue(codeName, out value) && value is T)
            {
                return (T)value;
            }
            return null;
        }

        public void Create ()
        {
            nameToVobDef.Add(this.CodeName, this);
        }

        public void Delete ()
        {
            nameToVobDef.Remove(this.CodeName);
        }
    }
}
