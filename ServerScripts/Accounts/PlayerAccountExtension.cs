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


namespace GUC.Server.Scripts
{
	public static class PlayerAccountExtension
	{
		public static Account getAccount(this Player player)
        {
            return (Account)player.getUserObjects("Account");
        }
    }
}
