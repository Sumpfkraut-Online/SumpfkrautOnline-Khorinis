using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.Types;
using GUC.GameObjects;

namespace GUC.Network
{
    public partial class GameClient : IDObject
    {
        #region ScriptObject

        /// <summary>
        /// The ScriptObject interface
        /// </summary>
        public partial interface IScriptClient : IScriptGameObject
        {
            void OnConnection();
            void OnDisconnection();

            void SetControl(NPC npc);
            void SetToSpectator(World world, Vec3f pos, Vec3f dir);
        }

        /// <summary>
        /// The ScriptObject of this object.
        /// </summary>
        public new IScriptClient ScriptObject { get { return (IScriptClient)base.ScriptObject; } }

        #endregion

        #region Constructors

        public GameClient(IScriptClient scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        NPC character;
        public NPC Character { get { return this.character; } }

        new public int ID { get { return base.ID; } }

        public bool IsIngame { get { return (this.character != null && this.character.IsSpawned) || this.isSpectating; } }

        /// <summary>
        /// Returns the world this client's character is in or this client is spectating.
        /// </summary>
        public World World
        {
            get
            {
                if (this.character != null)
                    return this.character.World;
                else if (this.isSpectating)
                    return this.specWorld;
                return null;
            }
        }

        #endregion

        #region Control

        partial void pSetControl(NPC npc);
        /// <summary> Lets this client take control of the given NPC. Can be null. </summary>
        public void SetControl(NPC npc)
        {
            if (this.character == npc)
                return;

            pSetControl(npc);
        }

        #endregion

        #region Spectate

        Vec3f specPos, specDir;

        World specWorld;
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
    }
}
