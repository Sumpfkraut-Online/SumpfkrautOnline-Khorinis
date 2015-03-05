using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCCamera : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {
            CamMatrix = 164,

        }


        public enum FuncOffsets : uint
        {
            Project = 0x0057A440,
            Project_Float = 0x00530030
        }

        public enum HookSize : uint
        {
            
        }
        #endregion

        #region Standard
        public zCCamera(Process process, int address)
            : base(process, address)
        {
            
        }

        public zCCamera()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        public static zCCamera getActiveCamera(Process process)
        {
            return new zCCamera(process, process.ReadInt(0x008D7F94));
        }
        #endregion

        #region Fields

        public zMat4 CamMatrix { get { return new zMat4(Process, Address + (int)Offsets.CamMatrix); } }

        #endregion



        #region methods
        public void Project(float x, float y, float z, out int oX, out int oY)
        {
            zVec3 in1 = zVec3.Create(Process);
            in1.X = x;
            in1.Y = y;
            in1.Z = z;

            Project(in1, out oX, out oY);
            in1.Dispose();
        }

        public void Project(zVec3 in1, out int oX, out int oY)
        {
            IntPtr out1 = Process.Alloc(4);
            IntPtr out2 = Process.Alloc(4);


            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Project, new CallValue[] { in1, new IntArg(out1.ToInt32()), new IntArg(out2.ToInt32()) });

            oX = Process.ReadInt(out1.ToInt32());
            oY = Process.ReadInt(out2.ToInt32());

            Process.Free(out1, 4);
            Process.Free(out2, 4);
        }

        public void Project(zVec3 in1, out float oX, out float oY)
        {
            IntPtr out1 = Process.Alloc(4);
            IntPtr out2 = Process.Alloc(4);


            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Project_Float, new CallValue[] { in1, new IntArg(out1.ToInt32()), new IntArg(out2.ToInt32()) });

            oX = Process.ReadFloat(out1.ToInt32());
            oY = Process.ReadFloat(out2.ToInt32());

            Process.Free(out1, 4);
            Process.Free(out2, 4);
        }
        #endregion
    }
}
