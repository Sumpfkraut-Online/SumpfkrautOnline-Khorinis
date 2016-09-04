using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic;
using Gothic.Objects;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class World
    {
        internal static World current;
        public static World Current { get { return current; } }

        #region ScriptObject

        public partial interface IScriptWorld : IScriptGameObject
        {
            void Load();
        }

        #endregion

        #region Properties

        /// <summary> The correlating gothic-object of this world. </summary>
        public zCWorld gWorld { get { return oCGame.GetWorld(); } }

        #endregion

        #region Gothic-Object Address Dictionary

        // Dictionary with all addresses of gothic-objects in this world.
        Dictionary<int, BaseVob> vobAddr = new Dictionary<int, BaseVob>();

        public bool TryGetVobByAddress(int address, out BaseVob vob)
        {
            return vobAddr.TryGetValue(address, out vob);
        }

        public bool TryGetVobByAddress<T>(int address, out T vob) where T : BaseVob
        {
            BaseVob v;
            if (vobAddr.TryGetValue(address, out v))
            {
                if (v is T)
                {
                    vob = (T)v;
                    return true;
                }
            }
            vob = null;
            return false;
        }

        #endregion

        #region Add & Remove

        partial void pAfterAddVob(BaseVob vob)
        {
            // add the vob to the gothic world
            gWorld.AddVob(vob.gVob);

            // add the gothic-object's address to the dictionary
            vobAddr.Add(vob.gVob.Address, vob);
        }

        partial void pBeforeRemoveVob(BaseVob vob)
        {
            var gVob = vob.gVob;

            // update position & direction one last time
            vob.GetPosition();
            vob.GetDirection();

            // remove gothic-object from the gothic-world
            gWorld.RemoveVob(gVob);

            // remove the gothic-object's address from the dictionary
            vobAddr.Remove(gVob.Address);
        }

        #endregion
    }
}
