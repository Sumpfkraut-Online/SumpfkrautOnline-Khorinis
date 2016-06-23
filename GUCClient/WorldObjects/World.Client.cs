using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class World
    {
        #region ScriptObject

        public partial interface IScriptWorld : IScriptGameObject
        {
            void Load();
        }

        #endregion

        internal static World current = null;
        public static World Current { get { return World.current; } }

        Dictionary<int, BaseVob> vobAddr = new Dictionary<int, BaseVob>();

        public bool ContainsVobAddress(int address)
        {
            return vobAddr.ContainsKey(address);
        }

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

        partial void pAddVob(BaseVob vob)
        {
            Vec3f pos = vob.GetPosition();
            Vec3f dir = vob.GetDirection();

            vob.gvob = vob.Instance.CreateVob(vob.gvob);

            oCGame.GetWorld().AddVob(vob.gVob);
            vobAddr.Add(vob.gvob.Address, vob);

            vob.SetPosition(pos);
            vob.SetDirection(dir);
        }

        partial void pRemoveVob(BaseVob vob)
        {
            var gVob = vob.gvob;

            // update position & direction a last time
            vob.GetPosition();
            vob.GetDirection();

            // remove vob from gothic world
            oCGame.GetWorld().RemoveVob(gVob);

            // remove vob from guc vob address dictionary
            vobAddr.Remove(gVob.Address);

            vob.gvob = null;

            // FIXME: Free allocated memory
        }
    }
}
