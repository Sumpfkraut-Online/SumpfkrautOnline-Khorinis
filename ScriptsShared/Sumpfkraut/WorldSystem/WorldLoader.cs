using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldLoader : ExtendedObject
    {

        new public static readonly string _staticName = "WorldLoader (static)";



        partial void pLoad ();
        partial void pCreate ();

    }
}
