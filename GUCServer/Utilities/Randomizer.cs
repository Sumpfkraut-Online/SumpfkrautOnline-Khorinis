using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Utilities
{
    public static class Randomizer
    {
        static readonly Random rand = new Random(42);

        /// <summary>
        /// Gibt eine nicht negative Zufallszahl zurück.
        /// </summary>
        /// <returns>Eine 32-Bit-Ganzzahl mit Vorzeichen, die größer als oder gleich 0 (null) und kleiner als System.Int32.MaxValue ist.</returns>
        public static int GetInt()
        {
            return rand.Next();
        }

        /// <summary>
        /// Gibt eine nicht negative Zufallszahl zurück, die kleiner als das angegebene Maximum ist.
        /// </summary>
        /// <param name="maxValue">Die exklusive obere Grenze der Zufallszahl, die generiert werden soll.maxValue muss größer oder gleich 0 (null) sein.</param>
        /// <returns>Eine 32-Bit-Ganzzahl mit Vorzeichen, die größer oder gleich 0 (null) und kleiner als maxValue ist, d. h., der Bereich der Rückgabewerte umfasst gewöhnlich 0,
        ///          aber nicht maxValue.Wenn jedoch maxValue 0 (null) entspricht, wird maxValue zurückgegeben.</returns>
        /// <exception cref="ArgumentOutOfRangeException">maxValue ist kleiner als null.</exception>
        public static int GetInt(int maxValue)
        {
            return rand.Next(maxValue);
        }

        /// <summary>
        /// Gibt eine Zufallszahl im angegebenen Bereich zurück.
        /// </summary>
        /// <param name="minValue">Die inklusive untere Grenze der zurückgegebenen Zufallszahl.</param>
        /// <param name="maxValue">Die exklusive obere Grenze der zurückgegebenen Zufallszahl.maxValue muss größer oder gleich minValue sein.</param>
        /// <returns>Eine 32-Bit-Ganzzahl mit Vorzeichen, die größer oder gleich minValue und kleiner als maxValue ist, d. h., der Bereich der Rückgabewerte umfasst minValue, aber 
        ///          nicht maxValue.Wenn minValue gleich maxValue ist, wird minValue zurückgegeben.</returns>
        /// <exception cref="ArgumentOutOfRangeException">minValue ist größer als maxValue.</exception>
        public static int GetInt(int minValue, int maxValue)
        {
            return rand.Next(minValue, maxValue);
        }

        /// <summary>
        /// Füllt die Elemente eines angegebenen Bytearrays mit Zufallszahlen.
        /// </summary>
        /// <param name="buffer">Ein Bytearray, das für Zufallszahlen vorgesehen ist.</param>
        /// <exception cref="ArgumentNullException">buffer ist null.</exception>
        public static void GetBytes(byte[] buffer)
        {
            rand.NextBytes(buffer);
        }

        /// <summary>
        /// Gibt eine Zufallszahl zwischen 0,0 und 1,0 zurück.
        /// </summary>
        /// <returns>Eine Gleitkommazahl mit doppelter Genauigkeit, die größer oder gleich 0,0 und kleiner als 1,0 ist.</returns>
        public static double GetDouble()
        {
            return rand.NextDouble();
        }

        public static double GetDouble(double maxValue)
        {
            return rand.NextDouble() * maxValue;
        }

        public static double GetDouble(double minValue, double maxValue)
        {
            return rand.NextDouble() * (maxValue - minValue) + minValue;
        }
        
        public static Vec3f GetVec3fRad(Vec3f pos, float radius)
        {
            float x = (float)GetDouble(-radius, radius);
            double zMax = Math.Sqrt(radius * radius - x * x);
            float z = (float)GetDouble(-zMax, zMax);

            return new Vec3f(pos.X + x, pos.Y, pos.Z + z);
        }

        public static Vec3f GetVec3fBox(Vec3f pos, float xLen, float yLen)
        {
            return new Vec3f(pos.X + (float)GetDouble(-xLen, xLen), pos.Y, pos.Z + (float)GetDouble(-yLen, yLen));
        }
    }
}
