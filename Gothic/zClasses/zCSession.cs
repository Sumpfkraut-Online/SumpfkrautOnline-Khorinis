using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSession : zCInputCallback
    {
        #region OffsetLists
        public enum Offsets : uint
        {
            vtbl = 0,
            zCCSManager = 4,
            world = 8,
            camera = 12,
            aiCamera = 16,
            camVob = 20,
            viewPort = 24
        }

        public enum FuncOffsets : uint
        {

        }

        public enum HookSize : uint
        {

        }
        #endregion

        #region Standard
        public zCSession(Process process, int address) : base (process, address)
        {
            
        }

        public zCSession()
        {

        }
        #endregion

        #region statics
        

        #endregion

        #region Fields
        public int VBTL { get { return Process.ReadInt(this.Address + (int)Offsets.vtbl); } }

        public zCWorld World { get { return new zCWorld(Process, Process.ReadInt(this.Address + (int)Offsets.world)); } }
        public zCAICamera AICamera { get { return new zCAICamera(Process, Process.ReadInt(this.Address + (int)Offsets.aiCamera)); } }
        public zCCamera Camera { get { return new zCCamera(Process, Process.ReadInt(this.Address + (int)Offsets.camera)); } }

        public zCVob CamVob { get { return new zCVob(Process, Process.ReadInt(this.Address + (int)Offsets.camVob)); } }
        public zCView ViewPort { get { return new zCView(Process, Process.ReadInt(this.Address + (int)Offsets.viewPort)); } }
        #endregion

        #region methods

        #endregion
    }
}
