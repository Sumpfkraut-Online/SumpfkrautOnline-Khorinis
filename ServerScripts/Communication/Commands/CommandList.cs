using GUC.Enumeration;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.AI;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripts.Items;
using System;
using System.Collections.Generic;
using GUC.Server.Scripts.Communication.Notifications;

#if SSM_ACCOUNT
using GUC.Server.Scripts.Accounts;
#endif

namespace GUC.Server.Scripts.Communication
{
  /// <summary>
  /// This is a list of commands to use in the textBox ingame
  /// </summary>
  public class CommandList
  {
    public List<Command> Commands { get; private set; }

    private static CommandList commandList = null;

    public static CommandList GetCommandList()
    {
      if (commandList == null)
      {
        commandList = new CommandList();
      }
      return commandList;
    }

    protected CommandList()
    {
      Commands = new List<Command>();
      FillCommandList();
    }

    private void FillCommandList()
    {
      Command command;

      command = new Command("giveitem", 2, delegate(Player player, string[] parameters)
      {
        string instance = parameters[0];
        int amount;
        if (!Int32.TryParse(parameters[1], out amount))
          return false;

        if (ItemInstance.getItemInstance(instance) == null)
          return false;
        if (amount <= 0)
          amount = 1;
        player.addItem(instance, amount);

        return true;
      });
      Commands.Add(command);

      command = new Command("playAni", 1, delegate(Player player, string[] parameters)
      {
        string animation = parameters[0];
        player.playAnimation(animation);

        return true;
      });
      Commands.Add(command);

      command = new Command("freeze", 0, delegate(Player player, string[] parameters)
      {
        player.freeze();

        return true;
      });
      Commands.Add(command);

      command = new Command("unfreeze", 0, delegate(Player player, string[] parameters)
      {
        player.unfreeze();

        return true;
      });
      Commands.Add(command);

      command = new Command("giveSkills", 0, delegate(Player player, string[] parameters)
      {
        for (int i = 0; i < (int)NPCAttribute.ATR_MAX; i++)
        {
          player.setAttribute((NPCAttribute)i, 100 + i);
        }

        for (int i = (int)NPCTalent.H1; i < (int)NPCTalent.MaxTalents; i++)
        {
          player.setTalentSkills((NPCTalent)i, 100 + i);
          player.setTalentValues((NPCTalent)i, 100 + i);
        }

        for (int i = (int)NPCTalent.H1; i <= (int)NPCTalent.CrossBow; i++)
        {
          player.setTalentSkills((NPCTalent)i, 2);
          player.setTalentValues((NPCTalent)i, 2);
          player.setHitchances((NPCTalent)i, 100);
        }
        return true;
      });
      Commands.Add(command);

      command = new Command("giveAttribute", 2, delegate(Player player, string[] parameters)
      {
        int attribute, value;
        if (!Int32.TryParse(parameters[0], out attribute))
          return false;

        if (!Int32.TryParse(parameters[1], out value))
          return false;

        player.setAttribute((NPCAttribute)attribute, value);
        return true;
      });
      Commands.Add(command);

      command = new Command("giveTalent", 3, delegate(Player player, string[] parameters)
      {
        int talent, value1, value2;
        if (!Int32.TryParse(parameters[0], out talent))
          return false;

        if (!Int32.TryParse(parameters[1], out value1))
          return false;

        if (!Int32.TryParse(parameters[2], out value2))
          return false;

        player.setTalentSkills((NPCTalent)talent, value1);
        player.setTalentValues((NPCTalent)talent, value2);

        return true;
      });
      Commands.Add(command);

      command = new Command("giveSpell", 0, delegate(Player player, string[] parameters)
      {
        player.addItem(ITSC_SHRINK.get(), 90);
        player.addItem(ITSC_TRFSHEEP.get(), 90);

        return true;
      });
      Commands.Add(command);
      command = new Command("setTime", 2, delegate(Player player, string[] parameters)
      {
        int hour, minute;
        if (!Int32.TryParse(parameters[0], out hour))
          return false;

        if (!Int32.TryParse(parameters[1], out minute))
          return false;

        DayTime.setTime(hour, minute);
        return true;
      });
      Commands.Add(command);

      command = new Command("toWP", 1, delegate(Player player, string[] parameters)
      {
        String wp = parameters[0];
        FreeOrWayPoint wayp = AISystem.getWaypoint(player.Map, wp);
        if (wayp == null)
        {
          return false;
        }
        player.setPosition(wayp.Position);
        return true;
      });
      Commands.Add(command);

      command = new Command("revive", 0, delegate(Player player, string[] parameters)
      {
        player.revive();
        return true;
      });
      Commands.Add(command);

      command = new Command("speedup", 0, delegate(Player player, string[] parameters)
      {
        player.ApplyOverlay("HUMANS_SPRINT.MDS");
        return true;
      });
      Commands.Add(command);

#if (SSM_ACCOUNT && SSM_WEB)
      command = new Command("sn", 0, delegate(Player player, string[] parameters)
      {
        lock (SLMessageManager.GetSLMessageManager().MessageList)
        {
          SLMessageManager.GetSLMessageManager().MessageList.Add(new SLMessageManager.SLMessage()
          {
            playerID = player.ID,
            accountID = player.getAccount().accountID,
            name = player.Name,
            msg = System.Net.WebUtility.HtmlDecode(parameters[0])
          });
        }
        return true;
      });
      Commands.Add(command);
#endif
      command = new Command("s", new string[]{"r"}, 1, delegate(Player player, string[] parameters)
      {
        ChatMessage chatMessage = new ChatMessage(player, null, ChatMessageType.Shout, parameters[0]);
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);

        return true;
      });
      Commands.Add(command);

      command = new Command("f", new string[] { "w" }, 1, delegate(Player player, string[] parameters)
      {
        ChatMessage chatMessage = new ChatMessage(player, null, ChatMessageType.Whisper, parameters[0]);
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);

        return true;
      });
      Commands.Add(command);
      command = new Command("ooc", new string[] { "o", "ot" }, 1, delegate(Player player, string[] parameters)
      {
        ChatMessage chatMessage = new ChatMessage(player, null, ChatMessageType.OOC, parameters[0]);
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);

        return true;
      });
      Commands.Add(command);

      command = new Command("me", new string[] { "ich", "a", "action" }, 1, delegate(Player player, string[] parameters)
      {
        ChatMessage chatMessage = new ChatMessage(player, null, ChatMessageType.Action, parameters[0]);
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);

        return true;
      });
      Commands.Add(command);
      command = new Command("pm", new string[] { "nachricht", "pn" }, 1, delegate(Player player, string[] parameters)
      {
        
        ChatMessage chatMessage = new ChatMessage(player, null, ChatMessageType.PM, parameters[0]);
        NotificationManager.GetNotificationManager().DisplayNotification(chatMessage);

        return true;
      });

      
      Commands.Add(command);

        command = new Command("exit", 0, delegate(Player player, string[] parameters)
        {
            player.exitGame();
            return true;
        });
        Commands.Add(command);
    }
  }
}