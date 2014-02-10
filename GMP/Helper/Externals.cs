using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GMP.Modules;
using Gothic.mClasses;
using Network;
using Gothic.zTypes;
using System.Reflection;
using Injection;

namespace GMP.Helper
{
    public class Externals
    {
        #region Gernal_External_Functions
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

                CreateExternalFunction(new zCParser(process, process.ReadInt(address + 4)), "GUC_GetPlayerCount".ToUpper(), typeof(Externals).GetMethod("GetPlayerCount"), "GUC.dll", Value_Types.Int, new Value_Types[] { });
                CreateExternalFunction(new zCParser(process, process.ReadInt(address + 4)), "GUC_GetPlayerID".ToUpper(), typeof(Externals).GetMethod("GetPlayerID"), "GUC.dll", Value_Types.Int, new Value_Types[] { Value_Types.NPC });
                CreateExternalFunction(new zCParser(process, process.ReadInt(address + 4)), "GUC_GetIsFriend".ToUpper(), typeof(Externals).GetMethod("GetIsFriend"), "GUC.dll", Value_Types.Int, new Value_Types[] { Value_Types.NPC });
                CreateExternalFunction(new zCParser(process, process.ReadInt(address + 4)), "GUC_IsKeyPressed".ToUpper(), typeof(Externals).GetMethod("GUC_Is_Key_Pressed"), "GUC.dll", Value_Types.Int, new Value_Types[] { Value_Types.Int });
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }

            return 0;
        }
        #endregion


        #region ExternalFunctions

        
        #region Tests
        public static Int32 GetPlayerCount(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                zCParser.getParser(process).SetReturn(StaticVars.playerlist.Count);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        /// <summary>
        /// External Funktion, die die Spieler-ID zurückgibt. Kann nur für Spieler angewendet werden, die sich auf der Map befinden.
        /// 
        /// GetIntParameter und folgende Zeile, ergeben das gleiche Ergebnis:
        //  oCNpc npc = new oCNpc(process, zCParser.getParser(process).GetSymbol(x).Offset);
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GetPlayerID(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int npc_ptr = zCParser.getParser(process).GetInstance();
                Player npcPlayer = StaticVars.spawnedPlayerDict[npc_ptr];
                zCParser.getParser(process).SetReturn(npcPlayer.id);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }


        public static Int32 GetIsFriend(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                //int x = process.STDCALL<IntArg>((uint)0x006DB090, new CallValue[] { (IntArg)0x0082E6F0, (IntArg)1 });
                int npc_ptr = zCParser.getParser(process).GetInstance();
                Player npcPlayer = StaticVars.spawnedPlayerDict[npc_ptr];
                if(npcPlayer.isFriend == 1)
                    zCParser.getParser(process).SetReturn(1);
                else
                    zCParser.getParser(process).SetReturn(0);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }
        #endregion

        #region Transforms
        public static oCNpc npcBeforeTransform = null;
        /// <summary>
        /// GUC_HERO_Transform
        /// </summary>
        /// <param name="instance">Instance-Name: String</param>
        ///<param name="instance">Zurücksetzen durch Enter: boolean</param>
        /// <returns></returns>
        public static Int32 GUC_HERO_Transform(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                //int x = process.STDCALL<IntArg>((uint)0x006DB090, new CallValue[] { (IntArg)0x0082E6F0, (IntArg)1 });
                int a = 0;
                int x = zCParser.getParser(process).getIntParameter();

                zCParser.getParser(process).SetReturn(-1);
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }
        #endregion

        #region Inputs

        public static Int32 GUC_Is_Key_Pressed(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int key = zCParser.getParser(process).getIntParameter();

                if(InputHooked.IsPressed(key))
                    zCParser.getParser(process).SetReturn(1);
                else
                    zCParser.getParser(process).SetReturn(0);

                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        #endregion

        #region Variables
        /// <summary>
        /// int GUC_GetVariable(int playerid, int variable)
        /// Gibt den Wert einer Variable zurück, die durch jeden Spieler gesetzt werden kann.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GUC_GetVariable(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int playerID = zCParser.getParser(process).getIntParameter();
                int variable = zCParser.getParser(process).getIntParameter();

                //SetableUserData
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }


        /// <summary>
        /// void GUC_SetVariable(int playerid, int variable, int value)
        /// Setzt den Wert für eine Variable. Dieser Wert wird automatisch übertragen.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GUC_SetVariable(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int playerID = zCParser.getParser(process).getIntParameter();
                int variable = zCParser.getParser(process).getIntParameter();
                int value = zCParser.getParser(process).getIntParameter();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }
        #endregion


        #region ControllerFunctions
        /// <summary>
        /// int GUC_HasControl(int npcid)
        /// Gibt 1 zurück, wenn der Spieler die Kontrolle über ein NPC hat
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GUC_HasControl(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int npcID = zCParser.getParser(process).getIntParameter();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        /// <summary>
        /// int GUC_GetControl(int npcid)
        /// Fordert die Kontrolle über einen NPC an.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GUC_GetControl(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int npcID = zCParser.getParser(process).getIntParameter();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        /// <summary>
        /// int GUC_CanGetControl(int npcid)
        /// Überprüft, ob die Kontrolle übernommen werden kann
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GUC_CanGetControl(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int npcID = zCParser.getParser(process).getIntParameter();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        #endregion

        #region PlayerFunctions

        /// <summary>
        /// int GUC_TeleportToPlayer(int playerid, int otherplayerid)
        /// Teleportiert den Spieler mit der playerid zu dem Spieler mit der otherplayerid
        /// playerid kann auch -1 annehmen. Dann werden alle Spieler teleportiert
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 GUC_TeleportToPlayer(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int playerID = zCParser.getParser(process).getIntParameter();
                int otherPlayerID = zCParser.getParser(process).getIntParameter();

                if (otherPlayerID < 0)
                {
                    zCParser.getParser(process).SetReturn(0);
                    return 0;
                }

                Player toPlayer = StaticVars.AllPlayerDict[otherPlayerID];
                if (toPlayer == null)//Wurde nicht gefunden, wird also nicht teleportiert!
                {
                    zCParser.getParser(process).SetReturn(0);
                    return 0;
                }

                if (playerID != -1)
                {
                    Player pl = StaticVars.AllPlayerDict[playerID];//Spieler der teleportiert wird!

                    //TODO: Teleport-Command

                    zCParser.getParser(process).SetReturn(1);
                    return 0;
                }
                else
                {
                    foreach (Player pl in Program.playerList)//Alle Spieler!
                    {
                        //TODO: Teleport-Command

                    }
                    zCParser.getParser(process).SetReturn(1);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "Externals.cs", 0);
            }
            return 0;
        }

        #endregion

        #endregion
    }
}
