using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Communication.Notifications;
using System.Collections.Generic;

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// This class handles command inputs from the player (over the TextBox input)
  /// </summary>
  public class CommandInterpreter
  {
    private static CommandInterpreter commandInterpreter = null;
    private Dictionary<string, Command> commandMapping = new Dictionary<string, Command>();

    public static CommandInterpreter GetCommandInterpreter()
    {
      if (commandInterpreter == null)
      {
        commandInterpreter = new CommandInterpreter();
      }
      return commandInterpreter;
    }

    protected CommandInterpreter()
    {
      foreach (var command in CommandList.GetCommandList().Commands)
      {
        AddCommand(command);
      }
    }

    public bool IsCommand(string message)
    {
      return message.StartsWith("/");
    }

    /// <summary>
    /// Executes a given command if possible
    /// </summary>
    /// <param name="message"></param>
    internal void Interpret(Player player, string message)
    {
      message = message.Trim();
      if (message.StartsWith("/"))
        message = message.Substring(1);

      if (message.Length == 0)
        return;

      string parameters;
      Command command = GetCommand(message, out parameters);
      if (command == null)//invalid command
        return;

      bool success=command.Execute(player, parameters);
      if(!success)
      {
        //TODO
        ChatMessage chatMessage = new ChatMessage(player, player, ChatMessageType.PM, "Command incorrect");
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);

      }
    }

    /// <summary>
    /// Returns the wirst word of a command message and the rest of the message as parameters
    /// without this word
    /// </summary>
    /// <param name="message"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    private Command GetCommand(string message, out string parameters)
    {
      int pos = message.IndexOf(' ');
      Command result = null;
      if (pos == -1)//no space
      {
        parameters = "";
        commandMapping.TryGetValue(message.ToLower(), out result);
        return result;
      }
      parameters = message.Substring(pos + 1).Trim();
      
      commandMapping.TryGetValue(message.Substring(0, pos).ToLower(), out result);

      return result;
        
      
    }

    public void AddCommand(Command command)
    {
      commandMapping.Add(command.Name, command);
      foreach (var alias in command.Alias)
      {
        commandMapping.Add(alias, command);
      }
    }
  }
}