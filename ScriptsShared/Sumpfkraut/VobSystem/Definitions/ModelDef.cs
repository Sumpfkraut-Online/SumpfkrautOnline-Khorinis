using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Definitions
{
    public partial class ModelDef : ScriptObject, Models.Model.IScriptModel
    {
        #region properties

        new public static readonly String _staticName = "ModelDef (static)";

        Models.Model baseDef;
        public Models.Model BaseDef { get { return baseDef; } }

        public int Id { get { return baseDef.ID; } }
        public bool IsStatic { get { return baseDef.IsStatic; } }

        public string Visual { get { return baseDef.Visual; } set { baseDef.Visual = value; } }

        public int Radius = 1;

        #endregion

        public ModelDef (PacketReader stream)
        {
            baseDef = new Models.Model();
            baseDef.ReadStream(stream);
        }

        partial void pCreate();
        public void Create()
        {
            baseDef.Create();
            pCreate();
        }

        partial void pDelete();
        public void Delete()
        {
            baseDef.Delete();
            pDelete();
        }

        public void OnReadProperties (PacketReader stream)
        { }

        public void OnWriteProperties (PacketWriter stream)
        { }
    }
}
