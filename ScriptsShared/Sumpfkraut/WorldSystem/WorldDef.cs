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

        new public static readonly string _staticName = "WorldDef (s)";

        protected WorldLoader loader = null;
        public WorldLoader Loader { get { return this.loader; } }

        protected WorldObjects.World baseWorld;
        public WorldObjects.World BaseWorld { get { return baseWorld; } }



        public WorldDef (WorldLoader loader)
            : this(loader, "WorldDef (default)")
        { }

        public WorldDef (WorldLoader loader, string objName)
        {
            SetObjName(objName);
            this.loader = loader;
        }



        partial void pCreate ();
        public void Create ()
        {
            baseWorld.Create();
            pCreate();
        }

        partial void pDelete ();
        public void Delete ()
        {
            baseWorld.Delete();
            pDelete();
        }

    }

}
