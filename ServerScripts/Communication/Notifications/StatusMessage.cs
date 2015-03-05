using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Communication.Notifications
{
  public class StatusMessage: Notification
  {
    public StatusMessage(Player player, string message)
    {
      this.Color = new ColorRGBA(0, 186, 56);//green
      this.Type = NotificationType.PlayerStatusMessage;
      this.Speaker = null;
      this.Listener = player;
      this.Range = 0;
      this.Message = message;
    }
  }
}
