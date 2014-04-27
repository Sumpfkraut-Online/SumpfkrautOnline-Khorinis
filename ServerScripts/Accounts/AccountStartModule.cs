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


using GUC.Server.Scripts.StartModules;

namespace GUC.Server.Scripts.Accounts
{
	public class AccountStartModule : AbstractModule
	{
		Texture background;
		Button register;
		Button login;
		Text textChoose;
		Text textName;
		Text textPassword;

		TextBox tbName;
		TextBox tbPassword;

		Button continueButton;

		public AccountStartModule() {
			textChoose = new Text("Wähle:", 0x1000-120, 0x1000);
			textName = new Text("Bitte gebe deinen Namen ein:", 1433, 0x500);
			textPassword = new Text("Bitte gebe dein Password ein:", 1433, 0x500);

			tbName = new TextBox("", "Font_Old_20_White.TGA", 1433, 0x600, 0, 0, 0);
			tbPassword = new TextBox("", "Font_Old_20_White.TGA", 1433, 0x600, 0, 0, 0);

			background = new Texture("LOG_PAPER.tga");
			register = new Button("Registrieren", 1433, 6432);
			login = new Button("Anmelden", 4998, 6432);

			continueButton = new Button("Weiter", 4998, 6432);

			login.Pressed += new Events.ButtonEventHandler(OnButton);
			register.Pressed += new Events.ButtonEventHandler(OnButton);

			continueButton.Pressed += new Events.ButtonEventHandler(OnContinueButton);

			tbName.TextSended += new Events.TextBoxMessageEventHandler(textBoxMessageReceived);
			tbPassword.TextSended += new Events.TextBoxMessageEventHandler(textBoxMessageReceived);
		}

		protected void reset(Player player) {
			player.getAccount().state = Account.State.Nothing;

			textName.hide(player);
			textPassword.hide(player);
			tbName.hide(player);
			tbPassword.hide(player);
			continueButton.hide(player);

			background.show(player);
			register.show(player);
			login.show(player);
			textChoose.show(player);
            
			Cursor.getCursor().hide(player);//toTop!
			Cursor.getCursor().show(player);
		}

		public override void start(Player player) {
			player.freeze();
            player.setAttribute(NPCAttributeFlags.ATR_HITPOINTS, 123);
			background.show(player);
			register.show(player);
			login.show(player);
			textChoose.show(player);

            
            
			Cursor.getCursor().hide(player);//toTop!
			Cursor.getCursor().show(player);

			player.setUserObject("Account", new Account(player));
		}

		public void OnButton(Button button, Player player) {
			if(button == register) {
				player.getAccount().state |= Account.State.Register;
			}else {
				player.getAccount().state |= Account.State.Login;
			}

			player.getAccount().state |= Account.State.Name;

			textName.show(player);
			tbName.show(player);
			tbName.StartWriting(player);
			continueButton.show(player);

			register.hide(player);
			login.hide(player);
			textChoose.hide(player);

			Cursor.getCursor().hide(player);//toTop!
			Cursor.getCursor().show(player);
		}

		public void OnContinueButton(Button button, Player player) {
			if(player.getAccount().state.HasFlag(Account.State.Name))
			{
				tbName.CallSendText(player);
			}else {
				tbPassword.CallSendText(player);
			}

			continueButton.hide(player);
		}

		public void textBoxMessageReceived(TextBox tb, Player pl, String message) {
			message = message.Trim();

			if(message.Length < 2)
			{
				continueButton.show(pl);
				if(tb == tbName)
					tbName.StartWriting(pl);
				else
					tbPassword.StartWriting(pl);
				return;
			}

			if(tb == tbName) {
				//Check if name exists!
				if(pl.getAccount().state.HasFlag(Account.State.Register))
				{
					if(Account.existsName(message))
					{
						continueButton.show(pl);
						tbName.StartWriting(pl);
						return;
					}
				}else if(pl.getAccount().state.HasFlag(Account.State.Login)) {
					if(!Account.existsName(message))
					{
						continueButton.show(pl);
						tbName.StartWriting(pl);
						return;
					}
				}

				pl.Name = message;

				textName.hide(pl);
				tb.hide(pl);

				textPassword.show(pl);
				tbPassword.show(pl);

				tbPassword.StartWriting(pl);

				pl.getAccount().state &= ~Account.State.Name;
				pl.getAccount().state |= Account.State.Password;
				continueButton.show(pl);
			}
			else if(tb == tbPassword) {
				if(pl.getAccount().state.HasFlag(Account.State.Register))
				{
					bool registered = pl.getAccount().register(pl.Name, message);
					if(!registered)
					{
						reset(pl);
						return;
					}
				}else if(pl.getAccount().state.HasFlag(Account.State.Login)) {
					bool loggedIn = pl.getAccount().login(pl.Name, message);
					if(!loggedIn)
					{
						reset(pl);
						return;
					}
				}

				end(pl);
			}
		}

		protected override void end(Player player) {
			background.hide(player);
			register.hide(player);
			login.hide(player);

			textName.hide(player);
			textPassword.hide(player);
			tbName.hide(player);
			tbPassword.hide(player);
			continueButton.hide(player);
			textChoose.hide(player);
			Cursor.getCursor().hide(player);
			
			player.unfreeze();
			
			base.end(player);
		}
    }
}
