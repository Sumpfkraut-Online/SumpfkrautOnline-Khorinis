using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using GUC.WorldObjects.Character;
using GUC.Enumeration;
using RakNet;

namespace GUC.Hooks
{
    class hSpell
    {


        public static int itemaddr;
        public static Int32 InitByScript(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                itemaddr = Process.ThisProcess().ReadInt(address);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }

        public static Int32 InitByScript_End(String message)
        {
            try
            {
                Process process = Process.ThisProcess();

                oCSpell spell = new oCSpell(process, itemaddr);

                Spell nSpell = null;
                Spell.SpellDict.TryGetValue(spell.SpellID, out nSpell);

                if (nSpell == null)
                {
                    nSpell = new Spell();//Set Default-Values... could make problems in later process...
                }

                nSpell.toSpell(spell);

                //Init Spell:
                //Defaults:
                //spell.TimePerMana = 500;
                //spell.DamagePerLevel = 1;
                //spell.DamageType = (int)Gothic.zStruct.oSDamageDescriptor.DamageTypes.DAM_MAGIC;
                //spell.SpellType = 2;
                //spell.CanTurnDuringInvest = 1;
                //spell.CanChangeTargetDuringInvest = 1;
                //spell.IsMultiEffect = 0;
                //spell.TargetCollectionAlgo = 4;
                //spell.TargetCollectType = 4;
                //spell.TargetCollectRange = 10000;
                //spell.TargetCollectAzi = 60;
                //spell.TargetCollectElev = 60;


                //spell.TimePerMana = 0;
                //spell.DamagePerLevel = 25;
                //spell.DamageType = (int)Gothic.zStruct.oSDamageDescriptor.DamageTypes.DAM_MAGIC;

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }


        public static Int32 GetSpellInstanceName(String message)
        {
            try
            {
                Process process = Process.ThisProcess();
                int address = Convert.ToInt32(message);
                int spelladdr = process.ReadInt(address);
                int spellString = process.ReadInt(address + 4);//Stack pointer will be pushed as variable
                int spellID = process.ReadInt(address + 8);

                zString str = new zString(process, spellString);

                Spell spell = null;
                Spell.SpellDict.TryGetValue(spellID, out spell);
                if (spell == null)
                    spell = new Spell();
                String value = spell.FXName;
                //Generating Buffer with String:
                
                System.Text.Encoding enc = System.Text.Encoding.Default;
                byte[] arr = enc.GetBytes(value);

                //Creating Pointer to char*
                IntPtr charArr = process.Alloc((uint)arr.Length + 1);
                if (arr.Length > 0)
                    process.Write(arr, charArr.ToInt32());

                //Calling constructor and free char*
                process.THISCALL<NullReturnCall>((uint)spellString, (uint)0x004010C0, new CallValue[] { new IntArg(charArr.ToInt32()) });
                process.Free(charArr, (uint)arr.Length + 1);





                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Spell: GetSpellInstanceName SpellID: " + spellID, 0, "hItem.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }

        public static Int32 GetName(String message)
        {
            try
            {
                Process process = Process.ThisProcess();
                int address = Convert.ToInt32(message);
                int spelladdr = process.ReadInt(address);
                int spellString = process.ReadInt(address + 4);//Stack pointer will be pushed as variable
                

                zString str = new zString(process, spellString);
                oCSpell gSpell = new oCSpell(process, spelladdr);

                Spell spell = null;
                Spell.SpellDict.TryGetValue(gSpell.SpellID, out spell);
                if (spell == null)
                    spell = new Spell();
                String value = spell.Name;


                //Generating Buffer with String:
                
                System.Text.Encoding enc = System.Text.Encoding.Default;
                byte[] arr = enc.GetBytes(value);

                //Creating Pointer to char*
                IntPtr charArr = process.Alloc((uint)arr.Length + 1);
                if (arr.Length > 0)
                    process.Write(arr, charArr.ToInt32());

                //Calling constructor and free char*
                process.THISCALL<NullReturnCall>((uint)spellString, (uint)0x004010C0, new CallValue[] { new IntArg(charArr.ToInt32()) });
                process.Free(charArr, (uint)arr.Length + 1);





                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Spell: getName SpellID: ", 0, "hItem.cs", 0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }



        public static Int32 MagBook_Spell_Cast(String message)
        {
            try
            {
                Process process = Process.ThisProcess();
                int address = Convert.ToInt32(message);

                oCMag_Book magBook = new oCMag_Book(process, process.ReadInt(address));

                oCNpc player = oCNpc.Player(process);
                if (player.MagBook.Address != magBook.Address)
                    return 0;

                //int spellID = player.GetActiveSpellNr();
                oCSpell gSpell = magBook.GetSelectedSpell();


                if (gSpell.Caster == null || gSpell.Caster.Address != player.Address)
                    return 0;

                NPCProto caster = Player.Hero;
                Vob target = null;
                if (gSpell.Target != null && gSpell.Target.Address != 0)
                {
                    sWorld.SpawnedVobDict.TryGetValue(gSpell.Target.Address, out target);
                    
                }

                //Item:
                oCItem spellItem = magBook.GetSpellItem(magBook.GetSelectedSpellNr());
                Vob spellI = null;
                sWorld.SpawnedVobDict.TryGetValue(spellItem.Address, out spellI);
                int itemID = 0;
                if (spellI != null && spellI is Item)
                    itemID = spellI.ID;

                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.CastSpell);
                stream.Write(itemID);
                stream.Write(caster.ID);
                if (target == null)
                    stream.Write(0);
                else
                    stream.Write(target.ID);
                stream.Write(gSpell.GetSpellID());
                
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Hook oCStartAnim_ModelAnim: " + ex.ToString(), 0, "Hooks.zCModelHook.cs", 0);
            }
            return 0;
        }



    }
}
