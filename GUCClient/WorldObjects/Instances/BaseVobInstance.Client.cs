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

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance : IDObject, VobTypeObject
    {
        #region Network Messages

        internal static class Messages
        {
            #region Create & Delete

            public static void ReadCreate(PacketReader stream)
            {
                BaseVobInstance inst = ScriptManager.Interface.CreateInstance((VobTypes)stream.ReadByte());
                inst.ReadStream(stream);
                inst.ScriptObject.Create();
            }

            public static void ReadDelete(PacketReader stream)
            {
                BaseVobInstance inst;
                if (BaseVobInstance.TryGet(stream.ReadUShort(), out inst))
                {
                    inst.ScriptObject.Delete();
                }
            }

            #endregion
        }

        #endregion

        public abstract zCVob CreateVob(zCVob vob = null);
    }
}
