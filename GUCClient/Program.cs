
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using WinApi;
using System.Threading;
using GUC.Client.Network;
using GUC.Client.States;
using Gothic.zClasses;
using GUC.Client.Hooks;
using GUC.Client.WorldObjects;
using Gothic.zTypes;
using Gothic.zStruct;

namespace GUC.Client
{
    public class Program
    {
        public static Process Process = Process.ThisProcess();

        public static Network.Client client;

        public static HookInfos insertItemToList;
        public static HookInfos removeItemFromList;
        public static HookInfos removeItemUsed;
        public static HookInfos IsUnconsciousHook;
        public static HookInfos ParSymbol_GetValueHook;

        public static AbstractState _state = null;

        public static Int32 InjectedMain(String message)
        {
            try
            {
                StartupState.SetUpConfig(); //read client options

                //add hooks etc
                addHooks(Process);
                StartupState.SetupFuncBlocking();
                Hooks.hParser.HookLoadDat();

                StartupState.Start(); //connect to the server

                // FOR TESTING PURPOSES
                // first and last byte only for cpu
                // for bytes in mid integer
                //Process.Write(new byte[] { 0xB8, 0x10, 0x00, 0x00, 0x00, 0x90 }, 0x005E1F78);
                //Process.Write(new byte[] { 0xB8, 0x10, 0x00, 0x00, 0x00, 0x90 }, 0x005E1F0F);

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', ex.Source + " " + ex.Message, 0, "Program.cs", 0);
            }
            return 0;
        }

        public static Int32 hook_MenuRender(String message)
        {
            try
            {
                if (_state == null)
                {
                    System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture; // damit ToString bei Floats nen Punkt setzt. Bullshit!

                    _state = new StartupState();
                    Network.Messages.AccountMessage.Login(); //try to log into last account
                    
                    // play our own menu theme
                    zCSndSys_MSS ss = zCSndSys_MSS.SoundSystem(Process);
                    using (zString z = zString.Create(Process, "MENUTHEME.WAV"))
                    {
                        zCSoundFX snd = ss.LoadSoundFX(z);
                        snd.isFixed = true; //so it continues playing during the loading screen
                        ss.PlaySound(snd, 0, 0, 0.8f*GetMusicVol()); //nerf volume
                    }
                }
                _state.Update();
            }
            catch (Exception e)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', e.Source + "\n" + e.Message + "\n" + e.StackTrace, 0, "Program.cs", 0);
            }
            return 0;
        }

        public static void addHooks(Process process)
        {
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Program).GetMethod("hook_MenuRender"), 0x004DC1C0, 10, 0);

            process.WriteJMP("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoTakeVob"), (int)oCNpc.FuncOffsets.DoTakeVob, (int)oCNpc.HookSize.DoTakeVob, 2);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Program).GetMethod("hook_Render"), 0x006C86A0, 7, 0);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(GUI.MenuRenderer).GetMethod("hook_OnDrawItems"), 0x007A6750, 5, 0);//for rendering Vobs in GUI

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(World).GetMethod("hook_StartChangeLevel"), (int)oCGame.FuncOffsets.ChangeLevel, (int)oCGame.HookSize.ChangeLevel, 2);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(World).GetMethod("hook_EndChangeLevel"), 0x006C7AD5, 7, 0);

            ParSymbol_GetValueHook = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hParser).GetMethod("hook_Symbol_GetValue"), 0x007A2040, 7, 2);

            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Externals).GetMethod("AddExternals"), (int)0x006D4780, (int)7, 1);

            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hModelAni).GetMethod("oCStartAni_ModelInt"), (int)zCModel.FuncOffsets.StartAni_ModelInt, (int)zCModel.FuncSize.StartAni_ModelInt, 2);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hGame).GetMethod("ExitGame"), (int)CGameManager.FuncOffsets.Done, (int)CGameManager.HookSize.Done, 0);

            /*
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("AniCtrl_InitAnimations"), (int)oCAniCtrl_Human.FuncOffsets.InitAnimations, (int)oCAniCtrl_Human.HookSize.InitAnimations, 0);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("OnDamage_DD"), (int)oCNpc.FuncOffsets.OnDamage_DD, (int)oCNpc.HookSize.OnDamage_DD, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoDie"), (int)oCNpc.FuncOffsets.DoDie, (int)oCNpc.HookSize.DoDie, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoDropVob"), (int)oCNpc.FuncOffsets.DoDropVob, (int)oCNpc.HookSize.DoDropVob, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoTakeVob"), (int)oCNpc.FuncOffsets.DoTakeVob, (int)oCNpc.HookSize.DoTakeVob, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_EquipItem"), (int)oCNpc.FuncOffsets.Equip, (int)oCNpc.HookSize.Equip, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_UnEquipItem"), (int)oCNpc.FuncOffsets.UnequipItem, (int)oCNpc.HookSize.UnequipItem, 1);


 
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("OnUnTrigger"), (int)oCMobInter.FuncOffsets.OnUnTrigger, (int)oCMobInter.HookSize.OnUnTrigger, 2);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("OnTrigger"), (int)oCMobInter.FuncOffsets.OnTrigger, (int)oCMobInter.HookSize.OnTrigger, 2);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("StartInteraction"), (int)oCMobInter.FuncOffsets.StartInteraction, (int)oCMobInter.HookSize.StartInteraction, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("StopInteraction"), (int)oCMobInter.FuncOffsets.StopInteraction, (int)oCMobInter.HookSize.StopInteraction, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("PickLock"), (int)oCMobLockable.FuncOffsets.PickLock, (int)oCMobLockable.HookSize.PickLock, 2);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("StartStateChange"), (int)oCMobInter.FuncOffsets.StartStateChange, (int)oCMobInter.HookSize.StartStateChange, 3);



            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItemContainer).GetMethod("oCItemContainer_Remove_2"), (int)oCNpcInventory.FuncOffsets.Remove_Item, (int)oCNpcInventory.HookSize.Remove_Item, 1);
            removeItemFromList = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItemContainer).GetMethod("oCItemContainer_Remove_2"), (int)oCItemContainer.FuncOffsets.Remove_2, (int)oCItemContainer.HookSizes.Remove_2, 2);
            insertItemToList = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItemContainer).GetMethod("oCItemContainer_Insert"), (int)oCItemContainer.FuncOffsets.Insert, (int)oCItemContainer.HookSizes.Insert, 1);

            removeItemUsed = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItemContainer).GetMethod("oCItemContainer_Remove"), (int)oCNpcInventory.FuncOffsets.Remove_Int_Int, (int)oCNpcInventory.HookSize.Remove_Int_Int, 2);//Will be called when using item!

            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_UseItem"), (int)oCNpc.FuncOffsets.UseItem, (int)oCNpc.HookSize.UseItem, 1);
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_EV_UseItemToState"), (int)oCNpc.FuncOffsets.EV_UseItemToState, (int)oCNpc.HookSize.EV_UseItemToState, 1);
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_EV_UseItem"), (int)oCNpc.FuncOffsets.EV_UseItem, (int)oCNpc.HookSize.EV_UseItem, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_EV_UseItemToState"), (int)oCNpc.FuncOffsets.EV_UseItemToState, (int)oCNpc.HookSize.EV_UseItemToState, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_EV_UseItemToState_CALLFUNC"), (int)oCNpc.FuncOffsets.EV_UseItemToState + 0x48D, 6, 0);

            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("CloseInventory"), (int)oCNpc.FuncOffsets.CloseInventory, (int)oCNpc.HookSize.CloseInventory, 0);
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("OpenInventory"), (int)oCNpc.FuncOffsets.OpenInventory, (int)oCNpc.HookSize.OpenInventory, 1);

            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("EV_CreateInteractItem"), (int)0x00754890, (int)7, 1);
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("EV_CreateInteractItem"), (int)0x007546F0, (int)5, 1);
            //process.Write(new byte[] { 0xB8, 0x01, 0x00, 0x00, 0x00, 0xC2, 0x04, 0x00 }, 0x007546F0);
            //

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItemContainer).GetMethod("StealContainer_setOwner"), (int)oCStealContainer.FuncOffsets.SetOwner, (int)oCStealContainer.HookSizes.SetOwner, 1);
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItemContainer).GetMethod("StealContainer_setOwner"), (int)0x0070B6CE, (int)5, 0);

            //Spells:
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("InitByScript_End"), 0x00484550 + 0x290, 7, 0);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("InitByScript"), 0x00484550, 7, 1);
            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("Constructor"), 0x00483DD0, 7, 1);

            process.FillWithNull(0x00484150, 0x004842DE);
            process.Write(new byte[] { 0xC2, 0x08, 0x00 }, 0x00484150 + 0x18E - 3);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("GetSpellInstanceName"), 0x00484150, 7, 2);

            process.FillWithNull(0x004864B0, 0x0048661F);
            process.Write(new byte[] { 0xC2, 0x04, 0x00 }, 0x0048661F - 3);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("GetName"), 0x004864B0, 7, 1);

            

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("MagBook_Spell_Cast"), 0x004767A0, 7, 0);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMagBook).GetMethod("Spell_Invest"), (int)oCMag_Book.FuncOffsets.Spell_Invest, (int)oCMag_Book.HookSize.Spell_Invest, 0);

            process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x08, 0x00 }, removeItemFromList.oldFuncInNewFunc.ToInt32());
            process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, insertItemToList.oldFuncInNewFunc.ToInt32());
            process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x08, 0x00 }, removeItemUsed.oldFuncInNewFunc.ToInt32());

            //process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xC3 }, (int)oCNpc.FuncOffsets.IsUnconscious + 5);//Removing function of IsUnconscious! Overwriting it in the Hook!
            //IsUnconsciousHook = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("IsUnconscious"), (int)oCNpc.FuncOffsets.IsUnconscious, (int)5, 0);
            //process.Write(new byte[] { 0x33, 0xC0, 0x90, 0x90, 0x90 }, Program.IsUnconsciousHook.oldFuncInNewFunc.ToInt32());//Default = false*/
        }

        public static Int32 hook_Render(String message)
        {
            try
            {
                _state.Update();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }

        public static float GetMusicVol()
        {
            return Single.Parse(zCOption.GetOption(Process).getEntryValue("SOUND", "musicVolume"));
        }

        public static float GetSoundVol()
        {
            return Single.Parse(zCOption.GetOption(Process).getEntryValue("SOUND", "soundVolume"));
        }
    }
}
