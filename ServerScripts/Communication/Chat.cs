using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.GUI;
using GUC.Server.Scripts.Communication.Notifications;

#if SSM_ACCOUNT_LOGGING

using GUC.Server.Scripts.Accounts.Logs;

#endif


namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// Controls the chat system (message window for communication)
  /// </summary>
  public class Chat
  {
    private static Chat chat = null;

   /* private int offsetX = 100;
    private int offsetY = 0x800;
    private int startButton = 0x54;
    private int abortButton = 0x1B;
    private int sendButton = 0x0D;
    private TextBox textBox;*/

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
        test = new Server.Sumpfkraut.Chat();
        test.OnReceiveMessage += ReceiveMessage;

      /*textBox = new TextBox("", "FONT_DEFAULT.TGA", offsetX, offsetY, sendButton, startButton, abortButton);
      textBox.show();
      textBox.TextSended += new Events.TextBoxMessageEventHandler(textBoxMessageSended);*/
    }

    private Server.Sumpfkraut.Chat test;

    private void ReceiveMessage(Player sender, string message)
    {
        if (message.StartsWith("/global"))
        {
            test.SendGlobal(message.Substring(7).Trim());
            return;
        }

        if (message.StartsWith("/oocg"))
        {
            test.SendOOCGlobal(sender, message.Substring(5).Trim());
            return;
        }

        if (message.StartsWith("/ooc"))
        {
            test.SendOOC(sender,sender,message.Substring(4).Trim());
            return;
        }

        if (message.StartsWith("/revive"))
        {
            sender.revive();
            return;
        }

        test.SendSay(sender, sender, message);
    }
      /*
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
    }*/
  }
}