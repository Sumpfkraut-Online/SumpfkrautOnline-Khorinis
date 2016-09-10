using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs
{
    public abstract class AniCatalog
    {
        protected virtual Dictionary<string, string> aniDict { get; }

        public bool AddJob(ScriptAniJob job)
        {
            if (aniDict != null)
            {
                string propName;
                if (aniDict.TryGetValue(job.AniName, out propName))
                {
                    AniCatalog catalog;
                    PropertyInfo prop = FindProperty(propName, out catalog);
                    if (prop != null)
                    {
                        catalog.SetProperty(prop, job, false);
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
                if (aniDict.TryGetValue(job.AniName, out propName))
                {
                    AniCatalog catalog;
                    PropertyInfo prop = FindProperty(propName, out catalog);
                    if (prop != null)
                    {
                        catalog.SetProperty(prop, job, true);
                    }
                }
            }
            return false;
        }

        public bool ContainsPropertyForJob(ScriptAniJob job)
        {
            if (aniDict != null)
            {
                string propName;
                if (aniDict.TryGetValue(job.AniName, out propName))
                {
                    AniCatalog catalog;
                    return FindProperty(propName, out catalog) != null;
                }
            }
            return false;
        }

        const BindingFlags searchFlags = BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.IgnoreCase | BindingFlags.Instance;
        PropertyInfo FindProperty(string propName, out AniCatalog catalog)
        {
            int cutIndex = propName.IndexOf('.');
            if (cutIndex != -1)
            {
                PropertyInfo subProp = this.GetType().GetProperty(propName.Remove(cutIndex), searchFlags);
                if (subProp != null && subProp.PropertyType.BaseType == typeof(AniCatalog)) // sub catalog
                {
                    AniCatalog subCatalog = (AniCatalog)subProp.GetValue(this, null);
                    return subCatalog.FindProperty(propName.Substring(cutIndex + 1), out catalog);
                }
            }

            catalog = this;
            PropertyInfo prop = this.GetType().GetProperty(propName, searchFlags);
            if (prop != null && (prop.PropertyType == typeof(ScriptAniJob)))
            {
                return prop;
            }
            return null;
        }

        void SetProperty(PropertyInfo prop, ScriptAniJob value, bool remove)
        {
            if (remove)
            {
                ScriptAniJob other = (ScriptAniJob)prop.GetValue(this, null);
                if (other == value)
                {
                    prop.SetValue(this, null, null);
                }
                return;
            }
            else
            {
                prop.SetValue(this, value, null);
            }
        }
    }
}
