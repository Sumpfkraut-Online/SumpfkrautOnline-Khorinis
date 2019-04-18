using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Models;
using GUC.Network;
using GUC.Scripting;
using GUC.GameObjects;

namespace GUC.Animations
{
    public partial class Overlay : IDObject
    {
        #region ScriptObject

        public interface IScriptOverlay : IScriptGameObject
        {
        }

        new public IScriptOverlay ScriptObject { get { return (IScriptOverlay)base.ScriptObject; } }

        #endregion

        #region Constructors

        public Overlay(IScriptOverlay scriptObject) : base(scriptObject)
        {
        }

        #endregion

        #region Properties

        string name = "";
        /// <summary>
        /// The Gothic name of the overlay. (case insensitive)
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                CanChangeNow();
                if (value == null)
                    this.name = "";
                else
                    this.name = value.ToUpper();
            }
        }

        GUCModelDef model;
        /// <summary>
        /// The associated model of this overlay.
        /// </summary>
        public GUCModelDef Model { get { return this.model; } }

        /// <summary>
        /// Is true when this is added to a model.
        /// </summary>
        public bool IsCreated { get { return this.isCreated; } }

        internal void SetModel(GUCModelDef model)
        {
            this.model = model;
            this.isCreated = model != null;
        }

        #endregion
        
        #region Read & Write

        protected override void ReadProperties(PacketReader stream)
        {
            base.ReadProperties(stream);
            this.Name = stream.ReadString();
        }

        protected override void WriteProperties(PacketWriter stream)
        {
            base.WriteProperties(stream);
            stream.Write(this.Name);
        }

        #endregion
    }
}
