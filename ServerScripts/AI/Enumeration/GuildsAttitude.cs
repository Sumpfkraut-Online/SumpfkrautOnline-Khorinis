using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Enumeration
{

    /**
     * AI-Enum
     */
    public enum GuildsAttitude
    {
        HOSTILE, /**< Guilds which are hostile to each other will kill/attack on sight.*/
        NEUTRAL, /**< Guilds which are neutral to each other will largely ignore each other. Attacking anyone will make a neutral guild temporarily hostile.*/
        FRIENDLY /**< Guilds which are friendly to each other will help when a member is attacked.*/
    }
}
