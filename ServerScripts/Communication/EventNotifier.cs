using GUC.Server.Log;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Server.Scripts.Communication.Notifications;
using System;
namespace GUC.Server.Scripts.Communication
{
  public class EventNotifier
  {

    private static EventNotifier eventNotifier = null;



    public static EventNotifier GetEventNotifier()
    {
      if (eventNotifier == null)
      {
        eventNotifier = new EventNotifier();
      }
      return eventNotifier;
    }
    protected EventNotifier()
    {
      Player.sOnPlayerSpawns += new Events.PlayerEventHandler(spawn);
      Player.sOnPlayerDisconnects += new Events.PlayerEventHandler(disconnect);


      MobInter.sOnStartInteraction += new Events.MobInterEventHandler(startInteract);
      MobInter.sOnStopInteraction += new Events.MobInterEventHandler(stopInteract);

      MobInter.sOnTrigger += new Events.MobInterEventHandler(trigger);
      MobInter.sOnUnTrigger += new Events.MobInterEventHandler(untrigger);
    }

    private void spawn(Player player)
    {
      
      player.Color = Utils.Utils.GetRandomPastelColor();

      Console.WriteLine("Spieler: " + player.ID + " " + player.Name);
      ServerMessage message = new ServerMessage(player.Name + " betritt das Spiel");


      NotificationManager.GetNotificationManager().DisplayNotification(message);
      Logger.log(Logger.LOG_INFO, player.Name + " betritt das Spiel");

    }

    private void disconnect(Player player)
    {      
      ServerMessage message = new ServerMessage(player.Name + " verlässt das Spiel");

      NotificationManager.GetNotificationManager().DisplayNotification(message);
      Logger.log(Logger.LOG_INFO, player.Name + " verlässt das Spiel");
    }

    private void startInteract(MobInter sender, NPCProto npc)
    {
      Logger.log(Logger.LOG_INFO, npc.Name + " starts Interaction " + sender);
    }

    private void stopInteract(MobInter sender, NPCProto npc)
    {
      Logger.log(Logger.LOG_INFO, npc.Name + " stop Interaction " + sender);
    }

    private void trigger(MobInter sender, NPCProto npc)
    {
      Logger.log(Logger.LOG_INFO, npc.Name + " triggers " + sender);
    }

    private void untrigger(MobInter sender, NPCProto npc)
    {
      Logger.log(Logger.LOG_INFO, npc.Name + " untriggers " + sender);
    }

    
  }
}