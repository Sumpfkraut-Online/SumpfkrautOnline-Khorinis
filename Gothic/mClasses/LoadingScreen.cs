using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace Gothic.mClasses
{
    public static class LoadingScreen
    {
        static zCView loadingView;
        public static void Show(Process process)
        {
            if (loadingView == null)
            {
                loadingView = zCView.Create(process, 0, 0, 0x2000, 0x2000);
                zString tex = zString.Create(process, "loading.tga");
                loadingView.InsertBack(tex);
                tex.Dispose();
            }

            zCView.GetStartscreen(process).InsertItem(loadingView, 0);
            

            //zCViewProgressBar progressbar = zCViewProgressBar.Create(process, 0x1254, 0x3e8, 0x1e0c, 0x5dc, Gothic.zClasses.zCView.zTviewID.VIEW_ITEM);
            ////oCGame.Game(process).Progressbar = progressbar;
            ////view.InsertItem(progressbar, 0);
            //progressbar.SetRange(0, 100);
            //progressbar.SetPercent(20, zString.Create(process, " Yay !"));
            
        }

        public static void Hide(Process process)
        {
            zCView.GetStartscreen(process).RemoveItem(loadingView);
        }
    }
}
