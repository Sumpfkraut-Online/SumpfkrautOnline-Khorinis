using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCWorld : zCObject, IDisposable
    {
        public zCWorld(Process process, int address)
            : base(process, address)
        { }

        public zCWorld()
        {

        }

        public enum FuncOffsets
        {
            RemoveVob = 0x007800C0,
            GetProgressBar = 0x006269C0,
            SetSkyControlerOutdoor = 0x00620410,
            AddVob = 0x00624810,
            InsertVobInWorld = 0x00780330,
            TraceRayNearestHit = 0x00621B80,
            DisableVob = 0x00780460,

        }

        public enum HookSize
        {
            RemoveVob = 7,
            GetProgressBar = 6
        }
        public enum Offsets
        {
            LevelName = 0x0068,
            ActiveVobs = 0x0240,
            WorldFileName = 25176,
            WorldName = 25196,
            globalVobTree = 0x24,
            waynet = 144,
            activeSkyControler = 0xE8,
            skyControlerOutdoor = 0xE4,
            VobList = 0x6280,

            raytrace_foundHit = 0x0038,
            raytrace_foundVob = 0x003C,
            raytrace_foundPoly = 0x0040,
            raytrace_foundIntersection = 0x0044, //zVec[]
            raytrace_foundPolyNormal = 0x0050, // zVec[]
            raytrace_foundVertex = 0x005C,
            m_bIsInventoryWorld = 136
        }

        [Flags]
        public enum zTraceRay
        {
            Ignore_Vob_No_Collision = 1 << 0,
            Ignore_Vob_All = 1 << 1,
            Test_Vob_BBox = 1 << 2, //Testet nur auf die Boundingbox
            Ignore_Static = 1 << 4, //Nur Vobs
            Return_POLY = 1 << 5, //Gibt Pointer im Return pointer auf Schnittpolygon
            Test_Portals = 1 << 6, //Schnitte mit Portalen ebenfalls werten
            Return_Normal = 1 << 7,
            Ignore_Alpha = 1 << 8,
            Test_Water = 1 << 9,
            Test_2Sided = 1 << 10,
            Ignore_NPC =  1 << 11,
            FirstHit = 1 << 12, //nicht unbedingt der nächste sondern Irgendein Schnittpunkt
            Test_HelperVisuals = 1 << 13,
            Ignore_Projectiles = 1 << 14
        }

        public Dictionary<zCVob.VobTypes, List<zCVob>> getVobLists(params zCVob.VobTypes[] vobTypes)
        {
            Dictionary<zCVob.VobTypes, List<zCVob>> rDic = new Dictionary<zCVob.VobTypes, List<zCVob>>();

            getAllVobObjects(ref rDic, this.GlobalVobTree, vobTypes);

            return rDic;
        }

        public List<zCVob> getVobList(zCVob.VobTypes vobType)
        {
            Dictionary<zCVob.VobTypes, List<zCVob>> rDic = new Dictionary<zCVob.VobTypes, List<zCVob>>();

            getAllVobObjects(ref rDic, this.GlobalVobTree, vobType);

            if (rDic.ContainsKey(vobType))
                return rDic[vobType];

            return new List<zCVob>();
        }

        public List<zCVob> getItemList(zCVob.VobTypes vobType)
        {
            List<zCVob> vobs = new List<zCVob>();

            zCListSort<zCVob> vobList = this.VobList;
            
            do
            {
                zCVob vob = vobList.Data;

                //Check if vob is item
                if (vob == null || vob.Address == 0 || vob.VobType != vobType)
                    continue;

                //add the vob to the itemlist
                vobs.Add(vob);

            } while ((vobList = vobList.Next).Address != 0);

            return vobs;
        }




        private void getAllVobObjects(ref Dictionary<zCVob.VobTypes, List<zCVob>> list, zCTree<zCVob> tree, params zCVob.VobTypes[] types)
        {
            do
            {
                
                if (tree.Data != null && tree.Data.Address != 0)
                {
                    
                    zCVob.VobTypes type = tree.Data.VobType;
                    bool isInList = false;
                    foreach (zCVob.VobTypes vt in types)
                    {
                        if (vt == type)
                        {
                            isInList = true;
                            break;
                        }
                    }
                    
                    if (isInList)
                    {
                        if (!list.ContainsKey(type))
                            list.Add(type, new List<zCVob>());
                        list[type].Add(tree.Data);
                    }
                }

                if (tree.FirstChild != null && tree.FirstChild.Address != 0)
                {
                    getAllVobObjects(ref list, tree.FirstChild, types);
                }
                

            } while ((tree = tree.Next).Address != 0);
        }


        public List<zCVob> getVobList()
        {
            List<zCVob> rDic = new List<zCVob>();

            getAllVobObjects(ref rDic, this.GlobalVobTree);

            return rDic;
        }
        private void getAllVobObjects(ref List<zCVob> list, zCTree<zCVob> tree)
        {
            do
            {

                if (tree.Data != null && tree.Data.Address != 0)
                {
                        list.Add(tree.Data);
                }

                if (tree.FirstChild != null && tree.FirstChild.Address != 0)
                {
                    getAllVobObjects(ref list, tree.FirstChild);
                }


            } while ((tree = tree.Next).Address != 0);
        }

        public bool IsInventoryWorld
        {
            get { return Process.ReadInt(Address + (int)Offsets.m_bIsInventoryWorld) >= 1; }
            set { Process.Write(value ? 1 : 0, Address + (int)Offsets.m_bIsInventoryWorld); }
        }

        public int Raytrace_FoundHit
        {
            get { return Process.ReadInt(Address + (int)Offsets.raytrace_foundHit); }
        }

        public zCVob Raytrace_FoundVob
        {
            get { return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.raytrace_foundVob)); }
        }

        public zVec3 Raytrace_FoundIntersection
        {
            get { return new zVec3(Process, Address + (int)Offsets.raytrace_foundIntersection); }
        }

        public zVec3 Raytrace_FoundNormal
        {
            get { return new zVec3(Process, Address + (int)Offsets.raytrace_foundPolyNormal); }
        }

        public zCSkyControler ActiveSkyControler
        {
            get { return new zCSkyControler(Process, Process.ReadInt(Address+(int)Offsets.activeSkyControler)); }
            set { Process.Write(value.Address, Address + (int)Offsets.activeSkyControler); }
        }

        public zCSkyControler_Outdoor SkyControlerOutdoor
        {
            get { return new zCSkyControler_Outdoor(Process, Process.ReadInt(Address + (int)Offsets.skyControlerOutdoor)); }
            set { Process.Write(value.Address, Address + (int)Offsets.skyControlerOutdoor); }
        }

        public zCTree<zCVob> GlobalVobTree
        {
            get { return new zCTree<zCVob>(Process, Address + (int)Offsets.globalVobTree); }
        }

        public zString LevelName
        {
            get { return new zString(Process, Address + (int)Offsets.LevelName); }
        }

        public zString WorldFileName
        {
            get { return new zString(Process, Address + (int)Offsets.WorldFileName); }
        }

        public zString WorldName
        {
            get { return new zString(Process, Address + (int)Offsets.WorldName); }
        }

        public zCWayNet WayNet
        {
            get { return new zCWayNet(Process, Process.ReadInt(Address + (int)Offsets.waynet)); }
        }

        public zCListSort<oCNpc> NPCList
        {
            get
            {
                return new zCListSort<oCNpc>(Process, Address + 25220);
            }
        }

        public zCListSort<oCItem> ItemList
        {
            get
            {
                return new zCListSort<oCItem>(Process, Address + 25224);
            }
        }

        /// <summary>
        /// This function does return a Voblist. The items are included, MobDoors and Containers aren't.
        /// </summary>
        public zCListSort<zCVob> VobList
        {
            get
            {
                return new zCListSort<zCVob>(Process, Address + 0x6280);
            }
        }

        public zCArray<zCVob> ActiveVobList
        {
            get
            {
                return new zCArray<zCVob>(Process, Address + (int)Offsets.ActiveVobs);
            }
        }


        public zCTree<zCVob> AddVob(zCVob vob)
        {
            return Process.THISCALL<zCTree<zCVob>>((uint)Address, (uint)FuncOffsets.AddVob, new CallValue[] { vob });
        }

        public void InsertVobInWorld(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.InsertVobInWorld, new CallValue[] { vob });
        }

        public void RemoveVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RemoveVob, new CallValue[] { vob });
        }

        public void DisableVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DisableVob, new CallValue[] { vob });
        }

        public zCViewProgressBar GetProgressBar()
        {
            return Process.THISCALL<zCViewProgressBar>((uint)Address, (uint)FuncOffsets.GetProgressBar, new CallValue[] { });
        }

        public void SetSkyControlerOutdoor(zCSkyControler vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetSkyControlerOutdoor, new CallValue[] { vob });
        }

        public int TraceRayNearestHit(zVec3 start, zVec3 end, zTraceRay rayType)
        {
            return Process.FASTCALL<IntArg>((uint)Address, (uint)start.Address, (uint)FuncOffsets.TraceRayNearestHit, 
                new CallValue[] { end, new IntArg(0), new IntArg((int)rayType) }).Address;
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public override int SizeOf()
        {
            return 0x6258;
        }



        public static zCWorld Create(Process process)
        {
            int ptr = process.Alloc(0x6258).ToInt32();

            process.THISCALL<NullReturnCall>((uint)ptr, (uint)0x0061FA40, new CallValue[]{});


            return new zCWorld(process, ptr);
        }


        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x006200F0, new CallValue[] { });
                Process.Free(new IntPtr(Address), 0x6258);
                disposed = true;
            }
        }
    }
}
