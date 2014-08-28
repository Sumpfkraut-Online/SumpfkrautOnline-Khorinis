using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.GUI;

#if SSM_ACCOUNT_LOGGING

using GUC.Server.Scripts.Accounts.Logs;

#endif
#if SSM_ACCOUNT

using GUC.Server.Scripts.Communication.Notifications;

#endif

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// Controls the chat system (message window for communication)
  /// </summary>
  public class Chat
  {
    private static Chat chat = null;

    private int offsetX = 100;
    private int offsetY = 0x800;
    private int startButton = 0x54;
    private int abortButton = 0x1B;
    private int sendButton = 0x0D;
    private TextBox textBox;

    /// <summary>
    /// Singleton object: call this to get a valid Chat object to work with
    /// </summary>
    /// <returns></returns>
    public static Chat GetChat()
    {
      if (chat == null)
      {
        chat = new Chat();
      }
      return chat;
    }

    /// <summary>
    /// protected constructor, use GetChat()
    /// </summary>
    protected Chat()
    {
      textBox = new TextBox("", "FONT_DEFAULT.TGA", offsetX, offsetY, sendButton, startButton, abortButton);
      textBox.show();
      textBox.TextSended += new Events.TextBoxMessageEventHandler(textBoxMessageSended);
    }

    private void textBoxMessageSended(TextBox sender, Player player, string message)
    {
      message = message.Trim();
      if (message.Length == 0)
        return;
      if (!CommandInterpreter.GetCommandInterpreter().IsCommand(message))
      {
        ChatMessage chatMessage = null;
        if(message.StartsWith("."))
        {
          chatMessage = new ChatMessage(player, null, ChatMessageType.Whisper, message.Substring(1));
          
        }
        else if (message.StartsWith("!"))
        {
          chatMessage = new ChatMessage(player, null, ChatMessageType.Shout, message.Substring(1));          
        }
        else
        {
          chatMessage = new ChatMessage(player, null, ChatMessageType.Talk, message);         
        }
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);
      }
      else
      {
        CommandInterpreter.GetCommandInterpreter().Interpret(player, message);
      }
#if SSM_ACCOUNT_LOGGING
      SQLiteLogger.log_Chat(player, message);
#endif
    }
  }
}