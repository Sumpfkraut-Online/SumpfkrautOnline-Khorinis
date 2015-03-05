using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using GUC.WorldObjects.Character;

namespace GUC.GUI
{
    public class Text3D : View
    {
        public class Text3DRow
        {
            public String Text {get; set;}
            public zCViewText GText { get; set; }
            public GUC.Types.ColorRGBA Color { get; set; }
            public long Time { get; set; }
            public long InsertTime { get; set; }
            public long BlendTime { get; set; }
        }
        protected GUC.Types.Vec3f m_Position = new GUC.Types.Vec3f();

        protected List<Text3DRow> m_Rows = new List<Text3DRow>();

        protected zCView m_View = null;
        protected float m_MaxDistance = 1000.0f;

        protected bool m_TempShown = false;
        protected String m_World = "";

        public Text3D(int id, String aWorld, float aMaxDistance, GUC.Types.Vec3f aPosition)
            : base(id, new Types.Vec2i(0, 0))
        {
            Process process = Process.ThisProcess();

            m_Position = aPosition;
            m_MaxDistance = aMaxDistance;
            m_World = aWorld;

            m_View = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            
        }

        public void Clear()
        {
            Text3DRow[] rows = m_Rows.ToArray();
            foreach (Text3DRow row in rows)
            {
                removeRow(row);
            }
        }

        public Text3DRow addRow(Text3DRow tr)
        {
            Process process = Process.ThisProcess();

            m_Rows.Add(tr);

            using (zString str = zString.Create(process, tr.Text))
            {
                tr.GText = m_View.CreateText(0x0000 + m_Rows.Count * 100, 0x0000 + m_View.FontY() * m_Rows.Count, str);
                tr.GText.Timed = 0;
                tr.GText.Timer = -1;

                tr.GText.PosX = 0;
                tr.GText.PosY = 0;


                tr.GText.Color.R = tr.Color.R;
                tr.GText.Color.G = tr.Color.G;
                tr.GText.Color.B = tr.Color.B;
                tr.GText.Color.A = tr.Color.A;

                tr.InsertTime = DateTime.Now.Ticks / 10000;
            }

            return tr;
        }

        public void removeRow(Text3DRow tr)
        {
            m_Rows.Remove(tr);
            tr.GText.Timed = 1;
            tr.GText.Timer = 0;
        }

        protected void setTempShown(bool aShown)
        {
            if (m_TempShown == aShown)
                return;
            m_TempShown = aShown;

            if (m_TempShown)
            {
                zCView.GetStartscreen(Process.ThisProcess()).InsertItem(m_View, 0);
                Program.OnRender += updatePosition;
                Program.OnRenderTimedSecond -= updatePosition;
            }
            else
            {
                zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(m_View);
                Program.OnRenderTimedSecond += updatePosition;
                Program.OnRender -= updatePosition;
            }
        }

        public virtual void updatePosition(Process process, long now)
        {
            if (m_Rows.Count == 0)
                return;

            if (Player.Hero == null || Player.Hero.Map == null || Player.Hero.Map.Length == 0)
                return;

            now /= 10000;

            if (m_MaxDistance > 0.0)
            {
                float distance = Player.Hero.Position.getDistance(this.m_Position);
                bool tempShown = distance <= m_MaxDistance;
                tempShown &= Player.Hero.Map == m_World;

                setTempShown(tempShown);
            }
            else if(Player.Hero.Map != m_World)
            {
                setTempShown(false);
            }else{
                setTempShown(true);
            }


            if (m_TempShown)
            {
                zCCamera camera = zCCamera.getActiveCamera(process);
                zCView screen = m_View;// zCView.GetStartscreen(process);// zCView.GetScreen(Process.ThisProcess());
                //zCCamera.getActiveCamera(process)

                int fontY = screen.FontY();

                using (zVec3 gPosition = zVec3.Create(process))
                {
                    gPosition.X = m_Position.X; gPosition.Y = m_Position.Y; gPosition.Z = m_Position.Z;


                    using (zVec3 newPos = camera.CamMatrix * gPosition)
                    {
                        if (newPos.Z > 0.0f)
                        {
                            float x = 0, y = 0;
                            camera.Project(newPos, out x, out y);
                            Text3DRow[] rows = m_Rows.ToArray();
                            for (int i = 0; i < rows.Length; i++)
                            {
                                int fontWidth = screen.FontSize(rows[i].Text + "\n");
                                int fontHeight = (rows.Length-(i)) * fontY;

                                int x2 = 0, y2 = 0;
                                x2 = screen.anx((int)x) - fontWidth / 2;
                                y2 = screen.any((int)y) - fontHeight;

                                rows[i].GText.PosX = x2;
                                rows[i].GText.PosY = y2;

                                if (rows[i].Time > 0)
                                {
                                    long t = now - rows[i].InsertTime;
                                    if (t > rows[i].Time)
                                    {
                                        removeRow(rows[i]);
                                    }

                                    if (t > rows[i].Time - rows[i].BlendTime)
                                    {
                                        float t2 = t - (rows[i].Time - rows[i].BlendTime);
                                        float alpha = 1.0f - (1.0f / rows[i].BlendTime * t2);
                                        rows[i].GText.Color.A = (byte)(rows[i].Color.A * alpha);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < m_Rows.Count; i++)
                            {
                                m_Rows[i].GText.PosX = 0x2000;
                                m_Rows[i].GText.PosY = 0x2000;
                            }
                        }
                    }

                }
            }
        }

        public void setPosition(Types.Vec3f position)
        {
            m_Position = position;
        }

        public override void setPosition(Types.Vec2i position)
        {
            throw new NotSupportedException("Text 3D uses 3 coordinates, use setPosition(Types.Vec3f) instead!");
        }

        public override void hide()
        {
            if (!this.isShown)
                return;

            if (m_TempShown)
            {
                zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(m_View);
                Program.OnRender -= updatePosition;
            }
            else
            {
                Program.OnRenderTimedSecond -= updatePosition;
            }
            
        }

        public override void show()
        {
            if (this.isShown)
                return;
            if (m_TempShown)
            {
                zCView.GetStartscreen(Process.ThisProcess()).InsertItem(m_View, 0);
                Program.OnRender += updatePosition;
            }
            else
            {
                Program.OnRenderTimedSecond += updatePosition;
            }
        }

        public override void Destroy()
        {
            hide();
        }
    }
}
