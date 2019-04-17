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
            void OnDisconnection(int id);

            void SetControl(GUCNPCInst npc);
            void SetToSpectator(World world, Vec3f pos, Angles ang);
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

        GUCNPCInst character;
        public GUCNPCInst Character { get { return this.character; } }

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

        bool loading;
        public bool Loading { get { return this.loading; } internal set { this.loading = value; } }

        partial void pSetControl(GUCNPCInst npc);
        /// <summary> Lets this client take control of the given NPC. Can be null. </summary>
        public void SetControl(GUCNPCInst npc)
        {
            if (this.character == npc)
                return;

            pSetControl(npc);
        }

        #endregion

        #region Spectate

        Vec3f specPos;
        Angles specAng;

        World specWorld;
        public World SpecWorld { get { return this.specWorld; } }

        bool isSpectating = false;
        public bool IsSpectating { get { return this.isSpectating; } }

        #region Position & Angles

        partial void pSpecGetPos();
        public Vec3f SpecGetPos()
        {
            pSpecGetPos();
            return this.specPos;
        }

        partial void pSpecGetAng();
        public Angles SpecGetAng()
        {
            pSpecGetAng();
            return this.specAng;
        }

        partial void pSpecSetPos();
        public void SpecSetPos(Vec3f position)
        {
            this.specPos = position.ClampToWorldLimits();
            pSpecSetPos();
        }

        partial void pSpecSetAng();
        public void SpecSetDir(Angles angles)
        {
            this.specAng = angles.Clamp();
            pSpecSetAng();
        }

        partial void pSpecSetPosAng();
        public void SpecSetPosAng(Vec3f position, Angles angles)
        {
            this.specPos = position.ClampToWorldLimits();
            this.specAng = angles.Clamp();
            pSpecSetPosAng();
        }

        #endregion

        #region Set to spectator mode

        partial void pSetToSpectate(World world, Vec3f pos, Angles ang);
        /// <summary>
        /// The client will lose control of its current NPC and move into spectator mode (free view).
        /// </summary>
        public void SetToSpectate(World world, Vec3f position, Angles angles)
        {
            if (world == null)
                throw new Exception("World is null!");
            if (!world.IsCreated)
                throw new Exception("World is not created!");
            
            if (this.isSpectating && specWorld == world)
            {
                SpecSetPosAng(position, angles);
                return;
            }

            this.specPos = position.ClampToWorldLimits();
            this.specAng = angles.Clamp();

            pSetToSpectate(world, specPos, specAng);
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
