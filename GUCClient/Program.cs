using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GUC.Network;
using Gothic;
using WinApi;
using System.IO;

namespace GUC.Client
{
    //zCMesh::Load(class zSTRING const &, int)
    //( zCMesh::MergeMesh(class zCMesh *, class zMAT4 const &) ? )
    //zCWorld::CompileWorld(zTBspTreeMode const &,float,int,int,zCArray<zCPolygon *> *) ???

    public static class Program
    {
        public static string ProjectName { get; private set; }
        public static string ProjectPath { get; private set; }
        public static string GUCDll { get; private set; }

        internal static void SetPaths(string projectName)
        {
            Program.ProjectName = projectName;

            string current = Path.GetFullPath(Environment.CurrentDirectory); // It's Gothic2/System when the process starts, Gothic2/ afterwards.

            if (File.Exists(current + "\\Gothic2.exe"))
            { // Gothic2/System/
                Program.ProjectPath = Path.GetFullPath(current + "\\Multiplayer\\UntoldChapters\\" + projectName + "\\");
            }
            else if (File.Exists(current + "\\System\\Gothic2.exe"))
            { // Gothic2/
                Program.ProjectPath = Path.GetFullPath(current + "\\System\\Multiplayer\\UntoldChapters\\" + projectName + "\\");
            }
            else
            {
                throw new Exception("Gothic 2 not found!");
            }

            Program.GUCDll = Program.ProjectPath + "GUC.dll";
        }
        
        public static void Exit()
        {
            GameClient.Client.Disconnect();
            Thread.Sleep(123);
            zCOption.GetSectionByName("INTERNAL").GetEntryByName("gameAbnormalExit").VarValue.Set("0");
            zCOption.Save("Gothic.ini");
            CGameManager.ExitGameVar = 1;
            //Process.CDECLCALL<NullReturnCall>(0x00425F30); // ExitGameFunc
        }
    }
}
