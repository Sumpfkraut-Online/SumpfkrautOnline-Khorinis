using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs
{
    public abstract class AniCatalog
    {
        public class AniJobCollection : IEnumerable<ScriptAniJob>
        {
            Dictionary<int, ScriptAniJob> dict = new Dictionary<int, ScriptAniJob>();

            public ScriptAniJob this[int index]
            {
                get
                {
                    ScriptAniJob job;
                    return dict.TryGetValue(index, out job) ? job : null;
                }
                set
                {
                    if (dict.ContainsKey(index))
                    {
                        if (value == null) dict.Remove(index);
                        else dict[index] = value;
                    }
                    else if (value != null)
                    {
                        dict.Add(index, value);
                    }
                }
            }

            public int Count { get { return dict.Count; } }

            public IEnumerator<ScriptAniJob> GetEnumerator()
            {
                return dict.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return dict.Values.GetEnumerator();
            }
        }

        const BindingFlags searchFlags = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Instance;

        #region Constructor

        public AniCatalog()
        {
            // Fills all properties with objects
            foreach (var prop in this.GetType().GetProperties(searchFlags))
            {
                Type type = prop.PropertyType;
                if (type.IsSubclassOf(typeof(AniCatalog)) || type == typeof(AniJobCollection))
                {
                    var ci = type.GetConstructor(Type.EmptyTypes);
                    var o = ci.Invoke(null);
                    prop.SetValue(this, o, null);
                }
            }
        }

        #endregion

        protected virtual Dictionary<string, string> aniDict { get; }

        #region Add & Remove

        public bool AddJob(ScriptAniJob job)
        {
            if (aniDict != null)
            {
                string propName;
                if (aniDict.TryGetValue(job.CodeName, out propName))
                {
                    int collectionIndex;
                    AniCatalog catalog;
                    PropertyInfo prop = FindProperty(propName, out catalog, out collectionIndex);
                    if (prop != null)
                    {
                        catalog.SetProperty(prop, job, collectionIndex);
                    }
                    else
                    {
                        Log.Logger.Log("AniParser: Couldn't find property '{0}' for animation '{1}'.", propName, job.CodeName);
                    }
                }
            }
            return false;
        }

        public bool RemoveJob(ScriptAniJob job)
        {
            if (aniDict != null)
            {
                string propName;
                if (aniDict.TryGetValue(job.CodeName, out propName))
                {
                    int collectionIndex;
                    AniCatalog catalog;
                    PropertyInfo prop = FindProperty(propName, out catalog, out collectionIndex);
                    if (prop != null)
                    {
                        catalog.RemoveProperty(prop, job, collectionIndex);
                    }
                }
            }
            return false;
        }

        #endregion

        #region Property search

        public bool ContainsPropertyForJob(ScriptAniJob job)
        {
            if (aniDict != null)
            {
                string propName;
                if (aniDict.TryGetValue(job.CodeName, out propName))
                {
                    AniCatalog catalog;
                    int collectionIndex;
                    return FindProperty(propName, out catalog, out collectionIndex) != null;
                }
            }
            return false;
        }

        PropertyInfo FindProperty(string propName, out AniCatalog catalog, out int index)
        {
            int cutIndex = propName.IndexOf('.');
            if (cutIndex != -1) // sub catalog
            {
                PropertyInfo subProp = this.GetType().GetProperty(propName.Remove(cutIndex), searchFlags);
                if (subProp != null && subProp.PropertyType.BaseType == typeof(AniCatalog)) // sub catalog
                {
                    AniCatalog subCatalog = (AniCatalog)subProp.GetValue(this, null);
                    return subCatalog.FindProperty(propName.Substring(cutIndex + 1), out catalog, out index);
                }
            }

            catalog = this;
            
            cutIndex = propName.IndexOf('[');
            if (cutIndex != -1) // job collection
            {
                string num = propName.Substring(cutIndex + 1, propName.IndexOf(']') - (cutIndex + 1));
                if (!int.TryParse(num, out index))
                    return null;

                PropertyInfo prop = this.GetType().GetProperty(propName.Remove(cutIndex), searchFlags);
                if (prop != null && prop.PropertyType == typeof(AniJobCollection))
                {
                    return prop;
                }
            }
            else
            {
                PropertyInfo prop = this.GetType().GetProperty(propName, searchFlags);
                if (prop != null && prop.PropertyType == typeof(ScriptAniJob))
                {
                    index = 0;
                    return prop;
                }
            }
            index = 0;
            return null;
        }

        #endregion

        #region Set Properties

        void SetProperty(PropertyInfo prop, ScriptAniJob value, int index)
        {
            if (prop == null)
                return;

            if (prop.PropertyType == typeof(ScriptAniJob))
            {
                prop.SetValue(this, value, null);
            }
            else if (prop.PropertyType == typeof(AniJobCollection))
            {
                var collection = (AniJobCollection)prop.GetValue(this, null);
                collection[index] = value;
            }
        }

        void RemoveProperty(PropertyInfo prop, ScriptAniJob value, int index)
        {
            if (prop == null)
                return;

            if (prop.PropertyType == typeof(ScriptAniJob))
            {
                ScriptAniJob other = (ScriptAniJob)prop.GetValue(this, null);
                if (other == value)
                {
                    prop.SetValue(this, null, null);
                }
            }
            else if (prop.PropertyType == typeof(AniJobCollection))
            {
                var collection = (AniJobCollection)prop.GetValue(this, null);
                if (collection[index] == value)
                    collection[index] = null;
            }
        }

        #endregion
    }
}
