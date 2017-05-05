
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.CommandConsole.InfoObjects
{
    public class VobInfo : GUC.Utilities.ExtendedObject
    {

        new public static readonly string _staticName = "VobInfo (s)";



        public VobInfo ()
        {
            this._objName = "VobInfo (default)";
        }



        public Vec3f Direction;
        public Vec3f Position;
        public int VobID;
        public VobTypes VobType;

    }
}
