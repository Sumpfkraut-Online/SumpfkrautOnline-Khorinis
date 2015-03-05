using GUC.Types;
using System;

namespace GUC.Server.Scripts.Communication.Notifications
{
  /// <summary>
  /// Message type for broadcast messages from server
  /// </summary>
  public class ServerMessage : Notification
  {
    public ServerMessage(string message)
    {
      this.Color = new ColorRGBA(0, 186, 56);//green
      this.Type = NotificationType.ServerMessage;
      this.Speaker = null;
      this.Listener = null;
      this.Range = -1;
      this.Message = message;
    }
  }
}