using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{

    public partial class WorldDef : ExtendedObject
    {

        new public static readonly string _staticName = "WorldDef (static)";

        protected WorldLoader loader = null;
        public WorldLoader Loader { get { return this.loader; } }



        public WorldDef (WorldLoader loader)
        {
            SetObjName("WorldLoader (default)");
            this.loader = loader;
        }

    }

}
