using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Weather
{
    public partial class SkyController
    {
        World world;
        public World World { get { return this.world; } }

        internal SkyController(World world)
        {
            if (world == null)
                throw new ArgumentNullException("World is null!");

            this.world = world;
        }

        /*
        float LinearInterpolate(float y0, float y1, float mu)   //LINEAR
        {
	        return y0*(1-mu)+y1*mu;
        }

        float CatmullRomSpline(  //SMOOTH
	         float y0,float y1,
           float y2,float y3,
           float mu)
        {
           float a0,a1,a2,a3,mu2;

           mu2 = mu*mu;
           a0 = -0.5*y0 + 1.5*y1 - 1.5*y2 + 0.5*y3;
           a1 = y0 - 2.5*y1 + 2*y2 - 0.5*y3;
           a2 = -0.5*y0 + 0.5*y2;
           a3 = y1;

           return (a0*mu*mu2+a1*mu2+a2*mu+a3);
        }
        */
    }
}
