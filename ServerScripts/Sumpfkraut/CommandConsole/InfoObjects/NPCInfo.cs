using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole.InfoObjects
{
    public class NpcInfo : ScriptObject
    {

        new public static readonly String _staticName = "NpcInfo (static)";



        public NpcInfo ()
        {
            this._objName = "NpcInfo (default)";
        }



        public int id;
        public String npcName;
        public String mapName;
        public Vec3f direction;
        public Vec3f position;

    }
}
