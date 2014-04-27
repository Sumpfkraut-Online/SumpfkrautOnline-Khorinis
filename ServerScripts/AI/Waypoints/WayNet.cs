using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using System.IO;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class WayNet
    {
        protected class WayPointPriority : IComparable, IEquatable<WayPointPriority>
        {
            public WayPoint wp;
            public float Distance;//Bisher zurückgelegte Distanz
            public float Priority = 0;
            public WayPointPriority previous = null;
            

            public WayPointPriority(WayPoint wp, float distance)
            {
                this.wp = wp;
                this.Distance = distance;
            }

            public override string ToString()
            {
                return wp.Name + " " + Priority + " | " + Distance;
            }

            public int CompareTo(object obj)
            {
                if (obj is WayPointPriority)
                {
                    WayPointPriority wpp = (WayPointPriority)obj;
                    if (wpp.Priority < Priority)
                        return 1;
                    else
                        return -1;
                }
                return 1;
            }


            public bool Equals(WayPointPriority other)
            {
                if (other == this || other.wp == this.wp)
                    return true;
                else
                    return false;
            }
        }
        protected List<WayPoint> mWayPointList = new List<WayPoint>();
        protected Dictionary<String, WayPoint> mWayPointDict = new Dictionary<string, WayPoint>();

        public static WayNet loadFromFile(String file)
        {
            WayNet wnet = new WayNet();
            String[] lines = File.ReadAllLines(file);
            foreach (String line in lines)
            {
                String[] values = line.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length < 5)
                    continue;
                
                String name = values[0].Trim().ToLower();
                float x = float.Parse(values[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                float y = float.Parse(values[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                float z = float.Parse(values[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);

                float dirX = float.Parse(values[4].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                float dirY = float.Parse(values[5].Trim(), System.Globalization.CultureInfo.InvariantCulture);

                String[] list = new String[values.Length - 6];
                Array.Copy(values, 6, list, 0, list.Length);

                WayPoint wp = new WayPoint(name, new Vec3f(x, y, z), new Vec3f(dirX, 0, dirY), list);
                wnet.mWayPointList.Add(wp);
                wnet.mWayPointDict.Add(wp.Name, wp);
            }

            foreach (WayPoint wp in wnet.mWayPointList)
            {
                WayPoint[] wpList = new WayPoint[wp.ConnectedWPString.Length];
                int i = 0; 
                foreach (String wpName in wp.ConnectedWPString)
                {
                    wpList[i] = wnet.mWayPointDict[wpName];
                    i += 1;
                }
                wp.ConnectedWP = wpList;
            }

            return wnet;
        }



        public WayPoint getWaypoint(String wp)
        {
            String wp2 = wp.ToLower().Trim();
            if (mWayPointDict.ContainsKey(wp2))
                return mWayPointDict[wp2];
            return null;
        }

        public WayPoint getRandomWaypoint()
        {
            if(mWayPointList.Count == 0)
                return null;

            Random rand = new Random();
            return mWayPointList[rand.Next(mWayPointList.Count)];
        }

        public int Count { get { return mWayPointList.Count; } }

        public WayPoint this[String wpName]
        {
            get { return mWayPointDict.ContainsKey(wpName) ? mWayPointDict[wpName] : null; }
        }

        public WayPoint this[int index]
        {
            get { return mWayPointList[index]; }
        }

        public WayPoint getNearestWaypoint(Vec3f position)
        {
            float nearestDistance = 0;
            WayPoint foundWP = null;
            foreach (WayPoint wp in mWayPointList)
            {
                float distance = wp.Position.getDistance(position);
                if(foundWP == null){
                    foundWP = wp;
                    nearestDistance = distance;
                    continue;
                }
                if (distance < nearestDistance)
                {
                    foundWP = wp;
                    nearestDistance = distance;
                }
            }

            return foundWP;
        }

        public WayRoute getWayRoute(WayPoint start, WayPoint end){

            WayPointPriority wpp = _wayRouteIntern(start, end);
            if (wpp == null)
                return null;

            LinkedList<WayPoint> wpList = new LinkedList<WayPoint>();
            while (wpp != null)
            {
                wpList.AddFirst(wpp.wp);
                wpp = wpp.previous;
            }


            return new WayRoute(wpList.ToArray());
        }

        protected WayPointPriority _wayRouteIntern(WayPoint start, WayPoint end)
        {
            PriorityQueue<WayPointPriority> openList = new PriorityQueue<WayPointPriority>();
            Dictionary<String, WayPoint> closedList = new Dictionary<string, WayPoint>();

            openList.add(new WayPointPriority(start, 0.0f));

            do
            {
                WayPointPriority wpp = openList.minRemove();
                if (wpp.wp == end)
                    return wpp;//Path found!

                closedList.Add(wpp.wp.Name, wpp.wp);
                _expandNode(wpp, ref openList, ref closedList, end);
            } while (!openList.isEmpty());

            return null;
        }

        protected void _expandNode(WayPointPriority wpp, ref PriorityQueue<WayPointPriority> openList, ref Dictionary<String, WayPoint> closedList, WayPoint endWP)
        {
            foreach (WayPoint wp in wpp.wp.ConnectedWP)
            {
                if (closedList.ContainsKey(wp.Name))
                    continue;

                float newValue = wpp.Distance + wpp.wp.Position.getDistance(wp.Position);

                int index = openList.getIndexOf(new WayPointPriority(wp, 0));
                WayPointPriority realConnected = null;

                if(index != -1)
                    realConnected = openList.getAt(index);
                if (realConnected != null && newValue >= realConnected.Distance)
                    continue;

                float priority = newValue + endWP.Position.getDistance(wp.Position);

                if (realConnected == null)
                {
                    realConnected = new WayPointPriority(wp, newValue);
                    realConnected.Priority = priority;
                    openList.add(realConnected);
                }
                else
                {
                    openList.remove(realConnected);
                    realConnected.Distance = newValue;
                    realConnected.Priority = priority;
                    openList.add(realConnected);
                }
                realConnected.previous = wpp;


            }
        }

        

    }
}
