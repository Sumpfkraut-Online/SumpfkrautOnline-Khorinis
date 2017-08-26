using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using WinApi.User.Enumeration;
using GUC.GUI;
using WinApi;
using GUC.Types;
using Gothic.Types;

namespace GUC.Scripts.Sumpfkraut.GUI.MainMenu
{
    class MainMenuCharacter : MainMenuItem, InputReceiver
    {
        GUC3DVisual vis;
        
        public void SetAdditionalVisuals(string bodyMesh, int bodyTex, string headMesh, int headTex)
        {
            thisVob.SetAdditionalVisuals(bodyMesh, bodyTex, 0, headMesh, headTex, 0, -1);
        }

        public void SetScale(Vec3f scale)
        {
            using (zVec3 vec = scale.CreateGVec())
            {
                thisVob.SetModelScale(vec);
            }
        }
        
        public void SetFatness(float fatness)
        {
            thisVob.SetFatness(fatness);
        }
        
        oCNpc thisVob;

        int rotation = 180;

        GUCVisual leftArrow, rightArrow;

        public MainMenuCharacter(string help, int x, int y, int w, int h)
        {
            HelpText = help;
            
            thisVob = oCNpc.Create();

            vis = new GUC3DVisual(x, y, w, h);
            vis.SetVob(thisVob);

            //Process.Write(thisVob.Address + oCItem.VarOffsets.inv_zbias, 140);
            //Process.Write(thisVob.Address + oCItem.VarOffsets.inv_roty, rotation);

            leftArrow = new GUCVisual(x + 150, y + h / 2 - 40, 15, 20);
            leftArrow.SetBackTexture("L.TGA");
            rightArrow = new GUCVisual(x + w - 170, y + h / 2 - 40, 15, 20);
            rightArrow.SetBackTexture("R.TGA");
        }

        public void SetVisual(string visual)
        {
            thisVob.SetVisual(visual.ToUpperInvariant());
        }

        public override void Show()
        {
            vis.Show();
        }

        public override void Hide()
        {
            vis.Hide();
            leftArrow.Hide();
            rightArrow.Hide();
        }

        public override void Select()
        {
            leftArrow.Show();
            rightArrow.Show();
        }

        public override void Deselect()
        {
            leftArrow.Hide();
            rightArrow.Hide();
        }

        public void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Left) {
                rotation += 5;
            }
            else if (key == VirtualKeys.Right) {
                rotation -= 5;
            } else return;

            //Process.Write(thisVob.Address + oCItem.VarOffsets.inv_roty, rotation);
        }
    }
}
