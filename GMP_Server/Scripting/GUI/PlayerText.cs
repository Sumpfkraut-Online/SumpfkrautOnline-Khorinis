using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using RakNet;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.Scripting.GUI
{
    public class PlayerText : Text3D
    {
        Player m_TextPlayer;

        public PlayerText(Player aPlayer, Player aTextPlayer, float aMaxDistance, ColorRGBA aDefaultColor, long aDefaultBlendTime)
            : this(aPlayer, aTextPlayer, aMaxDistance, aDefaultColor, aDefaultBlendTime, true)
        { }


        public PlayerText(Player aTextPlayer)
            : this(null, aTextPlayer, 4000, ColorRGBA.White, 1000, true)
        { }

        public PlayerText(Player aTextPlayer, float aMaxDistance)
            : this(null, aTextPlayer, aMaxDistance, ColorRGBA.White, 1000, true)
        { }

        public PlayerText(Player aTextPlayer, float aMaxDistance, ColorRGBA aDefaultColor)
            : this(null, aTextPlayer, aMaxDistance, aDefaultColor, 1000, true)
        { }

        public PlayerText(Player aTextPlayer, float aMaxDistance, ColorRGBA aDefaultColor, long aDefaultBlendTime)
            : this(null, aTextPlayer, aMaxDistance, aDefaultColor, aDefaultBlendTime, true)
        { }

        internal PlayerText(Player aPlayer, Player aTextPlayer, float aMaxDistance, ColorRGBA aDefaultColor, long aDefaultBlendTime, bool aUseCreate)
            : base(aPlayer, aTextPlayer.World, aMaxDistance, aDefaultColor, aDefaultBlendTime, new Types.Vec3f(0, 0, 0), false)
        {
            m_TextPlayer = aTextPlayer;
            if (aUseCreate)
                create(-1);
        }


        public override void setPosition(Types.Vec3f aPosition)
        {
            throw new NotSupportedException("You can not set the Player-Text position!");
        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateTextPlayer);

            stream.Write(this.id);
            stream.Write(this.m_TextPlayer.ID);
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
