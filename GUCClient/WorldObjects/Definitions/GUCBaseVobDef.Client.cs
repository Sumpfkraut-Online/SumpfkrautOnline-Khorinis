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
                byte type = stream.ReadByte();
                BaseVobInstance inst = ScriptManager.Interface.CreateInstance(type);
                inst.ReadStream(stream);
                inst.ScriptObject.Create();
            }

            public static void ReadDelete(PacketReader stream)
            {
                if (BaseVobInstance.TryGet(stream.ReadUShort(), out BaseVobInstance inst))
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
