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
        float distance = 130;

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
            
            UpdateOrientation();
        }

        void UpdateOrientation()
        {
            if (distance < 40)
                distance = 40;
            else if (distance > 180)
                distance = 180;

            float offsetY = (distance - 180) / 1.8f + 10;

            vis.RotationYaw = Angles.Deg2Rad(rotation);
            vis.OffsetY = offsetY;
            vis.OffsetZ = distance - 180.0f;
        }
    }
}
