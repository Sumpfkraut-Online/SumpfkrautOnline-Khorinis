using GUC.Server.Scripting.GUI;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Utils;
using GUC.Types;
using System;
using GUC.Enumeration;

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// A NotificationArea is a box of lines, where notifications (chat etc.) are displayed
  /// </summary>
  public class NotificationArea
  {
    

    public NotificationArea(int x, int y, int width, int height)
    {
      this.OffsetX = x;
      this.OffsetY = y;
      this.Width = width;
      this.Height = height;
      this.MessagesBox = new MessagesBox("FONT_DEFAULT.TGA", (byte)height, x, y,  (int)VirtualKey.Prior, (int)VirtualKey.Next, 0, true);
      this.MessagesBox.show();
    }

    public void DisplayMessage(Player player, ColorRGBA color, string message)
    {
      
      if (player == null)
        MessagesBox.addLine(color.R, color.G, color.B, color.A, message);
      else
        MessagesBox.addLine(player, color.R, color.G, color.B, color.A, message);
    }

    public int OffsetX { get; private set; }

    public int OffsetY { get; private set; }

    public int Height { get; private set; }

    public int Width { get; private set; }

    public MessagesBox MessagesBox { get; private set; }
  }
}