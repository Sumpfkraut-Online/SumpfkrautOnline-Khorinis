using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Visuals
{
    public partial class ScriptOverlay : ScriptObject, Overlay.IScriptOverlay
    {
        Overlay baseOv;
        public Overlay BaseOverlay { get { return this.baseOv; } }

        public int ID { get { return this.baseOv.ID; } }
        public ModelDef Model { get { return (ModelDef)this.baseOv.Model.ScriptObject; } }
        public string Name { get { return this.baseOv.Name; } set { this.baseOv.Name = value; } }

        public ScriptOverlay()
        {
            this.baseOv = new Overlay(this);
        }

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }
    }
}
