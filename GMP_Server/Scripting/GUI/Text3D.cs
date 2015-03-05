using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripting.GUI
{
    public class Text3D : View
    {
        protected Types.Vec3f m_Position;
        protected List<Text3DRow> m_Rows;

        protected Player m_Player = null;

        protected ColorRGBA m_DefaultColor = ColorRGBA.White;
        protected float m_MaxDistance;
        protected long m_DefaultBlendTime = 1000;
        protected World m_World;

        internal Text3D(Player aPlayer, World aWorld, float aMaxDistance, ColorRGBA aDefaultColor, long aDefaultBlendTime, Types.Vec3f aPosition, bool aUseCreate)
            : base(0, 0, aPlayer != null, aPlayer != null ? aPlayer.ID : 0, null)
        {
            if (aPosition == null)
                throw new ArgumentException("Parameter Position can not be null!");
            if(aDefaultColor == null)
                throw new ArgumentException("Parameter DefaultColor can not be null!");

            m_Position = aPosition;
            m_Rows = new List<Text3DRow>();
            m_Player = aPlayer;
            m_MaxDistance = aMaxDistance;
            m_DefaultColor = aDefaultColor;
            m_DefaultBlendTime = aDefaultBlendTime;
            m_World = aWorld;

            if (aUseCreate)
                create(-1);
        }

        public Text3D(Player aPlayer, float aMaxDistance, ColorRGBA aDefaultColor, World aWorld, Types.Vec3f aPosition)
            : this(aPlayer, aWorld, aMaxDistance, aDefaultColor, 500, aPosition, true)
        { }

        public Text3D(float aMaxDistance, ColorRGBA aDefaultColor, World aWorld, Types.Vec3f aPosition)
            : this(null, aWorld, aMaxDistance, aDefaultColor, 500, aPosition, true)
        { }




        public virtual void setPosition(Vec3f aPosition)
        {
            if (aPosition == null)
                throw new ArgumentNullException("Parameter Pos can not be null");
            this.m_Position = aPosition;

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.Text3DPosition);

            stream.Write(this.id);
            stream.Write(this.m_Position);

            sendStream(0, stream);
        }

        public void Clear()
        {
            m_Rows.Clear();


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.Text3DClear);

            stream.Write(this.id);
            sendStream(0, stream);
        }

        public override void setPosition(Vec2i aPosition)
        {
            throw new NotSupportedException("Use setPosition(Vec3f) instead!");
        }

        public void addRow(Player aPlayer, String aText, ColorRGBA aColor, long aTimer, long aBlendTime)
        {
            Text3DRow row = new Text3DRow();
            row.Color = aColor;
            row.Text = aText;
            row.Time = aTimer;
            row.BlendTime = aBlendTime;

            if (aColor == null)
                row.Color = m_DefaultColor;
            

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.Text3DAddRow);

            stream.Write(this.id);
            stream.Write(row.Text);
            stream.Write(row.Color);
            stream.Write(row.Time);
            stream.Write(row.BlendTime);

            sendStream(aPlayer == null ? 0 : aPlayer.ID, stream);

            if (aPlayer == null)
            {
                m_Rows.Add(row);
            }
        }

        

        public void addRow(Player aPlayer, String aText, long aTimer)
        {
            addRow(aPlayer, aText, m_DefaultColor, aTimer, m_DefaultBlendTime);
        }

        public void addRow(Player aPlayer, String aText)
        {
            addRow(aPlayer, aText, m_DefaultColor, 0, m_DefaultBlendTime);
        }
        
        public void addRow(String aText, ColorRGBA aColor, long aTimer, long aBlendTime)
        {
            addRow(null, aText, aColor, aTimer, aBlendTime);
        }

        public void addRow(String aText, ColorRGBA aColor, long aTimer)
        {
            addRow(null, aText, aColor, aTimer, m_DefaultBlendTime);
        }

        public void addRow(String aText, long aTimer)
        {
            addRow(null, aText, m_DefaultColor, aTimer, m_DefaultBlendTime);
        }

        public void addRow(String aText)
        {
            addRow(null, aText, m_DefaultColor, 0, m_DefaultBlendTime);
        }



        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateText3D);

            stream.Write(this.id);
            stream.Write(this.m_Position);
            stream.Write(this.m_World.Name);
            stream.Write(this.m_MaxDistance);
            stream.Write(ParentID);

            stream.Write(this.m_Rows.Count);
            for (int i = 0; i < this.m_Rows.Count; i++)
            {
                stream.Write(m_Rows[i].Text);
                stream.Write(m_Rows[i].Color);
                stream.Write(m_Rows[i].Time);
                stream.Write(m_Rows[i].BlendTime);
            }

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)GUC.WorldObjects.sWorld.VobDict[to]).ScriptingNPC);
            }
        }
    }
}
