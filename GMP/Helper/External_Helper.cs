using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using System.Reflection;

namespace GMP.Helper
{
    public class External_Helper
    {
        //MOD_Output
        public static void AI_Output(Process process, oCNpc self, oCNpc other, String svm)
        {
            StaticVars.STRHELPER.Name.Set(svm.Trim());

            zCParser.getParser(process).SetInstance(zString.Create(process, "SELF"), self.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "OTHER"), other.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "STRHELPER"), StaticVars.STRHELPER.Address);

            //MOD_Test_OutputSVM_Overlay
            zString str = zString.Create(process, "MOD_Output");
            int id = zCParser.getParser(process).GetIndex(str);

            str.Dispose();

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }

        public static void AI_OutputSVM(Process process, oCNpc self, oCNpc other, String svm)
        {
            StaticVars.STRHELPER.Name.Set(svm.Trim());

            zCParser.getParser(process).SetInstance(zString.Create(process, "SELF"), self.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "OTHER"), other.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "STRHELPER"), StaticVars.STRHELPER.Address);

            //MOD_Test_OutputSVM_Overlay
            zString str = zString.Create(process, "MOD_OutputSVM");
            int id = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }

        public static void AI_OutputSVM_Overlay(Process process, oCNpc self, oCNpc other, String svm)
        {
            StaticVars.STRHELPER.Name.Set(svm.Trim());

            zCParser.getParser(process).SetInstance(zString.Create(process, "SELF"), self.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "OTHER"), other.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "STRHELPER"), StaticVars.STRHELPER.Address);

            //MOD_Test_OutputSVM_Overlay
            zString str = zString.Create(process, "MOD_OutputSVM_Overlay");
            int id = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }

        public static void Print(Process process, String text)
        {
            StaticVars.STRHELPER.Name.Set(text);
            zCParser.getParser(process).SetInstance(zString.Create(process, "STRHELPER"), StaticVars.STRHELPER.Address);

            //MOD_Test_OutputSVM_Overlay
            zString str = zString.Create(process, "MOD_Print");
            int id = zCParser.getParser(process).GetIndex(str);

            str.Dispose();

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }





        
    }
}
