using System;
using System.Collections.Generic;
using System.Text;
using Gothic.mClasses;
using WinApi;
using AdminAndListModule;
using Gothic.mClasses.Elements;
using Gothic.zClasses;
using Gothic.zTypes;
using GMP.Modules;
using GMP.Helper;
using Injection;
using ListModule.Messages;

namespace ListModule.Lists
{
    public class SoundList : ManagedListBox
    {
        public SoundList(Process process)
            : base(process)
        {

        }

        public void Open()
        {
            Enable();
            Data.Clear();


            int activeId = 0;
            int count = 0;
            foreach (Animations anim in Loader.sClientOptions.SoundList)
            {
                mEButton button = new mEButton();
                button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(StartSound);
                button.Data = anim;
                Data.Add(button);

                if (anim == lastSound)
                    activeId = count;
                count++;
            }


            ActiveID = activeId;
            InputHooked.deaktivateFullControl(process);
            Program.chatBox.tb.Inputenabled = false;
        }


        public void Close()
        {
            lastSound = (Animations)Data[ActiveID].Data;
            Program.chatBox.tb.Inputenabled = true;
            InputHooked.activateFullControl(process);
            Disable();
        }

        static Animations lastSound = null;
        public void StartSound(object sender, mEButton.ButtonPressedEventArg args)
        {
            Animations anim = (Animations)args.Data;
            if(anim == null)
                return;
            lastSound = anim;

            if (Loader.sOptions.BlockOptionsSpeak.blockInWater && NPCHelper.isInWater(Program.Player)
                || Loader.sOptions.BlockOptionsSpeak.blockWhenDead && NPCHelper.isDead(Program.Player)
                || Loader.sOptions.BlockOptionsSpeak.blockWhenSleep && NPCHelper.isInMagicSleep(Program.Player)
                || Loader.sOptions.BlockOptionsSpeak.blockWhenUnconscious && NPCHelper.isUnconscious(Program.Player))
            {
                Close();
                return;
            }
            if (Loader.sOptions.speakListAvailable && anim.SVM != null && anim.SVM != "" && oCNpc.Player(process).HP != 0)
            {
                External_Helper.AI_OutputSVM_Overlay(process, oCNpc.Player(process), oCNpc.Player(process), anim.SVM);
                //new SoundSynchMessage().Write(Program.client.sentBitStream, Program.client, anim.SVM);
            }
            
            Close();
        }
    }
}
