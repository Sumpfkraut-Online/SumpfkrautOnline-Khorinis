using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using Gothic.zClasses;
using Gothic.zTypes;
using GUC.Enumeration;
using WinApi.User.Enumeration;

namespace GUC.Client.GUI.MainMenu
{
    class MainMenuCharacter : MainMenuItem, InputReceiver
    {
        GUCVisualVob vis;

        AccCharInfo info;
        public AccCharInfo Info
        {
            get { return info; }
            set
            {
                
                info = value;
                if (value != null)
                {
                    thisNpc.SetAdditionalVisuals(((HumBodyMesh)info.BodyMesh).ToString(), info.BodyTex, 0,
                                                 ((HumHeadMesh)info.HeadMesh).ToString(), info.HeadTex, 0, -1);

                    using (zVec3 vec = zVec3.Create(Program.Process))
                    {
                        vec.X = info.BodyWidth;
                        vec.Y = info.BodyHeight;
                        vec.Z = info.BodyWidth;
                        thisNpc.SetModelScale(vec);
                    }
                    thisNpc.SetFatness(info.Fatness);

                    vis.SetVob(thisVob);
                }
                else
                {
                    vis.SetVob(null);
                }
            }
        }

        zCVob thisVob;
        oCNpc thisNpc { get { return new oCNpc(Program.Process, thisVob.Address); } }

        int rotation = 180;

        GUCVisual leftArrow, rightArrow;

        public MainMenuCharacter(string help, int x, int y, int w, int h)
        {
            HelpText = help;
            vis = new GUCVisualVob(x, y, w, h);
            thisVob = zCVob.Create(Program.Process);
            thisVob.SetVisual("HUMANS.MDS");
            Program.Process.Write(140, thisVob.Address + (int)oCItem.Offsets.inv_zbias);
            Program.Process.Write(rotation, thisVob.Address + (int)oCItem.Offsets.inv_roty);
            Info = new AccCharInfo();

            leftArrow = new GUCVisual(x + 150, y + h / 2 - 40, 15, 20);
            leftArrow.SetBackTexture("L.TGA");
            rightArrow = new GUCVisual(x + w - 170, y + h / 2 - 40, 15, 20);
            rightArrow.SetBackTexture("R.TGA");
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

            Program.Process.Write(rotation, thisVob.Address + (int)oCItem.Offsets.inv_roty);
        }
    }
}
