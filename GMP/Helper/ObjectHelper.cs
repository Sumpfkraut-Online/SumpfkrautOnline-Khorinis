using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using Network.Types;

namespace GMP.Helper
{
    public class ObjectHelper
    {

        public static String getName(zCVob vob)
        {
            oCMobInter inter = new oCMobInter(Process.ThisProcess(), vob.Address);

            String vobName = "";
            if (vob.ObjectName.Address != 0 && vob.ObjectName.Value.Trim().Length != 0)
                vobName = vob.ObjectName.Value;
            else if (inter.Name.Address != 0 && inter.Name.Value.Trim().Length != 0)
                vobName = inter.Name.Value;

            return vobName;
        }
        public static zCVob findVob(String name, float[] pos, zCVob.VobTypes type, float distance)
        {
            Process Process = Process.ThisProcess();

            List<zCVob> vobList = oCGame.Game(Process).World.getVobList(type);

            foreach (zCVob vob in vobList)
            {
                float[] vobPos = new float[] { vob.TrafoObjToWorld.get(3), vob.TrafoObjToWorld.get(7), vob.TrafoObjToWorld.get(11) };

                if (new Vec3f(vobPos).getDistance((Vec3f)pos) > distance)
                    continue;

                String vobName = getName(vob);

                if (vobName.Trim().ToLower() != name.Trim().ToLower())
                    continue;

                return vob;
            }


            return null;
            zCTree<zCVob> vobtree = oCGame.Game(Process).World.GlobalVobTree.FirstChild;
            do
            {

                zCVob vob = vobtree.Data;
                float[] vobPos = new float[] { vob.TrafoObjToWorld.get(3), vob.TrafoObjToWorld.get(7), vob.TrafoObjToWorld.get(11) };
                String vobName = vob.ObjectName.Value;


                

                if (vob.VobType == type
                    && new Vec3f(vobPos).getDistance((Vec3f)pos) < distance
                    && vobName.Trim().ToLower() == name.Trim().ToLower())
                {
                    return vob;
                }
            } while ((vobtree = vobtree.Next).Address != 0);
            return null;
        }

        public static zCVob getNearestVob(String name, float[] pos, zCVob.VobTypes type, float distance)
        {
            Process Process = Process.ThisProcess();

            zCTree<zCVob> vobtree = oCGame.Game(Process).World.GlobalVobTree.FirstChild;
            zCVob nearestVob = null;
            float lastDistance = 0;
            do
            {

                zCVob vob = vobtree.Data;
                float[] vobPos = new float[] { vob.TrafoObjToWorld.get(3), vob.TrafoObjToWorld.get(7), vob.TrafoObjToWorld.get(11) };
                String vobName = vob.ObjectName.Value;
                float objDistance = new Vec3f(vobPos).getDistance((Vec3f)pos);


                if (vob.VobType == type
                    && objDistance < distance
                    && vobName.Trim().ToLower() == name.Trim().ToLower())
                {
                    if (nearestVob == null || lastDistance > objDistance)
                    {
                        nearestVob = vob;
                        lastDistance = objDistance;
                    }
                }
            } while ((vobtree = vobtree.Next).Address != 0);
            return nearestVob;
        }

        public static oCItem getNearestItem(String name, float[] pos, zCVob.VobTypes type, float distance, int amount)
        {
            Process Process = Process.ThisProcess();

            zCTree<zCVob> vobtree = oCGame.Game(Process).World.GlobalVobTree.FirstChild;
            oCItem nearestVob = null;
            float lastDistance = 0;
            do
            {

                zCVob vob = vobtree.Data;
                float[] vobPos = new float[] { vob.TrafoObjToWorld.get(3), vob.TrafoObjToWorld.get(7), vob.TrafoObjToWorld.get(11) };
                String vobName = vob.ObjectName.Value;
                float objDistance = new Vec3f(vobPos).getDistance((Vec3f)pos);


                if (vob.VobType == type
                    && objDistance < distance
                    && vobName.Trim().ToLower() == name.Trim().ToLower()
                    && new oCItem(Process, vob.Address).Amount == amount)
                {
                    if (nearestVob == null || lastDistance > objDistance)
                    {
                        nearestVob = new oCItem(Process, vob.Address);
                        lastDistance = objDistance;
                    }
                }
            } while ((vobtree = vobtree.Next).Address != 0);
            return nearestVob;
        }
    }
}
