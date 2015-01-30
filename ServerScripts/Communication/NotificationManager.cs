using GUC.Server.Log;
using GUC.Server.Scripting.Objects.Character;
using System.Collections.Generic;

#if SSM_ACCOUNT_LOGGING

#endif
#if SSM_ACCOUNT

#endif

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// Controls the NotificationManager system (message window for communication/notification)
  /// </summary>
  public class NotificationManager
  {
    private static NotificationManager notificationManager = null;

    private Dictionary<NotificationType, NotificationArea> notificationMapping
      = new Dictionary<NotificationType, NotificationArea>();

    /// <summary>
    /// Singleton object: call this to get a valid NotificationManager object to work with
    /// </summary>
    /// <returns></returns>
    public static NotificationManager GetNotificationManager()
    {
      if (notificationManager == null)
      {
        notificationManager = new NotificationManager();
      }
      return notificationManager;
    }

    /// <summary>
    /// protected constructor, use GetNotificationManager()
    /// </summary>
    protected NotificationManager()
    {
      Logger.log(Logger.LogLevel.INFO, "################### Initalise NotificationManager ##################");
    }

    /// <summary>
    /// Creates a new notification area at the given position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="acceptedTypes">What types of notifications should this area show?</param>
    public void AddNotificationArea(int x, int y, int width, int height, NotificationType[] acceptedTypes)
    {
      if (acceptedTypes.Length > 0)
      {
        NotificationArea area = new NotificationArea(x, y, width, height);
        foreach (var type in acceptedTypes)
        {
          notificationMapping.Add(type, area);
        }
      }
    }

    /// <summary>
    /// Used to display any notification automatically in the corect notification area
    /// </summary>
    /// <param name="notification"></param>
    public void DisplayNotification(Notification notification)
    {
      NotificationArea area = notificationMapping[notification.Type];
      if (area == null)
      {
        Logger.log(Logger.LogLevel.ERROR, "NotificationType " + notification.Type.ToString() + " has no notification area");
        return;
      }

      if (notification.Range < 1)//if broadcast/direct message
      {
        if (notification.Listener == null)//if broadcast
          area.DisplayMessage(null, notification.Color, notification.Message);
        else //if not a broadcast for everyone, send also a message to the speaker
        {
          if (notification.Speaker != null)//but only, if there is a speaker!
            area.DisplayMessage(notification.Speaker, notification.Color, notification.Message);

          area.DisplayMessage(notification.Listener, notification.Color, notification.Message);
        }
      }
      else if (notification.Speaker != null)//speaker must be not null for distance comparison
      {
        area.DisplayMessage(notification.Speaker, notification.Color, notification.Message);

        if (notification.Listener != null)
        {
          float distance = notification.Speaker.GetDistanceTo(notification.Listener);

          if (distance <= notification.Range)
            area.DisplayMessage(notification.Listener, notification.Color, notification.Message);
        }
        else //if not directed to a single other player: broadcast in range
        {
          Player[] players = notification.Speaker.getNearPlayers(notification.Range);
          foreach (Player player in players)
          {
            area.DisplayMessage(player, notification.Color, notification.Message);
          }
        }
      }
    }

    public void ClearNotificationAreas()
    {
      notificationMapping.Clear();
    }
  }
}