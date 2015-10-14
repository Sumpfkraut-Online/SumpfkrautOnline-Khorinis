using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCModel : zCVisual, IDisposable
    {
        #region OffsetLists
        public enum Offsets : uint 
        {
            Owner = 0x60,
            ModelPrototype = 0x64,
            BBox3D = 216,
            BBox3DLocal = 240
        }

        public enum FuncOffsets : uint
        {
            ShowAniList = 0x00582770,
            StartAni_ModelInt = 0x0057B0C0,
            StartAni_IntInt = 0x0057B070,
            StartAni_StrInt = 0x0057AF70,
            StopAni_Int = 0x0057ABE0,
            StopAni_AktivAni = 0x0057ACB0,
            StopAni_Ani = 0x0057AC60,
            GetActiveAni = 0x0057ABA0,
            GetActiveAni_ID = 0x0057AB60,
            GetAniFromAniID = 0x00472F50,
            FadeOutAni_AktivAni = 0x0057F020,
            FadeOutAnisLayerRange = 0x0057F1F0,
            StopAnisLayerRange = 0x0057F240,
            FadeOutAni_Int = 0x0057EF50,
            GetVisualName = 0x0057DF60,
            AdvanceAnis = 0x0057CA90,
            StartAnimation = 0x005765E0,
            StopAnimation = 0x005765F0,
            IsAnimationActive = 0x00576690,
            LoadVisualVirtual = 0x00578760,
            GetAniIDFromAniName = 0x00612070,
            RemoveMeshLibAll = 0x0057E3D0,
            SetModelScale = 0x0057DC30
        }

        public enum FuncSize : uint
        {
            ShowAniList = 9,
            StartAni_ModelInt = 5,
            StartAni_IntInt = 6,
            StopAni_Int = 6,
            StopAni_AktivAni = 6,
            StopAni_Ani = 5,
            GetActiveAni = 5,
            GetActiveAni_ID = 5,
            GetAniFromAniID= 7,
            FadeOutAni_AktivAni = 5,
            FadeOutAni_Int = 6,
            FadeOutAnisLayerRange = 7,
            StopAnisLayerRange = 7,
            GetVisualName=5,
            AdvanceAnis=6
        }
        #endregion


        public zCModel()
        {

        }

        public zCModel(Process process, int address)
            : base(process, address)
        {

        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x00577CC0, new CallValue[] { });
                disposed = true;
            }
        }

        public zCModelPrototype ModelPrototype
        {
            get { return new zCModelPrototype(Process, Process.ReadInt(Process.ReadInt(Address + (int)Offsets.ModelPrototype))); }
            set { Process.Write(value.Address, Address + (int)Offsets.ModelPrototype); }
        }

        public zCVob Owner
        {
            get { return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.Owner)); }
        }

        public zTBBox3D BBox3D
        {
            get { return new zTBBox3D(Process, Address + (int)Offsets.BBox3D); }
        }

        public zTBBox3D BBox3DLocal
        {
            get { return new zTBBox3D(Process, Address + (int)Offsets.BBox3DLocal); }
        }



        public void SetModelScale(float[] scale)
        {
            if (scale.Length != 3)
                return;

            using (zVec3 vec = zVec3.Create(Process))
            {
                vec.X = scale[0];
                vec.Y = scale[1];
                vec.Z = scale[2];

                SetModelScale(vec);
            }
        }

        public void SetModelScale(zVec3 scale)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetModelScale, new CallValue[] { scale });
        }

        public void LoadVisualVirtual(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.LoadVisualVirtual, new CallValue[] { str });
        }


        public void StartAnimation(String str)
        {
            zString zStr = zString.Create(Process, str);
            StartAnimation(zStr);
            zStr.Dispose();
        }
        public void StartAnimation(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartAnimation, new CallValue[] { str });
        }

        public void RemoveMeshLibAll()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RemoveMeshLibAll, new CallValue[] {  });
        }

        public void StopAnimation(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StopAnimation, new CallValue[] { str });
        }

        public void ShowAniList( int id )
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ShowAniList, new CallValue[] { new IntArg(id) });
        }

        public void AdvanceAnis()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.AdvanceAnis, new CallValue[] {  });
        }

        public void StartAni(zString ani, int id)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartAni_StrInt, new CallValue[] { ani, new IntArg(id) });
        }

        public void StartAni(zCModelAni ani, int id)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartAni_ModelInt, new CallValue[] { ani, new IntArg(id) });
        }

        public void StopAnisLayerRange(int ani, int id)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StopAnisLayerRange, new CallValue[] { new IntArg(ani), new IntArg(id) });
        }
        public void FadeOutAnisLayerRange(int ani, int id)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.FadeOutAnisLayerRange, new CallValue[] { new IntArg(ani), new IntArg(id) });
        }

        public void StartAni(int ani, int id)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartAni_IntInt, new CallValue[] { new IntArg(ani), new IntArg(id) });
        }

        public void StopAni(int ani)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StopAni_Int, new CallValue[] { new IntArg(ani) });
        }

        public void FadeOutAni(int ani)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.FadeOutAni_Int, new CallValue[] { new IntArg(ani) });
        }
        public int IsAnimationActive(String animname)
        {
            int result = 0;

            zString str = zString.Create(Process, animname);
            result = IsAnimationActive(str);
            str.Dispose();

            return result;
        }
        public int IsAnimationActive(zString animname)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsAnimationActive, new CallValue[] { animname });
        }

        public bool IsAniActive(zCModelAni ani)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)0x472F90, new CallValue[] { ani }) > 0;
        }

        public int GetAniIDFromAniName(String animname)
        {
            zString str = zString.Create(Process, animname);
            int x = GetAniIDFromAniName(str);
            str.Dispose();
            return x;
        }

        public int GetAniIDFromAniName(zString animname)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetAniIDFromAniName, new CallValue[] { animname });
        }

        public zCModelAniActive GetActiveAni(zCModelAni ani)
        {
            return Process.THISCALL<zCModelAniActive>((uint)Address, (uint)FuncOffsets.GetActiveAni, new CallValue[] { ani });
        }

        public zCModelAniActive GetActiveAni(int id)
        {
            return Process.THISCALL<zCModelAniActive>((uint)Address, (uint)FuncOffsets.GetActiveAni_ID, new CallValue[] { new IntArg(id) });
        }

        public zCModelAni GetAniFromAniID(int id)
        {
            return Process.THISCALL<zCModelAni>((uint)Address, (uint)FuncOffsets.GetAniFromAniID, new CallValue[] { new IntArg(id) });
        }

        public zString GetVisualNameZString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetVisualName, new CallValue[] { new IntArg(str) });
            return new zString(Process, arg.Address);
        }

        public String GetVisualName()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetVisualName, new CallValue[] { new IntArg(str) });
            zString zString = new zString(Process, arg.Address);

            String v = null;
            if (zString.Length < 500)
                v = zString.Value.Trim();
            zString.Dispose();

            return v;
        }

        

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
