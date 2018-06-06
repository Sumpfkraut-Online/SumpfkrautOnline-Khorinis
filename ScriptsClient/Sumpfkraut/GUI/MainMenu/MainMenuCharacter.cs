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
        GUCVobVisual vis;

        public bool Lighting { get { return vis.Lighting; } set { vis.Lighting = value; } }
        public int Distance { get { return vis.Distance; } set { vis.Distance = value; } }
        public int RotationX { get { return vis.RotationX; } set { vis.RotationX = value; } }
        public int RotationY { get { return vis.RotationY; } set { vis.RotationY = value; } }
        public int RotationZ { get { return vis.RotationZ; } set { vis.RotationZ = value; } }

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
        int distance = 150;

        GUCVisual leftArrow, rightArrow;

        public MainMenuCharacter(string help, int x, int y, int w, int h)
        {
            HelpText = help;

            thisVob = oCNpc.Create();

            vis = new GUCVobVisual(x, y, w, h)
            {
                Lighting = true,
                Distance = distance,
                RotationY = rotation
            };

            leftArrow = new GUCVisual(x + 150, y + h / 2 - 40, 15, 20);
            leftArrow.SetBackTexture("L.TGA");
            rightArrow = new GUCVisual(x + w - 170, y + h / 2 - 40, 15, 20);
            rightArrow.SetBackTexture("R.TGA");
        }

        public void SetVisual(string visual)
        {
            thisVob.SetVisual(visual.ToUpperInvariant());
            vis.SetVisualFromVob(thisVob);
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
            Log.Logger.Log(key);
            switch (key)
            {
                case VirtualKeys.Left:
                    rotation += 5;
                    break;
                case VirtualKeys.Right:
                    rotation -= 5;
                    break;
                case VirtualKeys.OEMPlus:
                case VirtualKeys.Add:
                    distance -= 5;
                    break;
                case VirtualKeys.OEMMinus:
                case VirtualKeys.Subtract:
                    distance += 5;
                    break;
                default:
                    return;
            }


            vis.RotationY = rotation;
            vis.Distance = distance;
        }
    }
}
