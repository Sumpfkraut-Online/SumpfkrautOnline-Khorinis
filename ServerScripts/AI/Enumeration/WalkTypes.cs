using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Enumeration
{
    /**
     * AI-Enum to differentiate between walking and running.
     */
    public enum WalkTypes
    {
        Walk = 0, /**< This character is walking rather slowly.*/
        Run = 1 /**< This character is running fast.*/
    }
}
