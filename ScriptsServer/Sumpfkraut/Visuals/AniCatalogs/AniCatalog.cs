using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs
{
    public interface AniCatalog
    {
        void AddJob(ScriptAniJob job);
        void RemoveJob(ScriptAniJob job);
    }
}
