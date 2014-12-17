using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace Gothic.mClasses
{
    public class Bar
    {
        public int value;
        public int max;

        private zCView frontView = null;
        private zCView backView = null;

        private float posX;
        private float posY;

        private Process Process;

        public Bar(Process process)
            : this(process, 0, 0, "Bar_Misc.tga", "Bar_Back.tga")
        {
            
        }

        public Bar(Process process, int posX, int posY, String frontTex, String backTex)
        {
            Process = process;


            int[] size = InputHooked.PixelToVirtual(process, new int[]{180, 20});
            int[] sizeB = InputHooked.PixelToVirtual(process, new int[] { 180-7, 20-3 });
            int[] posB = InputHooked.PixelToVirtual(process, new int[] { 7, 3 });


            backView = zCView.Create(process, posX, posY, size[0], size[1]);
            zString tex = zString.Create(process, backTex);
            backView.InsertBack(tex);
            tex.Dispose();

            frontView = zCView.Create(process, posX + posB[0], posY + posB[1], sizeB[0], sizeB[1]);
            tex = zString.Create(process, frontTex);
            frontView.InsertBack(tex);
            tex.Dispose();

        }

        public void setMax(int max)
        {
            this.max = max;
        }

        public void setValue(int value)
        {
            this.value = value;

            //Breite berechnen!
            int[] sizeB = InputHooked.PixelToVirtual(Process, new int[] { 180 - 7, 20 - 3 });

            double valProcent = 100.0 / (double)max * (double)value;
            double size = (double)sizeB[0] / 100.0 * valProcent;
            frontView.SetSize((int)size, sizeB[1]);
        }

        public void setPosition(int x, int y)
        {
            int[] sizeB = InputHooked.PixelToVirtual(Process, new int[] { 7, 3 });
            backView.SetPos(x, y);
            frontView.SetPos(x + sizeB[0], y + sizeB[1]);
        }

        bool show = false;
        public void Show()
        {
            if (show)
                return;
            zCView.GetStartscreen(Process).InsertItem(backView, 0);
            zCView.GetStartscreen(Process).InsertItem(frontView, 0);

            show = true;
        }

        public void Hide()
        {
            if (!show)
                return;
            zCView.GetStartscreen(Process).RemoveItem(backView);
            zCView.GetStartscreen(Process).RemoveItem(frontView);

            show = false;
        }
    }
}
