namespace GUC.Server.Scripts.Communication.Notifications
{
  /// <summary>
  /// There are different modes of communicating, loud, silent etc (different range)
  /// </summary>
  public enum ChatMessageType
  {
    Whisper,
    Shout,
    Talk,
    OOC,
    Action,
    PM
  }
}