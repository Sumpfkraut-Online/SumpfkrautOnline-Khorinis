using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities.Geometry
{

    public static class InGeometry
    {

        public static int CalcLeftness (Point testPoint, Point lineStartPoint, Point lineEndPoint)
        {
            return ( ((lineEndPoint.X - lineStartPoint.X) * (testPoint.Y - lineStartPoint.Y)) 
                - ((testPoint.X - lineStartPoint.X) * (lineEndPoint.Y - lineStartPoint.Y)) );
        }

        public static int CalcWindingNumber2D (Point point, List<Point> polygon)
        {
            return CalcWindingNumber2D(point, polygon.ToArray());
        }

        public static int CalcWindingNumber2D (Point point, Point[] polygon)
        {
            if (polygon.Length < 1) { return 0; }
            int wn = 0;
            int nextIndex;
            
            for (int i = 0; i < polygon.Length; i++)
            {
                if (i < (polygon.Length - 1)) { nextIndex = i + 1; }
                else { nextIndex = 0; }

                if (polygon[i].Y <= point.Y)
                {
                    if ((polygon[nextIndex].Y > point.Y) 
                        && (CalcLeftness(point, polygon[i], polygon[nextIndex]) > 0))
                    {
                        // winds counterclockwise around point
                        wn++;
                    }
                }
                else
                {
                    if ((polygon[nextIndex].Y <= point.Y) 
                        && (CalcLeftness(point, polygon[i], polygon[nextIndex]) < 0))
                    {
                        // winds clockwise around point
                        wn--;
                    }
                }
            }

            return wn;
        }

        public static bool IsInPolygon2D (Point point, List<Point> polygon)
        {
            return IsInPolygon2D(point, polygon.ToArray());
        }

        public static bool IsInPolygon2D (Point point, Point[] polygon)
        {
            return CalcWindingNumber2D(point, polygon) != 0;
        }



    }

}
