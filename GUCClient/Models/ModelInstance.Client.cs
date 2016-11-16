using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;
using GUC.Network;
using GUC.Scripting;

namespace GUC.Models
{
    public partial class ModelInstance : IDObject
    {
        #region Messages

        internal static class Messages
        {
            public static void ReadCreate(PacketReader stream)
            {
                ModelInstance instance = ScriptManager.Interface.CreateModelInstance();
                instance.ReadStream(stream);
                instance.ScriptObject.Create();
            }

            public static void ReadDelete(PacketReader stream)
            {
                ModelInstance instance;
                if (ModelInstance.TryGet(stream.ReadUShort(), out instance))
                {
                    instance.ScriptObject.Delete();
                }
            }
        }

        #endregion
    }
}
