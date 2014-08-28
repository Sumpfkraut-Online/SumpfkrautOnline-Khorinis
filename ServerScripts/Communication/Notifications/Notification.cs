using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Utils;
using GUC.Types;
using System;

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// Notifications are messages displayed to the user, like chat, server status etc...
  /// </summary>
  public class Notification
  {
    public ColorRGBA Color { get; set; }

    public String Message { get; set; }

    public NotificationType Type { get; set; }

    public Player Speaker { get; set; }

    public Player Listener { get; set; }

    /// <summary>
    /// Range in cm
    /// -1 means: infinite range
    /// 0 means: only for speaker
    /// </summary>
    public int Range { get; set; }
  }
}