using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects.Meshes
{
    public class zCModel : zCVisual
    {
        new public abstract class VarOffsets : zCVisual.VarOffsets
        {
            public const int Owner = 0x60,
            ModelPrototype = 0x64,
            BBox3D = 216,
            BBox3DLocal = 240;
        }

        new public abstract class FuncAddresses : zCVisual.FuncAddresses
        {
            public const int ShowAniList = 0x00582770,
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
            SetModelScale = 0x0057DC30;
        }

        /*public enum FuncSize : uint
        {
            ShowAniList = 9,
            StartAni_ModelInt = 5,
            StartAni_IntInt = 6,
            StopAni_Int = 6,
            StopAni_AktivAni = 6,
            StopAni_Ani = 5,
            GetActiveAni = 5,
            GetActiveAni_ID = 5,
            GetAniFromAniID = 7,
            FadeOutAni_AktivAni = 5,
            FadeOutAni_Int = 6,
            FadeOutAnisLayerRange = 7,
            StopAnisLayerRange = 7,
            GetVisualName = 5,
            AdvanceAnis = 6
        }*/

        public zCModel()
        {

        }

        public zCModel(int address)
            : base(address)
        {

        }

        public zCModelPrototype ModelPrototype
        {
            get { return new zCModelPrototype(Process.ReadInt(Address + VarOffsets.ModelPrototype)); }
            //set { Process.Write(value.Address, Address + VarOffsets.ModelPrototype); }
        }

        public zCVob Owner
        {
            get { return new zCVob(Process.ReadInt(Address + VarOffsets.Owner)); }
        }

        public zTBBox3D BBox3D
        {
            get { return new zTBBox3D(Address + VarOffsets.BBox3D); }
        }

        public zTBBox3D BBox3DLocal
        {
            get { return new zTBBox3D(Address + VarOffsets.BBox3DLocal); }
        }



        public void SetModelScale(float[] scale)
        {
            if (scale.Length != 3)
                return;

            using (zVec3 vec = zVec3.Create())
            {
                vec.X = scale[0];
                vec.Y = scale[1];
                vec.Z = scale[2];

                SetModelScale(vec);
            }
        }

        public void SetModelScale(zVec3 scale)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetModelScale, scale);
        }

        public void LoadVisualVirtual(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.LoadVisualVirtual, str);
        }


        public void StartAnimation(String str)
        {
            using (zString zStr = zString.Create(str))
                StartAnimation(zStr);
        }
        public void StartAnimation(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartAnimation, str);
        }

        public void RemoveMeshLibAll()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RemoveMeshLibAll);
        }

        public void StopAnimation(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StopAnimation, str);
        }

        public void ShowAniList(int id)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ShowAniList, new IntArg(id));
        }

        public void AdvanceAnis()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.AdvanceAnis);
        }

        public void StartAni(string ani, int id)
        {
            using (zString z = zString.Create(ani))
                StartAni(z, id);
        }

        public void StartAni(zString ani, int id)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartAni_StrInt, ani, new IntArg(id));
        }

        public void StartAni(zCModelAni ani, int id)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartAni_ModelInt, ani, new IntArg(id));
        }

        public void StopAnisLayerRange(int ani, int id)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StopAnisLayerRange, new IntArg(ani), new IntArg(id));
        }
        public void FadeOutAnisLayerRange(int ani, int id)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.FadeOutAnisLayerRange, new IntArg(ani), new IntArg(id));
        }

        public void StartAni(int ani, int id)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartAni_IntInt, new IntArg(ani), new IntArg(id));
        }

        public void StopAni(int ani)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StopAni_Int, new IntArg(ani));
        }

        public void StopAni(zCModelAniActive ani)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StopAni_AktivAni, ani);
        }

        public void FadeOutAni(int ani)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.FadeOutAni_Int, new IntArg(ani));
        }

        public void FadeOutAni(zCModelAniActive ani)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.FadeOutAni_AktivAni, ani);
        }

        public int IsAnimationActive(String animname)
        {
            int result = 0;

            using (zString str = zString.Create(animname))
                result = IsAnimationActive(str);

            return result;
        }

        public int IsAnimationActive(zString animname)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.IsAnimationActive, animname);
        }

        public bool IsAniActive(zCModelAni ani)
        {
            return Process.THISCALL<BoolArg>(Address, 0x472F90, ani).Value;
        }

        public int GetAniIDFromAniName(String animname)
        {
            int x;
            using (zString str = zString.Create(animname))
                x = GetAniIDFromAniName(str);
            return x;
        }

        public int GetAniIDFromAniName(zString animname)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetAniIDFromAniName, animname);
        }

        public zCModelAniActive GetActiveAni(zCModelAni ani)
        {
            return Process.THISCALL<zCModelAniActive>(Address, FuncAddresses.GetActiveAni, ani);
        }

        public zCModelAniActive GetActiveAni(int id)
        {
            return Process.THISCALL<zCModelAniActive>(Address, FuncAddresses.GetActiveAni_ID, new IntArg(id));
        }

        public zCModelAni GetAniFromAniID(int id)
        {
            return Process.THISCALL<zCModelAni>(Address, FuncAddresses.GetAniFromAniID, new IntArg(id));
        }

        public zString GetVisualNameZString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>(Address, FuncAddresses.GetVisualName, new IntArg(str));
            return new zString(arg.Value);
        }

        public String GetVisualName()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>(Address, FuncAddresses.GetVisualName, new IntArg(str));
            zString zString = new zString(arg.Value);

            String v = null;
            if (zString.Length < 500)
                v = zString.ToString().Trim();
            zString.Dispose();

            return v;
        }
    }
}
