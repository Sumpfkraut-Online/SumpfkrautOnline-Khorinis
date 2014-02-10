using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using Gothic.mClasses;
using AdminAndListModule.Lists;
using WinApi;
using ListModule.Lists;
using Gothic.zClasses;
using Gothic.zTypes;
using GMP.Helper;
using ListModule.Messages;
using Injection;

namespace AdminAndListModule
{
    public class Updater : UpdateModule, InputReceiver
    {
        bool started;
        public override void update(Network.Module module)
        {
            if (!started)
            {
                GETAVAILABLEKEYLIST();
                InputHooked.receivers.Add(this);
                started = true;
            }
        }

        List<Animations> availableAnimations = new List<Animations>();
        private void GETAVAILABLEKEYLIST()
        {
            
            if (Loader.sOptions.animationListWhiteList)
            {
                //WhiteList
                foreach (Animations keyAnim in Loader.sClientOptions.AnimationKeyList)
                {
                    //Nur hinzufügen wenns in der WhiteList enthalten ist
                    foreach (String whiteListAnim in Loader.sOptions.AvailableAnimations)
                    {
                        if (keyAnim.animation == whiteListAnim)
                        {
                            availableAnimations.Add(keyAnim);
                            break;
                        }
                    }
                }
            }
            else
            {
                //Blacklist
                foreach (Animations keyAnim in Loader.sClientOptions.AnimationKeyList)
                {
                    availableAnimations.Add(keyAnim);
                }

                //Löschen...
                Animations[] animArr = availableAnimations.ToArray();
                foreach (Animations keyAnim in animArr)
                {
                    foreach (String whiteListAnim in Loader.sOptions.AvailableAnimations)
                    {
                        if (keyAnim.animation == whiteListAnim)
                        {
                            availableAnimations.Remove(keyAnim);
                            break;
                        }
                    }
                }
            }
        }

        public void wheelChanged(int steps) { }

        public void KeyReleased(int key) 
        {
            //if (opened)
            //    return;
            //if (playerList != null && key == Loader.sClientOptions.playerListKey &&
            //        (Loader.sOptions.AdminAvailable || Loader.sOptions.PlayerListAvailable))
            //{
            //    OpenClosePlayerList();
            //}
            //else if (animationList != null && key == Loader.sClientOptions.animationKey
            //    && Loader.sOptions.animationListAvailable)
            //{
            //    OpenCloseAnimationList();
            //}
            //else if (soundList != null && key == Loader.sClientOptions.soundKey
            //    && Loader.sOptions.speakListAvailable)
            //{
            //    OpenCloseSoundList();
            //}
        }


        bool opened;
        public void KeyPressed(int key)
        {
            opened = false;
            if ( key == Loader.sClientOptions.playerListKey &&
                    (Loader.sOptions.AdminAvailable || Loader.sOptions.PlayerListAvailable))//&& anim,player,soundlist == null
            {
                OpenClosePlayerList();
                opened = true;
            }
            else if (  key == Loader.sClientOptions.animationKey
                && Loader.sOptions.animationListAvailable)
            {
                OpenCloseAnimationList();
                opened = true;

            }
            else if ( key == Loader.sClientOptions.soundKey
                && Loader.sOptions.speakListAvailable)
            {
                OpenCloseSoundList();
                opened = true;
            }



            //Keys Abfragen
            if (Loader.sOptions.speakListAvailable || Loader.sOptions.animationListAvailable)
            {
                Process process = Process.ThisProcess();
                foreach (Animations anim in availableAnimations)
                {
                    if (key != anim.key)
                        continue;

                    if (anim.animation != null && anim.animation != "" && oCNpc.Player(process).HP != 0 &&
                        Loader.sOptions.animationListAvailable)//Animation ausführen
                    {
                        zString animStr = zString.Create(process, anim.animation.ToUpper().Trim());
                        oCNpc.Player(process).GetModel().StartAnimation(animStr);
                        oCNpc.Player(process).GetModel().StartAni(animStr, 1);
                        animStr.Dispose();
                    }

                    if (Loader.sOptions.speakListAvailable &&
                        anim.SVM != null && anim.SVM != "" && oCNpc.Player(process).HP != 0)
                    {
                        External_Helper.AI_OutputSVM_Overlay(process, oCNpc.Player(process), oCNpc.Player(process), anim.SVM);
                        //new SoundSynchMessage().Write(Program.client.sentBitStream, Program.client, anim.SVM);
                    }
                }
            }
            
        }

        Playerlist playerList = null;
        public void OpenClosePlayerList()
        {
            if ((animationList != null && animationList.isEnabled())
                || (soundList != null && soundList.isEnabled()))
                return;
            Process process = Process.ThisProcess();
            if (playerList == null)
            {
                playerList = new Playerlist(process);
                playerList.Open();
            }
            else
            {
                playerList.Close();
                playerList = null;
            }
        }

        AnimationList animationList = null;
        public void OpenCloseAnimationList()
        {
            if ((playerList != null && playerList.isEnabled())
                || (soundList != null && soundList.isEnabled()))
                return;
            Process process = Process.ThisProcess();
            if (animationList == null || !animationList.isEnabled())
            {
                if (Loader.sOptions.BlockOptionsAnimation.blockInWater && NPCHelper.isInWater(Program.Player)
                || Loader.sOptions.BlockOptionsAnimation.blockWhenDead && NPCHelper.isDead(Program.Player)
                || Loader.sOptions.BlockOptionsAnimation.blockWhenSleep && NPCHelper.isInMagicSleep(Program.Player)
                || Loader.sOptions.BlockOptionsAnimation.blockWhenUnconscious && NPCHelper.isUnconscious(Program.Player))
                    return;

                animationList = new AnimationList(process);
                animationList.Open();
            }
            else
            {
                animationList.Close();
                animationList = null;
            }
        }

        SoundList soundList = null;
        public void OpenCloseSoundList()
        {
            if ((playerList != null && playerList.isEnabled()) || 
                (animationList != null && animationList.isEnabled()))
                return;
            Process process = Process.ThisProcess();
            if (soundList == null || !soundList.isEnabled())
            {
                if (Loader.sOptions.BlockOptionsSpeak.blockInWater && NPCHelper.isInWater(Program.Player)
                || Loader.sOptions.BlockOptionsSpeak.blockWhenDead && NPCHelper.isDead(Program.Player)
                || Loader.sOptions.BlockOptionsSpeak.blockWhenSleep && NPCHelper.isInMagicSleep(Program.Player)
                || Loader.sOptions.BlockOptionsSpeak.blockWhenUnconscious && NPCHelper.isUnconscious(Program.Player))
                    return;
                soundList = new SoundList(process);
                soundList.Open();
            }
            else
            {
                soundList.Close();
                soundList = null;
            }
        }
    }
}
