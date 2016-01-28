using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GUC.Client.Network;
using Gothic;
using WinApi;

namespace GUC.Client
{
    //zCMesh::Load(class zSTRING const &, int)
    //( zCMesh::MergeMesh(class zCMesh *, class zMAT4 const &) ? )
    //zCWorld::CompileWorld(zTBspTreeMode const &,float,int,int,zCArray<zCPolygon *> *) ???

    public static class Program
    {
        public static string ProjectName { get; internal set; }
        public static string ProjectPath { get; internal set; }

        public static void Exit()
        {
            GameClient.Disconnect();
            Thread.Sleep(123);
            zCOption.GetSectionByName("INTERNAL").GetEntryByName("gameAbnormalExit").VarValue.Set("0");
            zCOption.Save("Gothic.ini");
            CGameManager.ExitGameVar = 1;
            Process.CDECLCALL<NullReturnCall>(0x00425F30); // ExitGameFunc
        }
    }
}
