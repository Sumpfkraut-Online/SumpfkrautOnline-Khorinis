using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using WinApi;

namespace GUC.GUI
{
    internal class PlayerText :  Text3D
    {
        private Player m_Player;
        public PlayerText(int id, Player player, float aMaxDistance)
            : base(id, player.Map, aMaxDistance, player.Position)
        {
            m_Player = player;
        }

        public override void updatePosition(Process process, long now)
        {
            this.m_World = m_Player.Map;
            this.m_Position = new Types.Vec3f(m_Player.Position);

            if (m_Player.Address != 0)
            {
                Gothic.zClasses.oCNpc npc = new Gothic.zClasses.oCNpc(process, m_Player.Address);

                this.m_Position.Y += npc.GetModel().BBox3DLocal.Max.Y * 1.6f;
            }

            base.updatePosition(process, now);
        }
    }
}
