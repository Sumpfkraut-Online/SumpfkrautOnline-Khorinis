using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects.EventManager;
using Gothic.Objects.Meshes;

namespace Gothic.Objects
{
    public class zCVob : zCObject, IDisposable
    {
        new public abstract class VarOffsets : zCObject.VarOffsets
        {
            public const int globalVobTreeNode = 0x0024,
            lastTimeDrawn = 0x0028,
            lastTimeCollected = 0x002C,
            vobLeafList = 0x0030,//zCArray<zCBspLeaf*>
            trafoObjToWorld = 0x003C,//int[16]
            bbox3D = 0x007C,
            bsphere3D = 0x0094,
            touchVobList = 0x00A4,//zCArray<zCVob*>
            type = 0x00B0,
            groundShadowSizePacked = 0x00b4,
            homeWorld = 0x00B8,
            groundPoly = 0x00BC,
            callback_ai = 0x00C0,
            trafo = 0x00C4,
            visual = 0x00C8,
            visualAlpha = 0x00CC,
            vobFarClipZScale = 0x00D0,
            aniMode = 0x00D4,
            aniModeStrength = 0x00D8,
            zBias = 0x00DC,
            rigidBody = 0x00E0,
            lightColorStat = 0x00E4,
            lightColorDyn = 0x00E8,
            lightDirectionStat = 0x00EC,
            vobPresetName = 0x00F8,
            eventManager = 0x00FC,
            nextOnTimer = 0x0100,
            bitfield = 0x0104,
            CollisionObjectClass = 0x0118,
            CollisionObject = 0x011C;
        }

        public abstract class FuncAddresses
        {
            public const int RemoveVobFromWorld = 0x00601C40,
            GetEM = 0x005FFE10,
            SetPositionWorld = 0x0061BB70,
            SetAI = 0x005FE8F0,
            SetVisual = 0x6024F0,
            SetVisualStr = 0x00602680,
            ResetRotationsLocal = 0x0061BCF0,
            ResetRotationsWorld = 0x0061C000,
            RotateLocalY = 0x0061B720,
            RotateWorldY = 0x0061B830,
            SetHeadingLocal = 0x61C5E0,
            SetHeadingWorld = 0x61C6B0,
            SetHeadingAtLocal = 0x0061C860,
            SetHeadingAtWorld = 0x0061CBC0,
            SetHeadingYWorld = 0x61C280,
            SetHeadingYWorld_vob = 0x61C450,
            SetHeadingYLocal = 0x61C1B0,
            ResetXZRotationsWorld = 0x0061C090,
            BeginMovement = 0x0061DA80,
            EndMovement = 0x0061E0D0,
            RotateWorld = 0x0061B520,
            RotateWorldX = 0x0061B800,
            RotateWorldZ = 0x0061B860,
            RotateLocal = 0x0061B610,
            RotateLocalX = 0x0061B6B0,
            RotateLocalZ = 0x0061B790,
            GetSectorNameVobIsIn = 0x00600AE0,
            SetPhysicsEnabled = 0x0061D190,
            SetCollDetStat = 0x61CE50,
            SetCollDetDyn = 0x61CF40,
            MoveLocal = 0x61B3C0,
            MoveWorld = 0x61B350,
            SetSleeping = 0x602930;
        }

        /*public enum HookSize
        {
            RemoveVobFromWorld = 6,
            SetVisual = 6
        }*/

        public enum zTVobType
        {
            VOB,
            LIGHT = 1,
            SOUND = 2,
            STARTPOINT = 6,
            WAYPOINT = 7,
            MOB = 128,
            ITEM = 129,
            NPC = 130
        }

        public abstract class BitFlag0
        {
            public const int showVisual = 1,
            drawBBox3D = 1 << 1,
            visualAlphaEnabled = 1 << 2,
            physicsEnabled = 1 << 3,
            staticVob = 1 << 4,
            ignoredByTraceRay = 1 << 5,
            collDetectionStatic = 1 << 6,
            collDetectionDynamic = 1 << 7,
            castDynShadow = 1 << 8,
            lightColorStatDirty = 1 << 9,
            lightColorDynDirty = 1 << 10;
        }

        public int BitField1
        {
            get { return Process.ReadInt(Address + VarOffsets.bitfield); }
            set { Process.Write(Address + VarOffsets.bitfield, value); }
        }

        public int GroundPoly
        {
            get { return Process.ReadInt(Address + VarOffsets.groundPoly); }
            set { Process.Write(Address + VarOffsets.groundPoly, value); }
        }

        public zCCollisionObject CollObj
        {
            get { return new zCCollisionObject(Process.ReadInt(Address + VarOffsets.CollisionObject)); }
        }

        public const int ByteSize = 0x120;

        public zCVob(int address)
            : base(address)
        {
        }

        public zCVob()
        {
        }

        public void SetHeadingYLocal(float x, float y, float z)
        {
            using (zVec3 vec = zVec3.Create(x, y, z))
                SetHeadingYLocal(vec);
        }

        public void SetHeadingYLocal(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingYLocal, vec);
        }

        public void MoveWorld(float x, float y, float z)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.MoveWorld, (FloatArg)x, (FloatArg)y, (FloatArg)z);
        }

        public void MoveLocal(float x, float y, float z)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.MoveLocal, (FloatArg)x, (FloatArg)y, (FloatArg)z);
        }

        public void SetCollDetDyn(bool arg)
        {
            Process.FASTCALL<NullReturnCall>(Address, arg ? 1 : 0, FuncAddresses.SetCollDetDyn);
        }

        public void SetCollDetStat(bool arg)
        {
            Process.FASTCALL<NullReturnCall>(Address, arg ? 1 : 0, FuncAddresses.SetCollDetStat);
        }

        public void SetSleeping(bool arg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetSleeping, (BoolArg)arg);
        }

        public void SetAI(zCAIBase ai)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetAI, ai);
        }

        public void SetHeadingYWorld(float x, float y, float z)
        {
            using (zVec3 vec = zVec3.Create(x, y, z))
                SetHeadingYWorld(vec);
        }

        public void SetHeadingYWorld(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingYWorld, vec);
        }

        public void SetHeadingWorld(float x, float y, float z)
        {
            using (zVec3 vec = zVec3.Create(x, y, z))
                SetHeadingWorld(vec);
        }

        public void SetHeadingWorld(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingWorld, vec);
        }

        public void SetHeadingLocal(float x, float y, float z)
        {
            using (zVec3 vec = zVec3.Create(x, y, z))
                SetHeadingLocal(vec);
        }

        public void SetHeadingLocal(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingLocal, vec);
        }

        public void SetHeadingYWorld(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingYWorld_vob, vob);
        }

        public void RotateLocal(zVec3 ptr, float arg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateLocal, ptr, (FloatArg)arg);
        }

        public void RotateWorld(zVec3 ptr, float arg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateWorld, ptr, (FloatArg)arg);
        }

        public static zCVob Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x5FD940); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x5FE1E0); //Konstruktor...
            return new zCVob(address);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x5FE440, (BoolArg)true);
        }

        public int Type
        {
            get { return Process.ReadInt(Address + VarOffsets.type); }
            set { Process.Write(Address + VarOffsets.type, value); }
        }

        public virtual void SetVisual(zCVisual vis)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetVisual, vis);
        }

        public void SetVisual(String visual)
        {
            using (zString str = zString.Create(visual))
                SetVisual(str);
        }

        public void SetVisual(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetVisualStr, str);
        }

        public float VisualAlpha
        {
            get { return Process.ReadFloat(Address + VarOffsets.visualAlpha); }
            set { Process.Write(Address + VarOffsets.visualAlpha, value); }
        }

        public zMat4 TrafoObjToWorld
        {
            get { return new zMat4(Address + VarOffsets.trafoObjToWorld); }
        }

        public float[] Position
        {
            get { return TrafoObjToWorld.Position; }
            set { TrafoObjToWorld.Position = value; }
        }

        public float[] Direction
        {
            get { return TrafoObjToWorld.Direction; }
            set { TrafoObjToWorld.Direction = value; }
        }


        public zCAIBase callback_ai
        {
            get { return new zCAIBase(Process.ReadInt(Address + VarOffsets.callback_ai)); }
        }

        public zTBBox3D BBox3D
        {
            get { return new zTBBox3D(Address + VarOffsets.bbox3D); }
        }

        public zCVisual Visual
        {
            get { return new zCVisual(Process.ReadInt(Address + VarOffsets.visual)); }
            set { Process.Write(Address + VarOffsets.visual, value.Address); }
        }

        public zCEventManager EventManager
        {
            get { return new zCEventManager(Process.ReadInt(Address + VarOffsets.eventManager)); }
            set { Process.Write(Address + VarOffsets.eventManager, value.Address); }
        }

        public int LastTimeDrawn
        {
            get { return Process.ReadInt(Address + 0x28); }
            set { Process.Write(Address + 0x28, value); }
        }

        public void BeginMovement()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.BeginMovement);
        }

        public void EndMovement()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.EndMovement, (IntArg)0);
        }

        public void ResetRotationsLocal()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ResetRotationsLocal);
        }

        public void ResetRotationsWorld()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ResetRotationsWorld);
        }

        public void ResetXZRotationsWorld()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ResetXZRotationsWorld);
        }

        /// <summary>
        /// Freeze the game, when not completly loaded
        /// </summary>
        public void RotateLocalY(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateLocalY, new FloatArg(angle));
        }

        public void RotateLocalX(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateLocalX, new FloatArg(angle));
        }

        public void RotateLocalZ(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateLocalZ, new FloatArg(angle));
        }

        public void RotateWorldY(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateWorldY, new FloatArg(angle));
        }

        public void RotateWorldX(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateWorldX, new FloatArg(angle));
        }

        public void RotateWorldZ(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RotateWorldZ, new FloatArg(angle));
        }

        public void SetHeadingAtLocal(zVec3 dir)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingAtLocal, dir);
        }


        public void SetHeadingAtWorld(float x, float y, float z)
        {
            using (zVec3 p = zVec3.Create(x, y, z))
                SetHeadingAtWorld(p);
        }

        public void SetHeadingAtWorld(zVec3 dir)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetHeadingAtWorld, dir);
        }

        public void RemoveVobFromWorld()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RemoveVobFromWorld);
        }

        public zString GetSectorNameVobIsIn()
        {
            return Process.THISCALL<zString>(Address, FuncAddresses.GetSectorNameVobIsIn);
        }

        public zCEventManager GetEM(int x)
        {
            return Process.FASTCALL<zCEventManager>(Address, x, FuncAddresses.GetEM);
        }

        public void SetPositionWorld(float x, float y, float z)
        {
            using (zVec3 p = zVec3.Create(x, y, z))
                SetPositionWorld(p);
        }
        public void SetPositionWorld(zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetPositionWorld, pos);
        }

        /*public void SetAI(zCAIBase ai)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncOffsets.SetAI, ai);
        }*/

        public void SetPhysicsEnabled(bool arg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetPhysicsEnabled, new BoolArg(arg));
        }

        public void DoFrameActivity()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x00602C60);
        }

        public override string ToString()
        {
            return String.Format("({0}: {1})", this.Address, this.VTBL);
        }

    }
}
