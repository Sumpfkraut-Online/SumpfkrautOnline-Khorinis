using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{

    public partial class VobInstLoader : ExtendedObject
    {

        new public static readonly string _staticName = "VobInstLoader (s)";

        partial void pLoad ();
        partial void pSave ();
        partial void pCreate ();

    }

}
