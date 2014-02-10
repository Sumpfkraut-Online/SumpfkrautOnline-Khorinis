using System;
using System.Collections.Generic;
using System.Text;
using Gothic.mClasses;
using WinApi;
using Gothic.mClasses.Elements;
using Injection;
using System.Security.Cryptography;

namespace AccountModule.Login
{
    public class LoginBox : ManagedListBox
    {
        AccountStart start = null;
        public LoginBox(Process process, AccountStart start)
            : base(process)
        {
            this.start = start;
        }

        public static string GetMD5Hash(string TextToHash)
        {
            //Prüfen ob Daten übergeben wurden.
            if ((TextToHash == null) || (TextToHash.Length == 0))
            {
                return string.Empty;
            }

            //MD5 Hash aus dem String berechnen. Dazu muss der string in ein Byte[]
            //zerlegt werden. Danach muss das Resultat wieder zurück in ein string.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(TextToHash);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result);
        } 

        public void Open()
        {
            Enable();
            
            InputHooked.deaktivateFullControl(process);
            SetStandard();
        }

        public void SetStandard()
        {
            Data.Clear();

            mEButton button = new mEButton();
            button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(EntryBox);
            button.Data = "Login";
            button.actionID = 0;
            Data.Add(button);

            button = new mEButton();
            button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(EntryBox);
            button.Data = "Registrieren";
            button.actionID = 1;
            Data.Add(button);


            ActiveID = 0;
        }

        mETextBox username;
        mETextBox password;
        public void EntryBox(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();

            username = new mETextBox();
            username.hardText = "User: ";
            Data.Add(username);

            password = new mETextBox();
            password.hardText = "Password: ";
            Data.Add(password);

            mEButton button = new mEButton();
            button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Wait);
            if (((mEButton)sender).actionID == 0)
            {
                button.Data = "Login";
                button.actionID = 0;
            }
            else
            {
                button.Data = "Registrieren";
                button.actionID = 1;
            }
            Data.Add(button);

            mEButton backButton = new mEButton();
            backButton.Data = "Zuruck";
            backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(backButton);

            ActiveID = 0;
        }

        public void Wait(object sender, mEButton.ButtonPressedEventArg args)
        {
            //Anfrage senden
            if(username.Data.ToString().Trim().Length == 0 || password.Data.ToString().Trim().Length == 0)
                return;
            new AccountMessage().Write(Program.client.sentBitStream, Program.client,((mEButton)sender).actionID, username.Data.ToString().Trim(), GetMD5Hash(password.Data.ToString()).Trim());

            //Buttons...
            Data.Clear();
            mEButton button = new mEButton();
            button.Data = "Warte...";
            Data.Add(button);
            ActiveID = 0;
        }

        public void Message(byte regType, byte type)
        {
            Data.Clear();
            mEButton button = new mEButton();
            

            if (type == 0)
            {
                if (regType == 0)
                    button.Data = "Erfolgreich eingeloggt!";
                else
                    button.Data = "Erfolgreich registriert!";
                Program.clientOptions.name = username.Data.ToString().Trim();
                button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Next);
            }
            else if (type == 1)
            {
                if (regType == 0)
                    button.Data = "Falsche Daten";
                else
                    button.Data = "Character schon vorhanden!";
                button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            }
            else if (type == 2)
            {
                button.Data = "Account schon eingelogged!";
                button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            }

            Data.Add(button);
            ActiveID = 0;
        }

        public void BackButton(object sender, mEButton.ButtonPressedEventArg args)
        {
            SetStandard();
        }

        public void Next(object sender, mEButton.ButtonPressedEventArg args)
        {
            start.Next(start.module);
        }

        public void Close()
        {
            InputHooked.activateFullControl(process);
            Disable();
        }




    }
}
