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
            BeginMovement = 0x0061DA80,
            RotateWorldX = 0x0061B800,
            RotateWorldZ = 0x0061B860,
            RotateLocalX = 0x0061B6B0,
            RotateLocalZ = 0x0061B790,
            GetSectorNameVobIsIn = 0x00600AE0
        }

        public enum HookSize
        {
            RemoveVobFromWorld = 6,
            SetVisual = 6
        }

        //public enum VobTypes : ObjectTypes
        //{
        //    //ERROR = 0,
        //    //Item = 8636420,
        //    //Npc = 8640292,
        //    //Mob = 8639700,
        //    //MobFire = 8638876,
        //    //Mover = 8627324,
        //    //MobInter = 8639884,
        //    //MobLockable = 8637628,
        //    //MobContainer = 8637284,
        //    //MobDoor = 8638548,
        //    //VobLight = 8624756,
        //    //Trigger = 8625692,
        //    //TriggerList = 8625812,
        //    //Vob = 8624508,
        //    //Freepoint = 8643636,
        //    //Camera = 8624508,
        //    //TriggerScript = 8643756, //really? dunno
        //    //MobSwitch = 8636988,
        //    //MobBed = 8636692,
        //    //ZoneMusic = 8629644,
        //    //zCCSCamera = 8587500,
        //    //TouchDamage = 8642700,
        //    //MessageFilter = 8627196,
        //    //zCVobSound = 8629484,
        //    //zCVobAnimate = 8626668
        //    ////8624756? (MYLIGHT, LIGHT)
        //    ////8626188 pfx
            
        //}

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

        public enum BitFlag0
        {
            showVisual = 1,
            drawBBox3D = 1 << 1,
            visualAlphaEnabled = 1 << 2,
            physicsEnabled = 1 << 3,
            staticVob = 1 << 4,
            ignoredByTraceRay = 1 << 5,
            collDetectionStatic = 1 << 6,
            collDetectionDynamic = 1 << 7,
            castDynShadow = 1 << 8,
            lightColorStatDirty = 1 << 9,
            lightColorDynDirty = 1 << 10
        }

        public zCVob(Process process, int address)
            : base(process, address)
        { 
        }

        public zCVob()
        {

        }

        public static zCVob Create(Process process)
        {
            IntPtr ptr = process.Alloc(0x120);
            zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0x009A34D8);//0x00AB1518) => MobDoor;
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x005FE1E0, new CallValue[] { });

            return new zCVob(process, ptr.ToInt32());
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

        public zCVisual Visual
        {
            get { return new zCVisual(Process, Process.ReadInt(Address + (int)Offsets.visual)); }
            set { Process.Write(value.Address ,Address + (int)Offsets.visual); }
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

        public void SetVisual(String visual)
        {
            zString str = zString.Create(Process, visual);
            SetVisual(str);
            str.Dispose();
        }
        public void SetVisual(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetVisual, new CallValue[] { str });
        }

        public zString GetSectorNameVobIsIn()
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetSectorNameVobIsIn, new CallValue[] {  });
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
            if(!b)
                BitField1 &= ~(int)BitFlag0.showVisual;
            else
                BitField1 |= (int)BitFlag0.showVisual;
        }

        public bool getShowVisual()
        {
            return ((BitField1 & (int)BitFlag0.showVisual) == (int)BitFlag0.showVisual);
        }


        public override uint ValueLength()
        {
            return 4;
        }
    }
}
