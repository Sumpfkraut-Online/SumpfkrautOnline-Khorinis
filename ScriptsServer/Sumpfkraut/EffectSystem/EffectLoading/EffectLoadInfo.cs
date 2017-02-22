using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem
{

    public struct EffectLoadInfo
    {

        public int id;
        public List<List<object>> changeRows;



        public EffectLoadInfo (int id, List<List<object>> changeRows = null)
        {
            this.id = id;
            this.changeRows = changeRows ?? new List<List<object>>();
        }

    }

}
