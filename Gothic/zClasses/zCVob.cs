using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCVob : zCObject
    {
        public enum Types
        {
            npc = 8640292
        }
        public enum Offsets
        {
            globalVobTreeNode = 0x0024,
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
            CollisionObject = 0x011C
        }

        public enum FuncOffsets
        {
            RemoveVobFromWorld = 0x00601C40,
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
            BeginMovement = 0x0061DA80
        }

        public enum HookSize
        {
            RemoveVobFromWorld = 6
        }

        public enum VobTypes
        {
            ERROR = 0,
            Item = 8636420,
            Npc = 8640292,
            Mob = 8639700,
            MobFire = 8638876,
            Mover = 8627324,
            MobInter = 8639884,
            MobLockable = 8637628,
            MobContainer = 8637284,
            MobDoor = 8638548,
            VobLight = 8624756,
            Trigger = 8625692,
            TriggerList = 8625812,
            Vob = 8624508,
            Freepoint = 8643636,
            Camera = 8624508,
            TriggerScript = 8643756 //really? dunno
        }

        enum zTVobType
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

        public zCVob(Process process, int address)
            : base(process, address)
        { 
        }

        public zCVob()
        {

        }

        public int Type
        {
            get { return Process.ReadInt(Address + (int)Offsets.type); }
        }

        public VobTypes VobType
        {
            get
            {
                return (VobTypes)VTBL;
            }
            set { VTBL = (int)value; }

        }

        public Matrix4 TrafoObjToWorld
        {
            get { return Matrix4.read(Process, Address + (int)Offsets.trafoObjToWorld); }
        }

        public zTBBox3D BBox3D
        {
            get { return new zTBBox3D(Process, Address + (int)Offsets.bbox3D); }
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

        public void SetVisual(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetVisual, new CallValue[] { str });
        }

        public zCEventManager GetEM(int x)
        {
            return Process.FASTCALL<zCEventManager>((uint)Address, (uint)x, (uint)FuncOffsets.GetEM, new CallValue[] { });
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
            if(!b)
                BitField1 &= ~(1 << 0);
            else
                BitField1 |= 1 << 0;
        }

        public bool getShowVisual()
        {
            int zCVob_bitfield0_showVisual = ((1 << 1) - 1) << 0;
            return ((BitField1 & zCVob_bitfield0_showVisual) == 1);
        }


        public override uint ValueLength()
        {
            return 4;
        }
    }
}
