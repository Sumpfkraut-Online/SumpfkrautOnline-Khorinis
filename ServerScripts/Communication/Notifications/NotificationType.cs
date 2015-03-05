namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// There are different Notification types for the player
  /// Based on the type, the NotificationManager decides how to dispay them appropriately
  /// </summary>
  public enum NotificationType
  {
    ServerMessage,
    ChatMessage,
    PersonalMessage,
    Sound,
    MobsiMessage,
    PlayerStatusMessage
  }
}