using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Utils;
using GUC.Types;
using System;
using System.Text;

namespace GUC.Server.Scripts.Communication.Notifications
{
  /// <summary>
  /// Message type for chat communication
  /// </summary>
  public class ChatMessage : Notification
  {
    //Ranges in cm
    private const int RANGE_WHISPER = 200;

    private const int RANGE_TALK = 900;
    private const int RANGE_SHOUT = 18000;
    private const int RANGE_OOC = RANGE_TALK;
    private const int RANGE_ACTION = RANGE_TALK;

    public ChatMessage(Player speaker, Player listener, ChatMessageType type, String message)
    {
      if (speaker!=null && speaker.Color != null)
        this.Color = speaker.Color;
      else
        this.Color = new ColorRGBA();

      this.Type = NotificationType.ChatMessage;
      this.Speaker = speaker;
      this.Listener = listener;

      StringBuilder sb = new StringBuilder();
      String speakerName = speaker == null ? "" : speaker.Name;;
      String listenerName = listener == null ? "" : " zu " + listener.Name;

      switch (type)
      {
        case ChatMessageType.Whisper:
          CreateChatMessage(sb, speakerName, listenerName, message, "flüstert");
          this.Range = RANGE_WHISPER;
          break;

        case ChatMessageType.Shout:
          CreateChatMessage(sb, speakerName, listenerName, message, "ruft");
          this.Range = RANGE_SHOUT;
          break;

        case ChatMessageType.OOC:
          sb.Append("// ").Append(speakerName).Append(": ").Append(message);
          this.Message = sb.ToString();
          this.Range = RANGE_OOC;
          break;

        case ChatMessageType.Action:
          sb.Append("[ ").Append(speakerName).Append(" ").Append(message).Append(" ]");
          this.Message = sb.ToString();
          this.Range = RANGE_ACTION;
          break;
        case ChatMessageType.PM:
          sb.Append("@").Append(speakerName).Append(" -> ").Append(listenerName).Append(": ").Append(message);
          this.Color = new ColorRGBA(255, 0, 0);
          this.Message = sb.ToString();
          this.Range = -1;
          break;
        default:
          CreateChatMessage(sb, speakerName, listenerName, message, "sagt");
          this.Range = RANGE_TALK;
          break;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="speakerName"></param>
    /// <param name="listenerName"></param>
    /// <param name="message"></param>
    /// <param name="verb">describes the action of speaking, like "talks" or "shouts"</param>
    private void CreateChatMessage(StringBuilder sb, String speakerName, String listenerName, String message, String verb)
    {
      sb.Append(speakerName).Append(" ").Append(verb)
        .Append(listenerName).Append(": ").Append(message);

      if (message[message.Length - 1] != '.'
        || message[message.Length - 1] != '?'
        || message[message.Length - 1] != '!')
      {
        sb.Append(".");
      }
      this.Message = sb.ToString();
    }
  }
}