using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace GUC.Scripting
{
    public class Events
    {
        /* Delegates: */
        public delegate void RenderEvent(Process process, long now);
        public delegate void ProcessEvent(Process process);

        /* Static-Events (Hooks): */

        /// <summary>
        /// This Event will be called each time gothic renders.
        /// </summary>
        public static RenderEvent OnRender;

        /// <summary>
        /// This Events will be called when the game is closed, but before the client disconnects.
        /// </summary>
        public static ProcessEvent OnExitGame;

        /// <summary>
        /// This Events will be called on Startup when Gothic sets Daedalus-External functions. Here you can add your own external daedalus functions for the default-Scripts.
        /// </summary>
        public static ProcessEvent OnAddExternals;


    }
}
