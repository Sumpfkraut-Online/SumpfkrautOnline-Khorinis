using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class zCVob : zCObject
    {
        new public class VarOffsets : zCObject.VarOffsets
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

        public class FuncOffsets
        {
            public const int RemoveVobFromWorld = 0x00601C40,
            GetEM = 0x005FFE10,
            SetPositionWorld = 0x0061BB70,
            SetAI = 0x005FE8F0,
            SetVisual = 0x00602680,
            ResetRotationsLocal = 0x0061BCF0,
            ResetRotationsWorld = 0x0061C000,
            RotateLocalY = 0x0061B720,
            RotateWorldY = 0x0061B830,
            SetHeadingAtLocal = 0x0061C860,
            SetHeadingAtWorld = 0x0061CBC0,
            ResetXZRotationsWorld = 0x0061C090,
            BeginMovement = 0x0061DA80,
            RotateWorldX = 0x0061B800,
            RotateWorldZ = 0x0061B860,
            RotateLocalX = 0x0061B6B0,
            RotateLocalZ = 0x0061B790,
            GetSectorNameVobIsIn = 0x00600AE0;
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

        public class BitFlag0
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

        public const int ByteSize = 0x120;

        public zCVob(int address)
            : base(address)
        {
        }

        public zCVob()
        {
        }

        public static zCVob Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x5FD940); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x5FE1E0); //Konstruktor...
            return new zCVob(address);
        }

        public int Type
        {
            get { return Process.ReadInt(Address + VarOffsets.type); }
        }

        public void SetVisual(String visual)
        {
            using (zString str = zString.Create(visual))
                SetVisual(str);
        }

        public void SetVisual(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncOffsets.SetVisual, str);
        }

        /*public Matrix4 TrafoObjToWorld
        {
            get { return Matrix4.read(Process, Address + (int)Offsets.trafoObjToWorld); }
        }

        public zTBBox3D BBox3D
        {
            get { return new zTBBox3D(Process, Address + (int)Offsets.bbox3D); }
        }

        public zCVisual Visual
        {
            get { return new zCVisual(Process, Process.ReadInt(Address + (int)Offsets.visual)); }
            set { Process.Write(value.Address, Address + (int)Offsets.visual); }
        }

        public zCEventManager EventManager
        {
            get { return new zCEventManager(Process, Process.ReadInt(Address + (int)Offsets.eventManager)); }
            set { Process.Write(value.Address, Address + (int)Offsets.eventManager); }
        }

        public int BitField1
        {
            get { return Process.ReadInt(Address + (int)Offsets.bitfield); }
            set { Process.Write(value, Address + (int)Offsets.bitfield); }
        }


        public void BeginMovement()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.BeginMovement, new CallValue[] { });
        }

        public void ResetRotationsLocal()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ResetRotationsLocal, new CallValue[] { });
        }

        public void ResetRotationsWorld()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ResetRotationsWorld, new CallValue[] { });
        }

        public void ResetXZRotationsWorld()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ResetXZRotationsWorld, new CallValue[] { });
        }

        /// <summary>
        /// Freeze the game, when not completly loaded
        /// </summary>
        /// <param name="angle"></param>
        public void RotateLocalY(float angle)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RotateLocalY, new CallValue[] { new FloatArg(angle) });
        }

        public void RotateWorldY(float angle)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RotateWorldY, new CallValue[] { new FloatArg(angle) });
        }

        public void RotateWorldX(float angle)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RotateWorldX, new CallValue[] { new FloatArg(angle) });
        }

        public void RotateWorldZ(float angle)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RotateWorldZ, new CallValue[] { new FloatArg(angle) });
        }

        public void SetHeadingAtLocal(zVec3 target)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetHeadingAtLocal, new CallValue[] { target });
        }

        public void SetHeadingAtWorld(zVec3 target)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetHeadingAtWorld, new CallValue[] { target });
        }



        public void RemoveVobFromWorld()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RemoveVobFromWorld, new CallValue[] { });
        }



        public zString GetSectorNameVobIsIn()
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetSectorNameVobIsIn, new CallValue[] { });
        }

        public zCEventManager GetEM(int x)
        {
            return Process.FASTCALL<zCEventManager>((uint)Address, (uint)x, (uint)FuncOffsets.GetEM, new CallValue[] { });
        }

        public zVec3 GetPosition()
        {
            Matrix4 traf = this.TrafoObjToWorld;
            zVec3 pos = zVec3.Create(Process);
            pos.X = Process.ReadFloat(traf.Address + 3 * 4);
            pos.Y = Process.ReadFloat(traf.Address + 7 * 4);
            pos.Z = Process.ReadFloat(traf.Address + 11 * 4);
            return pos;
        }

        public float[] GetPositionArray()
        {
            float[] posArray = new float[3];

            Matrix4 traf = this.TrafoObjToWorld;

            posArray[0] = Process.ReadFloat(traf.Address + 3 * 4);
            posArray[1] = Process.ReadFloat(traf.Address + 7 * 4);
            posArray[2] = Process.ReadFloat(traf.Address + 11 * 4);
            return posArray;
        }

        public void SetPositionWorld(float[] pos)
        {
            zVec3 p = zVec3.Create(Process);
            p.X = pos[0];
            p.Y = pos[1];
            p.Z = pos[2];

            SetPositionWorld(p);

            p.Dispose();
        }
        public void SetPositionWorld(zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetPositionWorld, new CallValue[] { pos });
        }

        public void SetAI(zCAIBase ai)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetAI, new CallValue[] { ai });
        }


        public void setShowVisual(bool b)
        {
            if (!b)
                BitField1 &= ~(int)BitFlag0.showVisual;
            else
                BitField1 |= (int)BitFlag0.showVisual;
        }

        public bool getShowVisual()
        {
            return ((BitField1 & (int)BitFlag0.showVisual) == (int)BitFlag0.showVisual);
        }
        */

        public override string ToString()
        {
            return String.Format("({0}: {1})", this.Address, this.VTBL);
        }
    }
}
