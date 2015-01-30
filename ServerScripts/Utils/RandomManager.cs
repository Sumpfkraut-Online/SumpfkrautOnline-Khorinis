using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Utils
{
  /// <summary>
  /// Singleton class for random numbers
  /// Usefull for testing because of seed initialization
  /// </summary>
  public class RandomManager
  {
    private static Random random = null;
    public static Random GetRandom()
    {
      if (random==null)
      {
        random = new Random();
      }
      return random;
    }
    public static Random GetRandom(int seed)
    {
      if (random == null)
      {
        random = new Random(seed);
      }
      return random;
    }

    protected RandomManager ()
    {

    }


  }
}
