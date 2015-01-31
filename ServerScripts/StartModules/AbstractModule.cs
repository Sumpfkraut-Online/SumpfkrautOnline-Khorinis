using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;


namespace GUC.Server.Scripts.StartModules
{
	public class AbstractModule
	{
		public AbstractModule Next;
		public AbstractModule Previous;

		public virtual void start(Player player) {
		}

		protected virtual void end(Player player) {
			if(Next != null)
				Next.start(player);
			else
				player.Spawn();
            
		}
		
		
    }
}
