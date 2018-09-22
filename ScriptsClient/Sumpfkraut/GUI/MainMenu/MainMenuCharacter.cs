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
        public float OffsetX { get { return vis.OffsetX; } set { vis.OffsetX = value; } }
        public float OffsetY { get { return vis.OffsetY; } set { vis.OffsetY = value; } }
        public float OffsetZ { get { return vis.OffsetZ; } set { vis.OffsetZ = value; } }
        public float RotationPitch { get { return vis.RotationPitch; } set { vis.RotationPitch = value; } }
        public float RotationYaw { get { return vis.RotationYaw; } set { vis.RotationYaw = value; } }
        public float RotationRoll { get { return vis.RotationRoll; } set { vis.RotationRoll = value; } }

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

        float rotation = 180;
        float distance = 100;

        GUCVisual leftArrow, rightArrow;

        public MainMenuCharacter(string help, int x, int y, int w, int h)
        {
            HelpText = help;

            thisVob = oCNpc.Create();

            vis = new GUCVobVisual(x, y, w, h)
            {
                Lighting = true,
            };
            UpdateOrientation();

            leftArrow = new GUCVisual(x + 150, y + h / 2 - 40, 15, 20);
            leftArrow.SetBackTexture("L.TGA");
            rightArrow = new GUCVisual(x + w - 170, y + h / 2 - 40, 15, 20);
            rightArrow.SetBackTexture("R.TGA");

            vis.CreateText("Zoom +/-", 120, 10);
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
            switch (key)
            {
                case VirtualKeys.Left:
                    rotation -= 8;
                    break;
                case VirtualKeys.Right:
                    rotation += 8;
                    break;
                case VirtualKeys.Add:
                    distance -= 8;
                    break;
                case VirtualKeys.Subtract:
                    distance += 8;
                    break;
                default:
                    return;
            }
            
            UpdateOrientation();
        }

        void UpdateOrientation()
        {
            const float distMax = 180;
            const float distMin = 0;
            if (distance < distMin)
                distance = distMin;
            else if (distance > distMax)
                distance = distMax;

            float offsetY = (distance - 100) * 0.4f - 20;

            vis.RotationYaw = Angles.Deg2Rad(rotation);
            vis.OffsetY = offsetY;
            vis.OffsetZ = distance;
        }
    }
}
