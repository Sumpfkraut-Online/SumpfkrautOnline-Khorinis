using System;
using System.Collections.Generic;

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// I have no idea, what this is for
  /// </summary>
  public class SLMessageManager
  {
    public class SLMessage
    {
      private static int id = 0;

      public SLMessage()
      {
        id++;
        ID = id;
      }

      public int ID;
      public int playerID;
      public long accountID;
      public String name;
      public String msg;

      public bool IsAnswered = false;
    }

    public List<SLMessage> MessageList { get; set; }

    private static SLMessageManager sLMessageManager = null;

    public static SLMessageManager GetSLMessageManager()
    {
      if (sLMessageManager == null)
      {
        sLMessageManager = new SLMessageManager();
      }
      return sLMessageManager;
    }

    protected SLMessageManager()
    {
      MessageList = new List<SLMessage>();
    }
  }
}