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
    public class AnimationList : ManagedListBox
    {
        public AnimationList(Process process)
            : base(process)
        {

        }

        public void Open()
        {
            Enable();
            Data.Clear();

            int activeId = 0;
            int count = 0;

            if (Loader.sOptions.animationListWhiteList)
            {
                foreach (String anim in Loader.sOptions.AvailableAnimations)
                {
                    mEButton button = new mEButton();
                    button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(StartAnim);
                    button.Data = new Animations() { animation = anim };
                    Data.Add(button);

                    if (anim == lastAnim.animation)
                        activeId = count;
                    count++;
                }
            }
            else
            {
                foreach (Animations anim in Loader.sClientOptions.AnimationList)
                {
                    if (Loader.sOptions.AvailableAnimations.Contains(anim.animation))
                        continue;
                    mEButton button = new mEButton();
                    button.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(StartAnim);
                    button.Data = anim;
                    Data.Add(button);

                    if (anim == lastAnim)
                        activeId = count;
                    count++;
                }
            }

            ActiveID = activeId;
            InputHooked.deaktivateFullControl(process);
            Program.chatBox.tb.Inputenabled = false;
        }


        public void Close()
        {
            lastAnim = (Animations)Data[ActiveID].Data;
            InputHooked.activateFullControl(process);
            Disable();

            Program.chatBox.tb.Inputenabled = true;
        }

        static Animations lastAnim = null;
        public void StartAnim(object sender, mEButton.ButtonPressedEventArg args)
        {
            Animations anim = (Animations)args.Data;
            if(anim == null)
                return;

            lastAnim = anim;

            if (Loader.sOptions.BlockOptionsAnimation.blockInWater && NPCHelper.isInWater(Program.Player)
                || Loader.sOptions.BlockOptionsAnimation.blockWhenDead && NPCHelper.isDead(Program.Player)
                || Loader.sOptions.BlockOptionsAnimation.blockWhenSleep && NPCHelper.isInMagicSleep(Program.Player)
                || Loader.sOptions.BlockOptionsAnimation.blockWhenUnconscious && NPCHelper.isUnconscious(Program.Player))
            {
                Close();
                return;
            }
            if (anim.animation != null && anim.animation != "" && oCNpc.Player(process).HP != 0)
            {
                zString animStr = zString.Create(process, anim.animation.ToUpper().Trim());
                oCNpc.Player(process).GetModel().StartAnimation(animStr);
                oCNpc.Player(process).GetModel().StartAni(animStr, 1);
                animStr.Dispose();
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
