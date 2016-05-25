using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using RakNet;
using Gothic;
using Gothic.Objects;

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
            vob.gvob = vob.Instance.CreateVob(vob.gvob);

            oCGame.GetWorld().AddVob(vob.gVob);
            vobAddr.Add(vob.gvob.Address, vob);

            vob.SetPosition(vob.GetPosition());
            vob.SetDirection(vob.GetDirection());
        }

        partial void pRemoveVob(BaseVob vob)
        {
            var gVob = vob.gvob;
            oCGame.GetWorld().RemoveVob(gVob);
            vobAddr.Remove(gVob.Address);
        }
    }
}
