using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using System.Reflection;
using Gothic.zTypes;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    class Externals
    {
        #region General_External_Functions
        public enum Value_Types
        {
            Void = 0,
            End = 0,//Wird immer als letztes hinzugefügt
            NPC = 7,
            String = 3,
            Int = 2,
            Func = 5

        }

        public static void CreateExternalFunction(zCParser parser, String methodname, MethodInfo mi, String dll, Value_Types returnValue, Value_Types[] vTypes)
        {
            Process process = Process.ThisProcess();
            IntPtr injectFunction = WinApi.Kernel.Process.GetProcAddress(WinApi.Kernel.Process.GetModuleHandle("NetInject.dll"), "LoadNetDllEx");
            NETINJECTPARAMS parameters = NETINJECTPARAMS.Create(process, dll, mi.DeclaringType.FullName, mi.Name, methodname);

            List<byte> list = new List<byte>();
            list.Add(0x60);//pushad

            list.Add(0x68);//Parameter für LoadNetDllEx pushen
            list.AddRange(BitConverter.GetBytes(parameters.Address));

            int length = list.Count + 1 + 4 + 1 + 1 + 3;
            IntPtr newASM = process.Alloc((uint)length);
            ////Funktion callen
            list.Add(0xE8);
            list.AddRange(BitConverter.GetBytes(injectFunction.ToInt32() - (newASM.ToInt32() + list.Count) - 4));

            list.Add(0x59);//pop
            list.Add(0x61);//popad

            list.AddRange(new byte[] { 0x33, 0xC0, 0xC3 });
            process.Write(list.ToArray(), newASM.ToInt32());

            zString str = zString.Create(process, methodname);
            CallValue[] values = new CallValue[4 + vTypes.Length + 1];
            values[0] = parser; values[1] = str; values[2] = (IntArg)newASM.ToInt32();
            values[3] = (IntArg)((int)returnValue);

            for (int i = 0; i < vTypes.Length; i++)
            {
                values[i + 4] = (IntArg)((int)vTypes[i]);
            }

            values[values.Length - 1] = (IntArg)((int)Value_Types.End);
            zCParser.DefineExternal(process, values);

        }
        #endregion


        #region External_Creation
        public static Int32 AddExternals(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);


                CreateExternalFunction(new zCParser(process, process.ReadInt(address + 4)), "GUC_SPELL_PROCESSMANA".ToUpper(), typeof(Externals).GetMethod("GUC_PROCESSMANA"), "UntoldChapter\\DLL\\GUC.dll", Value_Types.Int, new Value_Types[] { Value_Types.Int, Value_Types.NPC });
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }

            return 0;
        }
        #endregion

        #region Inputs

        public static Int32 GUC_PROCESSMANA(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int npc_ptr = zCParser.getParser(process).GetInstance();
                int manaInvested = zCParser.getParser(process).getIntParameter();
                
                

                oCNpc npc = new oCNpc(process, npc_ptr);
                int spellID = npc.GetActiveSpellNr();

                Spell spell = null;
                Spell.SpellDict.TryGetValue(spellID, out spell);

                if (spell == null)//Stop the spell!
                {
                    zCParser.getParser(process).SetReturn(3);
                    return 0;
                }
                
                
                if (spell.processMana.Length == 0)
                {
                    zCParser.getParser(process).SetReturn(3);//Stop
                    return 0;
                }
                
                if (spell.processMana.Length == 1)
                {
                    if(npc.MP >= spell.processMana[0])
                        zCParser.getParser(process).SetReturn(2);//Start
                    else
                        zCParser.getParser(process).SetReturn(3);//Stop
                    return 0;
                }
                
                

                int level = npc.GetActiveSpellLevel();
                if (npc.MP < spell.processMana[level])
                {
                    zCParser.getParser(process).SetReturn(0);
                    return 0;
                }

                if (level + 1 == spell.processMana.Length)//Dont Invest anymore
                {
                    zCParser.getParser(process).SetReturn(0);
                    return 0;
                }

                npc.MP -= (npc.MP >= spell.processMana[level]) ? spell.processMana[level] : npc.MP;
                zCParser.getParser(process).SetReturn(0);
                return 4;//Next Level!
                
                //zERROR.GetZErr(process).Report(2, 'G', "GUC: PROCESS MANA "+ npc_ptr+" | "+npc.ObjectName.Value + " | "+oCNpc.Player(process).Address, 0, "Externals.cs", 0);

                //0 => Dont Invest
                //1 => ReceiveInvest
                //2 => SendCast
                //3 => SendStop
                //4 => Next Level
                //8 => SPL_STATUS_CANINVEST_NO_MANADEC
                //SPL_FORCEINVEST		 = 1 << 16
                
                //zCParser.getParser(process).SetReturn(2);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        #endregion

    }
}
