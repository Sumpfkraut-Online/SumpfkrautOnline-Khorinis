using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects.Collections;


namespace GUC.Scripts.Sumpfkraut.GUI
{
    class GUCDialog : GUCView
    {

        GUCVisual background;
        GUCVisualText info;

        public GUCDialog()
        {

            background = new GUCVisual(200, 200, 200, 200);
            info = background.CreateText("Hello");
            background.SetBackTexture("Inv_Desc.tga");
        }

        public override void Show()
        {
            background.Show();
        }

        public override void Hide()
        {
            background.Hide();
        }
    }
}
