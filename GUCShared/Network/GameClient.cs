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

        NPC character;
        public NPC Character { get { return this.character; } }

        new public int ID { get { return base.ID; } }

        World loadedWorld;

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

        Vec3f specPos, specDir;

        World specWorld = null;
        public World SpecWorld { get { return this.specWorld; } }

        bool isSpectating = false;
        public bool IsSpectating { get { return this.isSpectating; } }

        #region Position & Direction

        partial void pSpecGetPos();
        public Vec3f SpecGetPos()
        {
            pSpecGetPos();
            return this.specPos;
        }

        partial void pSpecGetDir();
        public Vec3f SpecGetDir()
        {
            pSpecGetDir();
            return this.specDir;
        }

        partial void pSpecSetPos();
        public void SpecSetPos(Vec3f position)
        {
            this.specPos = position.CorrectPosition();
            pSpecSetPos();
        }

        partial void pSpecSetDir();
        public void SpecSetDir(Vec3f direction)
        {
            this.specDir = direction.CorrectDirection();
            pSpecSetDir();
        }

        partial void pSpecSetPosDir();
        public void SpecSetPosDir(Vec3f position, Vec3f direction)
        {
            this.specPos = position.CorrectPosition();
            this.specDir = direction.CorrectDirection();
            pSpecSetPosDir();
        }

        #endregion

        #region Set to spectator mode
        
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

            if (this.isSpectating && specWorld == world)
            {
                SpecSetPosDir(position, direction);
                return;
            }

            this.specPos = position.CorrectPosition();
            this.specDir = direction.CorrectDirection();

            pSetToSpectate(world, specPos, specDir);
        }

        #endregion

        #endregion

        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
        }

        protected override void WriteProperties(PacketWriter stream)
        {
        }

        #endregion

        #region Vob guiding

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
