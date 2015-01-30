
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gothic.zClasses;
using WinApi;
using System.Threading;
using GUC.WorldObjects;
using GUC.States;
using GUC.Network;
using Gothic.mClasses;
using WinApi.User.Enumeration;
using Gothic.zTypes;
using GUC.Hooks;
using GUC.WorldObjects.Character;
using GUC.Network.Messages.Connection;
using Gothic.zStruct;
using GUC.Tests;

namespace GUC
{
    public class Program
    {
        public static Client client;
        public static AbstractState _state;

        public static bool newWorld = false;

        public static HookInfos insertItemToList;
        public static HookInfos removeItemFromList;
        public static HookInfos removeItemUsed;

        public static HookInfos IsUnconsciousHook;

        public static HookInfos ParSymbol_GetValueHook;

        public static List<timer.Timer> TimerList = new List<timer.Timer>();


        public static Int32 InjectedMain(String message)
        {
            try
            {
                Process process = Process.ThisProcess();
                
                StartupState.SetUpConfig();

                if (StartupState.clientOptions.SaveMode)
                {
                    //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("OnUnTrigger"), (int)oCMobInter.FuncOffsets.OnUnTrigger, (int)oCMobInter.HookSize.OnUnTrigger, 2);
                    //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMobInter).GetMethod("OnTrigger"), (int)oCMobInter.FuncOffsets.OnTrigger, (int)oCMobInter.HookSize.OnTrigger, 2);

                    if(StartupState.clientOptions.SaveModeMapName.Trim().ToUpper().Length > @"NewWorld\NewWorld.zen".Length){
                        throw new Exception("SaveModeMapName-Name can not be longer than " + @"NewWorld\NewWorld.zen".Length);
                    }
                    
                    ASCIIEncoding enc = new ASCIIEncoding();
                    process.Write(enc.GetBytes(StartupState.clientOptions.SaveModeMapName.Trim().ToUpper()), 0x008907B0);
                    process.Write(new byte[] { 0 }, 0x008907B0 + StartupState.clientOptions.SaveModeMapName.Trim().ToUpper().Length);

                    process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Program).GetMethod("hook_Render_SaveMode"), 0x006C8AB2, 5, 0);
                    process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Program).GetMethod("setVisual_SaveMode"), (int)zCVob.FuncOffsets.SetVisual, (int)zCVob.HookSize.SetVisual, 1);
                    process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Program).GetMethod("hook_Sound"), (int)0x4F10F0, 7, 4);
                }
                else
                {
                    addHooks(process);
                    StartupState.SetUpStartMap();
                    StartupState.SetupFuncBlocking();
                    Hooks.hParser.HookLoadDat();





                    StartupState.Start();

                    _state = new StartupState();
                }

                while (true)
                    Thread.Sleep(10000);
                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }


        public static void addHooks(Process process)
        {
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Externals).GetMethod("AddExternals"), (int)0x006D4780, (int)7, 1);


            //process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("AniCtrl_InitAnimations"), (int)oCAniCtrl_Human.FuncOffsets.InitAnimations, (int)oCAniCtrl_Human.HookSize.InitAnimations, 0);



            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hGame).GetMethod("ExitGame"), (int)CGameManager.FuncOffsets.Done, (int)CGameManager.HookSize.Done, 0);


            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(sWorld).GetMethod("hook_StartChangeLevel"), (int)oCGame.FuncOffsets.ChangeLevel, (int)oCGame.HookSize.ChangeLevel, 2);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(sWorld).GetMethod("hook_EndChangeLevel"), 0x006C7AD5, 7, 0);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(Program).GetMethod("hook_Render"), 0x006C86A0, 7, 0);//Alt: End of Game::Render => 0x006C8AB2, 5



            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItem).GetMethod("InitByScript_End"), 0x007120D1, 6, 0);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hItem).GetMethod("InitByScript"), 0x00711BD0, 6, 2);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("OnDamage_DD"), (int)oCNpc.FuncOffsets.OnDamage_DD, (int)oCNpc.HookSize.OnDamage_DD, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoDie"), (int)oCNpc.FuncOffsets.DoDie, (int)oCNpc.HookSize.DoDie, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoDropVob"), (int)oCNpc.FuncOffsets.DoDropVob, (int)oCNpc.HookSize.DoDropVob, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("DoTakeVob"), (int)oCNpc.FuncOffsets.DoTakeVob, (int)oCNpc.HookSize.DoTakeVob, 1);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_EquipItem"), (int)oCNpc.FuncOffsets.Equip, (int)oCNpc.HookSize.Equip, 1);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("oCNpc_UnEquipItem"), (int)oCNpc.FuncOffsets.UnequipItem, (int)oCNpc.HookSize.UnequipItem, 1);


            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hModelAni).GetMethod("oCStartAnim_ModelAnim"), (int)zCModel.FuncOffsets.StartAni_ModelInt, (int)zCModel.FuncSize.StartAni_ModelInt, 2);

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
            process.Write(new byte[]{0xC2, 0x08, 0x00}, 0x00484150 + 0x18E - 3);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("GetSpellInstanceName"), 0x00484150, 7, 2);

            process.FillWithNull(0x004864B0, 0x0048661F);
            process.Write(new byte[] { 0xC2, 0x04, 0x00 }, 0x0048661F - 3);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("GetName"), 0x004864B0, 7, 1);

            ParSymbol_GetValueHook = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hParser).GetMethod("hook_Symbol_GetValue"), 0x007A2040, 7, 2);

            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hSpell).GetMethod("MagBook_Spell_Cast"), 0x004767A0, 7, 0);
            process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hMagBook).GetMethod("Spell_Invest"), (int)oCMag_Book.FuncOffsets.Spell_Invest, (int)oCMag_Book.HookSize.Spell_Invest, 0);






            process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x08, 0x00 }, removeItemFromList.oldFuncInNewFunc.ToInt32());
            process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, insertItemToList.oldFuncInNewFunc.ToInt32());
            process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x08, 0x00 }, removeItemUsed.oldFuncInNewFunc.ToInt32());


            //process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0xC3 }, (int)oCNpc.FuncOffsets.IsUnconscious + 5);//Removing function of IsUnconscious! Overwriting it in the Hook!
            //IsUnconsciousHook = process.Hook("UntoldChapter\\DLL\\GUC.dll", typeof(hNpc).GetMethod("IsUnconscious"), (int)oCNpc.FuncOffsets.IsUnconscious, (int)5, 0);
            //process.Write(new byte[] { 0x33, 0xC0, 0x90, 0x90, 0x90 }, Program.IsUnconsciousHook.oldFuncInNewFunc.ToInt32());//Default = false
        }


        public static Int32 hook_Sound(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                Process process = Process.ThisProcess();
                zCSoundFX sound = new zCSoundFX(process, process.ReadInt(address + 4));
                zCVob vob = new zCVob(process, process.ReadInt(address + 8));
                int x = process.ReadInt(address + 12);
                zTSound3DParams param = new zTSound3DParams(process, process.ReadInt(address + 16));
                //X => 0,1,2,8      2 => Gespräch, 0 => Ambient? z.B. TorchBourn
                if (x == 2 && vob.VobType == zCVob.VobTypes.Npc)
                {

                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Vob: " + vob.VobType + " | " + vob.ObjectName.Value + " | " + x + " | ", 0, "Program.cs", 0);
                    sound.testValues(1024);
                    for (int i = 0; i < 8; i++)
                    {
                        int add = process.ReadInt(param.Address + (i * 4));
                        zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Param i)" + i + ": 0x" + add.ToString("X4"), 0, "Program.cs", 0);
                        if(add != 0)
                            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Param i)" + i + ": " + new zCVob(process, add).VobType, 0, "Program.cs", 0);
                    }
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Error: "+ex.ToString(), 0, "Program.cs", 0);
            }


            return 0;
        }


        public static Dictionary<int, String> VobVisual = new Dictionary<int, string>();
        public static Int32 setVisual_SaveMode(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                Process process = Process.ThisProcess();
                zString str = new zString(process, process.ReadInt(address + 4));
                String stri = str.Value.Trim();


                if (stri.Length != 0)
                {
                    int addr = process.ReadInt(address);
                    if (VobVisual.ContainsKey(addr))
                        VobVisual[addr] = stri;
                    else
                        VobVisual.Add(addr, stri);
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }
        static bool saveItemInstances = false;
        static bool saveMapVobs = false;
        static bool saveMapItems = false;
        public static Int32 hook_Render_SaveMode(String message)
        {
            Process process = Process.ThisProcess();
            try
            {


                InputHooked.Update(process);




                if (InputHooked.IsPressed((int)DIK_Keys.DIK_O))
                {
                    InputHooked.receivers.Add(new Keytest());
                    //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', new oCMobLockable(process, oCNpc.Player(process).FocusVob.Address).keyInstance.Value + ";" + new oCMobLockable(process, oCNpc.Player(process).FocusVob.Address).PickLockStr.Value, 0, "Program.cs", 0);
                    //new oCMobInter(process, oCNpc.Player(process).FocusVob.Address).StartInteraction(oCNpc.Player(process));
                    //oCGame.Game(process).DiveBar.SetPos(-0x2000, -0x2000);
                    first = false;

                }


                if (InputHooked.IsPressed((int)VirtualKeys.F1) && !saveItemInstances)
                {
                    StringBuilder sb = new StringBuilder();
                    zCArray<zCPar_Symbol> symbolArray = zCParser.getParser(process).Table;
                    int len = symbolArray.Size;
                    //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Symbols: " + len, 0, "Program.cs", 0);
                    for (int i = 0; i < len; i++)
                    {
                        zCPar_Symbol symbol = symbolArray.get(i);
                        String symbolName = symbol.Name.Value.Trim().ToUpper();

                        //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Symbol found: "+symbolName+" | "+symbol.Offset, 0, "Program.cs", 0);
                        if (symbolName.StartsWith("IT"))
                        {
                            oCItem item = oCObjectFactory.GetFactory(process).CreateItem(symbolName);
                            if (item.Address == 0 || item.VobType != zCVob.VobTypes.Item || item.Visual.Value.Trim().Length == 0)
                                continue;

                            String muni = null;
                            if (item.Munition != 0)
                            {
                                oCItem munit = oCObjectFactory.GetFactory(process).CreateItem(item.Munition);
                                muni = munit.ObjectName.Value.Trim().ToUpper();
                            }

                            sb.Append("new ItemInstance(");
                            //DamageType dmgType, int totalDamage, int range,
                            sb.Append("\"" + symbolName + "\", ");
                            sb.Append("\"" + item.Name + "\", ");
                            sb.Append("\"" + item.ScemeName + "\", ");
                            sb.Append("new int[]{" + item.Protection[0] + ", " + item.Protection[1] + ", " + item.Protection[2] + ", " + item.Protection[3] + ", " + item.Protection[4] + ", " + item.Protection[5] + ", " + item.Protection[6] + ", " + item.Protection[7] + "},");
                            sb.Append("new int[]{" + item.Damage[0] + ", " + item.Damage[1] + ", " + item.Damage[2] + ", " + item.Damage[3] + ", " + item.Damage[4] + ", " + item.Damage[5] + ", " + item.Damage[6] + ", " + item.Damage[7] + "},");
                            sb.Append(item.Value + ", ");
                            sb.Append("(MainFlags)(" + item.MainFlag + "), ");
                            sb.Append("(Flags)(" + item.Flags + "), ");
                            sb.Append("(ArmorFlags)(" + item.Wear + "), ");
                            sb.Append("(DamageType)(" + item.DamageType + "), ");
                            sb.Append(item.DamageTotal + ", ");
                            sb.Append(item.Range + ", ");
                            sb.Append("\"" + item.Visual.Value.Trim() + "\", ");
                            sb.Append("\"" + item.VisualChange.Value.Trim() + "\", ");
                            sb.Append("\"" + item.Effect.Value.Trim() + "\", ");
                            sb.Append(item.VisualSkin + ", ");
                            sb.Append("(MaterialTypes)(" + item.Material + "), ");
                            if (muni != null)
                                sb.Append("ItemInstance.getItemInstance(\"" + muni + "\") ");
                            else
                                sb.Append("null");
                            sb.Append(");\r\n");
                        }
                    }


                    File.WriteAllText("cinstances.cs", sb.ToString());

                    saveItemInstances = true;
                }

                if (InputHooked.IsPressed((int)VirtualKeys.F2) && !saveMapVobs)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("MobInter mi = null;");
                    sb.AppendLine("String mapName = @\"" + oCGame.Game(process).World.WorldFileName.Value.Trim() + "\";");
                    Dictionary<zCVob.VobTypes, List<zCVob>> vobDict = oCGame.Game(process).World.getVobLists(zCVob.VobTypes.MobInter, zCVob.VobTypes.MobBed, zCVob.VobTypes.MobSwitch, zCVob.VobTypes.MobDoor, zCVob.VobTypes.MobContainer);


                    foreach (KeyValuePair<zCVob.VobTypes, List<zCVob>> vobList in vobDict)
                    {
                        foreach (zCVob vob in vobList.Value)
                        {
                            if (!VobVisual.ContainsKey(vob.Address))
                            {
                                sb.Append("//");
                            }
                            if (vob.VobType == zCVob.VobTypes.MobInter || vob.VobType == zCVob.VobTypes.MobBed || vob.VobType == zCVob.VobTypes.MobSwitch)
                            {
                                oCMobInter mi = new oCMobInter(process, vob.Address);
                                if (vob.VobType == zCVob.VobTypes.MobInter)
                                    sb.Append("mi = new MobInter(");
                                else if (vob.VobType == zCVob.VobTypes.MobBed)
                                    sb.Append("mi = new MobBed(");
                                else if (vob.VobType == zCVob.VobTypes.MobSwitch)
                                    sb.Append("mi = new MobSwitch(");
                                if (VobVisual.ContainsKey(vob.Address))
                                    sb.Append("\"" + VobVisual[vob.Address] + "\", ");
                                else
                                    sb.Append("\"\", ");
                                sb.Append("\"" + mi.Name.Value.Trim() + "\", ");
                                //sb.Append("" + mi.Rewind.ToString().ToLower() + ", ");
                                //sb.Append("" + mi.StateNum + ", ");
                                if (mi.UseWithItem.Address != 0 && mi.UseWithItem.getCheckedValue() != null)
                                    sb.Append("ItemInstance.getItemInstance(\"" + mi.UseWithItem.Value.Trim().ToUpper() + "\"), ");
                                else
                                    sb.Append("null, ");

                                if (mi.TriggerTarget.Address != 0 && mi.TriggerTarget.getCheckedValue() != null)
                                    sb.Append("\"" + mi.TriggerTarget.Value.Trim() + "\", ");
                                else
                                    sb.Append("null, ");
                                sb.Append(((mi.BitField1 & (int)zCVob.BitFlag0.collDetectionDynamic) == (int)zCVob.BitFlag0.collDetectionDynamic).ToString().ToLower() + ", ");
                                sb.Append(((mi.BitField1 & (int)zCVob.BitFlag0.collDetectionStatic) == (int)zCVob.BitFlag0.collDetectionStatic).ToString().ToLower() + "); \r\n");
                            }

                            if (vob.VobType == zCVob.VobTypes.MobDoor)
                            {
                                oCMobDoor mi = new oCMobDoor(process, vob.Address);
                                sb.Append("mi = new MobDoor(");
                                if (VobVisual.ContainsKey(vob.Address))
                                    sb.Append("\"" + VobVisual[vob.Address] + "\", ");
                                else
                                    sb.Append("\"\", ");
                                sb.Append("\"" + mi.Name.Value.Trim() + "\", ");
                                sb.Append("" + mi.isLocked.ToString().ToLower() + ", ");
                                if (mi.keyInstance.Address != 0 && mi.keyInstance.getCheckedValue() != null)
                                    sb.Append("ItemInstance.getItemInstance(\"" + mi.keyInstance.Value.Trim().ToUpper() + "\"), ");
                                else
                                    sb.Append("null, ");

                                if (mi.PickLockStr.Address != 0 && mi.PickLockStr.getCheckedValue() != null)
                                    sb.Append("\"" + mi.PickLockStr.Value.Trim() + "\", ");
                                else
                                    sb.Append("null, ");

                                if (mi.UseWithItem.Address != 0 && mi.UseWithItem.getCheckedValue() != null)
                                    sb.Append("ItemInstance.getItemInstance(\"" + mi.UseWithItem.Value.Trim().ToUpper() + "\"), ");
                                else
                                    sb.Append("null, ");

                                if (mi.TriggerTarget.Address != 0 && mi.TriggerTarget.getCheckedValue() != null)
                                    sb.Append("\"" + mi.TriggerTarget.Value.Trim() + "\", ");
                                else
                                    sb.Append("null, ");
                                sb.Append(((mi.BitField1 & (int)zCVob.BitFlag0.collDetectionDynamic) == (int)zCVob.BitFlag0.collDetectionDynamic).ToString().ToLower() + ", ");
                                sb.Append(((mi.BitField1 & (int)zCVob.BitFlag0.collDetectionStatic) == (int)zCVob.BitFlag0.collDetectionStatic).ToString().ToLower() + "); \r\n");
                            }

                            if (vob.VobType == zCVob.VobTypes.MobContainer)
                            {
                                oCMobContainer mi = new oCMobContainer(process, vob.Address);
                                sb.Append("mi = new MobContainer(");
                                if (VobVisual.ContainsKey(vob.Address))
                                    sb.Append("\"" + VobVisual[vob.Address] + "\", ");
                                else
                                    sb.Append("\"\", ");
                                sb.Append("\"" + mi.Name.Value.Trim() + "\", ");

                                //ItemLists:
                                sb.Append("new ItemInstance[]{");
                                List<oCItem> itemList = mi.getItemList();
                                for (int i = 0; i < itemList.Count; i++)
                                {
                                    oCItem item = itemList[i];
                                    sb.Append("ItemInstance.getItemInstance(\"" + item.ObjectName.Value.Trim().ToUpper() + "\")");
                                    if (i + 1 < itemList.Count)
                                        sb.Append(", ");
                                }
                                sb.Append("},");
                                sb.Append("new int[]{");
                                for (int i = 0; i < itemList.Count; i++)
                                {
                                    oCItem item = itemList[i];
                                    sb.Append("" + item.Amount);
                                    if (i + 1 < itemList.Count)
                                        sb.Append(", ");
                                }
                                sb.Append("},");


                                sb.Append("" + mi.isLocked.ToString().ToLower() + ", ");
                                if (mi.keyInstance.Address != 0 && mi.keyInstance.getCheckedValue() != null)
                                    sb.Append("ItemInstance.getItemInstance(\"" + mi.keyInstance.Value.Trim().ToUpper() + "\"), ");
                                else
                                    sb.Append("null, ");

                                if (mi.PickLockStr.Address != 0 && mi.PickLockStr.getCheckedValue() != null)
                                    sb.Append("\"" + mi.PickLockStr.Value.Trim() + "\", ");
                                else
                                    sb.Append("null, ");

                                if (mi.UseWithItem.Address != 0 && mi.UseWithItem.getCheckedValue() != null)
                                    sb.Append("ItemInstance.getItemInstance(\"" + mi.UseWithItem.Value.Trim().ToUpper() + "\"), ");
                                else
                                    sb.Append("null, ");

                                if (mi.TriggerTarget.Address != 0 && mi.TriggerTarget.getCheckedValue() != null)
                                    sb.Append("\"" + mi.TriggerTarget.Value.Trim() + "\", ");
                                else
                                    sb.Append("null, ");
                                sb.Append(((mi.BitField1 & (int)zCVob.BitFlag0.collDetectionDynamic) == (int)zCVob.BitFlag0.collDetectionDynamic).ToString().ToLower() + ", ");
                                sb.Append(((mi.BitField1 & (int)zCVob.BitFlag0.collDetectionStatic) == (int)zCVob.BitFlag0.collDetectionStatic).ToString().ToLower() + "); \r\n");
                            }


                            sb.Append("mi.Spawn(mapName, new Vec3f(");
                            sb.Append(vob.TrafoObjToWorld.getPosition()[0].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getPosition()[1].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getPosition()[2].ToString().Replace(",", ".") + "f), new Vec3f(");
                            sb.Append(vob.TrafoObjToWorld.getDirection()[0].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getDirection()[1].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getDirection()[2].ToString().Replace(",", ".") + "f));\r\n\r\n");

                        }
                    }


                    File.WriteAllText("cVobs.cs", sb.ToString());
                    saveMapVobs = true;

                }


                if (InputHooked.IsPressed((int)VirtualKeys.F3) && !saveMapItems)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Item mi = null;");
                    sb.AppendLine("String mapName = @\"" + oCGame.Game(process).World.WorldFileName.Value.Trim() + "\";");
                    Dictionary<zCVob.VobTypes, List<zCVob>> vobDict = oCGame.Game(process).World.getVobLists(zCVob.VobTypes.Item);


                    foreach (KeyValuePair<zCVob.VobTypes, List<zCVob>> vobList in vobDict)
                    {
                        foreach (zCVob vob in vobList.Value)
                        {
                            if (vob.VobType == zCVob.VobTypes.Item)
                            {
                                oCItem mi = new oCItem(process, vob.Address);

                                sb.Append("mi = new Item(");
                                sb.Append("ItemInstance.getItemInstance(\"" + mi.ObjectName.Value.Trim().ToUpper() + "\"), ");
                                sb.Append("" + mi.Amount + ");\r\n");

                            }


                            sb.Append("mi.Spawn(mapName, new Vec3f(");
                            sb.Append(vob.TrafoObjToWorld.getPosition()[0].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getPosition()[1].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getPosition()[2].ToString().Replace(",", ".") + "f), new Vec3f(");
                            sb.Append(vob.TrafoObjToWorld.getDirection()[0].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getDirection()[1].ToString().Replace(",", ".") + "f, " + vob.TrafoObjToWorld.getDirection()[2].ToString().Replace(",", ".") + "f));\r\n\r\n");

                        }
                    }


                    File.WriteAllText("cItems.cs", sb.ToString());
                    saveMapItems = true;
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            
            return 0;
        }

        static bool first = true;
        static bool spawned = false;

        public delegate void hook_RenderEvent(Process process, long now);
        public static hook_RenderEvent OnRender;
        public static hook_RenderEvent OnRenderTimedSecond;//Each second 1 call!

        public static long s_OnRenderCalledLastSecond = 0;
        public static Int32 hook_Render(String message)
        {
            try
            {
                Process process = Process.ThisProcess();


                InputHooked.Update(process);

                
                _state.Init();
                _state.update();


                long time = DateTime.Now.Ticks;
                timer.Timer[] arr = TimerList.ToArray();
                foreach (timer.Timer timer in arr)
                {
                    timer.iUpdate(time);
                }
                if(OnRender != null)
                    OnRender(process, time);

                if (OnRenderTimedSecond != null && s_OnRenderCalledLastSecond < time)
                {
                    OnRenderTimedSecond(process, time);
                    s_OnRenderCalledLastSecond = time + 10000000;
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }
    }
}
