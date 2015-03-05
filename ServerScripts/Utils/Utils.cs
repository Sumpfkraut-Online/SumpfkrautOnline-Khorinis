using System;
using System.Collections.Generic;

using System.Text;
using GUC.Types;

namespace GUC.Server.Scripts.Utils
{
  public class Utils
  {
    public static void Shuffle<T>(T[] array)
    {
      var random = RandomManager.GetRandom();
      //http://www.dotnetperls.com/fisher-yates-shuffle
      for (int i = array.Length; i > 1; i--)
      {
        // Pick random element to swap.
        int j = random.Next(i); // 0 <= j <= i-1
        // Swap.
        T tmp = array[j];
        array[j] = array[i - 1];
        array[i - 1] = tmp;
      }
    }
    public static ColorRGBA GetRandomPastelColor()
    {
      Random r = RandomManager.GetRandom();
      int[] colors = new int[3];

      colors[0] = 255;
      colors[1] = r.Next(120, 255);
      colors[2] = r.Next(120, 190);

      Utils.Shuffle(colors);

      return new ColorRGBA((byte)colors[0], (byte)colors[1], (byte)colors[2]);
    }
  }
 
}
