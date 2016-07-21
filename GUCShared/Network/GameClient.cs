using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Types;

namespace GUC.Network
{
    public partial class GameClient : GameObject
    {
        #region ScriptObject

        /// <summary>
        /// The ScriptObject interface
        /// </summary>
        public partial interface IScriptClient : IScriptGameObject
        {
            void SetControl(NPC npc);
            void SetToSpectator(World world, Vec3f pos, Vec3f dir);
        }

        /// <summary>
        /// The ScriptObject of this Vob.
        /// </summary>
        public new IScriptClient ScriptObject
        {
            get { return (IScriptClient)base.ScriptObject; }
            set { base.ScriptObject = value; }
        }

        #endregion

        #region Properties

        NPC character = null;
        public NPC Character { get { return this.character; } }

        new public int ID { get { return base.ID; } }

        Vec3f specPos, specDir;
        /// <summary>
        /// Returns the current spectator position or position of controlled NPC.
        /// </summary>
        public Vec3f GetPosition()
        {
            if (this.isSpectating)
            {
                return this.specPos;
            }
            return this.character.GetPosition();
        }

        /// <summary>
        /// Returns the current spectator direction or direction of controlled NPC.
        /// </summary>
        public Vec3f GetDirection()
        {
            if (this.isSpectating)
            {
                return this.specDir;
            }
            return this.character.GetDirection();
        }

        World specWorld;
        public World GetWorld()
        {
            if (this.isSpectating)
            {
                return this.specWorld;
            }
            return this.character.World;
        }

        bool isSpectating = false;
        public bool IsSpectating { get { return this.isSpectating; } }

        #endregion

        #region Control

        partial void pSetControl(NPC npc);
        public void SetControl(NPC npc)
        {
            if (this.character == npc)
                return;

            pSetControl(npc);
        }

        #endregion

        #region Spectate

        partial void pSetToSpectate(World world, Vec3f pos, Vec3f dir);
        /// <summary>
        /// The client will lose control of its current NPC and move into spectator mode (free view).
        /// </summary>
        public void SetToSpectate(World world, Vec3f position, Vec3f direction)
        {
            if (world == null)
                throw new Exception("World is null!");
            if (!world.IsCreated)
                throw new Exception("World is not created!");

            pSetToSpectate(world, position, direction);
        }

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
        }

        protected override void WriteProperties(PacketWriter stream)
        {
        }

        #endregion

        #region Commanding

        List<Vob> cmdList = new List<Vob>();

        partial void pAddToCmdList(Vob vob);
        internal void AddToCmdList(Vob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (vob.Guide != null)
                throw new ArgumentNullException("Vob commander is not null!");

            cmdList.Add(vob);
            vob.Guide = this;

            pAddToCmdList(vob);
        }

        partial void pRemoveFromCmdList(Vob vob);
        internal void RemoveFromCmdList(Vob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            if (vob.Guide != this)
                throw new ArgumentNullException("Client is not commanding this vob!");

            cmdList.Remove(vob);
            vob.Guide = null;

            pRemoveFromCmdList(vob);
        }

        #endregion
    }
}
