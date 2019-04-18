using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;
using GUC.Network;
using GUC.Scripting;

namespace GUC.Models
{
    public partial class GUCModelDef : IDObject
    {
        #region Messages

        internal static class Messages
        {
            public static void ReadCreate(PacketReader stream)
            {
                GUCModelDef def = ScriptManager.Interface.CreateModelDefinition();
                def.ReadStream(stream);
                def.ScriptObject.Create();
            }

            public static void ReadDelete(PacketReader stream)
            {
                if (GUCModelDef.TryGet(stream.ReadUShort(), out GUCModelDef def))
                {
                    def.ScriptObject.Delete();
                }
            }
        }

        #endregion
    }
}
