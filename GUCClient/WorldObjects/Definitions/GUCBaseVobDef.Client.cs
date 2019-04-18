using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using GUC.GameObjects;
using GUC.WorldObjects.Collections;
using GUC.Network;
using GUC.Scripting;
using GUC.Types;

namespace GUC.WorldObjects.Definitions
{
    public abstract partial class GUCBaseVobDef : IDObject, VobTypeObject
    {
        #region Network Messages

        internal static class Messages
        {
            #region Create & Delete

            public static void ReadCreate(PacketReader stream)
            {
                byte type = stream.ReadByte();
                GUCBaseVobDef def = ScriptManager.Interface.CreateVobDefinition(type);
                def.ReadStream(stream);
                def.ScriptObject.Create();
            }

            public static void ReadDelete(PacketReader stream)
            {
                if (GUCBaseVobDef.TryGet(stream.ReadUShort(), out GUCBaseVobDef def))
                {
                    def.ScriptObject.Delete();
                }
            }

            #endregion
        }

        #endregion

        public abstract zCVob CreateVob(zCVob vob = null);
    }
}
