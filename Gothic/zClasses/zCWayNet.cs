using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCWayNet : zCObject
    {
        #region OffsetLists
        public enum Offsets : uint
        {
            wpList = 40
        }

        public enum FuncOffsets : uint
        {
            GetWaypoint = 0x007B0330
        }

        public enum HookSize : uint
        {
            GetWaypoint = 5
        }
        #endregion

        #region Standard
        public zCWayNet(Process process, int address) : base (process, address)
        {
            
        }

        public zCWayNet()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        

        #endregion

        #region Fields
        public zCListSort<zCWaypoint> WpList
        {
            get { return new zCListSort<zCWaypoint>(Process, Address + (int)Offsets.wpList); }
        }
        #endregion

        public zCWaypoint GetWaypointByName(String name)
        {

            zCListSort<zCWaypoint> wpL = WpList;

            do 
            {
                zCWaypoint wp = wpL.Data;
                if (wp.Name.Value.Trim().ToLower() == name.Trim().ToLower())
                    return wp;
            } while ((wpL = wpL.Next).Address != 0);

            return null;
        }

        public float[] getWaypointPosition(String name)
        {
            zCListSort<zCWaypoint> wpL = WpList;

            do
            {
                zCWaypoint wp = wpL.Data;
                if (wp.Name.Value.Trim().ToLower() == name.Trim().ToLower())
                    return new float[]{wp.Position.X, wp.Position.Y, wp.Position.Z, wp.Direction.X, wp.Direction.Y, wp.Direction.Z};
            } while ((wpL = wpL.Next).Address != 0);


            List<zCVob> vobs = oCGame.Game(Process).World.getVobList(zCVob.VobTypes.Freepoint);

            foreach (zCVob vob in vobs)
            {
                if (vob.ObjectName.Value.Trim().ToLower() == name.ToLower().Trim())
                {
                    return new float[] { vob.TrafoObjToWorld.get(3), vob.TrafoObjToWorld.get(7), vob.TrafoObjToWorld.get(11) };
                }
            }

            return null;
        }

        #region methods
        /// <summary>
        /// Funktion liefer den Wegpunkt mit dem als Argument angegebenen Namen zurück
        /// Vorsicht: Hat bei meinen Versuchen Crashes verursacht, lieber GetWaypointByName nutzen!
        /// </summary>
        /// <param name="str">Name des Wegpunktes</param>
        /// <returns>Instanz des Wegpunkt</returns>
        public zCWaypoint GetWaypoint(zString str)
        {
            return Process.FASTCALL<zCWaypoint>((uint)Address, (uint)str.Address, (uint)FuncOffsets.GetWaypoint, new CallValue[] { });
        }
        #endregion
    }
}
