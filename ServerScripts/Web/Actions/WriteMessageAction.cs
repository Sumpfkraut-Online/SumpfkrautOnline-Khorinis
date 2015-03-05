#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using GUC.Server.Scripts.Communication.Notifications;
using GUC.Server.Scripts.Communication;

namespace GUC.Server.Scripts.Web.Actions
{
    public class WriteMessageAction: Action
    {
        int mToNpc = 0;
        String mMessage;
        byte mR;
        byte mG;
        byte mB;
        byte mA;

        public bool completed = false;

        public WriteMessageAction(int toNpc, String message, byte r, byte g, byte b, byte a)
        {
            mToNpc = toNpc;
            mMessage = message;
            mR = r;
            mG = g;
            mB = b;
            mA = a;
        }

        public override void update(ActionTimer timer)
        {
            
            if(mToNpc == 0){
                //Startup.chat.MessageBox.addLine(new ColorRGBA(mR, mG, mB, mA), mMessage);
                ChatMessage message = new ChatMessage(null, null, ChatMessageType.PM, mMessage);
                message.Color = new ColorRGBA(mR, mG, mB, mA);
                NotificationManager.GetNotificationManager().DisplayNotification(message);
                completed = true;
            }else{
                Vob v = Player.getVob(mToNpc);
                if (v != null && v is Player)
                {
                    //Startup.chat.MessageBox.addLine((Player)v, new ColorRGBA(mR, mG, mB, mA), mMessage);
                    ChatMessage message = new ChatMessage(null, (Player)v, ChatMessageType.PM, mMessage);
                    message.Color = new ColorRGBA(mR, mG, mB, mA);
                    NotificationManager.GetNotificationManager().DisplayNotification(message);  
                    completed = true;
                }
            }
           


            isFinished = true;
            timer.removeAction(this);
        }
    }
}

#endif